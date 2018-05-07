﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [Range(0, 3)]
    private int health;

    public Sprite twoHealthBlock;
    public Sprite oneHealthBlock;
    private SpriteRenderer sr;
    private int playerTeam;


    // Use this for initialization
    void Start()
    {
        health = 3;
        sr = gameObject.GetComponent<SpriteRenderer>();
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
            Debug.Log(health);
            sr.sprite = (Sprite)Resources.Load<Sprite>("Textures/Block_2Health");
        }
        else if (health == 1)
        {
            Debug.Log(health);
                sr.sprite = Resources.Load<Sprite>("Textures/Block_1Health");
        }
        else if (health <= 0)
        {
            Debug.Log(health);
            DestroyBlock();
        }

    }

    public void DestroyBlock()
    {
        Destroy(this.gameObject);
    }

    public void SetColor(Color col)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;
    }

    public void SetPlayerTeam(int i)
    {
        playerTeam = i;
    }

    public int GetPlayerTeam()
    {
        return playerTeam;
    }
}
