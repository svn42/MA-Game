using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private GameState gameState;
    private BallSpawner ballSpawner;
    private Rigidbody2D rb;
    public GameObject ballExplosion;

    public static int globalId;    //statische ID, um jedem Ball eine eindeutige ID zuzuweisen.
    public int instanceID;     //instanceID, um die ID des Balles zwischenzuspeichern
    public int lastHitBy;   //beinhaltet die Team Nummer des Spielers, der als letztes diesen Ball mit einem Schuss getroffen hat. So kann festgestellt werden, durch wen das Tor erzielt wurde.

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
    void FixedUpdate () {

        rb.velocity = new Vector2 (rb.velocity.x * 0.99f, rb.velocity.y *0.99f);    //Bremst den Ball ab
    }

    // wenn der Ball mit dem GoalCollider in Berührung kommt
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Goal"))
        {
            //wird dem GameState mitgeteilt, welches Tor betroffen ist
            gameState.GoalScored(other.gameObject.name, lastHitBy);
            //und der Ball zerstört
            DestroyBall();
        } 
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))   //bei Berührung mit dem Spieler
        {
            rb.velocity = new Vector2( 0, 0);    //wird die Bewegung des Balles gestoppt
        }
    }

    //Die Methode gibt die instanceID des Balles wider
    public int GetBallID()
    {
        return instanceID;
    }

    //zerstört den Ball, entfernt den Ball aus der Liste und spawnt u.U. einen neuen Ball
    public void DestroyBall()
    {
		DeleteFromBlockSpawnCollider (gameObject.name);
        Instantiate(ballExplosion, transform.position, transform.rotation);  //Die BallExplosion wird dabei instanziiert
        //Der Ball wird aus der Liste der GameState entfernt
        gameState.RemoveBall(instanceID);
        //Ein neuer Ball wird gespawnt, sofern der Spawn nicht geblockt wird
        ballSpawner.CheckSpawnBall();
        //Der Ball wird zerstört
        Destroy(this.gameObject);
    }

    public void SetLastHitBy(int playerTeam)
    {
        lastHitBy = playerTeam;
}

    public void PlayHitSound(float vol)
    {
        gameState.PlaySound("ball_hit", vol);  
    }

	//löscht den Ball aus den Blockspawner Collider Listen
	public void DeleteFromBlockSpawnCollider(string name){
		BlockSpawner[] blockspawner = GameObject.FindObjectsOfType<BlockSpawner>();
		foreach (BlockSpawner bs in blockspawner) {
			bs.RemoveObject (name);
		}
	}


}
