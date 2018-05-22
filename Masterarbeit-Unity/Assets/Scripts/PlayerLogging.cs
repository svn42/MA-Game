using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogging : MonoBehaviour
{
    public int playerTeam;
    public string currentZone;  //aktuelle Zone des Spielers

    //time per Zone
    public float timeCenterZone;
    public float timeOwnZone;
    public float timeOwnGoalZone;
    public float timeOpponentZone;
    public float timeOpponentGoalZone;

    //Shots
    public int normalShotsFired;
    public int mediumShotsFired;
    public int largeShotsFired;
    public int totalShotsFired;    //am Ende alle types addieren

    //Precision
    //aufteilen nach Schusstyp?
    // public int shotsHitBlock;   //
    //public int shotsHitBall;   //
    //public int shotsHitPlayer;   //
    //public int shotsHitEnemyShot;   //

    //Blocks
    public int blocksInOwnZone;
    public int blocksInOwnGoalZone;
    public int blocksInCenterZone;
    public int blocksInOpponentZone;
    public int blocksInOpponentGoalZone;
    public int totalBlocksPlaced;       //am Ende alle types addieren

    public int goalsScored;
    public int ownGoalsScored;
    //public float distanceTravelled;   //

    //Stun
    //public int totalEnemyStunned;   //
    //public int normalEnemyStunned;   //
    //public int mediumEnemyStunned;   //
    //public int largeEnemyStunned;   //
    //public float EnemyStunnedTotalTime;   //

    //Stunned by ball
    //public int stunnedByBall;   //


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
                    timeCenterZone += Time.deltaTime;
                    currentZone = "Center Zone";
                    break;
                case "Zone_Team1":
                    timeOwnZone += Time.deltaTime;
                    currentZone = "Own Zone";
                    break;
                case "Zone_Team1_Goal":
                    timeOwnGoalZone += Time.deltaTime;
                    currentZone = "Own Goal Zone";
                    break;
                case "Zone_Team2":
                    timeOpponentZone += Time.deltaTime;
                    currentZone = "Opponent Zone";
                    break;
                case "Zone_Team2_Goal":
                    timeOpponentGoalZone += Time.deltaTime;
                    currentZone = "Opponent Goal Zone";
                    break;
            }
        }
        else if (playerTeam == 2)
        {
            switch (zone)
            {
                case "Zone_Center":
                    timeCenterZone += Time.deltaTime;
                    currentZone = "Center Zone";
                    break;
                case "Zone_Team1":
                    timeOpponentZone += Time.deltaTime;
                    currentZone = "Opponent Zone";
                    break;
                case "Zone_Team1_Goal":
                    timeOpponentGoalZone += Time.deltaTime;
                    currentZone = "Opponent Goal Zone";
                    break;
                case "Zone_Team2":
                    timeOwnZone += Time.deltaTime;
                    currentZone = "Own Zone";
                    break;
                case "Zone_Team2_Goal":
                    timeOwnGoalZone += Time.deltaTime;
                    currentZone = "Own Goal Zone";
                    break;
            }
        }
    }

    public void AddShot(string shotType)
    {
        switch (shotType)
        {
            case "normal":
                normalShotsFired++;
                break;
            case "medium":
                mediumShotsFired++;
                break;
            case "large":
                largeShotsFired++;
                break;
        }
    }

    public void AddBlock()
    {
        switch (currentZone)
        {
            case "Center Zone":
                blocksInCenterZone++;
                break;
            case "Own Zone":
                blocksInOwnZone++;
                break;
            case "Own Goal Zone":
                blocksInOwnGoalZone++;
                break;
            case "Opponent Zone":
                blocksInOpponentZone++;
                break;
            case "Opponent Goal Zone":
                blocksInOpponentGoalZone++;
                break;
        }
    }

    public void AddGoal(string goalType)
    {
        switch (goalType)
        {
            case "owngoal":
                ownGoalsScored++;
                break;
            case "goal":
                goalsScored++;
                break;
        }

    }
}
