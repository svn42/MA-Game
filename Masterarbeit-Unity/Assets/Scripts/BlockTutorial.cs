﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockTutorial : MonoBehaviour
{
    [Range(0, 3)]
    public int health;

    public Sprite twoHealthBlock;
    public Sprite oneHealthBlock;
    private SpriteRenderer sr;
    private int playerTeam;
    private int blockID;
    private TutorialGameState tutorialGameState;
    SpriteRenderer spriteRenderer;
    public GameObject blockDestructionPrefab;

    // Use this for initialization
    void Start()
    {
        health = 3;
        sr = gameObject.GetComponent<SpriteRenderer>();
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));

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
            sr.sprite = Resources.Load<Sprite>("Textures/Block_2Health");
            tutorialGameState.PlaySound("soundSlap", 0.5f);
        }
        else if (health == 1)
        {
            sr.sprite = Resources.Load<Sprite>("Textures/Block_1Health");
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
        Destroy(gameObject);
    }

    public void SetColor(Color col)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;
    }

    public Color GetColor()
    {
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