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
	//Die Farbe des Spielers, die anhand der Teamzugehörigkeit ermittelt wird

	public GameObject speechBubblePrefab;
	private GameObject speechBubble;
	private SpriteRenderer speechbubbleRenderer;
	private SpriteRenderer emojiRenderer;
	public float emoteTimer;
	public float emoteDelay;
	public float emoteDisplayTime;

	//Audios
	private AudioSource audioSource;
	private AudioClip soundBoing;
	public List<AudioClip> soundsEmoteNice = new List<AudioClip> ();
	public List<AudioClip> soundsEmoteCry = new List<AudioClip> ();
	public List<AudioClip> soundsEmoteHaha = new List<AudioClip> ();
	public List<AudioClip> soundsEmoteAngry = new List<AudioClip> ();

	//networking
	PhotonView pvGamestate;
	PhotonView pvPlayer;

	// Use this for initialization
	void Start ()
	{
		gameState = (GameStatePhoton)FindObjectOfType (typeof(GameStatePhoton));
		gameType = gameState.gameType;

		pvGamestate = gameState.gameObject.GetComponent<PhotonView> ();
		pvPlayer = gameObject.GetComponent<PhotonView> ();
			
		SetPlayerInformation ();	//VP, Rating, PlayerTeam, Color setzen
		SetUpEmotes ();

		//Logging
		playerLogging = this.gameObject.GetComponent<PlayerLogging> ();  //der PlayerLogger wird verknüpft
		playerLogging.SetPlayerTeam (playerTeam);                        //dem playerLogging wird mitgeteilt, zu welchem Team sein Spieler gehört

		positionTracker = gameObject.GetComponent<PositionTracker> ();
		positionTracker.StartTracking (); //das Tracken der Position wird gestartet

		audioSource = GetComponent<AudioSource> ();
		soundBoing = Resources.Load<AudioClip> ("Sounds/boing");

		pvGamestate.RPC ("RegisterPlayer", PhotonTargets.All, gameObject.name, playerTeam);
		Debug.Log ("Register Player");

	}


	public void SetUpEmotes ()
	{
		SetUpSpeechBubble ();

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

		if (!gameState.GetGamePaused ()) {
			if (!stunned) {
				CheckInput ();   //zunächst wird der Input überprüft
				Move (); //dann der Spieler bewegt
				CheckExhaust (); //sowie überprüft, ob das Abgas erzeugt werden soll
			}
			shotTimer += Time.deltaTime;
			emoteTimer += Time.deltaTime;
		} else if (gameState.GetGamePaused ()) {
			CheckPlayerReady ();
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

	public void CheckInput ()
	{

		//sofern die Horizontale Achse betätigt wird (linke oder rechte Pfeiltaste sowie A oder D)
		if ((Mathf.Abs (Input.GetAxis ("HorizontalP1")) > 0.1f)) {
			//wird die Accelerate-Methode mit dem Argument X aufgerufen
			Accelerate ("X");
		} else {
			//ansonsten wird die Brake-Methode mit dem Argument X verwendet
			Brake ("X");
		}


		//das gleiche geschieht mit der Vertikalen Achse (hoch oder runter Pfeiltaste sowie W und S)
		if ((Mathf.Abs (Input.GetAxis ("VerticalP1")) > 0.1f)) {
			Accelerate ("Y");
		} else {
			Brake ("Y");
		}

		//die BewegungsZeit wird erhöht, sofern mindestens eine der beiden Achsen eine Bewegung zurückliefern
		if ((Mathf.Abs (Input.GetAxis ("VerticalP1")) > 0.1f) || Mathf.Abs (Input.GetAxis ("HorizontalP1")) > 0.1f) {
			exhaustTime += Time.deltaTime;
		} else {
			//wenn die Figur nicht mehr bewegt wird, wird die BewegungsZeit auf 0 zurückgesetzt
			exhaustTime = 0;
		}

		//wenn der Block-Button (B) gedrückt wird
		if (Input.GetButton ("BlockP1")) {
			blockSpawn.AddBlockChargeTime (Time.deltaTime);    //wird die Zeit zum Spawnen des Blocks hochgezählt
			shotSpawn.ResetShotChargeTime ();
		}
		//wenn der Block-Button (B) losgelassen wird
		if (Input.GetButtonUp ("BlockP1")) {
			blockSpawn.SpawnBlock ();    //wird überprüft, ob der Block gespawnt werden kann (wenn die Zeit groß genug ist)
		}

		//wenn der Schuss-Button (A) gedrückt wird
		if (Input.GetButton ("ShootP1")) {
			if (shotTimer > shotDelay) {  //und der ShotTimer größer ist als die gewünschte Wartezeit zwischen zwei Schüssen
				shotSpawn.AddShotChargeTime (Time.deltaTime);  //wird die Zeit zum Aufladen des Schuss hochgezählt
				blockSpawn.ResetBlockChargeTime ();
			}
		}
		//wenn der Schuss-Button (A) losgelassen wird und der ShotTimer größer ist als die gewünschte Wartezeit zwischen zwei Schüssen 
		if (Input.GetButtonUp ("ShootP1") && shotTimer > shotDelay) {
			shotSpawn.SpawnShot ();  //wird der Schuss gespawnt 
		}

		/*
         * 
        Emotes
         * 
         * */

		if (Input.GetButtonUp ("LBP1") && emoteTimer > emoteDelay) {
			CastEmote ("nice");
		}
		if (Input.GetButtonUp ("RBP1") && emoteTimer > emoteDelay) {
			CastEmote ("angry");
		}
		if (Input.GetAxis ("RTP1") != 0 && emoteTimer > emoteDelay) {
			CastEmote ("cry");
		}
		if (Input.GetAxis ("LTP1") != 0 && emoteTimer > emoteDelay) {
			CastEmote ("haha");
		}

	}

	//Die Methode wird in jedem Update aufgerufen und regelt die Bewegung des Spielers
	public void Move ()
	{
		//es wird ein Bewegungsvektor erstellt und dieser bekommt die Geschwindigkeit auf der X und Y Achse übertragen.
		movementVector = new Vector3 (speedX, speedY, 0f);

		//der Spieler bewegt sich dann mit Hilfe deses Vektors auf dem Spielfeld. Die Bewegung ist immer relativ zur Spielwelt 
		transform.Translate (movementVector * Time.deltaTime, Space.World);


		//reference: https://answers.unity.com/questions/307150/rotate-an-object-toward-the-direction-the-joystick.html

		float yEuler = Mathf.Atan2 (Input.GetAxis ("HorizontalP1") * -1, Input.GetAxis ("VerticalP1")) * Mathf.Rad2Deg; //Horizontal *1
		yEuler -= 270;   //Korrektur durch das gedrehte Sprite
		Vector3 direction = new Vector3 (0, 0, yEuler);
		if (Mathf.Abs (Input.GetAxis ("HorizontalP1")) > 0.1f || Mathf.Abs (Input.GetAxis ("VerticalP1")) > 0.1f) {   //Damit die Richtung nicht durch die "Nullstellung" des Sticks genullt wird
			transform.eulerAngles = direction;
		}

	}

	//Die Beschleunigen-Methode ermittelt die Geschwindigketi des Spielers bei einem Input
	public void Accelerate (string axis)
	{
		//Sofern das Argument "X" übergeben wird 
		if (axis.Equals ("X")) {
			//erhöht sich die Geschwindigkeit auf der X-Achse um den Wert des Inputs, multipliziert mit der Beschleunigung
			speedX += Input.GetAxis ("HorizontalP1") * acceleration;
		}


		//sofern die Geschwindigkeit doch außerhalb der Grenzen von -maxSpeed und maxSpeed liegt, wird der Wert an diese Grenzwerte angepasst
		if (speedX > maxSpeed) {
			speedX = maxSpeed;
		} else if (speedX < -maxSpeed) {
			speedX = -maxSpeed;
		}


		//Sofern das Argument Y übergeben wird, wird die Ermittlung der Geschwindigkeit genauso ermittelt wie für die X-Achse
		else if (axis.Equals ("Y")) {
			speedY += Input.GetAxis ("VerticalP1") * acceleration;
		}

		if (speedY > maxSpeed) {
			speedY = maxSpeed;
		} else if (speedY < -maxSpeed) {
			speedY = -maxSpeed;
		}

		//wenn diagonal voll beschleunigt wird, wird der Spieler minimal langsamer
		if (Mathf.Abs (speedY) >= (maxSpeed - 10) && Mathf.Abs (speedX) >= (maxSpeed - 10)) {
			if (speedY >= (maxSpeed - 10)) {
				speedY = maxSpeed - 10;
			} else if (speedY <= (-maxSpeed + 10)) {
				speedY = -maxSpeed + 10;
			}

			if (speedX >= (maxSpeed - 10)) {
				speedX = maxSpeed - 10;
			} else if (speedX <= (-maxSpeed + 10)) {
				speedX = -maxSpeed + 10;
			}
		}


	}

	//Sofern kein Input seitens des Spielers kommt, wird die Brake-Methode zum Abbremsen der Figur verwendet
	public void Brake (string axis)
	{
		//sofern die Geschwindigkeit positiv ist
		if (axis.Equals ("X") && speedX > 0) {
			//wird in die negative Richtung abgebremst
			speedX -= brakingForce;
		}
		//sofern die GEschwindigkeit negativ ist,
		else if (axis.Equals ("X") && speedX < 0) {
			//wird in die positive Richtung abgebremst
			speedX += brakingForce;
		}


		//für die Y-Achse genauso
		if (axis.Equals ("Y") && speedY > 0) {
			speedY -= brakingForce;
		} else if (axis.Equals ("Y") && speedY < 0) {
			speedY += brakingForce;
		}

		//endgültiges abremsen, bei Geschwindigkeit um 0
		if (Mathf.Abs (speedX) >= -brakingForce / 2 && Mathf.Abs (speedX) <= brakingForce / 2) {
			speedX = 0;
		}
		if (Mathf.Abs (speedY) >= -brakingForce / 2 && Mathf.Abs (speedY) <= brakingForce / 2) {
			speedY = 0;
		}
	}

	//die Methode überprüft, ob ein Abgaspartikel erzeugt werden soll
	public void CheckExhaust ()
	{
		//sofern sich der Spieler eine bestimmte Zeit bewegt und diese Zeit über der festgelegten Zeit bis zum Spawnen eines Abgaspartikels liegt
		if (exhaustTime > exhaustSpawnTime) {
			//wird ein Abgaspartikel an der Position des ExhaustSpawners erstellt
			GameObject exhaust = Instantiate (exhaustPrefab, exSpawner.transform.position, exSpawner.transform.rotation);
			exhaust.GetComponent<ExhaustPhoton> ().SetColor (teamColor);    //das Partikel bekommt die Farbe des Spielers
			exhaust.GetComponent<ExhaustPhoton> ().SetDirection (new Vector3 (speedX, speedY, 0));
			//und die Zeit zum Spawnen eines Partikels auf null gesetzt
			exhaustTime = 0;
		}
	}

	public void CheckPlayerReady ()
	{
		if (Input.GetButtonUp ("ShootP1")) {
			pvGamestate.RPC ("SetPlayerReady", PhotonTargets.All, true, playerTeam);
		}
	}

	//setzt die Farve des Spielers sowie seiner Spawner fest
	public void SetColor (Color col)
	{
		teamColor = col;	//die Team Color wird gesetzt
		blockSpawn.GetComponent<BlockSpawnerPhoton> ().SetColor (teamColor);    //ebenso wird die Farbe dem Blockspawner und dem    
		shotSpawn.GetComponent<ShotSpawnerPhoton> ().SetColor (teamColor);      //ShotSpawner bekannt gemacht

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

	//Methode fürs Benutzen der Emotes
	public void CastEmote (string type)
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
		StartCoroutine (DisplayEmote (type));
		playerLogging.AddEmote (type);   //dem Logging wird die Art des Emotes mitgeteilt    
		emoteTimer = 0;
	}

	IEnumerator DisplayEmote (string type)
	{
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
		shotSpawn.StopSoundByStun ();
		blockSpawn.StopSoundByStun ();
	}

	public void CalculateLogData (string endingCondition, string gameType)
	{

		playerLogging.SetSubjectNr (subjectNr, subjectNrEnemy);
		playerLogging.SetRating (rating, ratingEnemy);
		playerLogging.SetGameType (gameType);
		positionTracker.CalculateWalkedDistance ();
		playerLogging.SetEndingCondition (endingCondition);

	}

	public void FindEnemyPlayer (GameObject enePlayer)
	{
		enemyPlayer = enePlayer;
		playerLoggingEnemy = enemyPlayer.GetComponent<PlayerLogging> (); //das playerLogging-Skript des Gegners wird verknüpft, um die Betäubungen abzuspeichern.
		subjectNrEnemy = enemyPlayer.GetComponent<PlayerPhoton> ().subjectNr;
		ratingEnemy = enemyPlayer.GetComponent<PlayerPhoton> ().rating;
	}

	public void SetUpSpeechBubble ()
	{
		speechBubble = Instantiate (speechBubblePrefab);
		speechBubble.GetComponent<FollowPlayer> ().SetFollowPlayer (this.gameObject);
	}

	public void SetPlayerInformation ()
	{
		rating = PlayerPrefs.GetInt (subjectNr + "Rating");
		subjectNr = PlayerPrefs.GetInt ("VP");

		if (subjectNr % 2 == 0) {
			playerTeam = 2;
		} else if (subjectNr % 2 == 1) {
			playerTeam = 1;
		}
		pvPlayer.RPC ("SetPlayerName", PhotonTargets.All, subjectNr);
		CheckTeamColor ();   //zu Beginn bekommt der Spieler die richtige Farbe
	}

	[PunRPC]
	public void SetPlayerName (int vpNr){
		gameObject.name = "VP" + vpNr; 
	}


	//die Methode überprüft die Teamzugehörigkeit und ändert die Farbe des Spielers dementsprechend
	private void CheckTeamColor ()
	{
		switch (playerTeam) {
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