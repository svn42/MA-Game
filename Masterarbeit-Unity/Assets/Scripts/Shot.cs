using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    public float acceleration;  //der Beschleunigungswert des Partikels
    [Range(1, 3)]
    public int strength;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();        //mit jedem Frame wird das Objekt bewegt
    }

    //Methode zum Bewegen
    public void Move()
    {
        transform.Translate(transform.right * acceleration * Time.deltaTime, Space.World);
    }

    //das Partikel bekommt in der Methode den Inputvektor des Spielers übergeben
    public void SetDirection(Quaternion rotation)
    {
        //dieser wird mit der Beschleunigung des Partikels multipliziert
        transform.rotation = rotation;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject collidingObject = coll.gameObject;

        switch (collidingObject.tag)
        {
            case "Block":
                collidingObject.GetComponent<Block>().ReduceStrength(this.strength);
                DestroyShot();
                break;
            case "player":
                DestroyShot();
                //Stun player
                break;
            case "Boundary":
                DestroyShot();
                break;
            case "Ball":
                coll.rigidbody.AddForce(new Vector3(22f, 22f, 0f), ForceMode2D.Impulse);
                DestroyShot();
                break;
        }

    }

    public void DestroyShot()
    {
        Destroy(this.gameObject);
    }

    public void SetColor(Color col)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;
    }

}
