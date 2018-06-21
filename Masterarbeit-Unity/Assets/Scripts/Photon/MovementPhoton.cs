using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPhoton : MonoBehaviour {

	private GameStatePhoton gameState;
	private PlayerPhoton playerPhoton;
	private Vector3 movementVector;
	public GameObject exhaustPrefab;
	PhotonView pvPlayer;
	PhotonView pvGamestate;

	//trigger variablen
	private bool rightTriggerInUse = false;
	private bool leftTriggerInUse = false;


	// Use this for initialization
	void Start () {
		playerPhoton = gameObject.GetComponent<PlayerPhoton> ();
		pvPlayer = gameObject.GetComponent<PhotonView> ();

		gameState = (GameStatePhoton)FindObjectOfType (typeof(GameStatePhoton));
		pvGamestate = gameState.gameObject.GetComponent<PhotonView> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!gameState.GetGamePaused ()) {
			if (!playerPhoton.stunned) {
				CheckInput ();   //zunächst wird der Input überprüft
				Move (); //dann der Spieler bewegt
				CheckExhaust (); //sowie überprüft, ob das Abgas erzeugt werden soll
			}
			playerPhoton.shotTimer += Time.deltaTime;
			playerPhoton.emoteTimer += Time.deltaTime;
		} else if (gameState.GetGamePaused ()) {
			CheckPlayerReady ();
		}
	}

	public void CheckInput ()
	{

		//sofern die Horizontale Achse betätigt wird (linke oder rechte Pfeiltaste sowie A oder D)
		if ((Mathf.Abs (Input.GetAxis ("HorizontalP1")) > 0.25f)) {
			//wird die Accelerate-Methode mit dem Argument X aufgerufen
			Accelerate ("X");
		} else {
			//ansonsten wird die Brake-Methode mit dem Argument X verwendet
			Brake ("X");
		}


		//das gleiche geschieht mit der Vertikalen Achse (hoch oder runter Pfeiltaste sowie W und S)
		if ((Mathf.Abs (Input.GetAxis ("VerticalP1")) > 0.25f)) {
			Accelerate ("Y");
		} else {
			Brake ("Y");
		}

		//die BewegungsZeit wird erhöht, sofern mindestens eine der beiden Achsen eine Bewegung zurückliefern
		if ((Mathf.Abs (Input.GetAxis ("VerticalP1")) > 0.25f) || Mathf.Abs (Input.GetAxis ("HorizontalP1")) > 0.25f) {
			playerPhoton.exhaustTime += Time.deltaTime;
		} else {
			//wenn die Figur nicht mehr bewegt wird, wird die BewegungsZeit auf 0 zurückgesetzt
			playerPhoton.exhaustTime = 0;
		}

		//wenn der Block-Button (B) gedrückt wird
		if (Input.GetButton ("BlockP1")) {
			playerPhoton.blockSpawn.GetComponent<PhotonView>().RPC ("AddBlockChargeTime", PhotonTargets.All, Time.deltaTime);
			//playerPhoton.blockSpawn.AddBlockChargeTime (Time.deltaTime);    //wird die Zeit zum Spawnen des Blocks hochgezählt
			playerPhoton.shotSpawn.GetComponent<PhotonView>().RPC ("ResetShotChargeTime", PhotonTargets.All);
		//	playerPhoton.shotSpawn.ResetShotChargeTime ();
		}
		//wenn der Block-Button (B) losgelassen wird
		if (Input.GetButtonUp ("BlockP1")) {
			playerPhoton.blockSpawn.GetComponent<PhotonView>().RPC ("SpawnBlock", PhotonTargets.All);
			//playerPhoton.blockSpawn.SpawnBlock ();    //wird überprüft, ob der Block gespawnt werden kann (wenn die Zeit groß genug ist)
		}

		//wenn der Schuss-Button (A) gedrückt wird
		if (Input.GetButton ("ShootP1")) {
			if (playerPhoton.shotTimer > playerPhoton.shotDelay) {  //und der ShotTimer größer ist als die gewünschte Wartezeit zwischen zwei Schüssen
				playerPhoton.shotSpawn.GetComponent<PhotonView>().RPC ("AddShotChargeTime", PhotonTargets.All, Time.deltaTime);
				//playerPhoton.shotSpawn.AddShotChargeTime (Time.deltaTime);  //wird die Zeit zum Aufladen des Schuss hochgezählt
				playerPhoton.blockSpawn.GetComponent<PhotonView>().RPC  ("ResetBlockChargeTime", PhotonTargets.All);
				//playerPhoton.blockSpawn.ResetBlockChargeTime ();
			}
		}
		//wenn der Schuss-Button (A) losgelassen wird und der ShotTimer größer ist als die gewünschte Wartezeit zwischen zwei Schüssen 
		if (Input.GetButtonUp ("ShootP1") && playerPhoton.shotTimer > playerPhoton.shotDelay) {
		//	playerPhoton.shotSpawn.SpawnShot ();  //wird der Schuss gespawnt 
			playerPhoton.shotSpawn.GetComponent<PhotonView>().RPC ("SpawnShot", PhotonTargets.All);

		}

		/*
         * 
        Emotes
         * 
         * */

		//emote nice
		if (Input.GetButtonUp ("LBP1") && playerPhoton.emoteTimer > playerPhoton.emoteDelay) {
			StopCoroutine("DisplayPreparedEmote");
			if (!playerPhoton.emoteNicePrepared) {
				playerPhoton.preparedEmojiType = "nice";
				playerPhoton.StartCoroutine("DisplayPreparedEmote");
				playerPhoton.SetEmotePrepared ("nice", true);
			} else if (playerPhoton.emoteNicePrepared) {
				playerPhoton.SetEmotePrepared ("nice", false);
				playerPhoton.emoteTimer = 0;
				pvPlayer.RPC ("StartDisplayEmoteCoroutine", PhotonTargets.All, "nice");
			}
		}
		//emote angry
		if (Input.GetButtonUp ("RBP1") && playerPhoton.emoteTimer > playerPhoton.emoteDelay) {
			playerPhoton.StopCoroutine("DisplayPreparedEmote");
			if (!playerPhoton.emoteAngryPrepared) {
				playerPhoton.preparedEmojiType = "angry";
				playerPhoton.StartCoroutine("DisplayPreparedEmote");
				playerPhoton.SetEmotePrepared ("angry", true);
			} else if (playerPhoton.emoteAngryPrepared) {
				playerPhoton.SetEmotePrepared ("angry", false);
				playerPhoton.emoteTimer = 0;
				pvPlayer.RPC ("StartDisplayEmoteCoroutine", PhotonTargets.All, "angry");
			}
		}		
		//emote cry
		if (Input.GetAxis ("RTP1") != 0 && playerPhoton.emoteTimer > playerPhoton.emoteDelay) {
			if (!rightTriggerInUse) {
				playerPhoton.StopCoroutine ("DisplayPreparedEmote");
				if (!playerPhoton.emoteCryPrepared) {
					playerPhoton.preparedEmojiType = "cry";
					playerPhoton.StartCoroutine ("DisplayPreparedEmote");
					playerPhoton.SetEmotePrepared ("cry", true);
				} else if (playerPhoton.emoteCryPrepared) {
					playerPhoton.SetEmotePrepared ("cry", false);
					playerPhoton.emoteTimer = 0;
					pvPlayer.RPC ("StartDisplayEmoteCoroutine", PhotonTargets.All, "cry");
				}
				rightTriggerInUse = true;
			}
		}
		if (Input.GetAxis ("RTP1") == 0)
		{
			rightTriggerInUse = false;
		}  

		if (Input.GetAxis ("LTP1") != 0 && playerPhoton.emoteTimer > playerPhoton.emoteDelay) {
			if (!leftTriggerInUse) {
				playerPhoton.StopCoroutine ("DisplayPreparedEmote");
				if (!playerPhoton.emoteHahaPrepared) {
					playerPhoton.preparedEmojiType = "haha";
					playerPhoton.StartCoroutine ("DisplayPreparedEmote");
					playerPhoton.SetEmotePrepared ("haha", true);
				} else if (playerPhoton.emoteHahaPrepared) {
					playerPhoton.SetEmotePrepared ("haha", false);
					playerPhoton.emoteTimer = 0;
					pvPlayer.RPC ("StartDisplayEmoteCoroutine", PhotonTargets.All, "haha");
				}
				leftTriggerInUse = true;
			}
		}
		if (Input.GetAxis ("LTP1") == 0)
		{
			leftTriggerInUse = false;
		}  



		/*
		 * 
		 * Pause
		 * 
		 */

		if (Input.GetButtonUp ("Start")) {
			pvGamestate.RPC ("SetGamePaused", PhotonTargets.All, true, "pause");
		}
	}



	//Die Methode wird in jedem Update aufgerufen und regelt die Bewegung des Spielers
	public void Move ()
	{
		//es wird ein Bewegungsvektor erstellt und dieser bekommt die Geschwindigkeit auf der X und Y Achse übertragen.
		movementVector = new Vector3 (playerPhoton.speedX, playerPhoton.speedY, 0f);

		//der Spieler bewegt sich dann mit Hilfe deses Vektors auf dem Spielfeld. Die Bewegung ist immer relativ zur Spielwelt 
		transform.Translate (movementVector * Time.deltaTime, Space.World);


		//reference: https://answers.unity.com/questions/307150/rotate-an-object-toward-the-direction-the-joystick.html

		float yEuler = Mathf.Atan2 (Input.GetAxis ("HorizontalP1") * -1, Input.GetAxis ("VerticalP1")) * Mathf.Rad2Deg; //Horizontal *1
		yEuler -= 270;   //Korrektur durch das gedrehte Sprite
		Vector3 direction = new Vector3 (0, 0, yEuler);
		if (Mathf.Abs (Input.GetAxis ("HorizontalP1")) > 0.15f || Mathf.Abs (Input.GetAxis ("VerticalP1")) > 0.15f) {   //Damit die Richtung nicht durch die "Nullstellung" des Sticks genullt wird
			transform.eulerAngles = direction;
		}

	}

	//Die Beschleunigen-Methode ermittelt die Geschwindigketi des Spielers bei einem Input
	public void Accelerate (string axis)
	{
		//Sofern das Argument "X" übergeben wird 
		if (axis.Equals ("X")) {
			//erhöht sich die Geschwindigkeit auf der X-Achse um den Wert des Inputs, multipliziert mit der Beschleunigung
			playerPhoton.speedX += Input.GetAxis ("HorizontalP1") * playerPhoton.acceleration;
		}


		//sofern die Geschwindigkeit doch außerhalb der Grenzen von -playerPhoton.maxSpeed und playerPhoton.maxSpeed liegt, wird der Wert an diese Grenzwerte angepasst
		if (playerPhoton.speedX > playerPhoton.maxSpeed) {
			playerPhoton.speedX = playerPhoton.maxSpeed;
		} else if (playerPhoton.speedX < -playerPhoton.maxSpeed) {
			playerPhoton.speedX = -playerPhoton.maxSpeed;
		}


		//Sofern das Argument Y übergeben wird, wird die Ermittlung der Geschwindigkeit genauso ermittelt wie für die X-Achse
		else if (axis.Equals ("Y")) {
			playerPhoton.speedY += Input.GetAxis ("VerticalP1") * playerPhoton.acceleration;
		}

		if (playerPhoton.speedY > playerPhoton.maxSpeed) {
			playerPhoton.speedY = playerPhoton.maxSpeed;
		} else if (playerPhoton.speedY < -playerPhoton.maxSpeed) {
			playerPhoton.speedY = -playerPhoton.maxSpeed;
		}

		//wenn diagonal voll beschleunigt wird, wird der Spieler minimal langsamer
		if (Mathf.Abs (playerPhoton.speedY) >= (playerPhoton.maxSpeed - 10) && Mathf.Abs (playerPhoton.speedX) >= (playerPhoton.maxSpeed - 10)) {
			if (playerPhoton.speedY >= (playerPhoton.maxSpeed - 10)) {
				playerPhoton.speedY = playerPhoton.maxSpeed - 10;
			} else if (playerPhoton.speedY <= (-playerPhoton.maxSpeed + 10)) {
				playerPhoton.speedY = -playerPhoton.maxSpeed + 10;
			}

			if (playerPhoton.speedX >= (playerPhoton.maxSpeed - 10)) {
				playerPhoton.speedX = playerPhoton.maxSpeed - 10;
			} else if (playerPhoton.speedX <= (-playerPhoton.maxSpeed + 10)) {
				playerPhoton.speedX = -playerPhoton.maxSpeed + 10;
			}
		}


	}

	//Sofern kein Input seitens des Spielers kommt, wird die Brake-Methode zum Abbremsen der Figur verwendet
	public void Brake (string axis)
	{
		//sofern die Geschwindigkeit positiv ist
		if (axis.Equals ("X") && playerPhoton.speedX > 0) {
			//wird in die negative Richtung abgebremst
			playerPhoton.speedX -= playerPhoton.brakingForce;
		}
		//sofern die GEschwindigkeit negativ ist,
		else if (axis.Equals ("X") && playerPhoton.speedX < 0) {
			//wird in die positive Richtung abgebremst
			playerPhoton.speedX += playerPhoton.brakingForce;
		}


		//für die Y-Achse genauso
		if (axis.Equals ("Y") && playerPhoton.speedY > 0) {
			playerPhoton.speedY -= playerPhoton.brakingForce;
		} else if (axis.Equals ("Y") && playerPhoton.speedY < 0) {
			playerPhoton.speedY += playerPhoton.brakingForce;
		}

		//endgültiges abremsen, bei Geschwindigkeit um 0
		if (Mathf.Abs (playerPhoton.speedX) >= -playerPhoton.brakingForce / 2 && Mathf.Abs (playerPhoton.speedX) <= playerPhoton.brakingForce / 2) {
			playerPhoton.speedX = 0;
		}
		if (Mathf.Abs (playerPhoton.speedY) >= -playerPhoton.brakingForce / 2 && Mathf.Abs (playerPhoton.speedY) <= playerPhoton.brakingForce / 2) {
			playerPhoton.speedY = 0;
		}
	}

	//die Methode überprüft, ob ein Abgaspartikel erzeugt werden soll
	public void CheckExhaust ()
	{
		//sofern sich der Spieler eine bestimmte Zeit bewegt und diese Zeit über der festgelegten Zeit bis zum Spawnen eines Abgaspartikels liegt
		if (playerPhoton.exhaustTime > playerPhoton.exhaustSpawnTime) {
			//wird ein Abgaspartikel an der Position des ExhaustSpawners erstellt
			GameObject exhaust = PhotonNetwork.Instantiate ("ExhaustPhoton", playerPhoton.exSpawner.transform.position, playerPhoton.exSpawner.transform.rotation,0);
			exhaust.GetComponent<PhotonView> ().RPC ("SetColor", PhotonTargets.All, playerPhoton.colorVector);
		//	exhaust.GetComponent<ExhaustPhoton> ().SetColor (playerPhoton.teamColor);    //das Partikel bekommt die Farbe des Spielers
			exhaust.GetComponent<ExhaustPhoton> ().SetDirection (new Vector3 (playerPhoton.speedX, playerPhoton.speedY, 0));
			//und die Zeit zum Spawnen eines Partikels auf null gesetzt
			playerPhoton.exhaustTime = 0;
		}
	}

	public void CheckPlayerReady ()
	{
		if (Input.GetButtonUp ("ShootP1")) {
			pvGamestate.RPC ("SetPlayerReady", PhotonTargets.All, true, playerPhoton.playerTeam);
		}
		if (Input.GetButtonUp ("Help")) {
			pvGamestate.RPC ("SetPlayerHelp", PhotonTargets.All, true, playerPhoton.playerTeam);
		} 
		if (Input.GetKeyUp (KeyCode.H)) {
			pvGamestate.RPC ("SetPlayerHelp", PhotonTargets.All, false, playerPhoton.playerTeam);
		}

	}
}
