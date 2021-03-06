﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockTutorial : MonoBehaviour
{
    [Range(0, 3)]
    public int health;

    public Sprite twoHealthBlock;
    public Sprite oneHealthBlock;
    private int playerTeam;
    private int blockID;
    private TutorialGameState tutorialGameState;
	private SpriteRenderer spriteRenderer;
    public GameObject blockDestructionPrefab;
	public TutorialBlockDestroyChallenge tbdc;

    // Use this for initialization
    void Start()
    {
        health = 3;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		if (tutorialGameState.challengeType.Equals("Goalkeeper")){
			Invoke ("DestroyBlock", 8);	//damit die Spieler sich nicht in der GoalkeeperChallenge verbarrikadieren, wird der Block dort nach 8 Sekunden zerstört
		}
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
            tutorialGameState.PlaySound("soundSlap", 0.5f);
        }
        else if (health == 1)
        {
			spriteRenderer.sprite = Resources.Load<Sprite>("Textures/Block_1Health");
            tutorialGameState.PlaySound("soundSlap", 0.5f);

        }
        else if (health <= 0)
        {
            tutorialGameState.PlaySound("soundPlop", 0.4f);
            DestroyBlock();
        }

    }

	public void DestroyBlock()
	{
		GameObject go = Instantiate(blockDestructionPrefab, transform.position, transform.rotation);  //Die Zerstörungsanimation des Shots wird  instanziiert
		go.GetComponent<BlockDestruction>().SetColor(spriteRenderer.color);
		DeleteFromBlockSpawnCollider (gameObject.name);

		//bei der BlockDestroy Challenge wird der zerstörte Block aus der Liste gelöscht
		if (tutorialGameState.challengeType.Equals ("BlockDestroy")) {
			tbdc = GameObject.FindObjectOfType<TutorialBlockDestroyChallenge> ();
			tbdc.RemoveBlock (gameObject);
		}

		Destroy(gameObject);


	}

	//löscht den Block aus den Blockspawner Collider Listen
	public void DeleteFromBlockSpawnCollider(string name){
		BlockSpawnerTutorial[] blockspawner = GameObject.FindObjectsOfType<BlockSpawnerTutorial>();
		foreach (BlockSpawnerTutorial bs in blockspawner) {
			bs.RemoveObject (name);
		}
	}

    public void SetColor(Color col)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;
    }

    public Color GetColor()
    {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        return spriteRenderer.color;
    }

    public void SetPlayerTeam(int i)
    {
        playerTeam = i;
    }

    public int GetPlayerTeam()
    {
        return playerTeam;
    }

    public void SetBlockID(int i)
    {
        blockID = i;
    }

    public int GetBlockID()
    {
        return blockID;
    }
}
