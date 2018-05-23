using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogging : MonoBehaviour
{
    private int playerTeam;
    private string currentZone;  //aktuelle Zone des Spielers

    public float distanceTravelled;

    //time per Zone
    public float timeCenterZone;
    public float timeOwnZone;
    public float timeOwnGoalZone;
    public float timeOpponentZone;
    public float timeOpponentGoalZone;

    //Shots
    public int totalShotsFired; 
    public int normalShotsFired;
    public int mediumShotsFired;
    public int largeShotsFired;

    //Accuracy
    public int shotsHitBlock;   
    public int shotsHitBall;   
    public int shotsHitPlayer;   
    public int shotsHitEnemyShot;   
    public int shotsDestroyed;
    public float shotsHitBlockPercent;
    public float shotsHitBallPercent;
    public float shotsHitPlayerPercent;
    public float shotsHitEnemyShotPercent;
    public float shotsDestroyedPercent;

    //Blocks
    public int totalBlocksPlaced;
    public int blocksInOwnZone;
    public int blocksInOwnGoalZone;
    public int blocksInCenterZone;
    public int blocksInOpponentZone;
    public int blocksInOpponentGoalZone;

    //goals Scored
    public int goalsScored;
    public int ownGoalsScored;

    //Stun
    public int totalEnemyStunned;   
    public int normalEnemyStunned;   
    public int mediumEnemyStunned;   
    public int largeEnemyStunned;   
    public float enemyStunnedTotalTime;   

    //Stunned by ball
    public int stunnedByBall;

    //Emotes
    public int totalEmotes;
    public int emoteNice;
    public int emoteAngry;
    public int emoteCry;
    public int emoteHaha;
    
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
        totalShotsFired++;
    }

    public void AddAccuracy(string action)
    {
        switch (action)
        {
            case "ball":
                shotsHitBall++;
                break;
            case "player":
                shotsHitPlayer++;
                break;
            case "block":
                shotsHitBlock++;
                break;
            case "shot":
                shotsHitEnemyShot++;
                break;
            case "destroy":
                shotsDestroyed++;
                break;
        }
    }

    public void CalculateAccuracy()
    {
        if (totalShotsFired > 0)
        {
            shotsHitBlockPercent = (float)shotsHitBlock / totalShotsFired;
            shotsDestroyedPercent = (float)shotsDestroyed / totalShotsFired;
            shotsHitBallPercent = (float)shotsHitBall / totalShotsFired;
            shotsHitEnemyShotPercent = (float)shotsHitEnemyShot / totalShotsFired;
            shotsHitPlayerPercent = (float)shotsHitPlayer / totalShotsFired;
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
        totalBlocksPlaced++;
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

    public void AddWalkedDistance(float walkedDistance)
    {
        distanceTravelled = walkedDistance;
    }

    public void AddStunnedByBall()
    {
        stunnedByBall++;
    }

    public void AddStunnedByEnemy(string shotType, float stunDuration)
    {
        switch (shotType)
        {
            case "normal":
                normalEnemyStunned++;
                break;
            case "medium":
                mediumEnemyStunned++;
                break;
            case "large":
                largeEnemyStunned++;
                break;
        }
        enemyStunnedTotalTime += stunDuration;
        totalEnemyStunned++;
    }
    
    public void AddEmote(string type)
    {
        switch (type)
        {
            case "nice":
                emoteNice++;
                break;
            case "angry":
                emoteAngry++;
                break;
            case "cry":
                emoteCry++;
                break;
            case "haha":
                emoteHaha++;
                break;
        }
        totalEmotes++;
    }

}
