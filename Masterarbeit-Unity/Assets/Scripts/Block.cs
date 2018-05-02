using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [Range(0, 3)]
    private int strength;

    Sprite twoStrengthBlock;
    Sprite oneStrengthBlock;

    // Use this for initialization
    void Start()
    {
        strength = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReduceStrength(int damage)
    {
        strength -= damage;
        if (strength == 2)
        {
            Debug.Log(strength);
            //sprite anpassen
        }
        else if (strength == 1)
        {
            Debug.Log(strength);
            //sprite anpassen
        }
        else if (strength <= 0)
        {
            Debug.Log(strength);
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

}
