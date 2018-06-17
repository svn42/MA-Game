using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockPhoton : MonoBehaviour
{
    [Range(0, 3)]
    public int health;

    public Sprite twoHealthBlock;
    public Sprite oneHealthBlock;
    private SpriteRenderer spriteRenderer;
    private int playerTeam;
    private int blockID;
	private GameStatePhoton gameState;
    public GameObject blockDestructionPrefab;

    // Use this for initialization
    void Start()
    {
        health = 3;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		gameState = (GameStatePhoton)FindObjectOfType(typeof(GameStatePhoton));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReduceHealth(int damage)
    {
        health -= damage;
        if (health == 2)
        {
			spriteRenderer.sprite = Resources.Load<Sprite>("Textures/Block_2Health");
            gameState.PlaySound("soundSlap", 0.5f);
        }
        else if (health == 1)
        {
			spriteRenderer.sprite = Resources.Load<Sprite>("Textures/Block_1Health");
            gameState.PlaySound("soundSlap", 0.5f);

        }
        else if (health <= 0)
        {
            gameState.PlaySound("soundPlop", 0.4f);
            DestroyBlock();
        }

    }

	public void DestroyBlock()
	{
		GameObject go = Instantiate(blockDestructionPrefab, transform.position, transform.rotation);  //Die Zerstörungsanimation des Shots wird  instanziiert
		go.GetComponent<BlockDestruction>().SetColor(spriteRenderer.color);
		DeleteFromBlockSpawnCollider (gameObject.name);
		Destroy(gameObject);
	}

	//löscht den Block aus den Blockspawner Collider Listen
	public void DeleteFromBlockSpawnCollider(string name){
		BlockSpawnerPhoton[] blockspawner = GameObject.FindObjectsOfType<BlockSpawnerPhoton>();
		foreach (BlockSpawnerPhoton bs in blockspawner) {
			bs.RemoveObject (name);
		}
	}

	[PunRPC]
	public void SetColor(Vector3 colVector)
    {
		Color colorNew = new Color(colVector.x, colVector.y, colVector.z, 1);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.color = colorNew;
    }

    public Color GetColor()
    {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        return spriteRenderer.color;
    }

	[PunRPC]
	public void SetBlockName(string name)
	{
		gameObject.name = name;
	}

	[PunRPC]
    public void SetPlayerTeam(int i)
    {
        playerTeam = i;
    }

    public int GetPlayerTeam()
    {
        return playerTeam;
    }

	[PunRPC]
    public void SetBlockID(int i)
    {
        blockID = i;
    }

    public int GetBlockID()
    {
        return blockID;
    }
}
