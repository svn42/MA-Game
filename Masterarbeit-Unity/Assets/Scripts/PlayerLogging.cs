using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogging : MonoBehaviour
{
    public int playerTeam;
    public float timeCenterZone;
    public float timeOwnZone;
    public float timeOwnGoalZone;
    public float timeOpponentZone;
    public float timeOpponentGoalZone;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerTeam(int team)
    {
        playerTeam = team;
    }

    public void AddZoneTime(string zone)
    {
        if (playerTeam == 1)
        {
            switch (zone)
            {
                case "Zone_Center":
                    timeCenterZone++;
                    break;
                case "Zone_Team1":
                    timeOwnZone++;
                    break;
                case "Zone_Team1_Goal":
                    timeOwnGoalZone++;
                    break;
                case "Zone_Team2":
                    timeOpponentZone++;
                    break;
                case "Zone_Team2_Goal":
                    timeOpponentGoalZone++;
                    break;
            }
        } else if (playerTeam == 2)
        {
            switch (zone)
            {
                case "Zone_Center":
                    timeCenterZone++;
                    break;
                case "Zone_Team1":
                    timeOpponentZone++;
                    break;
                case "Zone_Team1_Goal":
                    timeOpponentGoalZone++;
                    break;
                case "Zone_Team2":
                    timeOwnZone++;
                    break;
                case "Zone_Team2_Goal":
                    timeOwnGoalZone++;
                    break;
            }
        }
    }
}
