using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private GameState gameState;
    private BallSpawner ballSpawner;
    private Rigidbody2D rb;

    //statische ID, um jedem Ball eine eindeutige ID zuzuweisen.
    public static int globalId;
    //instanceID, um die ID des Balles zwischenzuspeichern
    public int instanceID;

    // Use this for initialization
    void Start () {
        //jeder Ball bekommt beim erstellen die nächsthöhere ID
        globalId++;
        instanceID = globalId;
        //und wird dementsprechend umbenannt
        this.name = "Ball " + globalId;
        //Der GameState werden dem Ball bekannt gemacht
        gameState = (GameState)FindObjectOfType(typeof(GameState));
        //und der Ball registriert sich bei desen Ballliste
        gameState.RegisterBallList(this);
        //Der Ballspawner wird dem Ball bekannt gemacht. Da es nur einen BallSpawner gibt, kann dies über die folgende Zeile geschehen
        ballSpawner = (BallSpawner)FindObjectOfType(typeof(BallSpawner));

        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
        Debug.Log(rb.velocity);

    }

    // wenn der Ball mit dem GoalCollider in Berührung kommt
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Goal"))
        {
            //wird dem GameState mitgeteilt, welches Tor betroffen ist
            gameState.GoalScored(other.gameObject.name);
            //und der Ball zerstört
            DestroyBall();
        }
    }

    //Die Methode gibt die instanceID des Balles wider
    public int GetBallID()
    {
        return this.instanceID;
    }

    //zerstört den Ball, entfernt den Ball aus der Liste und spawnt u.U. einen neuen Ball
    public void DestroyBall()
    {
        //Der Ball wird aus der Liste der GameState entfernt
        gameState.RemoveBall(instanceID);
        //Ein neuer Ball wird gespawnt, sofern der Spawn nicht geblockt wird
        ballSpawner.SpawnBall();
        //Der Ball wird zerstört
        Destroy(this.gameObject);
    }
}
