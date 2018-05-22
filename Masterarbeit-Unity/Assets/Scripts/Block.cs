using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [Range(0, 3)]
    public int health;

    public Sprite twoHealthBlock;
    public Sprite oneHealthBlock;
    private SpriteRenderer sr;
    private int playerTeam;
    private int blockID;


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
            sr.sprite = (Sprite)Resources.Load<Sprite>("Textures/Block_2Health");
        }
        else if (health == 1)
        {
                sr.sprite = Resources.Load<Sprite>("Textures/Block_1Health");
        }
        else if (health <= 0)
        {
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

    public void SetBlockID(int i)
    {
        blockID = i;
    }

    public int GetBlockID()
    {
        return blockID;
    }
}
