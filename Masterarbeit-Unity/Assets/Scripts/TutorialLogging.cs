using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogging : MonoBehaviour
{

    //Shots
    public int totalShotsFired;
    public int normalShotsFired;
    public int mediumShotsFired;
    public int largeShotsFired;

    //Accuracy
    public int totalObjectsHit;
    public int shotsHitBlock;
    public int shotsHitBall;
    public int shotsHitPlayer;
    public int shotsHitEnemyShot;
    public int shotsDestroyed;
	public int shotsHitTarget;

    //stunned by enemy
    public int totalStunnedByEnemy;
    public int normalStunnedByEnemy;
    public int mediumStunnedByEnemy;
    public int largeStunnedByEnemy;
    public float stunnedByEnemyTotalTime;

    
    // Use this for initialization
    void Start()
    {
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
		case "target":
			shotsHitTarget++;
			break;
        }
        totalObjectsHit++;

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


}
