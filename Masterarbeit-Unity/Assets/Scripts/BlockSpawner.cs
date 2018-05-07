using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{

    Color spawnColor;    //Farbe des spawnenden Blocks
    Vector3 standardScale;
    public float spawnTimer;    //Zeit des Aufladens, die benötigt wird, bis der Block spawnen soll in Sekunden
    public float blockChargeTime;   //aktuelle Zeit des Aufladens 
    public GameObject blockPrefab;  //der zu spawnende Block wird als Prefab in den Inspector gezogen und hier referenziert
    public bool spawnable = true;  //sofern ein Block spawnen darf (nicht kollidiert), ist die Variable true
    List<GameObject> collidingObjects = new List<GameObject>(); //Liste der GameObjects, die mit dem Spawner kollidieren
    public GameObject blockSpawnSprite; //Sprite des charging-Blocks (Child)
    private Player player;

    private int playerTeam;

    private Color blockColor;
    public int blockCount = 0;



    // Use this for initialization
    void Start()
    {
        spawnColor = Color.white;           //Zu Beginn wird die Farbe des spawnenden Blocks weiß
        standardScale = blockSpawnSprite.transform.localScale;
        SetSpawnerSize(0);            //und die Transparenz auf 0 gesetzt
        spawnTimer *= 60;               //der SpawnTimer wird in Frames umgerechnet (60 fps)
        player = transform.parent.GetComponent<Player>();
        playerTeam = player.playerTeam;
    }

    // Update is called once per frame
    void Update()
    {

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
                SetBlockTransparency(0.33f, spawnColor);             //und die Transparenz auf 33% gesetzt, sofern der Block aufgeladen wird.  
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
            SetBlockTransparency(1, spawnColor);  //wird die Transparenz richtig berechnet
            spawnable = true;   //und der spawner freigegeben
        }
    }

    //In der Methode wird die Transparenz des Blocks gesetzt. Übergeben wird ein Zeit Argument in fps
    public void SetBlockTransparency(float transparency, Color col)
    {
        col.a = transparency;    //Die Transparenz wird so modifiziert, dass der Block stetig weniger transparent wird. Wenn der Block komplett aufgeladen ist, wird er komplett transparent
        blockSpawnSprite.GetComponent<SpriteRenderer>().color = col;    //und die Farbe wird an das Objekt übergeben
    }

    //In der Methode wird die Größe des Blocks gesetzt. Übergeben wird ein Zeit Argument in fps
    public void SetSpawnerSize(float time)
    {
        blockSpawnSprite.transform.localScale = standardScale * (time / spawnTimer);
    }


    //Die Methode wird beim Festhalten des B-Buttons in jedem Frame aufgerufen und erhöht die blockChargeTime.
    public void AddBlockChargeTime()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        //sofern die Zeit noch geringer ist als die zu erreichende SpawnZeit
        if (blockChargeTime < spawnTimer)
        {
            blockChargeTime++;  //wird die Zeit erhöht
            SetSpawnerSize(blockChargeTime);  //und die Transparenz an den Block übergeben, sofern dieser nicht mit anderen Objekten kollidiert

            if (spawnable)
            {
                SetBlockTransparency(1, spawnColor);
            } else if (!spawnable)
            {
                SetBlockTransparency(0.33f, spawnColor);
            }
        }
        else if (blockChargeTime == spawnTimer && spawnable)   //wenn das Ziel erreicht wurde und spawenbar ist
        {
            //TODO: An die Farbe des Spielers anpassen
            blockSpawnSprite.GetComponent<SpriteRenderer>().color = blockColor;    //wird der BlockSpawner in der Farbe des Spielers eingefärbt.
        }
        else if (blockChargeTime == spawnTimer && !spawnable)     //wenn das Ziel erreicht wurde und kollidiert
        {
            SetBlockTransparency(0.33f, blockColor); //wird der Block in der Farbe des Spielers und transparent gefärbt
        }
    }

    public void ResetBlockChargeTime()
    {
        blockChargeTime = 0;    //die Zeit des Aufladens wird zurückgesetzt
        SetSpawnerSize(blockChargeTime);  //ebenfalls die Größe des Spawners 
        SetBlockTransparency(0, spawnColor);
        collidingObjects = new List<GameObject>();
        GetComponent<SpriteRenderer>().enabled = false; //und der Rahmen ausgeblendet
    }

    //die Methode wird aufgerufen, sofern der B-Button losgelassen wird 
    public void SpawnBlock()
    {
        //wenn das Ziel erreicht wurde und der Blockspawner nicht kollidiert
        if (blockChargeTime == spawnTimer && spawnable)
        {
            blockCount++;
            GameObject block = Instantiate(blockPrefab, this.transform.position, this.transform.rotation);  //wird der Block aus dem Prefab instanziiert
            block.GetComponent<Block>().SetColor(blockColor); //und entsprechend der Teamfarbe eingefärbt
            block.GetComponent<Block>().SetPlayerTeam(playerTeam);
            block.GetComponent<Block>().SetBlockID(blockCount);

            block.name = "Block_" + blockCount + "_Player_" + playerTeam;

        }
        ResetBlockChargeTime();

}

    //Methode, um die Farbe des Blocks zu setzen
    public void SetColor(Color col)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col; //sowohl das Sprite bekommt die Farbe des Spielers 
        blockColor = col;       //als auch die Variable für das Erstellen der neuen Blöcke
    }

}
