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
	public float destroyTime;
	public int penalty;
	private AudioClip abort; 
	private AudioSource audioSource;

	// Use this for initialization

	void Start () {
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		tutorialLogging = gameObject.GetComponent<TutorialLogging>();
		abort = Resources.Load<AudioClip>("Sounds/placement_blocked");
		audioSource = gameObject.GetComponent<AudioSource> ();
		SetUpTargets ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetUpTargets()
	{
		if (targets.Count > 0)
		{
			tutorialGameState.timeLeft = destroyTime + 0.5f;
			targets[0].SetActive(true);
			Invoke ("RemoveTargetTime", destroyTime);
			for (int i = 1; i < targets.Count; i++)
			{
				targets[i].SetActive(false);
			}
		}

	}

	public void RemoveTarget()
	{
		tutorialGameState.timeLeft = destroyTime + 0.5f;
		Destroy(targets[0]);
		targets.RemoveAt(0);
		CancelInvoke ();
		if (targets.Count != 0)
		{
			targets[0].SetActive(true);
			Invoke ("RemoveTargetTime", destroyTime);
		}
		else
		{
			tutorialGameState.EndChallenge(CalculateRating(), (maxRatingTime+maxRatingAccuracy));
		}
	}

	public void RemoveTargetTime()
	{
		tutorialGameState.timeLeft = destroyTime + 0.5f;
		Destroy(targets[0]);
		targets.RemoveAt(0);
		CancelInvoke ();
		PlaySound (abort, 0.8f);
		if (targets.Count != 0)
		{
			targets[0].SetActive(true);
			Invoke ("RemoveTargetTime", destroyTime);
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
		if (tutorialLogging.totalShotsFired > 0) {
			precision = ((float)tutorialLogging.shotsHitTarget / (float)tutorialLogging.totalShotsFired);
			ratingAccuracyFloat = precision * maxRatingAccuracy;
			ratingAccuracyInt = Mathf.RoundToInt(ratingAccuracyFloat);

		} else {
			precision = 0f;
			ratingAccuracyFloat = 0f;
		}
		ratingAccuracyInt = Mathf.RoundToInt(ratingAccuracyFloat);


		totalRating = ratingTimeInt + ratingAccuracyInt - penalty;

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
