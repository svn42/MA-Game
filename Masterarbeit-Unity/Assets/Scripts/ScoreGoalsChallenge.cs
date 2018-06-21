using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGoalsChallenge : MonoBehaviour {

	public List<GameObject> firstObjects;
	public List<GameObject> secondObjects;
	public List<GameObject> thirdObjects;
	public GameObject goal;
	private SpriteRenderer goalRenderer;

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
	public Color32 blue;
	public Color32 red;
	public float destroyTime;
	public int destroyPenalty;
	public int totalPenalty;

	private AudioClip abort; 
	private AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		goalRenderer = goal.GetComponent<SpriteRenderer> ();
		playerPosition = gameObject.transform.position;
		playerRotation = gameObject.transform.rotation;

		blue = new Color32 (87, 73,255, 255);
		red = new Color32 (255,100,100, 255);
		abort = Resources.Load<AudioClip>("Sounds/placement_blocked");
		audioSource = gameObject.GetComponent<AudioSource> ();

		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		tutorialLogging = gameObject.GetComponent<TutorialLogging>();
		player = gameObject.GetComponent<PlayerTutorial>();
		SetUpObjects();
		SetGoalColor ();

	}

	// Update is called once per frame
	void Update()
	{
	}


	private void SetGoalColor(){
		switch (player.playerTeam) {
		case 1: 
			goalRenderer.color = blue;
			break;
		case 2: 
			goalRenderer.color = red;
			break;
		}

	}

	public void SetUpObjects()
	{
		objects.Add (firstObjects);
		objects.Add (secondObjects);
		objects.Add (thirdObjects);
		tutorialGameState.timeLeft = destroyTime + 0.1f;
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
				Invoke ("RemoveTargetTime", destroyTime);
			}
		}
	}

	public void RemoveObjects()
	{
		tutorialGameState.timeLeft = destroyTime + 0.1f;
		tutorialGameState.timerBlink = false;
		this.gameObject.transform.position = playerPosition;
		this.gameObject.transform.rotation = playerRotation;
		CancelInvoke ();


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
				Invoke ("RemoveTargetTime", destroyTime);
			}
		} else
		{
			tutorialGameState.EndChallenge(CalculateRating(), (maxRatingTime+maxRatingAccuracy));
		}

	}

	public void RemoveTargetTime()
	{
		tutorialGameState.timeLeft = destroyTime + 0.1f;
		tutorialGameState.timerBlink = false;
		this.gameObject.transform.position = playerPosition;
		this.gameObject.transform.rotation = playerRotation;
		CancelInvoke ();
		totalPenalty += destroyPenalty;
		PlaySound (abort, 0.8f);
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
				Invoke ("RemoveTargetTime", destroyTime);
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
		if (tutorialLogging.totalShotsFired > 0) {
			precision = (((float)tutorialLogging.shotsHitBall + (float)tutorialLogging.shotsHitBlock) / (float)tutorialLogging.totalShotsFired);
		} else {
			precision = 0;
		}

		ratingAccuracyFloat = precision * maxRatingAccuracy;

		ratingAccuracyInt = Mathf.RoundToInt(ratingAccuracyFloat);

		//stun penalty
		float totalStunTime = tutorialLogging.stunnedByEnemyTotalTime;
		//total stun time * 5 --> großer treffer = 10 abzug; mittlerer = 5 abzug, kleiner = 2 abzug
		stunPenalty = (totalStunTime * 10);
		int stunPenaltyInt = Mathf.RoundToInt(stunPenalty);
		totalRating = ratingTimeInt + ratingAccuracyInt - stunPenaltyInt - totalPenalty;

		return totalRating;
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