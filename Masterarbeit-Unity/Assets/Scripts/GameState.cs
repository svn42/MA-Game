using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameState : MonoBehaviour
{

    public int maximumBalls;
    public int goalLimit;
    public float timeLeft;
    public float goalFreezeTime;
    private List<Ball> ballList = new List<Ball>();
    bool maximumBallsReached = false;
    public int goalsTeam1 = 0;
    public int goalsTeam2 = 0;
    public bool gamePaused;

    public GameObject gui;
    private Text scoreTeam1;
    private Text scoreTeam2;
    private Text timer;
    private GameObject pauseScreenGO;
    private Canvas pauseScreen;
    private GameObject transparentScreen;
    private GameObject popUp;
    private Canvas player1Box;
    private Canvas player2Box;
    private Image greenCheckP1;
    private Image greenCheckP2;

    private bool player1Ready;
    private bool player2Ready;
    private bool playerHelp;

    public bool levelEnded;
    public bool nextLevelReady;
    public int timeUntilNextLevel;
    public int depauseCountdown;
    private bool depauseCountdownStarted;
    private bool timerBlink;
    private bool popUp120Showed;
    private bool popUp60Showed;
    private Text topText;
    private Text middleText;
    private Text observerText;
    private Text helpText;
    public bool startCountdownActivated;    //regelt, ob der Startbildschirm mit dem Countdown angezeigt werden soll

    private string endingCondition;
    public GameObject player1;
    public GameObject player2;
    private Player player1Script;
    private Player player2Script;

    private PlayerLogging playerLoggingP1;
    private PlayerLogging playerLoggingP2;
    public GlobalTimer globalTimer;
    //Audios
    private AudioSource audioSource;
    public AudioClip soundCountdownRegular;
    public AudioClip soundCountdownEnd;
    public AudioClip soundGoalHorn;
    public AudioClip soundSlap;
    public AudioClip soundPlop;
    public AudioClip soundBallHit;
    public AudioClip soundWhistle;
    public AudioClip soundPopup;

    public AudioSource musicPlayer;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {

        timeLeft += 0.02f;
        if (timeLeft < 120)
        {
            popUp120Showed = true;
        }
        else if (timeLeft < 60)
        {
            popUp60Showed = true;
        }

        popUp = gui.transform.Find("PopUp").gameObject;
        pauseScreenGO = gui.transform.Find("PauseScreen").gameObject;
        pauseScreen = pauseScreenGO.GetComponent<Canvas>();
        transparentScreen = pauseScreenGO.transform.Find("TransparentScreen").gameObject;
        topText = transparentScreen.transform.Find("topText").GetComponent<Text>();
        middleText = transparentScreen.transform.Find("middleText").GetComponent<Text>();
        observerText = transparentScreen.transform.Find("observerText").GetComponent<Text>();
        helpText = transparentScreen.transform.Find("helpText").GetComponent<Text>();
        player1Box = transparentScreen.transform.Find("Spieler1").GetComponent<Canvas>();
        player2Box = transparentScreen.transform.Find("Spieler2").GetComponent<Canvas>();
        greenCheckP1 = transparentScreen.transform.Find("Spieler1").transform.Find("Spieler1_Check").GetComponent<Image>();
        greenCheckP2 = transparentScreen.transform.Find("Spieler2").transform.Find("Spieler2_Check").GetComponent<Image>();
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        scoreTeam1 = gui.transform.Find("UI_Spielstand").transform.Find("Spielstand Team 1").transform.Find("Score Team 1").GetComponent<Text>();
        scoreTeam2 = gui.transform.Find("UI_Spielstand").transform.Find("Spielstand Team 2").transform.Find("Score Team 2").GetComponent<Text>();
        timer = gui.transform.Find("UI_Spielstand").transform.Find("Timer_Background").transform.Find("Time").GetComponent<Text>();

        globalTimer = (GlobalTimer)FindObjectOfType(typeof(GlobalTimer));
        playerLoggingP1 = player1.GetComponent<PlayerLogging>();
        playerLoggingP2 = player2.GetComponent<PlayerLogging>();
        player1Script = player1.GetComponent<Player>();
        player2Script = player2.GetComponent<Player>();

        audioSource = GetComponent<AudioSource>();
        soundCountdownRegular = Resources.Load<AudioClip>("Sounds/countdown_regular");
        soundCountdownEnd = Resources.Load<AudioClip>("Sounds/countdown_ending");
        soundGoalHorn = Resources.Load<AudioClip>("Sounds/goal_horn");
        soundSlap = Resources.Load<AudioClip>("Sounds/slap");
        soundPlop = Resources.Load<AudioClip>("Sounds/plop");
        soundBallHit = Resources.Load<AudioClip>("Sounds/ball_hit");
        soundWhistle = Resources.Load<AudioClip>("Sounds/soundWhistle");
        soundPopup = Resources.Load<AudioClip>("Sounds/popup");

        SetGoalCount("Team1");
        SetGoalCount("Team2");


        playerLoggingP1.CheckResult();  //Die Playerloggings bekommen das Result zum Start mitgeteilt
        playerLoggingP2.CheckResult();

        if (startCountdownActivated)
        {
            SetGamePaused(true, "start");    //zu Beginn wird das Spiel pausiert 
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
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
        PlaySound(soundGoalHorn, 0.3f);
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
        playerLoggingP1.CheckResult();
        playerLoggingP2.CheckResult();
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

    private void CheckTimer()
    {
        timeLeft -= Time.deltaTime;
        timer.text = Mathf.RoundToInt(timeLeft).ToString();

        if (timeLeft <= 120 && !popUp120Showed)
        {
            StartCoroutine(ShowPopUp("2 Minuten"));
            popUp120Showed = true;
        }
        else if (timeLeft <= 60 && !popUp60Showed)
        {
            StartCoroutine(ShowPopUp("1 Minute"));
            popUp60Showed = true;
        }
        else if (timeLeft <= 10)
        {
            if (!timerBlink)
            {
                StartCoroutine(TimerBlinkEffect());
                timerBlink = true;
            }
        }
        else if (timeLeft <= 0)
        {
            if (!levelEnded)
            {
                PlaySound(soundWhistle, 0.4f);
                endingCondition = "Time";
                SetGamePaused(true, "end");
            }
        }
    }

    //Blinkeffekt des Stuns
    IEnumerator TimerBlinkEffect()
    {
        int blinkAmount = 10;      //und die Anzahl der Blinkeffekte ermittelt. Die Anzahl ergibt sich aus der Zeit, dividiert durch die Dauer des Blinkeffektes / 2.

        for (float i = 0; i < blinkAmount; i++)   //solange die Anzahl der Blinkeffekte nicht erreicht wurde
        {
            PlaySound(soundCountdownRegular, (i + 1 / 10));   //mit jeder Sekunde wird der Sound um 10% lauter. 
            timer.color = Color.red;     //wird der Renderer im Wechsel weiß und daraufhin in der ursprünglichen Farbe des Spielers eingefärbt
            yield return new WaitForSeconds(0.5f);
            timer.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void CheckGoalLimit()
    {
        if (goalsTeam1 == goalLimit || goalsTeam2 == goalLimit)
        {
            endingCondition = "Goals";
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

                if (Input.GetButtonUp("Help"))
                {
                    playerHelp = true;
                    observerText.enabled = true;
                    observerText.text = "Gib dem Versuchsleiter Bescheid, \nwenn du mit ihm im selben Raum sitzt.";
                    observerText.color = Color.red;
                }
                if (playerHelp)
                {
                    if (Input.GetKeyUp(KeyCode.H))
                    {
                        playerHelp = false;
                        observerText.text = "Ihr könnt das Spiel fortsetzen! :)";
                        observerText.color = Color.green;
                    }
                }
                if (!playerHelp)
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
                        if (!depauseCountdownStarted)
                        {
                            StartCoroutine(StartDepauseCountdown(depauseCountdown));
                            depauseCountdownStarted = true;
                        }
                    }
                }
            }
            else if (levelEnded && nextLevelReady)
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
            musicPlayer.Pause();
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
                    StartCoroutine(SetNextLevelReady());
                    break;
            }
        }
        else
        {
            Time.timeScale = 1;
            pauseScreen.enabled = false;
            musicPlayer.Play();

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
        yield return new WaitForSeconds(1 * Time.timeScale);
        BuildPauseScreen("countdown");
        for (int i = countdown; i > 0; i--)
        {
            topText.text = i.ToString();
            PlaySound(soundCountdownRegular, 0.5f);
            yield return new WaitForSeconds(1 * Time.timeScale);
        }
        PlaySound(soundCountdownEnd, 0.5f);
        SetGamePaused(false, "pause");
        SetPlayerReady(false, 1);
        SetPlayerReady(false, 2);
        pauseScreen.enabled = false;
        depauseCountdownStarted = false;
    }

    IEnumerator SetNextLevelReady()
    {
        yield return new WaitForSeconds(timeUntilNextLevel * Time.timeScale);
        BuildPauseScreen("endLevelReady");
        nextLevelReady = true;
    }



    public void BuildPauseScreen(string screenType)
    {
        pauseScreen.enabled = true;
        observerText.enabled = false;

        switch (screenType)
        {

            case "pause":
                helpText.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0.95f);
                player1Box.enabled = true;
                player2Box.enabled = true;
                topText.text = "Pause";
                topText.fontSize = 50;
                middleText.text = "Drücke A zum Fortsetzen!";
                break;
            case "start":
                helpText.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0.95f);
                player1Box.enabled = true;
                player2Box.enabled = true;
                topText.text = "Mach dich bereit für " + SceneManager.GetActiveScene().name + "!";
                topText.fontSize = 45;
                middleText.text = "Drücke A zum Starten!";
                break;
            case "countdown":
                helpText.enabled = false;
                transparentScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0.66f);
                player1Box.enabled = false;
                player2Box.enabled = false;
                topText.text = "";
                topText.fontSize = 80;
                middleText.text = "";

                break;
            case "endWait":
                helpText.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0.95f);
                player1Box.enabled = false;
                player2Box.enabled = false;

                if (goalsTeam1 > goalsTeam2)
                {
                    topText.text = "Spieler 1 gewinnt mit " + goalsTeam1 + " - " + goalsTeam2 + "!";
                }
                else if (goalsTeam1 < goalsTeam2)
                {
                    topText.text = "Spieler 1 gewinnt mit " + goalsTeam2 + " - " + goalsTeam1 + "!";
                }
                else
                {
                    topText.text = "Das Spiel endet " + goalsTeam2 + " - " + goalsTeam1 + " unentschieden!";
                }
                topText.fontSize = 45;
                middleText.text = "";
                break;
            case "endLevelReady":
                helpText.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0.95f);
                player1Box.enabled = true;
                player2Box.enabled = true;

                if (goalsTeam1 > goalsTeam2)
                {
                    topText.text = "Spieler 1 gewinnt mit " + goalsTeam1 + " - " + goalsTeam2 + "!";
                }
                else if (goalsTeam1 < goalsTeam2)
                {
                    topText.text = "Spieler 1 gewinnt mit " + goalsTeam2 + " - " + goalsTeam1 + "!";
                }
                else
                {
                    topText.text = "Das Spiel endet " + goalsTeam2 + " - " + goalsTeam1 + " unentschieden!";
                }
                topText.fontSize = 45;
                middleText.text = "Drücke A zum Fortfahren!";
                break;

        }
    }

    IEnumerator ShowPopUp(string timeleft)
    {
        popUp.GetComponent<Canvas>().enabled = true;
        PlaySound(soundPopup, 0.3f);
        popUp.transform.Find("TransparentScreen").transform.Find("topText").GetComponent<Text>().text = "Nur noch " + timeleft + "!";
        yield return new WaitForSeconds(3 * Time.timeScale);
        popUp.GetComponent<Canvas>().enabled = false;
    }

    public void EndScene()
    {
        levelEnded = true;
        BuildPauseScreen("endWait");
        globalTimer.SetEndTime();
        player1Script.CalculateLogData(endingCondition);
        player2Script.CalculateLogData(endingCondition);
        ExportData exportData = (ExportData)FindObjectOfType(typeof(ExportData));
        exportData.ExportAllData();
    }

    public void PlaySound(AudioClip ac, float volume)
    {
        float lastTimeScale = Time.timeScale;
        Time.timeScale = 1f;
        audioSource.PlayOneShot(ac, volume);
        Time.timeScale = lastTimeScale;

    }

    public void PlaySound(string file, float volume)
    {
        float lastTimeScale = Time.timeScale;
        Time.timeScale = 1f;
        switch (file)
        {
            case "soundSlap":
                audioSource.PlayOneShot(soundSlap, volume);
                break;
            case "soundPlop":
                audioSource.PlayOneShot(soundPlop, volume);
                break;
            case "ball_hit":
                audioSource.PlayOneShot(soundBallHit, volume);
                break;
                
        }

        Time.timeScale = lastTimeScale;

    }



}
