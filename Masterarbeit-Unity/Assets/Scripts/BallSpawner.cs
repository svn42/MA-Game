using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    public bool centerCircleBlocked = false;    //wenn im Mittelkreis der zuletzt gespawnte Ball liegt, wird true zurückgeliefert
    public bool spawnBlocked = false;
    //in dieser Variable wird der zuletzt gespawnte Ball zwischengespeichert. Wird benötigt, um zu überprüfen, ob der neueste Ball den Mittelkreis verlassen hat.
    private GameObject lastSpawnedBall;

    private GameState gameState;
    private List<Ball> ballList;
    public GameObject ballPrefab;
    private Vector3 ballPosition;
    private Quaternion ballRotation;
    public float ballSpawnDelay;


    // Use this for initialization
    void Start()
    {
        gameState = (GameState)FindObjectOfType(typeof(GameState));
        //die Ballposition der zukünftigen Bälle wird mit dem Mittelpunkt des Spawners gleichgesetzt. 
        ballPosition = this.transform.position - new Vector3(0,0,0.01f);
        ballRotation = new Quaternion(0, 0, 0, 0);
        //Zu Beginn einer Partie wird der erste Ball gespawnt
        StartCoroutine(SpawnBall(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSpawnBall()
    {
        //sofern der Mittelkreis nicht mehr vom letzten Ball geblockt wird und das Ball-Maximum noch nicht ereicht wurde && nicht von einem Block oder Spieler Blockiert wird
        if (!(gameState.MaximumBallsReached()) && !centerCircleBlocked)
        {
            if (!spawnBlocked)
            {
                StartCoroutine(SpawnBall(ballSpawnDelay));
            } 
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //Wenn der zuletzt gespawnte Ball den Mittelkreis verlässt
        if (other.gameObject == lastSpawnedBall)
        {
            //wird der Spawn wieder freigegeben und ein neuer Ball gespawnt (sofern dies möglich ist --> sofern das Maximum der Bälle nicht erreicht wurde. Siehe CheckSpawnBall Methode)
            centerCircleBlocked = false; 
            CheckSpawnBall();
        } 

    }

    public IEnumerator SpawnBall(float time)
    {
        yield return new WaitForSeconds(time);
        //wird ein neuer Ball gespawnt und als "neuester Ball" markiert
        lastSpawnedBall = Instantiate(ballPrefab, ballPosition, ballRotation);
        //der Mittelkreis wird zudem als vom neuesten Ball blockiert markiert
        centerCircleBlocked = true;
        SetSpawnBlocked(true);
    }

    public void SetSpawnBlocked(bool b)
    {
        spawnBlocked = b;
    }

}
