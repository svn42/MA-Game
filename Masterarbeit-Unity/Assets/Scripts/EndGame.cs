using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	private GameObject gui;
	private GameObject player1Info;
	private GameObject player2Info;
	private Text finalScoreTeam1;
	private Text finalScoreTeam2;
	private Text ratingTeam1;
	private Text ratingTeam2;


	// Use this for initialization
	void Start () {
		gui = GameObject.FindGameObjectWithTag ("GUI");
		player1Info = gui.transform.Find ("PauseScreen").gameObject.transform.Find ("TransparentScreen").gameObject.transform.Find ("Spieler1").gameObject; 
		player2Info = gui.transform.Find ("PauseScreen").gameObject.transform.Find ("TransparentScreen").gameObject.transform.Find ("Spieler2").gameObject; 
		finalScoreTeam1 = player1Info.transform.Find ("Siege-Spieler1").GetComponent<Text> ();
		finalScoreTeam2 = player2Info.transform.Find ("Siege-Spieler2").GetComponent<Text> ();
		ratingTeam1 = player1Info.transform.Find ("Rating").gameObject.transform.Find("RatingInt").GetComponent<Text> ();
		ratingTeam2 = player2Info.transform.Find ("Rating").gameObject.transform.Find("RatingInt").GetComponent<Text> ();
		Cursor.visible = true;
		finalScoreTeam1.text = PlayerPrefs.GetInt ("WinsP1").ToString();
		finalScoreTeam2.text = PlayerPrefs.GetInt ("WinsP2").ToString();
		ratingTeam1.text =	PlayerPrefs.GetInt ("TempRatingP1").ToString();
		ratingTeam2.text =	PlayerPrefs.GetInt ("TempRatingP2").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadMainMenu(){
		DisconnectPlayer ();
		SceneManager.LoadScene ("MainMenu");
	}

	void DisconnectPlayer(){
		PhotonNetwork.Disconnect ();
	}
}
