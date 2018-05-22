using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{

    public int maximumBalls;
    public int goalLimit;
    public float goalFreezeTime;
    private List<Ball> ballList = new List<Ball>();
    bool maximumBallsReached = false;
    public int goalsTeam1 = 0;
    public int goalsTeam2 = 0;
    public Text scoreTeam1;
    public Text scoreTeam2;
    private bool gamePaused;

    public Canvas pauseScreen;
    public Canvas pauseCountdownScreen;
    public Image greenCheckP1;
    public Image greenCheckP2;
    private bool player1Ready;
    private bool player2Ready;
    public int depauseCountdown;
    private Text pauseCountdownText;

    public GameObject player1;
    public GameObject player2;
    private PlayerLogging playerLoggingP1;
    private PlayerLogging playerLoggingP2;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        SetGoalCount("Team1");
        SetGoalCount("Team2");
        pauseCountdownText = (Text)pauseCountdownScreen.transform.Find("TransparentScreen").transform.Find("Countdown").GetComponent<Text>();
        playerLoggingP1 = player1.GetComponent<PlayerLogging>();
        playerLoggingP2 = player2.GetComponent<PlayerLogging>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckPause();
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
    public bool MaximumBallsReached()
    {
        if (ballList.Count < maximumBalls)
        {
            maximumBallsReached = false;
        }
        else
        {
            maximumBallsReached = true;
        }

        return maximumBallsReached;
    }

    //Wenn ein Ball mit dem entsprechendem Goal-Collider in Berührung kommt, wird dem anderen Team ein Tor zugeschrieben.
    public void GoalScored(string goal, int scoredByTeamNr)
    {
        if (goal.Equals("Goal1"))
        {
            goalsTeam2++;
            SetGoalCount("Team2");
            //Logging
            if (scoredByTeamNr == 1)
            {
                playerLoggingP1.AddGoal("owngoal");
            }
            else if (scoredByTeamNr == 2)
            {
                playerLoggingP2.AddGoal("goal");
            }
        }
        else if (goal.Equals("Goal2"))
        {
            goalsTeam1++;
            SetGoalCount("Team1");

            //Logging
            if (scoredByTeamNr == 1)
            {
                playerLoggingP1.AddGoal("goal");
            }
            else if (scoredByTeamNr == 2)
            {
                playerLoggingP2.AddGoal("owngoal");
            }
        }
        CheckGoalLimit();
        StartCoroutine(GoalFreeze());
    }

    //Hiermit kann die Anzeige für die Tore bearbeitet werden
    private void SetGoalCount(string s)
    {
        if (s.Equals("Team1"))
        {
            scoreTeam1.text = goalsTeam1.ToString();
        }
        else if (s.Equals("Team2"))
        {
            scoreTeam2.text = goalsTeam2.ToString();
        }

    }

    private void CheckGoalLimit()
    {
        if (goalsTeam1 == goalLimit)
        {
            Debug.Log("Team 1 wins with " + goalsTeam1 + " - " + goalsTeam2);
            Time.timeScale = 0;
        }
        else if (goalsTeam2 == goalLimit)
        {
            Debug.Log("Team 2 wins with " + goalsTeam2 + " - " + goalsTeam1);
            Time.timeScale = 0;
        }
    }

    private void CheckPause()
    {
        if (Input.GetButtonUp("Start"))
        {
            if (!gamePaused)
            {
                SetGamePaused(true);
            }
        }
        //sofern das Spiel pausiert wird
        if (gamePaused)
        {
            //überprüfe, ob die einzelnen Spieler bereit sind
            if (Input.GetButtonUp("ShootP1"))
            {
                SetPlayerReady(true, 1);
            }
            else if (Input.GetButtonUp("ShootP2"))
            {
                SetPlayerReady(true, 2);
            }

            if (player1Ready && player2Ready)
            {
                StartCoroutine(StartDepauseCountdown());
            }
        }
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }

    public void SetGamePaused(bool b)
    {
        gamePaused = b;
        if (gamePaused)
        {
            Time.timeScale = 0.0001f;
            pauseScreen.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseScreen.enabled = false;
        }
    }

    private void SetPlayerReady(bool b, int playerNr)
    {
        if (playerNr == 1)
        {
            player1Ready = b;
            greenCheckP1.enabled = b;
        }
        else if (playerNr == 2)
        {
            player2Ready = b;
            greenCheckP2.enabled = b;
        }

    }

    IEnumerator GoalFreeze()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(goalFreezeTime * Time.timeScale);
        Time.timeScale = 1;
    }

    IEnumerator StartDepauseCountdown()
    {
        pauseScreen.enabled = false;
        pauseCountdownScreen.enabled = true;
        for (int i = depauseCountdown; i > 0; i--)
        {
            pauseCountdownText.text = i.ToString();
            yield return new WaitForSeconds(1 * Time.timeScale);
        }
        SetGamePaused(false);
        SetPlayerReady(false, 1);
        SetPlayerReady(false, 2);
        pauseCountdownScreen.enabled = false;

    }

}
