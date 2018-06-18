using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerPhoton : MonoBehaviour
{

   // Color spawnColor;    //Farbe des spawnenden Blocks
	public Vector3 spawnColor;

    Vector3 standardScale;
    public float spawnTimer;    //Zeit des Aufladens, die benötigt wird, bis der Block spawnen soll in Sekunden
    public float blockChargeTime;   //aktuelle Zeit des Aufladens 
    public GameObject blockPrefab;  //der zu spawnende Block wird als Prefab in den Inspector gezogen und hier referenziert
    public bool spawnable = true;  //sofern ein Block spawnen darf (nicht kollidiert), ist die Variable true
    List<GameObject> collidingObjects = new List<GameObject>(); //Liste der GameObjects, die mit dem Spawner kollidieren
    public GameObject blockSpawnSprite; //Sprite des charging-Blocks (Child)
	public PlayerPhoton player;
	public PlayerLogging playerLogging;

	public int playerTeam;

	public Color blockColor;
    public int blockCount = 0;
    private AudioSource audioSource;
    public AudioClip soundPlaceBlock;
    public AudioClip soundChargeBlock;
    public AudioClip soundPlacementBlocked;
    private bool blockChargingSound;

	PhotonView pv;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	[PunRPC]
	public void Setup(){
		spawnColor = new Vector3(1,1,1); //Zu Beginn wird die Farbe des spawnenden Blocks weiß
		standardScale = new Vector3(1,1,1);
		SetSpawnerSize(0);            //und die Transparenz auf 0 gesetzt
		player = transform.parent.GetComponent<PlayerPhoton>();
		playerTeam = player.playerTeam;
		playerLogging = transform.parent.GetComponent<PlayerLogging>();
		audioSource = GetComponent<AudioSource>();
		soundPlaceBlock = Resources.Load<AudioClip>("Sounds/place_block");
		soundChargeBlock = Resources.Load<AudioClip>("Sounds/charge_block_1-5");
		soundPlacementBlocked = Resources.Load<AudioClip>("Sounds/placement_blocked");
		pv = gameObject.GetComponent<PhotonView> ();
	}


    //Bei Aktivierung des Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        //sofern das andere Objekt eine Bande, ein anderer Block, ein Ball oder der andere Spieler ist
        if (other.gameObject.tag.Equals("Boundary") || other.gameObject.tag.Equals("Block") || other.gameObject.tag.Equals("Ball") || other.gameObject.tag.Equals("Player"))
        {
            collidingObjects.Add(other.gameObject); //wird es in die Liste der kollidierenden Objekte aufgenommen

            spawnable = false;                  //wird der Spawner blockiert
            if (blockChargeTime > 0)
            {
				pv.RPC("SetBlockTransparency", PhotonTargets.All, 0.33f, spawnColor);
				//SetBlockTransparency(0.33f, spawnColor);             //und die Transparenz auf 33% gesetzt, sofern der Block aufgeladen wird.  
            }
        }

    }

    //Bei Verlassen des anderen Objektes
    private void OnTriggerExit2D(Collider2D other)
    {
        //sofern das andere Objekt eine Bande, ein anderer Block, ein Ball oder der andere Spieler ist
        if (other.gameObject.tag.Equals("Boundary") || other.gameObject.tag.Equals("Block") || other.gameObject.tag.Equals("Ball") || other.gameObject.tag.Equals("Player")) 
        {
            collidingObjects.Remove(other.gameObject);  //wird es aus der Liste entfernt
        }
        //wenn kein Objekt mit dem Spawner kollidiert
        if (collidingObjects.Count == 0)
        {
            pv.RPC("SetBlockTransparency", PhotonTargets.All, 1.0f, spawnColor);
			//SetBlockTransparency(1, spawnColor);  //wird die Transparenz richtig berechnet
            spawnable = true;   //und der spawner freigegeben
        }
    }

	public void RemoveObject(string name){
		GameObject go = GameObject.Find (name);
		if (collidingObjects.Contains (go)) {
			collidingObjects.Remove (go);
		}
	}

    //In der Methode wird die Transparenz des Blocks gesetzt. Übergeben wird ein Zeit Argument in fps
	[PunRPC]
	public void SetBlockTransparency(float transparency, Vector3 colVector)
    {
		Color colorNew = new Color(colVector.x, colVector.y,colVector.z);
		colorNew.a = transparency;    //Die Transparenz wird so modifiziert, dass der Block stetig weniger transparent wird. Wenn der Block komplett aufgeladen ist, wird er komplett transparent
		blockSpawnSprite.GetComponent<SpriteRenderer>().color = colorNew;    //und die Farbe wird an das Objekt übergeben
    }

    //In der Methode wird die Größe des Blocks gesetzt. Übergeben wird ein Zeit Argument in fps
	[PunRPC]
    public void SetSpawnerSize(float time)
    {
        blockSpawnSprite.transform.localScale = standardScale * (time / spawnTimer);
    }


    //Die Methode wird beim Festhalten des B-Buttons in jedem Frame aufgerufen und erhöht die blockChargeTime.
	[PunRPC]
    public void AddBlockChargeTime(float i)
    {
        if (!blockChargingSound)
        {
            PlaySound(soundChargeBlock, 0.4f);
            blockChargingSound = true;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        //sofern die Zeit noch geringer ist als die zu erreichende SpawnZeit
        if (blockChargeTime < spawnTimer)
        {
            blockChargeTime+= i;  //wird die Zeit erhöht
			pv.RPC("SetSpawnerSize", PhotonTargets.All, blockChargeTime);
			//SetSpawnerSize(blockChargeTime);  //und die Größe an den Block übergeben, sofern dieser nicht mit anderen Objekten kollidiert

            if (spawnable)
            {
              //  SetBlockTransparency(1, spawnColor);
				pv.RPC("SetBlockTransparency", PhotonTargets.All, 1.0f, spawnColor);
            } else if (!spawnable)
            {
				pv.RPC("SetBlockTransparency", PhotonTargets.All, 0.33f, spawnColor);
				//SetBlockTransparency(0.33f, spawnColor);
            }
        }
        if (blockChargeTime >= spawnTimer) {
            blockChargeTime = spawnTimer;
			pv.RPC("SetSpawnerSize", PhotonTargets.All,blockChargeTime);
            //SetSpawnerSize(blockChargeTime);

            if (spawnable)   //wenn das Ziel erreicht wurde und spawenbar ist
            {
                blockChargeTime = spawnTimer;
				pv.RPC("SetSpawnerSize", PhotonTargets.All,blockChargeTime);
				//SetSpawnerSize(blockChargeTime);

                //An die Farbe des Spielers anpassen
				pv.RPC("SetBlockTransparency", PhotonTargets.All, 1.0f, player.colorVector);
                //blockSpawnSprite.GetComponent<SpriteRenderer>().color = blockColor;    //wird der BlockSpawner in der Farbe des Spielers eingefärbt.
            }
            else if (!spawnable)     //wenn das Ziel erreicht wurde und kollidiert
            {
				pv.RPC("SetBlockTransparency", PhotonTargets.All, 0.33f, player.colorVector);
				//SetBlockTransparency(0.33f, blockColor); //wird der Block in der Farbe des Spielers und transparent gefärbt
            }
        }
    }

	[PunRPC]
    public void ResetBlockChargeTime()
    {
        blockChargingSound = false;
        blockChargeTime = 0;    //die Zeit des Aufladens wird zurückgesetzt
		pv.RPC("SetSpawnerSize", PhotonTargets.All,blockChargeTime);
		//SetSpawnerSize(blockChargeTime);  //ebenfalls die Größe des Spawners 
		pv.RPC("SetBlockTransparency", PhotonTargets.All, 0.33f, spawnColor);
		//SetBlockTransparency(0, spawnColor);
        GetComponent<SpriteRenderer>().enabled = false; //und der Rahmen ausgeblendet
    }

    //die Methode wird aufgerufen, sofern der B-Button losgelassen wird 
	[PunRPC]
    public void SpawnBlock()
    {
		pv.RPC ("StopAudio", PhotonTargets.All);
        //wenn das Ziel erreicht wurde und der Blockspawner nicht kollidiert
        if (blockChargeTime == spawnTimer && spawnable)
        {
			if (pv.isMine) {
				blockCount++;
				string blockName = "Block_" + blockCount + "_Player_" + playerTeam;
				GameObject block = PhotonNetwork.Instantiate ("BlockPrefabPhoton", this.transform.position, this.transform.rotation, 0);  //wird der Block aus dem Prefab instanziiert
				block.GetComponent<PhotonView> ().RPC ("SetColor", PhotonTargets.All, player.colorVector);
				block.GetComponent<PhotonView> ().RPC ("SetPlayerTeam", PhotonTargets.All, playerTeam);
				block.GetComponent<PhotonView> ().RPC ("SetBlockID", PhotonTargets.All, blockCount);
				block.GetComponent<PhotonView> ().RPC ("SetBlockName", PhotonTargets.All, blockName);

				//	block.GetComponent<BlockPhoton>().SetColor(blockColor); //und entsprechend der Teamfarbe eingefärbt
				//	block.GetComponent<BlockPhoton>().SetPlayerTeam(playerTeam);
				//	block.GetComponent<BlockPhoton>().SetBlockID(blockCount);
				playerLogging.AddBlock ();
			}
			pv.RPC("PlaySound", PhotonTargets.All, "soundPlaceBlock", 0.8f);
		//	PlaySound(soundPlaceBlock, 0.8f);
        } else if (blockChargeTime == spawnTimer && !spawnable)
        {
            PlaySound(soundPlacementBlocked, 0.6f);
        }
		pv.RPC("ResetBlockChargeTime", PhotonTargets.All);

}

    //Methode, um die Farbe des Blocks zu setzen
    public void SetColor(Color col)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col; //sowohl das Sprite bekommt die Farbe des Spielers 
        blockColor = col;       //als auch die Variable für das Erstellen der neuen Blöcke
    }

    private void PlaySound(AudioClip ac, float volume)
    {
        float lastTimeScale = Time.timeScale;
        Time.timeScale = 1f;
        audioSource.clip = ac;
        audioSource.volume = volume;
        audioSource.Play();
        Time.timeScale = lastTimeScale;
    }

	[PunRPC]
	public void PlaySound (string file, float volume)
	{
		float lastTimeScale = Time.timeScale;
		Time.timeScale = 1f;
		switch (file) {
		case "soundPlaceBlock":
			audioSource.PlayOneShot (soundPlaceBlock, volume);
			break;
		}

		Time.timeScale = lastTimeScale;
	}

	[PunRPC]
	public void StopAudio(){
		audioSource.Stop();
	}

}
