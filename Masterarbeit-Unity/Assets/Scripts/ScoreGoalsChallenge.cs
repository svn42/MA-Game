using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGoalsChallenge : MonoBehaviour {

	public List<GameObject> firstObjects;
	public List<GameObject> secondObjects;
	public List<GameObject> thirdObjects;
	public GameObject goalRed;
	public GameObject goalBlue;

	private List<List<GameObject> > objects = new List<List<GameObject> >(); 
	private TutorialGameState tutorialGameState;
	private TutorialLogging tutorialLogging;
	private PlayerTutorial player;

	//player data
	private Vector3 playerPosition;
	private Quaternion playerRotation;

	public int maxRatingTime;
	public int maxRatingAccuracy;
	public float bestTime;

	public int ratingTimeInt;
	public int ratingAccuracyInt;
	public float precision;
	public float stunPenalty;


	// Use this for initialization
	void Start()
	{
		playerPosition = gameObject.transform.position;
		playerRotation = gameObject.transform.rotation;

		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		tutorialLogging = gameObject.GetComponent<TutorialLogging>();
		player = gameObject.GetComponent<PlayerTutorial>();
		SetGoalColor ();
		SetUpObjects();

	}

	// Update is called once per frame
	void Update()
	{
	}

	private void SetGoalColor(){
		switch (player.playerTeam) {
		case 1: 
			goalBlue.SetActive (true);
			break;
		case 2: 
			goalRed.SetActive (true);
			break;
		}

	}

	public void SetUpObjects()
	{
		objects.Add (firstObjects);
		objects.Add (secondObjects);
		objects.Add (thirdObjects);

		if (objects.Count > 0)
		{
			for (int i = 1; i < objects.Count; i++)
			{
				foreach (GameObject go in objects[i]) {
					go.SetActive(false);
				}

			}
			foreach (GameObject go in objects[0]) {
				go.SetActive (true);
			}
		}
	}

	public void RemoveObjects()
	{
		this.gameObject.transform.position = playerPosition;
		this.gameObject.transform.rotation = playerRotation;


		foreach (GameObject go in objects[0]) {
			if (go != null) {
				Destroy (go);
			}
		}
		objects.RemoveAt(0);
		if (objects.Count != 0)
		{
			foreach (GameObject go in objects[0]) {
				go.SetActive (true);
			}
		} else
		{
			tutorialGameState.EndChallenge(CalculateRating(), (maxRatingTime+maxRatingAccuracy));
		}

	}

	public int CalculateRating()
	{
		int totalRating = 0;

		// rating Time
		float ratingTimeFloat = 0;
		float timePlayed = tutorialGameState.timePlayed;
		if (timePlayed < bestTime)
		{
			timePlayed = bestTime;
		}
		ratingTimeFloat = (bestTime / timePlayed) * maxRatingTime;
		ratingTimeInt = Mathf.RoundToInt(ratingTimeFloat);

		//rating Accuracy
		float ratingAccuracyFloat= 0;
		precision = (((float)tutorialLogging.shotsHitBall + (float)tutorialLogging.shotsHitBlock) / (float)tutorialLogging.totalShotsFired);

		ratingAccuracyFloat = precision * maxRatingAccuracy;

		ratingAccuracyInt = Mathf.RoundToInt(ratingAccuracyFloat);

		//stun penalty
		float totalStunTime = tutorialLogging.stunnedByEnemyTotalTime;
		//total stun time * 5 --> großer treffer = 10 abzug; mittlerer = 5 abzug, kleiner = 2 abzug
		stunPenalty = (totalStunTime * 10);
		int stunPenaltyInt = Mathf.RoundToInt(stunPenalty);
		totalRating = ratingTimeInt + ratingAccuracyInt - stunPenaltyInt;

		return totalRating;
	}
}