using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlockDestroyChallenge : MonoBehaviour {

	public List<GameObject> blockList;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RemoveBlock(GameObject go)
	{
		if (blockList.Contains (go)) {
			blockList.Remove (go);
		}
		if (blockList.Count == 0)
		{
			tutorialGameState.EndChallenge(CalculateRating());
		} 
	}

	public int CalculateRating()
	{
		int totalRating;
		// rating Time
		float ratingTimeFloat = 0;
		float timePlayed = tutorialGameState.timePlayed;
		if (timePlayed < bestTime)
		{
			timePlayed = bestTime;
		}
		ratingTimeFloat = (bestTime / timePlayed) * maxRatingTime;
		ratingTimeInt = Mathf.RoundToInt(ratingTimeFloat);

		//rating stun
		float ratingAccuracyFloat= 0;
		precision = ((float)tutorialLogging.shotsHitBlock / (float)tutorialLogging.totalShotsFired);
		//total stun time * 10 --> großer treffer = 10% abzug; mittlerer = 5% abzug, kleiner = 2% abzug
		ratingAccuracyFloat = precision * maxRatingAccuracy;

		ratingAccuracyInt = Mathf.RoundToInt(ratingAccuracyFloat);

		totalRating = ratingTimeInt + ratingAccuracyInt;
		return totalRating;
	}
}
