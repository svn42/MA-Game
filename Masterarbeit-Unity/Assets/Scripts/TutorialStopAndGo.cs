using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStopAndGo : MonoBehaviour
{
    public List<GameObject> zones;
    private TutorialGameState tutorialGameState;
    int rating;
    // Use this for initialization
    void Start()
    {
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
        zones[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveZone()
    {
        Destroy(zones[0]);
        zones.RemoveAt(0);
        if (zones.Count != 0)
        {
            zones[0].SetActive(true);
        } else
        {
            Debug.Log("Winner");
            // RatingBerechnen

            tutorialGameState.EndChallenge(rating);
        }
    }
}
