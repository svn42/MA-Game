using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideCircle : MonoBehaviour {

    bool spawnBlocked;
    private List<GameObject> collidingObjects; //Liste der GameObjects, die mit dem Spawner kollidieren
    public BallSpawner bs; //Der BallSpawner dem Objekt verknüpft


    // Use this for initialization
    void Start () {
        collidingObjects = new List<GameObject>(); 
        bs = transform.parent.GetComponent<BallSpawner>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        //sofern das andere Objekt ein Block oder ein Spieler ist
        if (other.gameObject.tag.Equals("Block") || other.gameObject.tag.Equals("Player"))
        {
            collidingObjects.Add(other.gameObject); //wird es in die Liste der kollidierenden Objekte aufgenommen

            spawnBlocked = true;                  //wird der Spawner als blockiert angesehen
            bs.SetSpawnBlocked(spawnBlocked);       //und dies dem BlockSpawner mitgeteilt
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //sofern das andere Objekt ein Block oder ein Spieler ist
        if (other.gameObject.tag.Equals("Block") || other.gameObject.tag.Equals("Player")) 
        {
            collidingObjects.Remove(other.gameObject);  //wird es aus der Liste entfernt
        }
        //wenn kein Objekt mit dem Spawner kollidiert
        if (collidingObjects.Count == 0)
        {
            spawnBlocked = false;   //wird der Spawner als frei angesehen
            bs.SetSpawnBlocked(spawnBlocked); //und dies dem BlockSpawner mitgeteilt
            bs.SpawnBall(); //zudem wird versucht, einen Ball zu spawnen. (Trifft ein, wenn ein Ball spawnen könnte, der Spawner allerdings durch einen Spieler blockiert wurde und daraufhin wieder frei wurde)
        }
    }

}
