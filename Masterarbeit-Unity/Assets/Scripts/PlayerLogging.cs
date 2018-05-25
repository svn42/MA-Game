using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogging : MonoBehaviour
{
    public int playerTeam;
    private string currentZone;  //aktuelle Zone des Spielers
    public string currentResult;
    public float distanceTravelled;

    //result
    public string finalResult;
    public int goalsScored;
    public int goalsConceded;
    //goals Scored
    public int correctGoalsScored;
    public int ownGoalsScored;

    //time per result
    public float timeInLead;
    public float timeTied;
    public float timeInDeficit;
    public float timeInLeadPercent;
    public float timeTiedPercent;
    public float timeInDeficitPercent;

    //alle variablen je nach result. 
    //in schleife in export?

    //time per Zone
    public float timeCenterZone;
    public float timeOwnZone;
    public float timeOwnGoalZone;
    public float timeOpponentZone;
    public float timeOpponentGoalZone;
    public float timeCenterZonePercent;
    public float timeOwnZonePercent;
    public float timeOwnGoalZonePercent;
    public float timeOpponentZonePercent;
    public float timeOpponentGoalZonePercent;

    //time per zone per result
    public float timeCenterZoneInLead;
    public float timeCenterZoneInTie;
    public float timeCenterZoneInDeficit;

    public float timeOwnZoneInLead;
    public float timeOwnZoneInTie;
    public float timeOwnZoneInDeficit;

    public float timeOwnGoalZoneInLead;
    public float timeOwnGoalZoneInTie;
    public float timeOwnGoalZoneInDeficit;

    public float timeOpponentZoneInLead;
    public float timeOpponentZoneInTie;
    public float timeOpponentZoneInDeficit;

    public float timeOpponentGoalZoneInLead;
    public float timeOpponentGoalZoneInTie;
    public float timeOpponentGoalZoneInDeficit;
    //time per zone per result Percent

    public float timeCenterZoneInLeadPercent;
    public float timeOwnZoneInLeadPercent;
    public float timeOwnGoalZoneInLeadPercent;
    public float timeOpponentZoneInLeadPercent;
    public float timeOpponentGoalZoneInLeadPercent;

    public float timeCenterZoneInTiePercent;
    public float timeOwnZoneInTiePercent;
    public float timeOwnGoalZoneInTiePercent;
    public float timeOpponentZoneInTiePercent;
    public float timeOpponentGoalZoneInTiePercent;

    public float timeCenterZoneInDeficitPercent;
    public float timeOwnZoneInDeficitPercent;
    public float timeOwnGoalZoneInDeficitPercent;
    public float timeOpponentZoneInDeficitPercent;
    public float timeOpponentGoalZoneInDeficitPercent;

    //Shots
    public int totalShotsFired;
    public int normalShotsFired;
    public int mediumShotsFired;
    public int largeShotsFired;
    public float normalShotsFiredPercent;
    public float mediumShotsFiredPercent;
    public float largeShotsFiredPercent;

    //Accuracy
    public int totalObjectsHit;
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
    public float blocksInOwnZonePercent;
    public float blocksInOwnGoalZonePercent;
    public float blocksInCenterZonePercent;
    public float blocksInOpponentZonePercent;
    public float blocksInOpponentGoalZonePercent;

    //Stun
    //enemy stunned
    public int totalEnemyStunned;
    public int normalEnemyStunned;
    public int mediumEnemyStunned;
    public int largeEnemyStunned;
    public float enemyStunnedTotalTime;
    //stunned by enemy
    public int totalStunnedByEnemy;
    public int normalStunnedByEnemy;
    public int mediumStunnedByEnemy;
    public int largeStunnedByEnemy;
    public float stunnedByEnemyTotalTime;


    //Stunned by ball
    public int stunnedByBall;

    //Emotes
    public int totalEmotes;
    public int emoteNice;
    public int emoteAngry;
    public int emoteCry;
    public int emoteHaha;
    public float emoteNicePercent;
    public float emoteAngryPercent;
    public float emoteCryPercent;
    public float emoteHahaPercent;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckResult();
        AddResultTime(currentResult);
    }

    public void SetPlayerTeam(int team)
    {
        playerTeam = team;
    }

    private string CheckResult()
    {
        if (goalsScored > goalsConceded)
        {
            currentResult = "in_lead";
            finalResult = "Sieg";
        }
        else if (goalsScored == goalsConceded)
        {
            currentResult = "in_tie";
            finalResult = "Remis";
        }
        else if (goalsScored < goalsConceded)
        {
            currentResult = "in_deficit";
            finalResult = "Niederlage";
        }
        return currentResult;
    }


    private void AddResultTime(string result)
    {
        switch (result)
        {
            case "in_lead":
                timeInLead += Time.deltaTime;
                break;
            case "in_tie":
                timeTied += Time.deltaTime;
                break;
            case "in_deficit":
                timeInDeficit += Time.deltaTime;
                break;
        }
    }

    public void CalculateResultTimePercentage()
    {

        float totalTimeResult = timeInLead + timeTied + timeInDeficit;
        if (totalTimeResult > 0)
        {
            timeInLeadPercent = timeInLead / totalTimeResult;
            timeTiedPercent = timeTied / totalTimeResult;
            timeInDeficitPercent = timeInDeficit / totalTimeResult;
        }
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
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeCenterZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeCenterZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeCenterZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team1":
                    timeOwnZone += Time.deltaTime;
                    currentZone = "Own Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOwnZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOwnZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOwnZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team1_Goal":
                    timeOwnGoalZone += Time.deltaTime;
                    currentZone = "Own Goal Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOwnGoalZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOwnGoalZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOwnGoalZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team2":
                    timeOpponentZone += Time.deltaTime;
                    currentZone = "Opponent Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOpponentZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOpponentZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOpponentZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;

                case "Zone_Team2_Goal":
                    timeOpponentGoalZone += Time.deltaTime;
                    currentZone = "Opponent Goal Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOpponentGoalZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOpponentGoalZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOpponentGoalZoneInDeficit += Time.deltaTime;
                            break;
                    }
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
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeCenterZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeCenterZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeCenterZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team1":
                    timeOpponentZone += Time.deltaTime;
                    currentZone = "Opponent Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOpponentZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOpponentZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOpponentZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team1_Goal":
                    timeOpponentGoalZone += Time.deltaTime;
                    currentZone = "Opponent Goal Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOpponentGoalZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOpponentGoalZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOpponentGoalZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team2":
                    timeOwnZone += Time.deltaTime;
                    currentZone = "Own Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOwnZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOwnZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOwnZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
                case "Zone_Team2_Goal":
                    timeOwnGoalZone += Time.deltaTime;
                    currentZone = "Own Goal Zone";
                    switch (currentResult)
                    {
                        case "in_lead":
                            timeOwnGoalZoneInLead += Time.deltaTime;
                            break;
                        case "in_tie":
                            timeOwnGoalZoneInTie += Time.deltaTime;
                            break;
                        case "in_deficit":
                            timeOwnGoalZoneInDeficit += Time.deltaTime;
                            break;
                    }
                    break;
            }
        }
    }

    public void CalculateZonePercentage()
    {

        float totalZoneTime = timeCenterZone + timeOwnZone + timeOwnGoalZone + timeOpponentZone + timeOpponentGoalZone;
        if (totalZoneTime > 0)
        {
            timeCenterZonePercent = timeCenterZone / totalZoneTime;
            timeOwnZonePercent = timeOwnZone / totalZoneTime;
            timeOwnGoalZonePercent = timeOwnGoalZone / totalZoneTime;
            timeOpponentZonePercent = timeOpponentZone / totalZoneTime;
            timeOpponentGoalZonePercent = timeOpponentGoalZone / totalZoneTime;
        }
    }

    public void CalculateResultZonePercentage()
    {

        timeCenterZoneInLeadPercent = timeCenterZoneInLead / timeInLead;
        timeOwnZoneInLeadPercent = timeOwnZoneInLead / timeInLead; ;
        timeOwnGoalZoneInLeadPercent = timeOwnGoalZoneInLead / timeInLead; ;
        timeOpponentZoneInLeadPercent = timeOpponentZoneInLead / timeInLead; ;
        timeOpponentGoalZoneInLeadPercent = timeOpponentGoalZoneInLead / timeInLead; ;

        timeCenterZoneInTiePercent = timeCenterZoneInTie / timeTied; ;
        timeOwnZoneInTiePercent = timeOwnZoneInTie / timeTied; ;
        timeOwnGoalZoneInTiePercent = timeOwnGoalZoneInTie / timeTied; ;
        timeOpponentZoneInTiePercent = timeOpponentZoneInTie / timeTied; ;
        timeOpponentGoalZoneInTiePercent = timeOpponentGoalZoneInTie / timeTied; ;

        timeCenterZoneInDeficitPercent = timeCenterZoneInDeficit / timeInDeficit; ;
        timeOwnZoneInDeficitPercent = timeOwnZoneInDeficit / timeInDeficit; ;
        timeOwnGoalZoneInDeficitPercent = timeOwnGoalZoneInDeficit / timeInDeficit; ;
        timeOpponentZoneInDeficitPercent = timeOpponentZoneInDeficit / timeInDeficit; ;
        timeOpponentGoalZoneInDeficitPercent = timeOpponentGoalZoneInDeficit / timeInDeficit; ;

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

    public void CalculateShots()
    {
        if (totalShotsFired > 0)
        {
            normalShotsFiredPercent = (float)normalShotsFired / totalShotsFired;
            mediumShotsFiredPercent = (float)mediumShotsFired / totalShotsFired;
            largeShotsFiredPercent = (float)largeShotsFired / totalShotsFired;
        }
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
        totalObjectsHit++;
    }

    public void CalculateAccuracy()
    {
        if (totalObjectsHit > 0)
        {
            shotsHitBlockPercent = (float)shotsHitBlock / totalObjectsHit;
            shotsDestroyedPercent = (float)shotsDestroyed / totalObjectsHit;
            shotsHitBallPercent = (float)shotsHitBall / totalObjectsHit;
            shotsHitEnemyShotPercent = (float)shotsHitEnemyShot / totalObjectsHit;
            shotsHitPlayerPercent = (float)shotsHitPlayer / totalObjectsHit;
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

    public void CalculateBlocks()
    {
        if (totalBlocksPlaced > 0)
        {
            blocksInOwnZonePercent = (float)blocksInOwnZone / totalBlocksPlaced;
            blocksInOwnGoalZonePercent = (float)blocksInOwnGoalZone / totalBlocksPlaced;
            blocksInCenterZonePercent = (float)blocksInCenterZone / totalBlocksPlaced;
            blocksInOpponentZonePercent = (float)blocksInOpponentZone / totalBlocksPlaced;
            blocksInOpponentGoalZonePercent = (float)blocksInOpponentGoalZone / totalBlocksPlaced;
        }
    }

    public void AddGoalType(string goalType)
    {
        switch (goalType)
        {
            case "owngoal":
                ownGoalsScored++;
                break;
            case "goal":
                correctGoalsScored++;
                break;
        }

    }

    public void AdjustResult(string type)
    {
        switch (type)
        {
            case "goalScored":
                goalsScored++;
                break;
            case "goalConceded":
                goalsConceded++;
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

    public void AddEnemyStunned(string shotType, float stunDuration)
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

    public void AddStunnedByEnemy(string shotType, float stunDuration)
    {
        switch (shotType)
        {
            case "normal":
                normalStunnedByEnemy++;
                break;
            case "medium":
                mediumStunnedByEnemy++;
                break;
            case "large":
                largeStunnedByEnemy++;
                break;
        }
        stunnedByEnemyTotalTime += stunDuration;
        totalStunnedByEnemy++;
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

    public void CalculateEmotes()
    {
        if (totalEmotes > 0)
        {
            emoteNicePercent = (float)emoteNice / totalEmotes;
            emoteAngryPercent = (float)emoteAngry / totalEmotes;
            emoteCryPercent = (float)emoteCry / totalEmotes;
            emoteHahaPercent = (float)emoteHaha / totalEmotes;
        }
    }


}
