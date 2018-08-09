using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

	public bool player1Ready;
	public bool player2Ready;
	public int playerTeam;
	PhotonView pv;

	private GameObject gui;
	private GameObject pauseScreenGO;
	private Canvas pauseScreen;
	private GameObject transparentScreen;
	private Canvas player1Box;
	private Canvas player2Box;
	private Image greenCheckP1;
	private Image greenCheckP2;


	// Use this for initialization
	void Start ()
	{
		
		pv = gameObject.GetComponent<PhotonView> ();
		PhotonNetwork.automaticallySyncScene = true;

		gui = GameObject.FindGameObjectWithTag ("GUI");
		pauseScreenGO = gui.transform.Find ("PauseScreen").gameObject;
		pauseScreen = pauseScreenGO.GetComponent<Canvas> ();
		transparentScreen = pauseScreenGO.transform.Find ("TransparentScreen").gameObject;

		player1Box = transparentScreen.transform.Find ("Spieler1").GetComponent<Canvas> ();
		player2Box = transparentScreen.transform.Find ("Spieler2").GetComponent<Canvas> ();
		greenCheckP1 = transparentScreen.transform.Find ("Spieler1").transform.Find ("Spieler1_Check").GetComponent<Image> ();
		greenCheckP2 = transparentScreen.transform.Find ("Spieler2").transform.Find ("Spieler2_Check").GetComponent<Image> ();
		AssignPlayerTeam ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckInput ();
		CheckPlayersReady ();
	}

	public void AssignPlayerTeam ()
	{
		if (PlayerPrefs.GetInt ("VP") % 2 == 1) {
			playerTeam = 1;
		} else if (PlayerPrefs.GetInt ("VP") % 2 == 0) {
			playerTeam = 2;
		}
	}

	public void CheckPlayersReady ()
	{
		if (player1Ready && player2Ready) {
			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.LoadLevel (SceneManager.GetActiveScene ().buildIndex + 1);
			}
		}
	}

	public void CheckInput ()
	{
		if (Input.GetButtonUp ("ShootP1")) {
			if (playerTeam == 1) {
				pv.RPC ("SetPlayerReady", PhotonTargets.All, 1);
			} else if (playerTeam == 2) {
				pv.RPC ("SetPlayerReady", PhotonTargets.All, 2);
			}
		}

	}

	[PunRPC]
	public void SetPlayerReady(int i){
		if (i == 1) {
			player1Ready = true;
			greenCheckP1.enabled = true;
		} else if (i == 2) {
			player2Ready = true;
			greenCheckP2.enabled = true;
		}
	}

}
