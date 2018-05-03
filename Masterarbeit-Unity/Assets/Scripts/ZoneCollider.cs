using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    PlayerLogging playerLogging;
    // Use this for initialization
    void Start()
    {
        playerLogging = transform.parent.GetComponent<PlayerLogging>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "Zone_Center":
                playerLogging.timeCenterZone++;
                break;
            case "Zone_Team1":
                playerLogging.timeOwnZone++;
                break;
            case "Zone_Team1_Goal":
                playerLogging.timeOwnGoalZone++;
                break;
            case "Zone_Team2":
                playerLogging.timeOpponentZone++;
                break;
            case "Zone_Team2_Goal":
                playerLogging.timeOpponentGoalZone++;
                break;
        }
    }
}
