﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogging : MonoBehaviour
{
    PositionTracker positionTracker;

    public int playerTeam;
    public int subjectNr;
    public int subjectNrEnemy;
    public int rating;
    public int ratingEnemy;
    public string gameType;

    private string currentZone;  //aktuelle Zone des Spielers
    public string currentResult;

    public float distanceTravelled;
    public float distanceTravelledInLead;
    public float distanceTravelledInTie;
    public float distanceTravelledInDeficit;

    //result
    public string finalResult;
    public string endingCondition;
    public int goalsScored;
    public int goalsConceded;
    //goals Scored
    public int correctGoalsScored;
    public int ownGoalsScored;

    //time per result
    public float timeInLead;
    public float timeTied;
    public float timeInDeficit;

    //time per Zone
    public float timeCenterZone;
    public float timeOwnZone;
    public float timeOwnGoalZone;
    public float timeOpponentZone;
    public float timeOpponentGoalZone;

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

    //Shots
    public int totalShotsFired;
    public int normalShotsFired;
    public int mediumShotsFired;
    public int largeShotsFired;
    //shots per Result
    public int totalShotsFiredInLead;
    public int normalShotsFiredInLead;
    public int mediumShotsFiredInLead;
    public int largeShotsFiredInLead;

    public int totalShotsFiredInTie;
    public int normalShotsFiredInTie;
    public int mediumShotsFiredInTie;
    public int largeShotsFiredInTie;

    public int totalShotsFiredInDeficit;
    public int normalShotsFiredInDeficit;
    public int mediumShotsFiredInDeficit;
    public int largeShotsFiredInDeficit;

    //Accuracy
    public int totalObjectsHit;
    public int shotsHitBlock;
    public int shotsHitBall;
    public int shotsHitPlayer;
    public int shotsHitEnemyShot;
    public int shotsDestroyed;
    //Accuracy per result
    public int totalObjectsHitInLead;
    public int shotsHitBlockInLead;
    public int shotsHitBallInLead;
    public int shotsHitPlayerInLead;
    public int shotsHitEnemyShotInLead;
    public int shotsDestroyedInLead;

    public int totalObjectsHitInTie;
    public int shotsHitBlockInTie;
    public int shotsHitBallInTie;
    public int shotsHitPlayerInTie;
    public int shotsHitEnemyShotInTie;
    public int shotsDestroyedInTie;

    public int totalObjectsHitInDeficit;
    public int shotsHitBlockInDeficit;
    public int shotsHitBallInDeficit;
    public int shotsHitPlayerInDeficit;
    public int shotsHitEnemyShotInDeficit;
    public int shotsDestroyedInDeficit;


    //Blocks
    public int totalBlocksPlaced;
    public int blocksInOwnZone;
    public int blocksInOwnGoalZone;
    public int blocksInCenterZone;
    public int blocksInOpponentZone;
    public int blocksInOpponentGoalZone;
    //blocks per result
    public int totalBlocksPlacedInLead;
    public int blocksInOwnZoneInLead;
    public int blocksInOwnGoalZoneInLead;
    public int blocksInCenterZoneInLead;
    public int blocksInOpponentZoneInLead;
    public int blocksInOpponentGoalZoneInLead;

    public int totalBlocksPlacedInTie;
    public int blocksInOwnZoneInTie;
    public int blocksInOwnGoalZoneInTie;
    public int blocksInCenterZoneInTie;
    public int blocksInOpponentZoneInTie;
    public int blocksInOpponentGoalZoneInTie;

    public int totalBlocksPlacedInDeficit;
    public int blocksInOwnZoneInDeficit;
    public int blocksInOwnGoalZoneInDeficit;
    public int blocksInCenterZoneInDeficit;
    public int blocksInOpponentZoneInDeficit;
    public int blocksInOpponentGoalZoneInDeficit;

    //Stun
    //enemy stunned
    public int totalEnemyStunned;
    public int normalEnemyStunned;
    public int mediumEnemyStunned;
    public int largeEnemyStunned;
    public float enemyStunnedTotalTime;
    //enemy stunned per result
    public int totalEnemyStunnedInLead;
    public int normalEnemyStunnedInLead;
    public int mediumEnemyStunnedInLead;
    public int largeEnemyStunnedInLead;
    public float enemyStunnedTotalTimeInLead;

    public int totalEnemyStunnedInTie;
    public int normalEnemyStunnedInTie;
    public int mediumEnemyStunnedInTie;
    public int largeEnemyStunnedInTie;
    public float enemyStunnedTotalTimeInTie;

    public int totalEnemyStunnedInDeficit;
    public int normalEnemyStunnedInDeficit;
    public int mediumEnemyStunnedInDeficit;
    public int largeEnemyStunnedInDeficit;
    public float enemyStunnedTotalTimeInDeficit;

    //stunned by enemy
    public int totalStunnedByEnemy;
    public int normalStunnedByEnemy;
    public int mediumStunnedByEnemy;
    public int largeStunnedByEnemy;
    public float stunnedByEnemyTotalTime;
    //stunned by enemy per result
    public int totalStunnedByEnemyInLead;
    public int normalStunnedByEnemyInLead;
    public int mediumStunnedByEnemyInLead;
    public int largeStunnedByEnemyInLead;
    public float stunnedByEnemyTotalTimeInLead;

    public int totalStunnedByEnemyInTie;
    public int normalStunnedByEnemyInTie;
    public int mediumStunnedByEnemyInTie;
    public int largeStunnedByEnemyInTie;
    public float stunnedByEnemyTotalTimeInTie;

    public int totalStunnedByEnemyInDeficit;
    public int normalStunnedByEnemyInDeficit;
    public int mediumStunnedByEnemyInDeficit;
    public int largeStunnedByEnemyInDeficit;
    public float stunnedByEnemyTotalTimeInDeficit;
    //Emotes
    public int totalEmotes;
    public int emoteNice;
    public int emoteAngry;
    public int emoteCry;
    public int emoteHaha;
    public int totalEmotesInLead;
    public int emoteNiceInLead;
    public int emoteAngryInLead;
    public int emoteCryInLead;
    public int emoteHahaInLead;

    public int totalEmotesInTie;
    public int emoteNiceInTie;
    public int emoteAngryInTie;
    public int emoteCryInTie;
    public int emoteHahaInTie;

    public int totalEmotesInDeficit;
    public int emoteNiceInDeficit;
    public int emoteAngryInDeficit;
    public int emoteCryInDeficit;
    public int emoteHahaInDeficit;
    
    // Use this for initialization
    void Start()
    {
        positionTracker = gameObject.GetComponent<PositionTracker>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        AddResultTime(currentResult);
    }

    public void SetPlayerTeam(int team)
    {
        playerTeam = team;
    }

    public void SetSubjectNr(int vp, int vpEnemy)
    {
        subjectNr = vp;
        subjectNrEnemy = vpEnemy;
    }

    public void SetRating(int rat, int ratEnemy)
    {
        rating = rat;
        ratingEnemy = ratEnemy;

    }

    public string CheckResult()
    {
        if (goalsScored > goalsConceded)
        {
            currentResult = "in_lead";
            finalResult = "Sieg";
            positionTracker.ChangeResult("in_lead");
        }
        else if (goalsScored == goalsConceded)
        {
            currentResult = "in_tie";
            finalResult = "Remis";
            positionTracker.ChangeResult("in_tie");
        }
        else if (goalsScored < goalsConceded)
        {
            currentResult = "in_deficit";
            finalResult = "Niederlage";
            positionTracker.ChangeResult("in_deficit");
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

    public void AddShot(string shotType)
    {
        switch (shotType)
        {
            case "normal":
                normalShotsFired++;
                switch (currentResult)
                {
                    case "in_lead":
                        normalShotsFiredInLead++;
                        break;
                    case "in_tie":
                        normalShotsFiredInTie++;
                        break;
                    case "in_deficit":
                        normalShotsFiredInDeficit++;
                        break;
                }

                break;
            case "medium":
                mediumShotsFired++;
                switch (currentResult)
                {
                    case "in_lead":
                        mediumShotsFiredInLead++;
                        break;
                    case "in_tie":
                        mediumShotsFiredInTie++;
                        break;
                    case "in_deficit":
                        mediumShotsFiredInDeficit++;
                        break;
                }
                break;
            case "large":
                largeShotsFired++;
                switch (currentResult)
                {
                    case "in_lead":
                        largeShotsFiredInLead++;
                        break;
                    case "in_tie":
                        largeShotsFiredInTie++;
                        break;
                    case "in_deficit":
                        largeShotsFiredInDeficit++;
                        break;
                }
                break;
        }
        totalShotsFired++;
        switch (currentResult)
        {
            case "in_lead":
                totalShotsFiredInLead++;
                break;
            case "in_tie":
                totalShotsFiredInTie++;
                break;
            case "in_deficit":
                totalShotsFiredInDeficit++;
                break;
        }

    }

    public void AddAccuracy(string action)
    {
        switch (action)
        {
            case "ball":
                shotsHitBall++;
                switch (currentResult)
                {
                    case "in_lead":
                        shotsHitBallInLead++;
                        break;
                    case "in_tie":
                        shotsHitBallInTie++;
                        break;
                    case "in_deficit":
                        shotsHitBallInDeficit++;
                        break;
                }

                break;
            case "player":
                shotsHitPlayer++;
                switch (currentResult)
                {
                    case "in_lead":
                        shotsHitPlayerInLead++;
                        break;
                    case "in_tie":
                        shotsHitPlayerInTie++;
                        break;
                    case "in_deficit":
                        shotsHitPlayerInDeficit++;
                        break;
                }

                break;
            case "block":
                shotsHitBlock++;
                switch (currentResult)
                {
                    case "in_lead":
                        shotsHitBlockInLead++;
                        break;
                    case "in_tie":
                        shotsHitBlockInTie++;
                        break;
                    case "in_deficit":
                        shotsHitBlockInDeficit++;
                        break;
                }

                break;
            case "shot":
                shotsHitEnemyShot++;
                switch (currentResult)
                {
                    case "in_lead":
                        shotsHitEnemyShotInLead++;
                        break;
                    case "in_tie":
                        shotsHitEnemyShotInTie++;
                        break;
                    case "in_deficit":
                        shotsHitEnemyShotInDeficit++;
                        break;
                }

                break;
            case "destroy":
                shotsDestroyed++;
                switch (currentResult)
                {
                    case "in_lead":
                        shotsDestroyedInLead++;
                        break;
                    case "in_tie":
                        shotsDestroyedInTie++;
                        break;
                    case "in_deficit":
                        shotsDestroyedInDeficit++;
                        break;
                }

                break;
        }
        totalObjectsHit++;
        switch (currentResult)
        {
            case "in_lead":
                totalObjectsHitInLead++;
                break;
            case "in_tie":
                totalObjectsHitInTie++;
                break;
            case "in_deficit":
                totalObjectsHitInDeficit++;
                break;
        }

    }

    public void AddBlock()
    {
        switch (currentZone)
        {
            case "Center Zone":
                blocksInCenterZone++;
                switch (currentResult)
                {
                    case "in_lead":
                        blocksInCenterZoneInLead++;
                        break;
                    case "in_tie":
                        blocksInCenterZoneInTie++;
                        break;
                    case "in_deficit":
                        blocksInCenterZoneInDeficit++;
                        break;
                }
                break;
            case "Own Zone":
                blocksInOwnZone++;
                switch (currentResult)
                {
                    case "in_lead":
                        blocksInOwnZoneInLead++;
                        break;
                    case "in_tie":
                        blocksInOwnZoneInTie++;
                        break;
                    case "in_deficit":
                        blocksInOwnZoneInDeficit++;
                        break;
                }
                break;
            case "Own Goal Zone":
                blocksInOwnGoalZone++;
                switch (currentResult)
                {
                    case "in_lead":
                        blocksInOwnGoalZoneInLead++;
                        break;
                    case "in_tie":
                        blocksInOwnGoalZoneInTie++;
                        break;
                    case "in_deficit":
                        blocksInOwnGoalZoneInDeficit++;
                        break;
                }
                break;
            case "Opponent Zone":
                blocksInOpponentZone++;
                switch (currentResult)
                {
                    case "in_lead":
                        blocksInOpponentZoneInLead++;
                        break;
                    case "in_tie":
                        blocksInOpponentZoneInTie++;
                        break;
                    case "in_deficit":
                        blocksInOpponentZoneInDeficit++;
                        break;
                }
                break;
            case "Opponent Goal Zone":
                blocksInOpponentGoalZone++;
                switch (currentResult)
                {
                    case "in_lead":
                        blocksInOpponentGoalZoneInLead++;
                        break;
                    case "in_tie":
                        blocksInOpponentGoalZoneInTie++;
                        break;
                    case "in_deficit":
                        blocksInOpponentGoalZoneInDeficit++;
                        break;
                }
                break;
        }
        totalBlocksPlaced++;
        switch (currentResult)
        {
            case "in_lead":
                totalBlocksPlacedInLead++;
                break;
            case "in_tie":
                totalBlocksPlacedInTie++;
                break;
            case "in_deficit":
                totalBlocksPlacedInDeficit++;
                break;
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

    public void AddWalkedDistance(string result, float walkedDistance)
    {
        switch (result)
        {
            case "in_lead":
                distanceTravelledInLead += walkedDistance;
                break;
            case "in_tie":
                distanceTravelledInTie += walkedDistance;
                break;
            case "in_deficit":
                distanceTravelledInDeficit += walkedDistance;
                break;
        }
    }
    public void AddWalkedDistance(float walkedDistance)
    {
        distanceTravelled += walkedDistance;
    }


    public void AddEnemyStunned(string shotType, float stunDuration)
    {
        switch (shotType)
        {
            case "normal":
                normalEnemyStunned++;
                switch (currentResult)
                {
                    case "in_lead":
                        normalEnemyStunnedInLead++;
                        break;
                    case "in_tie":
                        normalEnemyStunnedInTie++;
                        break;
                    case "in_deficit":
                        normalEnemyStunnedInDeficit++;
                        break;
                }

                break;
            case "medium":
                mediumEnemyStunned++;
                switch (currentResult)
                {
                    case "in_lead":
                        mediumEnemyStunnedInLead++;
                        break;
                    case "in_tie":
                        mediumEnemyStunnedInTie++;
                        break;
                    case "in_deficit":
                        mediumEnemyStunnedInDeficit++;
                        break;
                }

                break;
            case "large":
                largeEnemyStunned++;
                switch (currentResult)
                {
                    case "in_lead":
                        largeEnemyStunnedInLead++;
                        break;
                    case "in_tie":
                        largeEnemyStunnedInTie++;
                        break;
                    case "in_deficit":
                        largeEnemyStunnedInDeficit++;
                        break;
                }

                break;
        }
        enemyStunnedTotalTime += stunDuration;
        totalEnemyStunned++;
        switch (currentResult)
        {
            case "in_lead":
                enemyStunnedTotalTimeInLead++;
                totalEnemyStunnedInLead++;
                break;
            case "in_tie":
                enemyStunnedTotalTimeInTie++;
                totalEnemyStunnedInTie++;
                break;
            case "in_deficit":
                enemyStunnedTotalTimeInDeficit++;
                totalEnemyStunnedInDeficit++;
                break;
        }

    }

    public void AddStunnedByEnemy(string shotType, float stunDuration)
    {
        switch (shotType)
        {
            case "normal":
                normalStunnedByEnemy++;
                switch (currentResult)
                {
                    case "in_lead":
                        normalStunnedByEnemyInLead++;
                        break;
                    case "in_tie":
                        normalStunnedByEnemyInTie++;
                        break;
                    case "in_deficit":
                        normalStunnedByEnemyInDeficit++;
                        break;
                }

                break;
            case "medium":
                mediumStunnedByEnemy++;
                switch (currentResult)
                {
                    case "in_lead":
                        mediumStunnedByEnemyInLead++;
                        break;
                    case "in_tie":
                        mediumStunnedByEnemyInTie++;
                        break;
                    case "in_deficit":
                        mediumStunnedByEnemyInDeficit++;
                        break;
                }

                break;
            case "large":
                largeStunnedByEnemy++;
                switch (currentResult)
                {
                    case "in_lead":
                        largeStunnedByEnemyInLead++;
                        break;
                    case "in_tie":
                        largeStunnedByEnemyInTie++;
                        break;
                    case "in_deficit":
                        largeStunnedByEnemyInDeficit++;
                        break;
                }

                break;
        }
        stunnedByEnemyTotalTime += stunDuration;
        totalStunnedByEnemy++;
        switch (currentResult)
        {
            case "in_lead":
                enemyStunnedTotalTimeInLead++;
                stunnedByEnemyTotalTimeInLead++;
                break;
            case "in_tie":
                totalStunnedByEnemyInTie++;
                stunnedByEnemyTotalTimeInTie++;
                break;
            case "in_deficit":
                totalStunnedByEnemyInDeficit++;
                stunnedByEnemyTotalTimeInDeficit++;
                break;
        }

    }

    public void AddEmote(string type)
    {
        switch (type)
        {
            case "nice":
                emoteNice++;
                switch (currentResult)
                {
                    case "in_lead":
                        emoteNiceInLead++;
                        break;
                    case "in_tie":
                        emoteNiceInTie++;
                        break;
                    case "in_deficit":
                        emoteNiceInDeficit++;
                        break;
                }

                break;
            case "angry":
                emoteAngry++;
                switch (currentResult)
                {
                    case "in_lead":
                        emoteAngryInLead++;
                        break;
                    case "in_tie":
                        emoteAngryInTie++;
                        break;
                    case "in_deficit":
                        emoteAngryInDeficit++;
                        break;
                }

                break;
            case "cry":
                emoteCry++;
                switch (currentResult)
                {
                    case "in_lead":
                        emoteCryInLead++;
                        break;
                    case "in_tie":
                        emoteCryInTie++;
                        break;
                    case "in_deficit":
                        emoteCryInDeficit++;
                        break;
                }

                break;
            case "haha":
                emoteHaha++;
                switch (currentResult)
                {
                    case "in_lead":
                        emoteHahaInLead++;
                        break;
                    case "in_tie":
                        emoteHahaInTie++;
                        break;
                    case "in_deficit":
                        emoteHahaInDeficit++;
                        break;
                }

                break;
        }
        totalEmotes++;
        switch (currentResult)
        {
            case "in_lead":
                totalEmotesInLead++;
                break;
            case "in_tie":
                totalEmotesInTie++;
                break;
            case "in_deficit":
                totalEmotesInDeficit++;
                break;
        }

    }

    public void SetEndingCondition(string condition)
    {
        endingCondition = condition;
    }

    public void SetGameType(string type)
    {
        gameType = type;
    }


}
