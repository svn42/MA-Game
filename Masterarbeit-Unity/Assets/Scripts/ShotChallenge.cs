using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotChallenge : MonoBehaviour {


	public List<GameObject> targets;
	private TutorialGameState tutorialGameState;
	private TutorialLogging tutorialLogging;
	public int maxRatingTime;
	public int maxRatingAccuracy;
	public float bestTime;

	public int ratingTimeInt;
	public int ratingAccuracyInt;
	public float precision;

	// Use this for initialization

	void Start () {
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		tutorialLogging = gameObject.GetComponent<TutorialLogging>();

		SetUpTargets ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUpTargets()
	{
		if (targets.Count > 0)
		{
			targets[0].SetActive(true);
			for (int i = 1; i < targets.Count; i++)
			{
				targets[i].SetActive(false);
			}
		}

	}

	public void RemoveTarget()
	{
		Destroy(targets[0]);
		targets.RemoveAt(0);
		if (targets.Count != 0)
		{
			targets[0].SetActive(true);
		}
		else
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
		precision = ((float)tutorialLogging.shotsHitTarget / (float)tutorialLogging.totalShotsFired);

		ratingAccuracyFloat = precision * maxRatingAccuracy;

		ratingAccuracyInt = Mathf.RoundToInt(ratingAccuracyFloat);

		totalRating = ratingTimeInt + ratingAccuracyInt;

		return totalRating;
	}

}
