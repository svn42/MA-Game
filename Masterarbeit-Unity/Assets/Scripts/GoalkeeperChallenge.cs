﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperChallenge : MonoBehaviour {

	public List<GameObject> balls;
	public GameObject goal;

	private SpriteRenderer goalRenderer;


	private TutorialGameState tutorialGameState;
	private PlayerTutorial player;
	public int startBallCount;
	public int goalsConceded;
	public int maxRating;
	public float goalPenalty;
	public Color32 blue;
	public Color32 red;

	// Use this for initialization
	void Start()
	{
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		player = gameObject.GetComponent<PlayerTutorial>();

		goalRenderer = goal.GetComponent<SpriteRenderer> ();

		blue = new Color32 (87, 73,255, 255);
		red = new Color32 (255, 100, 100, 255);
			
		SetGoalColor();
		SetUpObjects();
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void SetGoalColor(){
		switch (player.playerTeam) {
		case 1: 
			goalRenderer.color = red;
			break;
		case 2: 
			goalRenderer.color = blue;
			break;
		}

	}

	public void SetUpObjects()
	{
		Invoke("RemoveObjects", 9);

		if (balls.Count > 0)
		{
			balls[0].SetActive(true);
			for (int i = 1; i < balls.Count; i++)
				{
				balls[i].SetActive(false);
				}
			}
		startBallCount = balls.Count;

	}

	public void RemoveObjects()
	{

		if (balls [0] != null) {
			Destroy (balls [0]);
		}
		balls.RemoveAt(0);
		if (balls.Count != 0)
		{
			balls[0].SetActive(true);
			Invoke("RemoveObjects", 9); //nach 9 Sekunden, wird der Ball entfernt. 

		} else
		{
			tutorialGameState.EndChallenge(CalculateRating(), maxRating);
		}

	}

	public void AddGoal(){
		goalsConceded++;
	}

	public int CalculateRating()
	{
		int totalRating = 0;
		goalPenalty = (float)maxRating * ((float)goalsConceded / (float)startBallCount);
		totalRating = maxRating - Mathf.RoundToInt(goalPenalty);

		return totalRating;
	}
}
