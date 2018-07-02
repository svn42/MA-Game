using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPhoton : MonoBehaviour
{
	[Range (1, 2)]
	public int playerTeam;
	//Teamzugehörigkeit (1 oder 2)
	private string gameType;

	public int subjectNr;
	public int subjectNrEnemy;
	public int rating;
	public int ratingEnemy;
	public float brakingForce;
	//Stärke des Abbremsens
	public float maxSpeed;
	public float speedX;
	//Geschwindigkeit auf der X-Achse
	public float speedY;
	//Geschwindigkeit auf der Y-Achse
	public float acceleration;
	//Beschleunigungsvariable
	private Vector3 movementVector;
	//Bewegungsvektor

	public bool stunned;
	//Wenn der Spieler betäubt wurde, wird die Variable true
	public float stunBlinkEffect;
	//Zeitliches Intervall (in Sekunden), in dem das Blinken beim Stun stattfindet

	public GameObject exhaustPrefab;
	//das Prefab des Abgaspartikels wird über den Inspector bekannt gemacht
	public GameObject exSpawner;
	// der Spawner für die Abgaspartikel wird ebenfalls über den Inspektor bekannt gemacht
	public float exhaustTime;
	//Die Zeit der aktuellen Bewegung in Frames. wird erhöht, sofern sich der Spieler bewegt und dient der Überprüfung, ob ein ABgaspartikel gespawnt werden soll
	public float exhaustSpawnTime;
	//Die Zeit, die erreicht werden muss, bis ein Abgaspartikel gespawnt werden kann

	public BlockSpawnerPhoton blockSpawn;
	//Der Blockspawner des Spielers wird über den Inspector mit dem Spieler verknüpft
	public ShotSpawnerPhoton shotSpawn;
	//Der Shotspawner des Spielers wird über den Inspector mit dem Spieler verknüpft
	public float shotDelay;
	//der shotDelay gibt die Zeit in Sekunden wieder, die nach einem Schuss vergehen muss, damit ein neuer Schuss gespawnt werden kann. (damit der normalShot nicht gespammt werden kann)
	public float shotTimer;
	//der shotTimer gibt die Zeit in Frames wieder, die bereits auf den nächsten Schuss gewartet wurde.

	public GameObject enemyPlayer;
	public PlayerLogging playerLogging;
	//das eigene PlayerLogging
	public PlayerLogging playerLoggingEnemy;
	private PositionTracker positionTracker;
	private GameStatePhoton gameState;
	public Color teamColor;
	public Vector3 colorVector;
	//Die Farbe des Spielers, die anhand der Teamzugehörigkeit ermittelt wird

	public GameObject speechBubblePrefab;
	private GameObject speechBubble;
	private SpriteRenderer speechbubbleRenderer;
	private SpriteRenderer emojiRenderer;
	public float emoteTimer;
	public float emoteDelay;
	public float emoteDisplayTime;

	public bool emoteNicePrepared;
	public bool emoteHahaPrepared;
	public bool emoteAngryPrepared;
	public bool emoteCryPrepared;
	public string preparedEmojiType;

	//Audios
	private AudioSource audioSource;
	private AudioClip soundBoing;
	public List<AudioClip> soundsEmoteNice = new List<AudioClip> ();
	public List<AudioClip> soundsEmoteCry = new List<AudioClip> ();
	public List<AudioClip> soundsEmoteHaha = new List<AudioClip> ();
	public List<AudioClip> soundsEmoteAngry = new List<AudioClip> ();

	//trigger variablen
	private bool playersRegistered;

	//networking
	PhotonView pvGamestate;
	PhotonView pvPlayer;

	// Use this for initialization
	void Start ()
	{
		gameState = (GameStatePhoton)FindObjectOfType (typeof(GameStatePhoton));
		gameType = gameState.gameType;

		pvGamestate = gameState.gameObject.GetComponent<PhotonView> ();
	}

	public void SetUpEmotes ()
	{
		emoteTimer = emoteDelay; //sorgt dafür, dass sofort ein Emote benutzt werden kann
		speechbubbleRenderer = speechBubble.GetComponent<SpriteRenderer> ();
		emojiRenderer = speechBubble.transform.Find ("Emoji").GetComponent<SpriteRenderer> ();
		speechbubbleRenderer.enabled = false;
		emojiRenderer.enabled = false;

		AudioClip a;
		for (int i = 1; i < 5; i++) {
			a = Resources.Load<AudioClip> ("Sounds/Emotes/emote_nice_" + i);
			soundsEmoteNice.Add (a);
			a = Resources.Load<AudioClip> ("Sounds/Emotes/emote_cry_" + i);
			soundsEmoteCry.Add (a);
			a = Resources.Load<AudioClip> ("Sounds/Emotes/emote_haha_" + i);
			soundsEmoteHaha.Add (a);
			a = Resources.Load<AudioClip> ("Sounds/Emotes/emote_angry_" + i);
			soundsEmoteAngry.Add (a);
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (!playersRegistered) {
			GameObject[] playerList = GameObject.FindGameObjectsWithTag ("Player");

			if (playerList.Length == 2){
				pvPlayer = gameObject.GetComponent<PhotonView> ();

			if (pvPlayer.isMine) {
				pvPlayer.RPC ("StartPlayerRegistration", PhotonTargets.All);	//VP, Rating, PlayerTeam, Color setzen
				pvPlayer.RPC ("SetUpSpeechBubble",PhotonTargets.All) ;
				pvPlayer.RPC ("SetPlayerName", PhotonTargets.All, subjectNr);
				pvPlayer.RPC ("CheckTeamColor", PhotonTargets.All, playerTeam);
				pvGamestate.RPC ("RegisterPlayer", PhotonTargets.All, gameObject.name, playerTeam, subjectNr, rating);
				playersRegistered = true;
			}

			}

		}

	}

	//Sofern es zu einer Collision kommt
	public void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Boundary") {  //und es sich um eine Bande handelt
			speedX /= 6;    //Wird die Geschwindigkeit reduziert. 
			speedY /= 6;

			switch (coll.gameObject.name) {
			case "Boundary_Top":    //sofern es sich um die obere Bande handelt,
				transform.Translate (new Vector3 (0f, -50, 0f) * Time.deltaTime, Space.World);    //wird der Spieler nach unten "geschubst"
				break;
			case "Boundary_Bottom": //bei der unteren Bande 
				transform.Translate (new Vector3 (0f, 50, 0f) * Time.deltaTime, Space.World); //nach oben
				break;
			case "Boundary_Left":   //bei der linken Bande 
				transform.Translate (new Vector3 (50, 0f, 0f) * Time.deltaTime, Space.World); //nach rechts
				break;
			case "Boundary_Right":  //und bei der rechten Bande 
				transform.Translate (new Vector3 (-50, 0, 0f) * Time.deltaTime, Space.World); //nach links
				break;
			}
		}
		if (coll.gameObject.tag == "Ball") {  //sofern das Objekt den Tag Ball hat
			speedX /= 6;    //wird die Geschwindigkeit reduziert
			speedY /= 6;
			this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
		if (coll.gameObject.tag == "Shot") {
			if (coll.gameObject.GetComponent<ShotPhoton> ().GetPlayerTeam () != playerTeam) { //wenn der Schuss vom gegenerischen Spieler ist
				ShotPhoton collidingShot = coll.gameObject.GetComponent<ShotPhoton> ();
				speedX /= 2;    //wird die Geschwindigkeit reduziert
				speedY /= 2;
				StartCoroutine (StunPlayer (collidingShot.GetStunDuration ()));  //und der Spieler für die Zeit "GetStunDuration" gestunnt
				blockSpawn.ResetBlockChargeTime ();  //das Spawnen des eines Blockes 
				shotSpawn.ResetShotChargeTime ();    //sowie eines Schusses wird unterbrochen
				playerLoggingEnemy.AddEnemyStunned (collidingShot.GetShotType (), collidingShot.GetStunDuration ());
				playerLogging.AddStunnedByEnemy (collidingShot.GetShotType (), collidingShot.GetStunDuration ());
			}
		}

	}


	//setzt die Farve des Spielers sowie seiner Spawner fest
	public void SetColor (Color col)
	{
		teamColor = col;	//die Team Color wird gesetzt
		blockSpawn.GetComponent<BlockSpawnerPhoton> ().SetColor (teamColor);    //ebenso wird die Farbe dem Blockspawner und dem    
	//	shotSpawn.GetComponent<ShotSpawnerPhoton> ().SetColor (teamColor);      //ShotSpawner bekannt gemacht

	}

	//Methode zum Übermitteln der Betäubung an den Spieler. Als Argument wird die Zeit übergeben
	IEnumerator StunPlayer (float time)
	{
		StartCoroutine (StunEffect (time));   //Es wird eine Coroutine für den Blinkeffekt gestartet und die Zeit der Betäubung übergeben.
		stunned = true;                     //die Stun-Variable auf True gesetzt
		StopSound ();
		PlaySound (soundBoing, time / 2);
		yield return new WaitForSeconds (time);  //Die Zeit der Betäubung abgewartet 
		stunned = false;                        //und daraufhin die Stun-Variable auf false gesetzt
	}

	//Blinkeffekt des Stuns
	IEnumerator StunEffect (float time)
	{
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();  //der spriteRenderer Des Spielers wird der lokalen Variable zugewiesen
		int blinkAmount = (int)Mathf.Floor (time / stunBlinkEffect);      //und die Anzahl der Blinkeffekte ermittelt. Die Anzahl ergibt sich aus der Zeit, dividiert durch die Dauer des Blinkeffektes / 2.

		for (float i = 0; i < blinkAmount; i++) {   //solange die Anzahl der Blinkeffekte nicht erreicht wurde
			spriteRenderer.color = Color.white;     //wird der Renderer im Wechsel weiß und daraufhin in der ursprünglichen Farbe des Spielers eingefärbt
			yield return new WaitForSeconds (stunBlinkEffect / 2);
			spriteRenderer.color = teamColor;
			yield return new WaitForSeconds (stunBlinkEffect / 2);

		}
	}

	//Methode, um den ShotTimer von außerhalb zu setzen. (Geschieht in der ShotSpawner-Klasse nach dem Schießen)
	public void SetShotTimer (int i)
	{
		shotTimer = i;
	}

	[PunRPC]
	public void StartDisplayEmoteCoroutine(string type){
		SetEmotePrepared (type, false);
		StartCoroutine (DisplayEmote(type));
	}

	IEnumerator DisplayPreparedEmote(){

		emojiRenderer.color = new Color (1, 1, 1, 0.60f);
		speechbubbleRenderer.color = new Color (1, 1, 1, 0.40f);
		emojiRenderer.sprite = Resources.Load<Sprite> ("Textures/Emojis/" + preparedEmojiType);
		speechbubbleRenderer.enabled = true;
		emojiRenderer.enabled = true;
		yield return new WaitForSeconds (emoteDisplayTime);
		SetEmotePrepared (preparedEmojiType, false);
	
	}

	public void SetEmotePrepared(string type, bool b){

		emoteNicePrepared = false;
		emoteHahaPrepared = false;
		emoteAngryPrepared = false;
		emoteCryPrepared = false;

		switch (type) {
		case "nice": 
			emoteNicePrepared = b;
			break;
		case "haha": 
			emoteHahaPrepared = b;
			break;
		case "angry": 
			emoteAngryPrepared = b;
			break;
		case "cry": 
			emoteCryPrepared = b;
			break;
		}

		if (!b) {
			speechbubbleRenderer.enabled = false;
			emojiRenderer.enabled = false;
		}

	}

	IEnumerator DisplayEmote (string type)
	{
		int randomInt = Random.Range (0, 3);
		switch (type) {
		case ("nice"):
			PlaySound (soundsEmoteNice [randomInt], 0.1f);
			break;
		case ("haha"):
			PlaySound (soundsEmoteHaha [randomInt], 0.1f);
			break;
		case ("cry"):
			PlaySound (soundsEmoteCry [randomInt], 0.1f);
			break;
		case ("angry"):
			PlaySound (soundsEmoteAngry [randomInt], 0.1f);
			break;
		}
		playerLogging.AddEmote(type);   //dem Logging wird die Art des Emotes mitgeteilt    

		emojiRenderer.color = new Color (1, 1, 1, 0.85f);
		speechbubbleRenderer.color = new Color (1, 1, 1, 0.8f);
		emojiRenderer.sprite = Resources.Load<Sprite> ("Textures/Emojis/" + type);
		speechbubbleRenderer.enabled = true;
		emojiRenderer.enabled = true;
		yield return new WaitForSeconds (emoteDisplayTime);
		speechbubbleRenderer.enabled = false;
		emojiRenderer.enabled = false;
	}

	private void PlaySound (AudioClip ac, float volume)
	{
		float lastTimeScale = Time.timeScale;
		Time.timeScale = 1f;
		audioSource.clip = ac;
		audioSource.volume = volume;
		audioSource.Play ();
		Time.timeScale = lastTimeScale;
	}

	public void StopSound ()
	{
		audioSource.Stop ();
		shotSpawn.StopAudio ();
		blockSpawn.StopAudio ();
	}

	public void CalculateLogData (string endingCondition, string gameType)
	{
		playerLogging.SetSubjectNr (subjectNr, subjectNrEnemy);
		playerLogging.SetRating (rating, ratingEnemy);
		playerLogging.SetGameType (gameType);
		positionTracker.CalculateWalkedDistancePerResult (playerLogging.currentResult);
		positionTracker.CalculateWalkedDistance ();
		positionTracker.ChangeResult(playerLogging.currentResult);
		playerLogging.SetEndingCondition (endingCondition);
	}

	//der aktive Spieler registriert sich beim Gamestate
	[PunRPC]
	public void StartPlayerRegistration ()
	{
		if (pvPlayer != null) {
			if (pvPlayer.isMine) {
				subjectNr = PlayerPrefs.GetInt ("VP");
				rating = PlayerPrefs.GetInt (subjectNr + "Rating");
				if (subjectNr % 2 == 0) {
					playerTeam = 2;
				} else if (subjectNr % 2 == 1) {
					playerTeam = 1;
				}
			}
		}
	}

	public Vector3 ConvertColorToVector(){
		Color c = teamColor;
		Vector3 colVector = new Vector3(c.r, c.g, c.b);
		return colVector;
	}

	//Hier werden die Informationen an den Spieler weitergeben. Wichtig für die "gegnerischen" Spieler, damit diese ihre Daten (Farbe, Assets und co) bekommen.
	[PunRPC]
	public void SetPlayerInformation (string name, int plTeam)
	{
		playerTeam = plTeam;
		colorVector = ConvertColorToVector ();
		//subjectNrEnemy = enemyPlayer.GetComponent<PlayerPhoton> ().subjectNr;
		//ratingEnemy = enemyPlayer.GetComponent<PlayerPhoton> ().rating;

		//Logging
		playerLogging = this.gameObject.GetComponent<PlayerLogging> ();  //der PlayerLogger wird verknüpft
		playerLogging.SetPlayerTeam (playerTeam);                        //dem playerLogging wird mitgeteilt, zu welchem Team sein Spieler gehört

		positionTracker = gameObject.GetComponent<PositionTracker> ();
		positionTracker.StartTracking (); //das Tracken der Position wird gestartet

		blockSpawn.gameObject.GetComponent<PhotonView>().RPC("Setup", PhotonTargets.All);
		shotSpawn.gameObject.GetComponent<PhotonView>().RPC("Setup", PhotonTargets.All);

		//Enemy Information
		enemyPlayer = GameObject.Find(name);
		playerLoggingEnemy = enemyPlayer.GetComponent<PlayerLogging> (); //das playerLogging-Skript des Gegners wird verknüpft, um die Betäubungen abzuspeichern.
		if (playerTeam == 1) {
			subjectNrEnemy = gameState.player2VP;
			ratingEnemy = gameState.player2Rating;

			//sofern das Rating manipuliert wurde, bekommt der Spieler für den gegnerischen Spieler das manipulierte angezeigt
			if (PlayerPrefs.GetInt ("RatingManipulated") != 0) {
				if (pvPlayer.isMine) {
					ratingEnemy = PlayerPrefs.GetInt ("RatingManipulated");
				}
			}
		

		} else if (playerTeam == 2) {
			subjectNrEnemy = gameState.player1VP;
			ratingEnemy = gameState.player1Rating;

			if (PlayerPrefs.GetInt ("RatingManipulated") != 0) {
				if (pvPlayer.isMine) {
					ratingEnemy = PlayerPrefs.GetInt ("RatingManipulated");
				}
			}

			audioSource = GetComponent<AudioSource> ();
			soundBoing = Resources.Load<AudioClip> ("Sounds/boing");

			SetUpEmotes ();
		}
	}

	[PunRPC]
	public void SetUpSpeechBubble ()
	{
		speechBubble = Instantiate (speechBubblePrefab);
		speechBubble.GetComponent<FollowPlayer> ().SetFollowPlayer (this.gameObject);
	}


	[PunRPC]
	public void SetPlayerName (int vpNr){
		gameObject.name = "VP" + vpNr; 
	}

	//die Methode überprüft die Teamzugehörigkeit und ändert die Farbe des Spielers dementsprechend
	[PunRPC]
	private void CheckTeamColor (int teamNr)
	{
		switch (teamNr) {
		case 1: //Team 1 bekommt die rote Farbe
			SetColor (Color.red);
			break;
		case 2: //Team 2 die Blaue
			SetColor (Color.blue);
			break;
		}
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = teamColor;   //Die Farbe wird an das Sprite übergeben
	}





}