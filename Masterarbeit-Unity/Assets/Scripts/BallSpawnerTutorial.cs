﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerTutorial : MonoBehaviour
{

    public bool centerCircleBlocked = false;    //wenn im Mittelkreis der zuletzt gespawnte Ball liegt, wird true zurückgeliefert
    public bool spawnBlocked = false;
    //in dieser Variable wird der zuletzt gespawnte Ball zwischengespeichert. Wird benötigt, um zu überprüfen, ob der neueste Ball den Mittelkreis verlassen hat.
    private GameObject lastSpawnedBall;
    public GameObject ballInflater;

    private TutorialGameState tutorialGameState;
    private List<Ball> ballList;
    public GameObject ballPrefab;
    private Vector3 ballPosition;
    private Quaternion ballRotation;

    public float ballSpawnDelay;
    public Vector3 ballInflaterMaxSize;
    private AudioSource audioSource;
    public AudioClip soundSpawnBall;
    public AudioClip soundInflateBall;

    public bool spawnBallatBegin;

    // Use this for initialization
    void Start()
    {
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
        ballInflaterMaxSize = ballInflater.transform.localScale;

        //die Ballposition der zukünftigen Bälle wird mit dem Mittelpunkt des Spawners gleichgesetzt. 
        ballPosition = this.transform.position - new Vector3(0, 0, 0.01f);
        ballRotation = new Quaternion(0, 0, 0, 0);
        //Zu Beginn einer Partie wird der erste Ball gespawnt
        if (spawnBallatBegin){
            StartCoroutine(SpawnBall(0, false));    //der erste Ball wird ohne Sound gespawnt
        } else
        {
            ballInflater.transform.localScale = new Vector3(0, 0, 0);

        }
        audioSource = GetComponent<AudioSource>();
        soundSpawnBall = Resources.Load<AudioClip>("Sounds/ball_spawn");
        soundInflateBall = Resources.Load<AudioClip>("Sounds/inflate_ball");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckSpawnBall()
    {
        //sofern der Mittelkreis nicht mehr vom letzten Ball geblockt wird und das Ball-Maximum noch nicht ereicht wurde && nicht von einem Block oder Spieler Blockiert wird
        if (!(tutorialGameState.MaximumBallsReached()) && !centerCircleBlocked)
        {
            if (!spawnBlocked) //und der spawner nicht durch einen anderen Ball belegt wird
            {
                StartCoroutine(SpawnBall(ballSpawnDelay, true)); //wird ein neuer Ball gespawnt
                SetSpawnBlocked(true);      //und der spawner als belegt markiert
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

    public IEnumerator SpawnBall(float time, bool soundOn)
    {
        StartCoroutine(InflateBall(time));
        if (soundOn)
        {
            PlaySound(soundInflateBall, 0.2f);
        }
        yield return new WaitForSeconds(time);
        //wird ein neuer Ball gespawnt und als "neuester Ball" markiert
        lastSpawnedBall = Instantiate(ballPrefab, ballPosition, ballRotation);
        if (soundOn)
        {
            PlaySound(soundSpawnBall, 0.25f);
        }
        //der Mittelkreis wird zudem als vom neuesten Ball blockiert markiert
        centerCircleBlocked = true;
        SetSpawnBlocked(false);
    }

    public void SetSpawnBlocked(bool b)
    {
        spawnBlocked = b;
    }

    public IEnumerator InflateBall(float time)
    {
        float inflateTime = time * 60; //Zeit an Frames anpassen
        for (float i = 0; i < inflateTime; i++)
        {
            ballInflater.transform.localScale = ballInflaterMaxSize * (i / inflateTime);
            yield return new WaitForSeconds(1 / inflateTime);
        }
        ballInflater.transform.localScale = new Vector3(0, 0, 0);

    }

    private void PlaySound(AudioClip ac, float volume)
    {
        float lastTimeScale = Time.timeScale;
        Time.timeScale = 1f;
        audioSource.clip = ac;
        audioSource.volume = volume;
        audioSource.Play();
        Time.timeScale = lastTimeScale;
    }


}
