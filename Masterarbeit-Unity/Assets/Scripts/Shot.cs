using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    public float acceleration;  //der Beschleunigungswert des Partikels
    [Range(1, 3)]
    public int strength;    //Stärke des Schusses. Normaler Schuss =1, Mittlerer Schuss = 2, Großer Schuss = 3.
    public int ballImpact;     //Wirkung des Schusses auf den Ball bei einer Kollision
    private int playerTeam;
    private int shotID;
    public float stunDuration;


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
                collidingObject.GetComponent<Block>().ReduceHealth(this.strength);
                DestroyShot();
                break;
            case "Player":
                // calculate force vector
                
                Vector3 force = coll.transform.position - transform.position;
                // normalize force vector to get direction only and trim magnitude
                force.Normalize();
                coll.rigidbody.AddForce(force * ballImpact); 
                DestroyShot();
                break;
            case "Boundary":
                DestroyShot();
                break;
            case "Shot":
                if (collidingObject.GetComponent<Shot>().GetPlayerTeam() != this.playerTeam)
                {
                    DestroyShot();
                } else
                {
                    if (shotID > collidingObject.GetComponent<Shot>().GetShotID())
                    {
                        DestroyShot();
                    }
                }
                break;
            case "Ball":
                // reference: https://answers.unity.com/questions/1100879/push-object-in-opposite-direction-of-collision.html
                // calculate force vector
                force = coll.transform.position - transform.position;
                // normalize force vector to get direction only and trim magnitude
                force.Normalize();
                coll.rigidbody.AddForce(force * ballImpact);
                collidingObject.GetComponent<Ball>().SetLastHitBy(playerTeam);
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

    public void SetPlayerTeam(int i)
    {
         playerTeam = i;
    }

    public int GetPlayerTeam()
    {
        return playerTeam;
    }

    public void SetShotID(int i)
    {
        shotID = i;
    }

    public int GetShotID()
    {
        return shotID;
    }

    public float GetStunDuration()
    {
        return stunDuration;
    }
}
