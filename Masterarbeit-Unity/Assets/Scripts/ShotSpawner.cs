﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawner : MonoBehaviour
{


    public float spawnTimerMedium;    //Zeit des Aufladens, die benötigt wird, bis der mittlere Schuss spawnen soll in Sekunden
    public float spawnTimerLarge;    //Zeit des Aufladens, die benötigt wird, bis der mittlere Schuss spawnen soll in Sekunden
    public float spawnTimerLimit;   //maximale Zeit bis zum Resetten
    public float shotChargeTime;   //aktuelle Zeit des Aufladens 
    public GameObject normalShotPrefab;  //der normale Schuss wird als Prefab in den Inspector gezogen und hier referenziert
    public GameObject mediumShotPrefab;  //gleiches für den mittleren Schuss
    public GameObject largeShotPrefab;  //und den großen
    public bool spawnable = true;  //sofern ein Schuss spawnen darf (nicht kollidiert), ist die Variable true

    bool spawnNormalShot = false;
    bool spawnMediumShot = false;
    bool spawnLargeShot = false;

    public GameObject chargingShotSprite; //Sprite des charging-Shots (Child)
    private Player player;
    private PlayerLogging playerLogging;
    private int playerTeam;
    public int shotCount = 0;

    private Color shotColor;
    //Audios
    private AudioSource audioSource;
    public AudioClip audioShotNormal;
    public AudioClip audioShotMedium;
    public AudioClip audioShotLarge;
    public AudioClip shotCharge;
    private bool normalShotChargingSound;
    private bool mediumShotChargingSound;
    private bool largeShotChargingSound;
    public AudioClip shotAbort;
    private bool shotAborted;



    // Use this for initialization
    void Start()
    {
        chargingShotSprite.transform.localScale = new Vector3(0f, 0f, 0f);  //und die Visualisierung des ChargingShots "unsichtbar" gemacht
        player = transform.parent.GetComponent<Player>();
        playerTeam = player.playerTeam;
        playerLogging = transform.parent.GetComponent<PlayerLogging>();
        //Audio
        audioSource = GetComponent<AudioSource>();
        audioShotNormal = Resources.Load<AudioClip>("Sounds/normal_shot");
        audioShotMedium = Resources.Load<AudioClip>("Sounds/medium_shot");
        audioShotLarge = Resources.Load<AudioClip>("Sounds/large_shot");
        shotCharge = Resources.Load<AudioClip>("Sounds/shot_charge");
        shotAbort = Resources.Load<AudioClip>("Sounds/shot_abort");

    }

    // Update is called once per frame
    void Update()
    {

    }

    //In der Methode wird die Transparenz des Blocks gesetzt. Übergeben wird ein Zeit Argument in fps
    public void SetShotType(GameObject prefab)
    {
    }

    //Die Methode wird beim Festhalten des A-Buttons in jedem Frame aufgerufen und erhöht die shotChargeTime.
    public void AddShotChargeTime(float i)
    {
        shotChargeTime+=i;
        if (shotChargeTime != 0 && shotChargeTime < spawnTimerMedium)
        {
            spawnNormalShot = true;
            spawnMediumShot = false;
            spawnLargeShot = false;
            chargingShotSprite.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            chargingShotSprite.transform.localPosition = new Vector3(-1f, 0f, 0f);

            if (!normalShotChargingSound)
            {
                normalShotChargingSound = true;
                audioSource.loop = true;
                PlaySound(shotCharge, 0.1f);
            }

        }
        //sofern die Zeit noch geringer ist als die zu erreichende SpawnZeit
        else if (shotChargeTime > spawnTimerMedium && shotChargeTime < spawnTimerLarge)
        {
            spawnNormalShot = false;
            spawnMediumShot = true;
            spawnLargeShot = false;
            chargingShotSprite.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            chargingShotSprite.transform.localPosition = new Vector3(-0.6f, 0f, 0f);
            if (!mediumShotChargingSound)
            {
                mediumShotChargingSound = true;
                audioSource.loop = true;
                PlaySound(shotCharge, 0.2f);
            }

        }
        else if (shotChargeTime > spawnTimerLarge && shotChargeTime < spawnTimerLimit)
        {
            spawnNormalShot = false;
            spawnMediumShot = false;
            spawnLargeShot = true;
            chargingShotSprite.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
            chargingShotSprite.transform.localPosition = new Vector3(0f, 0f, 0f);
            if (!largeShotChargingSound)
            {
                largeShotChargingSound = true;
                audioSource.loop = true;
                PlaySound(shotCharge, 0.4f);
            }

        }
        else if (shotChargeTime > spawnTimerLimit)
        {
            if (!shotAborted)
            {
                shotAborted = true;
                spawnNormalShot = false;
                spawnMediumShot = false;
                spawnLargeShot = false;
                chargingShotSprite.transform.localScale = new Vector3(0f, 0f, 0f);
                audioSource.Stop();
                audioSource.loop = false;
                PlaySound(shotAbort, 0.4f);
                normalShotChargingSound = false;
                mediumShotChargingSound = false;
                largeShotChargingSound = false;
            }
        }
    }

    //die Methode wird aufgerufen, sofern der A-Button losgelassen wird 
    public void SpawnShot()
    {
        //wenn das Ziel erreicht wurde und der ShotSpawner nicht kollidiert
        if (spawnNormalShot && spawnable)
        {
            GameObject shot = Instantiate(normalShotPrefab, chargingShotSprite.transform.position, this.transform.rotation);  //wird der Shot aus dem Prefab instanziiert
            spawnNormalShot = false;
            playerLogging.AddShot("normal");
            SetShotProperties(shot);
            PlaySound(audioShotNormal,0.5f);
        }
        else if (spawnMediumShot && spawnable)
        {
            GameObject shot = Instantiate(mediumShotPrefab, chargingShotSprite.transform.position, this.transform.rotation);  //wird der Shot aus dem Prefab instanziiert
            spawnMediumShot = false;
            playerLogging.AddShot("medium");
            SetShotProperties(shot);
            PlaySound(audioShotMedium, 0.5f);
        }
        else if (spawnLargeShot && spawnable)
        {
            GameObject shot = Instantiate(largeShotPrefab, chargingShotSprite.transform.position, this.transform.rotation);  //wird der Shot aus dem Prefab instanziiert
            spawnLargeShot = false;
            playerLogging.AddShot("large");
            SetShotProperties(shot);
            PlaySound(audioShotLarge, 0.4f);
        }
        ResetShotChargeTime();
        normalShotChargingSound = false;
        mediumShotChargingSound = false;
        largeShotChargingSound = false;
        shotAborted = false;

        audioSource.loop = false;
    }

    private void SetShotProperties(GameObject shot){
        shotCount++;
        shot.GetComponent<Shot>().SetDirection(this.transform.rotation);    //Der Schuss bekommt die Rotation des Spielers übergeben
        shot.GetComponent<Shot>().SetColor(shotColor);                      //dessen Farbe
        shot.GetComponent<Shot>().SetPlayerTeam(playerTeam);                //dessen Team
        shot.GetComponent<Shot>().SetShotID(shotCount);                     //sowie seine ID
        shot.name = "Shot_" + shotCount + "_Player_" + playerTeam;       //der Name wird aus dem Count und der PlayerID gebaut.

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

    //Methode, um die Farbe des Schusses zu setzen
    public void SetColor(Color col)
    {
        shotColor = col;    //Die Farbvariable für das Erstellen der neuen Schüsse bekommt die Farbe
    }

    public void ResetShotChargeTime()
    {
        shotChargeTime = 0;    //die Zeit des Aufladens wird zurückgesetzt
        player.SetShotTimer(0);
        chargingShotSprite.transform.localScale = new Vector3(0f, 0f, 0f); //und das Sprite zum aufladen auf 0 gesetzt
    }

}
