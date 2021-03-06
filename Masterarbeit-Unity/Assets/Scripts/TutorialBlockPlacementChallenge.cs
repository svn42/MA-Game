﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlockPlacementChallenge : MonoBehaviour {

	public List<GameObject> zones;
	private TutorialGameState tutorialGameState;
	public int maxRating;
	public float bestTime;
	// Use this for initialization
	void Start()
	{
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
	}

	// Update is called once per frame
	void Update()
	{
		SetUpZones();
	}

	public void SetUpZones()
	{
		if (zones.Count > 0)
		{
			zones[0].SetActive(true);
			zones[0].GetComponent<TutorialBlockPlacementZone> ().SetTutorialChallengeScript (this);
			for (int i = 1; i < zones.Count; i++)
			{
				zones[i].SetActive(false);
				zones[i].GetComponent<TutorialBlockPlacementZone> ().SetTutorialChallengeScript (this);
			}
		}

	}

	public void RemoveZone(BlockTutorial block)
	{
		Destroy(zones[0]);
		zones.RemoveAt(0);
		StartCoroutine (DestroyBlock(block));
		if (zones.Count != 0)
		{
			zones[0].SetActive(true);
		} else
		{
			tutorialGameState.EndChallenge(CalculateRating(), maxRating);
		}
	}

	IEnumerator DestroyBlock(BlockTutorial block){
		yield return new WaitForSeconds (1);
		block.ReduceHealth (3);
	}

	public int CalculateRating()
	{
		float rating = 0;
		float timePlayed = tutorialGameState.timePlayed;
		if (timePlayed < bestTime)
		{
			timePlayed = bestTime;
		}

		rating = ( bestTime / timePlayed) * maxRating;
		int ratingInt = Mathf.RoundToInt(rating);
		return ratingInt;
	}
}
