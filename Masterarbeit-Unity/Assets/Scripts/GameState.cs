﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public int maximumBalls;
    private List<Ball> ballList = new List<Ball>();
    bool maximumBallsReached = false;
    public int goalsTeam1 = 0;
    public int goalsTeam2 = 0;
    public Text scoreTeam1;
    public Text scoreTeam2;


    // Use this for initialization
    void Start () {
        SetGoalCount("Team1");
        SetGoalCount("Team2");
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Über diese Methode werden neue Bälle an die ballList übergeben
    public void RegisterBallList(Ball ball)
    {
        ballList.Add(ball);
    }

    //Die Methode liefert als return Wert die aktuelle ballList zurück
    public List<Ball> GetBalllist()
    {
        return ballList;
    }

    //Die Methode löscht den Ball mit der entsprechenden ID aus der Liste
    public void RemoveBall(int ID)
    {
        for (int i = 0; i < ballList.Count; i++)
        {
            if (ballList[i].GetBallID() == ID)
            {
                ballList.RemoveAt(i);
            }
        }
    }

    //Die Methode überprüft, ob das Maximum der zugelassenen Bälle erreicht wurde und liefert das Ergebnis als bool zurück
    public bool CheckMaximumBalls()
    {
        if (ballList.Count < maximumBalls)
        {
            maximumBallsReached = false;
        } else
        {
            maximumBallsReached = true;
        }

        return maximumBallsReached;
    }

    //Wenn ein Ball mit dem entsprechendem Goal-Collider in Berührung kommt, wird dem anderen Team ein Tor zugeschrieben.
    public void GoalScored(string goal)
    {
        if (goal.Equals("Goal1"))
        {
            goalsTeam2++;
            SetGoalCount("Team2");
        } else if (goal.Equals("Goal2"))
        {
            goalsTeam1++;
            SetGoalCount("Team1");

        }
    }

    //Hiermit kann die Anzeige für die Tore bearbeitet werden
    private void SetGoalCount(string s)
    {
        if (s.Equals("Team1"))
        {
            scoreTeam1.text = goalsTeam1.ToString();
        } else if (s.Equals("Team2"))
        {
            scoreTeam2.text = goalsTeam2.ToString();
        }

    }
}
