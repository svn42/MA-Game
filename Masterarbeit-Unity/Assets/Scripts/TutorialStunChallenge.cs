
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStunChallenge : MonoBehaviour
{
    public List<GameObject> zones;
    private TutorialGameState tutorialGameState;
    private TutorialLogging tutorialLogging;

    public int maxRatingTime;
    public int maxRatingStun;
    public int ratingTimeInt;
    private float ratingStunMultiplier;
    public float ratingStunFloat;
    public int ratingStunInt;

    public float bestTime;
    // Use this for initialization
    void Start()
    {
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
        tutorialLogging = gameObject.GetComponent<TutorialLogging>();
        ratingStunMultiplier = maxRatingStun * 0.01f;
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
            for (int i = 1; i < zones.Count; i++)
            {
                zones[i].SetActive(false);
            }
        }

    }

    public void RemoveZone()
    {
        Destroy(zones[0]);
        zones.RemoveAt(0);
        if (zones.Count != 0)
        {
            zones[0].SetActive(true);
        }
        else
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
        ratingStunFloat = 0;
        float totalStunTime = tutorialLogging.stunnedByEnemyTotalTime;
        //total stun time * 10 --> großer treffer = 10% abzug; mittlerer = 5% abzug, kleiner = 2% abzug
        float ratingStunPenalty = ratingStunMultiplier * (totalStunTime * 10);
        ratingStunFloat = maxRatingStun - ratingStunPenalty;
        if  (ratingStunFloat < 0)
        {
            ratingStunFloat = 0;
        }
        ratingStunInt = Mathf.RoundToInt(ratingStunFloat);

        totalRating = ratingTimeInt + ratingStunInt;
        return totalRating;
    }
}
