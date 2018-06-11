using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TutorialGameState : MonoBehaviour
{
    public string gameType;
    public int maximumBalls;
    public int goalLimit;
    public float timeLeft;
    public float timePlayed;
    public float goalFreezeTime;
    private List<BallTutorial> ballList = new List<BallTutorial>();
    bool maximumBallsReached = false;
    public int goalsTeam1 = 0;
    public int goalsTeam2 = 0;
    public bool gamePaused;

    public GameObject gui;
    public GameObject startScreen;
    private GameObject pauseScreenGO;
    private Canvas pauseScreen;
    private GameObject transparentScreen;
    private Canvas player1Box;
    private Image greenCheckP1;
    private Text timer;

    private bool player1Ready;

    public bool levelEnded;
    public bool nextLevelReady;
    public int timeUntilNextLevel;
    public int timeUntilTutorialStart;
    public bool tutorialReady;
    public int depauseCountdown;
    private bool depauseCountdownStarted;
    private bool timerBlink;
    private Text topText;
    private Text middleText;
    private Text bottomText;
    //private Text helpText;
    public bool startCountdownActivated;    //regelt, ob der Startbildschirm mit dem Countdown angezeigt werden soll

    public int rating;
    public int vpNummer;

    public GlobalTimer globalTimer;
    //Audios
    private AudioSource audioSource;
    private AudioClip soundCountdownRegular;
    private AudioClip soundCountdownEnd;
    private AudioClip soundGoalHorn;
    private AudioClip soundSlap;
    private AudioClip soundPlop;
    private AudioClip soundBallHit;
    private AudioClip soundWhistle;
    public AudioSource musicPlayer;

    public GameObject player1;
    public string tutorialFinishedText1;
    public string tutorialFinishedText2;
	public string pauseScreenText;
    public GameObject videoPlayer;

    public bool inverseTime;
    public bool showRatingChange;

    public string challengeType;

    private void Awake()
    {
        gameType = PlayerPrefs.GetString("GameType");
    }

    // Use this for initialization
    void Start()
    {
        timeLeft += 0.02f;

        vpNummer = PlayerPrefs.GetInt("VP");
        rating = 0;

		timer = gui.transform.Find("UI_Spielstand").transform.Find("Timer_BackgroundDark").transform.Find("Time").GetComponent<Text>();

        pauseScreenGO = gui.transform.Find("PauseScreen").gameObject;
        pauseScreen = pauseScreenGO.GetComponent<Canvas>();
        transparentScreen = pauseScreenGO.transform.Find("TransparentScreen").gameObject;
        topText = transparentScreen.transform.Find("topText").GetComponent<Text>();
        middleText = transparentScreen.transform.Find("middleText").GetComponent<Text>();
        bottomText = transparentScreen.transform.Find("bottomText").GetComponent<Text>();
        //helpText = transparentScreen.transform.Find("helpText").GetComponent<Text>();
        player1Box = transparentScreen.transform.Find("Spieler1").GetComponent<Canvas>();
        greenCheckP1 = transparentScreen.transform.Find("Spieler1").transform.Find("Spieler1_Check").GetComponent<Image>();
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();


        globalTimer = (GlobalTimer)FindObjectOfType(typeof(GlobalTimer));
        //player1Script = player1.GetComponent<PlayerTutorial>();

        audioSource = GetComponent<AudioSource>();
        soundCountdownRegular = Resources.Load<AudioClip>("Sounds/countdown_regular");
        soundCountdownEnd = Resources.Load<AudioClip>("Sounds/countdown_ending");
        soundGoalHorn = Resources.Load<AudioClip>("Sounds/goal_horn");
        soundSlap = Resources.Load<AudioClip>("Sounds/slap");
        soundPlop = Resources.Load<AudioClip>("Sounds/plop");
        soundBallHit = Resources.Load<AudioClip>("Sounds/ball_hit");
        soundWhistle = Resources.Load<AudioClip>("Sounds/whistle");

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
    public void RegisterBallList(BallTutorial ball)
    {
        ballList.Add(ball);
    }

    //Die Methode liefert als return Wert die aktuelle ballList zurück
    public List<BallTutorial> GetBalllist()
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
            //  SetGoalCount("Team2");
            //Logging
            if (scoredByTeamNr == 1)
            {
            }
            else if (scoredByTeamNr == 2)
            {
            }
        }
        else if (goal.Equals("Goal2"))
        {
            goalsTeam1++;
            // SetGoalCount("Team1");

            //Logging
            if (scoredByTeamNr == 1)
            {
            }
            else if (scoredByTeamNr == 2)
            {
            }
        }
        CheckGoalLimit();

    }


    private void CheckTimer()
    {
        timePlayed = globalTimer.playTime;

        timeLeft -= Time.deltaTime;

        if (!inverseTime)
        {
            timer.text = Mathf.RoundToInt(timeLeft).ToString();


            if (timeLeft <= 10)
            {
                if (!timerBlink)
                {
                    StartCoroutine(TimerBlinkEffect());
                    timerBlink = true;
                }
            }
        } 
         if (timeLeft <= 0)
        {
            if (!levelEnded)
            {
                PlaySound(soundWhistle, 0.4f);
                SetGamePaused(true, "end");
            }
        }

         if (inverseTime)
        {
            
            timer.text = Mathf.RoundToInt(timePlayed).ToString();

        }
    }

    private void StartTimer()
    {

    }

    //Blinkeffekt des Stuns
    IEnumerator TimerBlinkEffect()
    {
        int blinkAmount = 10;      //und die Anzahl der Blinkeffekte ermittelt. Die Anzahl ergibt sich aus der Zeit, dividiert durch die Dauer des Blinkeffektes / 2.

        for (int i = 0; i < blinkAmount; i++)   //solange die Anzahl der Blinkeffekte nicht erreicht wurde
        {
            PlaySound(soundCountdownRegular, 0.5f);   
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
        if (gamePaused && !depauseCountdownStarted)
        {
            if (!levelEnded)
            {
                if (tutorialReady)
                {
                    //überprüfe, ob die einzelnen Spieler bereit sind
                    if (Input.GetButtonUp("ShootP1"))
                    {
                        SetPlayerReady(true, 1);
                    }

                    if (player1Ready)
                    {
                        if (!depauseCountdownStarted)
                        {
                            StartCoroutine(StartDepauseCountdown(depauseCountdown));
                            depauseCountdownStarted = true;
                            player1.gameObject.SetActive(true);
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

                if (player1Ready)
                {
                    int newRating = PlayerPrefs.GetInt(vpNummer.ToString() + "Rating") + rating;
                    PlayerPrefs.SetInt(vpNummer.ToString() + "Rating", newRating);
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
        startScreen.GetComponent<Canvas>().enabled = false;
        if (videoPlayer != null)
        {
            videoPlayer.SetActive(false);
        }
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
        pauseScreen.enabled = false;
        depauseCountdownStarted = false;
    }

    IEnumerator SetNextLevelReady()
    {
        yield return new WaitForSeconds(timeUntilNextLevel * Time.timeScale);
        BuildPauseScreen("endLevelReady");
        nextLevelReady = true;
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(timeUntilTutorialStart * Time.timeScale);
        startScreen.transform.Find("ReadyText").GetComponent<Text>().enabled = true;
        tutorialReady = true;

    }


    public void BuildPauseScreen(string screenType)
    {
        // observerText.enabled = false;
        Color col = transparentScreen.GetComponent<Image>().color;

        switch (screenType)
        {
            case "pause":
                //  helpText.enabled = true;
                pauseScreen.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.95f);
                player1Box.enabled = true;
                topText.text = "Pause";
                topText.fontSize = 100;
				middleText.text = pauseScreenText;
				middleText.fontSize = 30;
			                break;
            case "start":
                //   helpText.enabled = true;
                startScreen.transform.Find("ReadyText").GetComponent<Text>().enabled = false;
                startScreen.GetComponent<Canvas>().enabled = true;
                StartCoroutine(StartTutorial());
                break;
            case "countdown":
                pauseScreen.enabled = true;

                // helpText.enabled = false;
                transparentScreen.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.8f);
                player1Box.enabled = false;
                topText.text = "";
                topText.fontSize = 200;
                middleText.text = "";

                break;
            case "endWait":
                pauseScreen.enabled = true;

                // helpText.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.95f);
                player1Box.enabled = false;
                topText.fontSize = 80;
				middleText.fontSize = 40;
				topText.text = tutorialFinishedText1;
                middleText.text = tutorialFinishedText2;
                if (showRatingChange)
                {
                    bottomText.text = " Rating: +" + rating;
                }
                break;
            case "endLevelReady":
                pauseScreen.enabled = true;

                //   helpText.enabled = true;
                transparentScreen.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.95f);
                player1Box.enabled = true;
                topText.text = tutorialFinishedText1;
				middleText.fontSize = 40;
				middleText.text = tutorialFinishedText2;
                topText.fontSize = 80;
                if (showRatingChange)
                {
                    bottomText.text = " Rating: +" + rating;
                }

                break;
        }
    }


    public void EndScene()
    {
        levelEnded = true;
        BuildPauseScreen("endWait");
        globalTimer.SetEndTime();
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

	public void EndChallenge(int challengeRating)
    {
        PlaySound(soundWhistle, 0.4f);
        rating = challengeRating;
        SetGamePaused(true, "end");
    }

}
