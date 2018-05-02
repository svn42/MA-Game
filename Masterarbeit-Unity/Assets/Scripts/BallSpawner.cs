using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    public bool spawnBlocked = false;
    //in dieser Variable wird der zuletzt gespawnte Ball zwischengespeichert. Wird benötigt, um zu überprüfen, ob der neueste Ball den Mittelkreis verlassen hat.
    private GameObject lastSpawnedBall;

    private GameState gameState;
    private List<Ball> ballList;
    public GameObject ballPrefab;
    private Vector3 ballPosition;
    private Quaternion ballRotation;


    // Use this for initialization
    void Start()
    {
        gameState = (GameState)FindObjectOfType(typeof(GameState));
        //die Ballposition der zukünftigen Bälle wird mit dem Mittelpunkt des Spawners gleichgesetzt. 
        ballPosition = this.transform.position - new Vector3(0,0,0.01f);
        ballRotation = new Quaternion(0, 0, 0, 0);
        //Zu Beginn einer Partie wird der erste Ball gespawnt
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnBall()
    {
        //sofern der Mittelkreis nicht mehr vom letzten Ball geblockt wird und das Ball-Maximum noch nicht ereicht wurde
        if (!(gameState.CheckMaximumBalls()) && !spawnBlocked)
        {
            //wird ein neuer Ball gespawnt und als "neuester Ball" markiert
            lastSpawnedBall = Instantiate(ballPrefab, ballPosition, ballRotation);
            //der Mittelkreis wird zudem als Blockiert markiert
            spawnBlocked = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //Wenn der zuletzt gespawnte Ball den Mittelkreis verlässt
        if (other.gameObject == lastSpawnedBall)
        {
            //wird der Spawn wieder freigegeben und ein neuer Ball gespawnt (sofern dies möglich ist --> sofern das Maximum der Bälle nicht erreicht wurde. Siehe SpawnBall Methode)
            spawnBlocked = false; 
            SpawnBall();
        } 

    }



}
