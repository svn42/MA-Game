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
                    timeCenterZone+= Time.deltaTime;
                    break;
                case "Zone_Team1":
                    timeOwnZone += Time.deltaTime;
                    break;
                case "Zone_Team1_Goal":
                    timeOwnGoalZone += Time.deltaTime;
                    break;
                case "Zone_Team2":
                    timeOpponentZone += Time.deltaTime;
                    break;
                case "Zone_Team2_Goal":
                    timeOpponentGoalZone += Time.deltaTime;
                    break;
            }
        } else if (playerTeam == 2)
        {
            switch (zone)
            {
                case "Zone_Center":
                    timeCenterZone += Time.deltaTime;
                    break;
                case "Zone_Team1":
                    timeOpponentZone += Time.deltaTime;
                    break;
                case "Zone_Team1_Goal":
                    timeOpponentGoalZone += Time.deltaTime;
                    break;
                case "Zone_Team2":
                    timeOwnZone += Time.deltaTime;
                    break;
                case "Zone_Team2_Goal":
                    timeOwnGoalZone += Time.deltaTime;
                    break;
            }
        }
    }
}
