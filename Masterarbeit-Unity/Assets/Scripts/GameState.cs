using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public bool gamePaused;

    public Canvas pauseScreen;
    public Canvas player1Box;
    public Canvas player2Box;
    private Image greenCheckP1;
    private Image greenCheckP2;

    private bool player1Ready;
    private bool player2Ready;
    public bool levelEnded;
    public int depauseCountdown;
    private Text topText;
    private Text middleText;
    public bool startCountdownActivated;    //regelt, ob der Startbildschirm mit dem Countdown angezeigt werden soll

    public GameObject player1;
    public GameObject player2;
    private Player player1Script;
    private Player player2Script;

    private PlayerLogging playerLoggingP1;
    private PlayerLogging playerLoggingP2;
    public GlobalTimer globalTimer;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        SetGoalCount("Team1");
        SetGoalCount("Team2");

        topText = pauseScreen.transform.Find("TransparentScreen").transform.Find("topText").GetComponent<Text>();
        middleText = pauseScreen.transform.Find("TransparentScreen").transform.Find("middleText").GetComponent<Text>();
        player1Box = pauseScreen.transform.Find("TransparentScreen").transform.Find("Spieler1").GetComponent<Canvas>();
        player2Box = pauseScreen.transform.Find("TransparentScreen").transform.Find("Spieler2").GetComponent<Canvas>();
        greenCheckP1 = pauseScreen.transform.Find("TransparentScreen").transform.Find("Spieler1").transform.Find("Spieler1_Check").GetComponent<Image>();
        greenCheckP2 = pauseScreen.transform.Find("TransparentScreen").transform.Find("Spieler2").transform.Find("Spieler2_Check").GetComponent<Image>();

        globalTimer = (GlobalTimer)FindObjectOfType(typeof(GlobalTimer));
        playerLoggingP1 = player1.GetComponent<PlayerLogging>();
        playerLoggingP2 = player2.GetComponent<PlayerLogging>();
        player1Script = player1.GetComponent<Player>();
        player2Script = player2.GetComponent<Player>();
        if (startCountdownActivated)
        {
            SetGamePaused(true, "start");    //zu Beginn wird das Spiel pausiert 
        }
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
            playerLoggingP1.AdjustResult("goalConceded");
            playerLoggingP2.AdjustResult("goalScored");
            SetGoalCount("Team2");
            //Logging
            if (scoredByTeamNr == 1)
            {
                playerLoggingP1.AddGoalType("owngoal");
            }
            else if (scoredByTeamNr == 2)
            {
                playerLoggingP2.AddGoalType("goal");
            }
        }
        else if (goal.Equals("Goal2"))
        {
            goalsTeam1++;
            playerLoggingP2.AdjustResult("goalConceded");
            playerLoggingP1.AdjustResult("goalScored");
            SetGoalCount("Team1");

            //Logging
            if (scoredByTeamNr == 1)
            {
                playerLoggingP1.AddGoalType("goal");
            }
            else if (scoredByTeamNr == 2)
            {
                playerLoggingP2.AddGoalType("owngoal");
            }
        }
        CheckGoalLimit();
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
        if (goalsTeam1 == goalLimit || goalsTeam2 == goalLimit)
        {
            SetGamePaused(true, "end");
        }
        else
        {
            StartCoroutine(GoalFreeze());
        }
    }

    private void CheckPause()
    {
        if (Input.GetButtonUp("Start"))
        {
            if (!gamePaused)
            {
                SetGamePaused(true, "pause");
            }
        }
        //sofern das Spiel pausiert wird
        if (gamePaused)
        {
            if (!levelEnded)
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
                    StartCoroutine(StartDepauseCountdown(depauseCountdown));
                }
            } else if (levelEnded)
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
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }

    public void SetGamePaused(bool b, string screenType)
    {
        gamePaused = b;
        if (gamePaused)
        {
            Time.timeScale = 0.0001f;
            switch (screenType)
            {
                case "pause":
                    BuildPauseScreen("pause");
                    break;
                case "start":
                    BuildPauseScreen("start");
                    break;
                case "end":
                    EndScene();
                    break;
            }
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

    IEnumerator StartDepauseCountdown(int countdown)
    {
        BuildPauseScreen("countdown");
        for (int i = countdown; i > 0; i--)
        {
            topText.text = i.ToString();
            yield return new WaitForSeconds(1 * Time.timeScale);
        }
        SetGamePaused(false, "pause");
        SetPlayerReady(false, 1);
        SetPlayerReady(false, 2);
        pauseScreen.enabled = false;
    }

    public void BuildPauseScreen(string screenType)
    {
        pauseScreen.enabled = true;
        switch (screenType)
        {
            case "pause":
                player1Box.enabled = true;
                player2Box.enabled = true;
                topText.text = "Pause";
                topText.fontSize = 50;
                middleText.text = "Drücke A zum Fortsetzen!";
                break;
            case "start":
                player1Box.enabled = true;
                player2Box.enabled = true;
                topText.text = "Mach dich bereit für " + SceneManager.GetActiveScene().name + "!";
                topText.fontSize = 45;
                middleText.text = "Drücke A zum Starten!";
                break;
            case "countdown":
                player1Box.enabled = false;
                player2Box.enabled = false;
                topText.text = "";
                topText.fontSize = 80;
                middleText.text = "";

                break;
            case "end":
                player1Box.enabled = true;
                player2Box.enabled = true;

                if (goalsTeam1 > goalsTeam2)
                {
                    Debug.Log("Team 1 wins with " + goalsTeam1 + " - " + goalsTeam2);
                    topText.text = "Spieler 1 gewinnt mit " + goalsTeam1 + " - " + goalsTeam2 + "!";
                } else if (goalsTeam1 < goalsTeam2)
                {
                    Debug.Log("Team 2 wins with " + goalsTeam2 + " - " + goalsTeam1);
                    topText.text = "Spieler 1 gewinnt mit " + goalsTeam2 + " - " + goalsTeam1 + "!";
                } else
                {
                    Debug.Log("Das Spiel endet " + goalsTeam2 + " - " + goalsTeam1 +" unentschieden!");
                    topText.text = "Das Spiel endet " + goalsTeam2 + " - " + goalsTeam1 + " unentschieden!";
                }
                topText.fontSize = 45;
                middleText.text = "Drücke A zum Fortfahren!";
                break;
        }
    }

    public void EndScene()
    {
        levelEnded = true;
        BuildPauseScreen("end");
        globalTimer.SetEndTime();
        player1Script.CalculateLogData();
        player2Script.CalculateLogData();
        ExportData exportData = (ExportData)FindObjectOfType(typeof(ExportData));
        exportData.ExportAllData();
    }
}
