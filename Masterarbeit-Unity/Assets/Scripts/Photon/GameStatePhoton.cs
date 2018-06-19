using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameStatePhoton : MonoBehaviour
{
	public string gameType;
	public int maximumBalls;
	public int goalLimit;
	public float timeLeft;
	public float goalFreezeTime;
	private List<BallPhoton> ballList = new List<BallPhoton> ();
	bool maximumBallsReached = false;
	public int goalsTeam1 = 0;
	public int goalsTeam2 = 0;
	public bool gamePaused;

	private GameObject gui;
	private Text scoreTeam1;
	private Text scoreTeam2;
	private Text ratingTeam1;
	private Text ratingTeam2;
	private Text vpnTeam1;
	private Text vpnTeam2;
	private Text timer;
	private GameObject pauseScreenGO;
	private Canvas pauseScreen;
	private GameObject transparentScreen;
	private GameObject popUp;
	private Canvas player1Box;
	private Canvas player2Box;
	private Image greenCheckP1;
	private Image greenCheckP2;
	private GameObject startScreenRatingInfos;
	private Text startScreenRatingp1;
	private Text startScreenRatingp2;
	private GameObject startScreenRatingBoxP1;
	private GameObject startScreenRatingBoxP2;

	private bool player1Ready;
	private bool player2Ready;
	private bool playerHelp;

	public bool levelEnded;
	public bool nextLevelReady;
	public int timeUntilNextLevel;
	public int depauseCountdown;
	private bool depauseCountdownStarted;
	private bool timerBlink;
	public bool popUp120Showed;
	public bool popUp60Showed;
	private Text topText;
	private Text middleText;
	private Text observerText;
	private Text helpText;
	public bool startCountdownActivated;
	//regelt, ob der Startbildschirm mit dem Countdown angezeigt werden soll

	private string endingCondition;

	//playerinformation
	public GameObject player1;
	public GameObject player2;
	public GameObject activePlayer; 
	public PlayerPhoton player1Script;
	public PlayerPhoton player2Script;
	private PlayerLogging playerLoggingP1;
	private PlayerLogging playerLoggingP2;
	public int player1VP;
	public int player2VP;
	public int player1Rating;
	public int player2Rating;

	//Endergebnis
	public int winsP1;
	public int winsP2;

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
	private AudioClip soundPopup;

	public AudioSource musicPlayer;

	public bool allPlayersLoggedIn;
	public bool gameStarted;
	private int helpOrderedBy;

	//networking
	PhotonView photonView;
	public GameObject spawnPosition1;
	public GameObject spawnPosition2;

	private void Awake ()
	{
		photonView = gameObject.GetComponent<PhotonView> ();
		PhotonNetwork.automaticallySyncScene = true;
		Cursor.visible = false;
		SetUpAudio ();
		SetUpGUI ();
		gameType = PlayerPrefs.GetString ("GameType");

	}

	// Use this for initialization
	void Start ()
	{
		winsP1= PlayerPrefs.GetInt ("WinsP1");
		winsP2= PlayerPrefs.GetInt ("WinsP2");
		InstantiatePlayers ();

		timeLeft += 0.05f;

		globalTimer = (GlobalTimer)FindObjectOfType (typeof(GlobalTimer));

		SetGoalCount ("Team1");
		SetGoalCount ("Team2");

		if (startCountdownActivated) {
			musicPlayer.Pause ();
			SetGamePaused (true, "start");
		//	photonView.RPC("SetGamePaused", PhotonTargets.All, true, "start");
		}
		PlayerPrefs.SetString("LastOnline", SceneManager.GetActiveScene().name);

	}

	// Update is called once per frame
	void Update ()
	{
		CheckTimer ();
		CheckPause ();
	}

	public void SetUpGUI(){
		gui = GameObject.FindGameObjectWithTag ("GUI");

		popUp = gui.transform.Find ("PopUp").gameObject;
		pauseScreenGO = gui.transform.Find ("PauseScreen").gameObject;
		pauseScreen = pauseScreenGO.GetComponent<Canvas> ();
		transparentScreen = pauseScreenGO.transform.Find ("TransparentScreen").gameObject;
		topText = transparentScreen.transform.Find ("topText").GetComponent<Text> ();
		middleText = transparentScreen.transform.Find ("middleText").GetComponent<Text> ();
		observerText = transparentScreen.transform.Find ("observerText").GetComponent<Text> ();
		helpText = transparentScreen.transform.Find ("helpText").GetComponent<Text> ();
		player1Box = transparentScreen.transform.Find ("Spieler1").GetComponent<Canvas> ();
		player2Box = transparentScreen.transform.Find ("Spieler2").GetComponent<Canvas> ();
		greenCheckP1 = transparentScreen.transform.Find ("Spieler1").transform.Find ("Spieler1_Check").GetComponent<Image> ();
		greenCheckP2 = transparentScreen.transform.Find ("Spieler2").transform.Find ("Spieler2_Check").GetComponent<Image> ();

		startScreenRatingInfos = transparentScreen.transform.Find ("RatingInfos").gameObject;
		startScreenRatingp1 = startScreenRatingInfos.transform.Find ("RatingSpieler1").transform.Find ("Rating").GetComponent<Text> ();
		startScreenRatingp2 = startScreenRatingInfos.transform.Find ("RatingSpieler2").transform.Find ("Rating").GetComponent<Text> ();
		startScreenRatingBoxP1 = startScreenRatingInfos.transform.Find ("RatingSpieler1").transform.Find ("RatingBox").gameObject;
		startScreenRatingBoxP2 = startScreenRatingInfos.transform.Find ("RatingSpieler2").transform.Find ("RatingBox").gameObject;

		scoreTeam1 = gui.transform.Find ("UI_Spielstand").transform.Find ("Spielstand Team 1").transform.Find ("Score Team 1").GetComponent<Text> ();
		scoreTeam2 = gui.transform.Find ("UI_Spielstand").transform.Find ("Spielstand Team 2").transform.Find ("Score Team 2").GetComponent<Text> ();
		timer = gui.transform.Find ("UI_Spielstand").transform.Find ("Timer_Background").transform.Find ("Time").GetComponent<Text> ();

		ratingTeam1 = gui.transform.Find ("PlayerInformation").transform.Find ("BackgroundRating1").transform.Find ("Rating Spieler 1").transform.Find ("Rating").GetComponent<Text> ();
		ratingTeam2 = gui.transform.Find ("PlayerInformation").transform.Find ("BackgroundRating2").transform.Find ("Rating Spieler 2").transform.Find ("Rating").GetComponent<Text> ();
		vpnTeam1 = gui.transform.Find ("PlayerInformation").transform.Find ("VP Spieler 1").transform.Find ("VP1").transform.Find ("VPN").GetComponent<Text> ();
		vpnTeam2 = gui.transform.Find ("PlayerInformation").transform.Find ("VP Spieler 2").transform.Find ("VP2").transform.Find ("VPN").GetComponent<Text> ();

		if (gameType.Equals ("Online")) {
			gui.transform.Find ("PlayerInformation").GetComponent<Canvas> ().enabled = true;
		}


		//sorgt dafür, dass die PopUps bei 2 und 1 Minute nicht gezeigt werden, wenn das Level mit weniger als 2 bzw 1 Minute startet
		if (timeLeft <= 121) {
			popUp120Showed = true;
		}
		if (timeLeft <= 60) {
			popUp60Showed = true;
		}
	}

	public void SetUpAudio(){
		audioSource = GetComponent<AudioSource> ();
		musicPlayer = GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ();
		soundCountdownRegular = Resources.Load<AudioClip> ("Sounds/countdown_regular");
		soundCountdownEnd = Resources.Load<AudioClip> ("Sounds/countdown_ending");
		soundGoalHorn = Resources.Load<AudioClip> ("Sounds/goal_horn");
		soundSlap = Resources.Load<AudioClip> ("Sounds/slap");
		soundPlop = Resources.Load<AudioClip> ("Sounds/plop");
		soundBallHit = Resources.Load<AudioClip> ("Sounds/ball_hit");
		soundWhistle = Resources.Load<AudioClip> ("Sounds/whistle");
		soundPopup = Resources.Load<AudioClip> ("Sounds/popup");
	}

	public void InstantiatePlayers(){
		if (PlayerPrefs.GetInt ("VP") % 2 == 1) {
			PhotonNetwork.Instantiate ("PlayerPhoton", spawnPosition1.transform.position, spawnPosition1.transform.rotation, 0);

		} else if (PlayerPrefs.GetInt ("VP") % 2 == 0) {
			PhotonNetwork.Instantiate ("PlayerPhoton", spawnPosition2.transform.position, spawnPosition2.transform.rotation, 0);
		}
	
	}

	//Über diese Methode werden neue Bälle an die ballList übergeben
	public void RegisterBallList (BallPhoton ball)
	{
		ballList.Add (ball);
	}

	//Die Methode liefert als return Wert die aktuelle ballList zurück
	public List<BallPhoton> GetBalllist ()
	{
		return ballList;
	}

	//Die Methode löscht den Ball mit der entsprechenden ID aus der Liste
	public void RemoveBall (int ID)
	{
		for (int i = 0; i < ballList.Count; i++) {
			if (ballList [i].GetBallID () == ID) {
				ballList.RemoveAt (i);
			}
		}
	}

	//Die Methode überprüft, ob das Maximum der zugelassenen Bälle erreicht wurde und liefert das Ergebnis als bool zurück
	public bool MaximumBallsReached ()
	{
		if (ballList.Count < maximumBalls) {
			maximumBallsReached = false;
		} else {
			maximumBallsReached = true;
		}

		return maximumBallsReached;
	}

	//Wenn ein Ball mit dem entsprechendem Goal-Collider in Berührung kommt, wird dem anderen Team ein Tor zugeschrieben.
	public void GoalScored (string goal, int scoredByTeamNr)
	{
		PlaySound (soundGoalHorn, 0.3f);
		if (goal.Equals ("Goal1")) {
			goalsTeam2++;
			playerLoggingP1.AdjustResult ("goalConceded");
			playerLoggingP2.AdjustResult ("goalScored");
			SetGoalCount ("Team2");
			//Logging
			if (scoredByTeamNr == 1) {
				playerLoggingP1.AddGoalType ("owngoal");
			} else if (scoredByTeamNr == 2) {
				playerLoggingP2.AddGoalType ("goal");
			}
		} else if (goal.Equals ("Goal2")) {
			goalsTeam1++;
			playerLoggingP2.AdjustResult ("goalConceded");
			playerLoggingP1.AdjustResult ("goalScored");
			SetGoalCount ("Team1");

			//Logging
			if (scoredByTeamNr == 1) {
				playerLoggingP1.AddGoalType ("goal");
			} else if (scoredByTeamNr == 2) {
				playerLoggingP2.AddGoalType ("owngoal");
			}
		}
		playerLoggingP1.CheckResult ();
		playerLoggingP2.CheckResult ();
		CheckGoalLimit ();

	}

	//Hiermit kann die Anzeige für die Tore bearbeitet werden
	private void SetGoalCount (string s)
	{
		if (s.Equals ("Team1")) {
			scoreTeam1.text = goalsTeam1.ToString ();
		} else if (s.Equals ("Team2")) {
			scoreTeam2.text = goalsTeam2.ToString ();
		}

	}

	//Hiermit kann die Anzeige für die Spielerinfos bearbeitet werden
	private void SetGUIPlayerInformation (int playerTeam)
	{

		switch (playerTeam) {
		case 1: 
		//	vpnTeam1.text = "VP: " + player1VP.ToString();
			ratingTeam1.text = player1Rating.ToString ();
			startScreenRatingp1.text = player1Rating.ToString ();
			float boxSize1 = ((float)425 / (float)3000 * (float)player1Rating);
			startScreenRatingBoxP1.transform.localScale = new Vector3 (1, boxSize1, 1); 
			PlayerPrefs.SetInt ("TempRatingP1", player1Rating);
			break;
		case 2: 
		//	vpnTeam2.text = "VP: " + player2VP.ToString();
			ratingTeam2.text = player2Rating.ToString();
			startScreenRatingp2.text = player2Rating.ToString();
			float boxSize2 = ((float)425 / (float)3000 * (float)player2Rating);
			startScreenRatingBoxP2.transform.localScale = new Vector3 (1, boxSize2, 1); 
			PlayerPrefs.SetInt ("TempRatingP2",player2Rating);
			break;
		}

	}

	private void CheckTimer ()
	{
		timeLeft -= Time.deltaTime;
		timer.text = Mathf.RoundToInt (timeLeft).ToString ();

		if (timeLeft <= 120 && !popUp120Showed) {
			StartCoroutine (ShowPopUp ("2 Minuten"));
			popUp120Showed = true;
		}
		if (timeLeft <= 60 && !popUp60Showed) {
			StartCoroutine (ShowPopUp ("1 Minute"));
			popUp60Showed = true;
		}
		if (timeLeft <= 10) {
			if (!timerBlink) {
				StartCoroutine (TimerBlinkEffect ());
				timerBlink = true;
			}
		}
		if (timeLeft <= 0) {
			if (!levelEnded) {

				PlaySound (soundWhistle, 0.4f);
				endingCondition = "Time";
				//photonView.RPC("SetGamePaused", PhotonTargets.All, true, "end");
				SetGamePaused(true,"end");

			}
		}
	}

	//Blinkeffekt des Stuns
	IEnumerator TimerBlinkEffect ()
	{
		int blinkAmount = 10;      //und die Anzahl der Blinkeffekte ermittelt. Die Anzahl ergibt sich aus der Zeit, dividiert durch die Dauer des Blinkeffektes / 2.

		for (int i = 0; i < blinkAmount; i++) {   //solange die Anzahl der Blinkeffekte nicht erreicht wurde
			PlaySound (soundCountdownRegular, 0.5f);
			timer.color = Color.red;     //wird der Renderer im Wechsel weiß und daraufhin in der ursprünglichen Farbe des Spielers eingefärbt
			yield return new WaitForSeconds (0.5f);
			timer.color = Color.black;
			yield return new WaitForSeconds (0.5f);
			audioSource.Stop ();
		}
	}

	private void CheckGoalLimit ()
	{
		if (goalsTeam1 == goalLimit || goalsTeam2 == goalLimit) {
			endingCondition = "Goals";
			//photonView.RPC("SetGamePaused", PhotonTargets.All, true, "end");
			SetGamePaused(true, "end");

		} else {
			StartCoroutine (GoalFreeze ());
		}
	}

	private void CheckPause ()
	{

		//sofern das Spiel pausiert wird
		if (gamePaused && !depauseCountdownStarted) {

				if (playerHelp) {
				observerText.enabled = true;
				observerText.text = "Hilfe angefordert von Spieler " + helpOrderedBy+ "! \nGib dem Versuchsleiter Bescheid, wenn du mit ihm im selben Raum sitzt.";
				observerText.color = Color.red;
			}
			if (!playerHelp) {
					observerText.text = "Ihr könnt das Spiel fortsetzen! :)";
					observerText.color = Color.green;
				}

			if (!levelEnded) {


				if (!playerHelp) {

					if (player1Ready && player2Ready) {
						if (!depauseCountdownStarted) {
							StartCoroutine (StartDepauseCountdown (depauseCountdown));
							depauseCountdownStarted = true;
						}
					}
				}
			} else if (levelEnded && nextLevelReady) {
				if (!playerHelp) {

					if (player1Ready && player2Ready) {
							
						if (gameType.Equals ("Online")) {
								
							if (PhotonNetwork.isMasterClient) {
							PhotonNetwork.LoadLevel (SceneManager.GetActiveScene ().buildIndex + 1);

							}
						}
					}

					
				}
			}
		}

	}

	public bool GetGamePaused ()
	{
		return gamePaused;
	}

	[PunRPC]
	public void SetGamePaused (bool b, string screenType)
	{
		gamePaused = b;
		if (gamePaused) {
			Time.timeScale = 0.0001f;
			musicPlayer.Pause ();
			switch (screenType) {
			case "pause":
				BuildPauseScreen ("pause");
				break;
			case "start":
				BuildPauseScreen ("start");
				break;
			case "end":
				EndScene ();
				StartCoroutine (SetNextLevelReady ());
				break;
			}
		} else {
			Time.timeScale = 1;
			pauseScreen.enabled = false;
			musicPlayer.UnPause ();

		}
	}

	IEnumerator GoalFreeze ()
	{
		Time.timeScale = 0.1f;
		yield return new WaitForSeconds (goalFreezeTime * Time.timeScale);
		Time.timeScale = 1;
	}

	IEnumerator StartDepauseCountdown (int countdown)
	{
		
		yield return new WaitForSeconds (1 * Time.timeScale);
		BuildPauseScreen ("countdown");
		for (int i = countdown; i > 0; i--) {
			topText.text = i.ToString ();
			PlaySound (soundCountdownRegular, 0.5f);
			yield return new WaitForSeconds (1 * Time.timeScale);
		}

		if (!gameStarted) {
			musicPlayer.Play ();
			if (player1.GetComponent<PhotonView> ().isMine) {	//für den aktiven Spieler werden die Informationen des anderen Spielers gesetzt
				player1.GetComponent<PhotonView> ().RPC ("SetPlayerInformation", PhotonTargets.All, player2.name, 1);
				activePlayer = player1;
				//player1.GetComponent<PlayerPhoton> ().SetPlayerInformation (player2.name, 1);
			}
			
			playerLoggingP1 = player1.GetComponent<PlayerLogging> ();
			player1Script = player1.GetComponent<PlayerPhoton> ();
			if (player2.GetComponent<PhotonView> ().isMine) {	//für den aktiven Spieler werden die Informationen des anderen Spielers gesetzt
				player2.GetComponent<PhotonView> ().RPC ("SetPlayerInformation", PhotonTargets.All, player1.name, 2);
				activePlayer = player2;
				//player2.GetComponent<PlayerPhoton> ().SetPlayerInformation (player1.name, 2);
			}

			playerLoggingP2 = player2.GetComponent<PlayerLogging> ();
			player2Script = player2.GetComponent<PlayerPhoton> ();
			gameStarted = true;
		}
		PlaySound (soundCountdownEnd, 0.5f);
		photonView.RPC("SetGamePaused", PhotonTargets.All, false, "pause");
		//SetGamePaused(false, "pause");
		SetPlayerReady (false, 1);
		SetPlayerReady (false, 2);
		pauseScreen.enabled = false;
		depauseCountdownStarted = false;

		playerLoggingP1.CheckResult ();  //Die Playerloggings bekommen das Result zum Start mitgeteilt
		playerLoggingP2.CheckResult ();

	}

	IEnumerator SetNextLevelReady ()
	{
		yield return new WaitForSeconds (timeUntilNextLevel / 2 * Time.timeScale);
		BuildLoggingData ();
		yield return new WaitForSeconds (timeUntilNextLevel / 2 * Time.timeScale);
		BuildPauseScreen ("endLevelReady");
		nextLevelReady = true;
	}

	public void BuildPauseScreen (string screenType)
	{
		pauseScreen.enabled = true;
		observerText.enabled = false;
		Color col = transparentScreen.GetComponent<Image> ().color;

		switch (screenType) {

		case "pause":
			startScreenRatingInfos.SetActive (false);
			helpText.enabled = true;
			transparentScreen.GetComponent<Image> ().color = new Color (col.r, col.g, col.b, 0.95f);
			player1Box.enabled = true;
			player2Box.enabled = true;
			topText.text = "Pause";
			topText.fontSize = 100;
			middleText.text = "Drücke A zum Fortsetzen!";
			break;
		case "start":
			startScreenRatingInfos.SetActive (true);
			helpText.enabled = true;
			transparentScreen.GetComponent<Image> ().color = new Color (col.r, col.g, col.b, 0.95f);
			player1Box.enabled = true;
			player2Box.enabled = true;
			topText.text = "Mach dich bereit für " + SceneManager.GetActiveScene ().name + "!";
			topText.fontSize = 70;
			middleText.text = "Drücke A zum Starten!";
			break;
		case "countdown":
			startScreenRatingInfos.SetActive (false);
			helpText.enabled = false;
			transparentScreen.GetComponent<Image> ().color = new Color (col.r, col.g, col.b, 0.8f);
			player1Box.enabled = false;
			player2Box.enabled = false;
			topText.text = "";
			topText.fontSize = 200;
			middleText.text = "";
			break;
		case "endWait":
			startScreenRatingInfos.SetActive (false);
			helpText.enabled = true;
			transparentScreen.GetComponent<Image> ().color = new Color (col.r, col.g, col.b, 0.95f);
			player1Box.enabled = false;
			player2Box.enabled = false;

			if (goalsTeam1 > goalsTeam2) {
				topText.text = "Spieler 1 gewinnt mit " + goalsTeam1 + " - " + goalsTeam2 + "!";
			} else if (goalsTeam1 < goalsTeam2) {
				topText.text = "Spieler 1 gewinnt mit " + goalsTeam2 + " - " + goalsTeam1 + "!";
			} else {
				topText.text = "Das Spiel endet " + goalsTeam2 + " - " + goalsTeam1 + " unentschieden!";
			}
			topText.fontSize = 70;
			middleText.text = "";
			break;
		case "endLevelReady":
			startScreenRatingInfos.SetActive (false);
			helpText.enabled = true;
			transparentScreen.GetComponent<Image> ().color = new Color (col.r, col.g, col.b, 0.95f);
			player1Box.enabled = true;
			player2Box.enabled = true;

			if (goalsTeam1 > goalsTeam2) {
				topText.text = "Spieler 1 gewinnt mit " + goalsTeam1 + " - " + goalsTeam2 + "!";
			} else if (goalsTeam1 < goalsTeam2) {
				topText.text = "Spieler 1 gewinnt mit " + goalsTeam2 + " - " + goalsTeam1 + "!";
			} else {
				topText.text = "Das Spiel endet " + goalsTeam2 + " - " + goalsTeam1 + " unentschieden!";
			}
			topText.fontSize = 70;
			middleText.text = "Drücke A zum Fortfahren!";
			break;
		}
	}

	IEnumerator ShowPopUp (string timeleft)
	{
		popUp.GetComponent<Canvas> ().enabled = true;
		PlaySound (soundPopup, 0.2f);
		popUp.transform.Find ("TransparentScreen").transform.Find ("topText").GetComponent<Text> ().text = "Nur noch " + timeleft + "!";
		yield return new WaitForSeconds (3 * Time.timeScale);
		popUp.GetComponent<Canvas> ().enabled = false;
	}

	public void EndScene ()
	{
		SetFinalResult ();
		levelEnded = true;
		BuildPauseScreen ("endWait");
		globalTimer.SetEndTime ();
	}

	private void BuildLoggingData(){
		activePlayer.GetComponent<PlayerPhoton> ().CalculateLogData (endingCondition, gameType);
		//	player1Script.CalculateLogData (endingCondition, gameType);
		//	player2Script.CalculateLogData (endingCondition, gameType);
		ExportData exportData = (ExportData)FindObjectOfType (typeof(ExportData));
		exportData.StartUpExportData ();
		//exportData.FindPlayerLogging ();
		exportData.ExportAllData (activePlayer.GetComponent<PlayerLogging>());

	}

	public void PlaySound (AudioClip ac, float volume)
	{
		float lastTimeScale = Time.timeScale;
		Time.timeScale = 1f;
		audioSource.PlayOneShot (ac, volume);
		Time.timeScale = lastTimeScale;
	}

	public void PlaySound (string file, float volume)
	{
		float lastTimeScale = Time.timeScale;
		Time.timeScale = 1f;
		switch (file) {
		case "soundSlap":
			audioSource.PlayOneShot (soundSlap, volume);
			break;
		case "soundPlop":
			audioSource.PlayOneShot (soundPlop, volume);
			break;
		case "ball_hit":
			audioSource.PlayOneShot (soundBallHit, volume);
			break;

		}

		Time.timeScale = lastTimeScale;

	}

	[PunRPC]
	public void SetPlayerHelp (bool b, int vp)
	{
		playerHelp = b;
		helpOrderedBy = vp;
	}


	[PunRPC]
	public void SetPlayerReady (bool b, int playerNr)
	{
		if (!playerHelp) {
			if (playerNr == 1) {
				player1Ready = b;
				greenCheckP1.enabled = b;
			} else if (playerNr == 2) {
				player2Ready = b;
				greenCheckP2.enabled = b;
			}
		}
	}

	[PunRPC]
	public void RegisterPlayer(string name, int playerTeam, int subjNr, int ratin){
		if (playerTeam == 1) {
			player1 = GameObject.Find(name);
			player1VP = subjNr;
			player1Rating = ratin;

		} else if (playerTeam == 2) {
			player2 = GameObject.Find(name);
			player2VP = subjNr;
			player2Rating = ratin;
			}
		SetGUIPlayerInformation (playerTeam);
	}

	void SetFinalResult(){
		if (goalsTeam1 == goalLimit){
			winsP1++;
			PlayerPrefs.SetInt ("WinsP1", winsP1); 
		} else if (goalsTeam2 == goalLimit){
			winsP2++;
			PlayerPrefs.SetInt ("WinsP2", winsP2); 
		}
			
	}

	/*private GameObject FindActivePlayer(){
		GameObject[] playerList = GameObject.FindGameObjectsWithTag ("Player");
		foreach(GameObject go in playerList) {
			if (go.GetComponent<PhotonView> ().isMine) {
				return go;
			}
		}
	}
	*/



}
