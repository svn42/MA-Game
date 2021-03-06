﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCannon : MonoBehaviour
{

	public int cannonTeam = 55;
	public Color teamColor;    //Die Farbe des Spielers, die anhand der Teamzugehörigkeit ermittelt wird
	private AudioSource audioSource;
	public float shotIntervall; //der shotDelay gibt das Schussintervall wieder 
	public float startShooting; //der wann soll das schießen beginnen? 
	public GameObject shotPrefab;
	public GameObject spawnPosition;
	public AudioClip soundShotNormal;
	public AudioClip soundShotMedium;
	public AudioClip soundShotLarge;

	// Use this for initialization
	void Start()
	{
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		//spriteRenderer.enabled = false;  
		teamColor = spriteRenderer.color;

		//Audio
		audioSource = GetComponent<AudioSource>();
		soundShotNormal = Resources.Load<AudioClip>("Sounds/normal_shot");
		soundShotMedium = Resources.Load<AudioClip>("Sounds/medium_shot");
		soundShotLarge = Resources.Load<AudioClip>("Sounds/large_shot");
		InvokeRepeating("SpawnShot", startShooting, shotIntervall);

	}

	// Update is called once per frame
	void Update()
	{

	}


	public void SpawnShot()
	{
		GameObject shot = Instantiate(shotPrefab, spawnPosition.transform.position, transform.rotation);  //wird der Shot aus dem Prefab instanziiert
		SetShotProperties(shot);

		switch (shotPrefab.name){
		case "ShotNormalTutorial":
			PlaySound(soundShotNormal, 0.1f);
			break;
		case "ShotMediumTutorial":
			PlaySound(soundShotMedium, 0.2f);
			break;
		case "ShotLargeTutorial":
			PlaySound(soundShotLarge, 0.3f);
			break;
		}
	}

	private void SetShotProperties(GameObject shot)
	{
		shot.GetComponent<ShotTutorial>().SetDirection(this.transform.rotation);    //Der Schuss bekommt die Rotation des Spielers übergeben
		shot.GetComponent<ShotTutorial>().SetColor(teamColor);                      //dessen Farbe
		shot.GetComponent<ShotTutorial>().SetPlayerTeam(cannonTeam);                //dessen Team
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
