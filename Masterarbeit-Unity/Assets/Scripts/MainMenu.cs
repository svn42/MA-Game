﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{
	public InputField passwortField;
	public InputField VPInputField;
	public GameObject mainMenu;
	public GameObject VLPWMenu;
	public GameObject VLMenu;
	public Button tutorialButton;
	public Button localGameButton;
	public Button onlineGameButton;
	public Button disconnectButton;
	public GameObject waitForPlayer;

	public GameObject passwortErrorText;
	public GameObject VPErrorText;
	public int VPNummer;

	public void Start ()
	{
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.sendRate = 80;
		PhotonNetwork.sendRateOnSerialize = 80;
		LoadData (PlayerPrefs.GetInt ("VP"));
		mainMenu.SetActive (true);
		waitForPlayer.GetComponent<Text> ().enabled = false;

		passwortErrorText = passwortField.gameObject.transform.Find ("Error-Text").gameObject;
		VPErrorText = VPInputField.gameObject.transform.Find ("Error-Text").gameObject;

		//wenn der Spieler das Tutorial absolviert hat, wird der OnlineButton aktiviert und das Rating eingeblendet
		if (PlayerPrefs.GetString (VPNummer.ToString () + "TutorialSolved").Equals ("Yes")) {
			onlineGameButton.interactable = true;
			mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: " + PlayerPrefs.GetInt (VPNummer.ToString () + "Rating");
		}

	}

	public void Update ()
	{
		//Debug.Log (PhotonNetwork.connectionStateDetailed.ToString ());
		Debug.Log ("Anzahl weiterer Spieler: " +PhotonNetwork.otherPlayers.Length);
	}


	public void StartTutorial ()
	{
		PlayerPrefs.SetString ("GameType", "Tutorial");
		SceneManager.LoadScene ("Tutorial_1");
	}

	public void StartLocalGame ()
	{
		PlayerPrefs.SetString ("GameType", "Local");
		SceneManager.LoadScene ("Level 1_local");
	}

	public void StartOnlineGame ()
	{
		PlayerPrefs.SetInt ("WinsP1", 0);
		PlayerPrefs.SetInt ("WinsP2", 0);
		PlayerPrefs.SetString ("GameType", "Online");
		PhotonNetwork.ConnectUsingSettings ("v01");
		PhotonNetwork.player.NickName = PlayerPrefs.GetInt ("VP").ToString();
	}

	void OnConnectedToMaster ()
	{
		PhotonNetwork.JoinLobby ();
	}

	void OnJoinedLobby ()
	{
		mainMenu.SetActive (false);
		waitForPlayer.GetComponent<Text> ().enabled = true;
		disconnectButton.interactable = true;
		disconnectButton.gameObject.SetActive (true);
		PhotonNetwork.JoinRandomRoom();

	}

	void OnPhotonRandomJoinFailed(){
		PhotonNetwork.CreateRoom(null);
	}		


	void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
		if (PhotonNetwork.playerList.Length == 2) {
			if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.LoadLevel ("Level 1_photon");
			}
		}
	}

	public void RestartLobby(){
		PhotonNetwork.Disconnect ();
		waitForPlayer.GetComponent<Text> ().enabled = false;
		mainMenu.SetActive (true);
		disconnectButton.interactable = false;
		disconnectButton.gameObject.SetActive (false);
	}

	public void QuitGame ()
	{
		Application.Quit ();
		PlayerPrefs.SetInt ("VP", 0);
	}

	public void CheckPassword ()
	{
		string enteredPassword = "";
		enteredPassword = passwortField.text;

		if (enteredPassword.Equals ("apu")) {
			passwortErrorText.SetActive (false);
			VLMenu.SetActive (true);
			VLPWMenu.SetActive (false);
		} else {
			passwortErrorText.SetActive (true);
		}
		passwortField.text = "";

	}

	public void CheckVP ()
	{
		string enteredVPStr = "";
		enteredVPStr = VPInputField.text;
		int enteredVPInt;
		if (int.TryParse (enteredVPStr, out enteredVPInt)) {
			if (!enteredVPStr.Equals ("")) {
				VPErrorText.SetActive (false);
				LoadData (enteredVPInt);
				PlayerPrefs.SetInt ("VP", enteredVPInt);
			} else {
				VPErrorText.SetActive (true);
			}
			VPInputField.text = "";
		}
        
	}

	public void LoadData (int i)
	{
		VPNummer = i;
		if (VPNummer == 0) {

			mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: ?";
			VLMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: ?";
			mainMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: ?";

			tutorialButton.interactable = false;
			//
			onlineGameButton.interactable = true;
		} else {

			VLMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: " + VPNummer;
			mainMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: " + VPNummer;

			if (PlayerPrefs.GetString (VPNummer.ToString () + "TutorialSolved").Equals ("Yes")) {
				mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: " + PlayerPrefs.GetInt (VPNummer.ToString () + "Rating");
				tutorialButton.interactable = false;
				onlineGameButton.interactable = true;
			} else {
				mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: ?";

				tutorialButton.interactable = true;
//TODO: MUSS raus, bevor alles beginnt
				onlineGameButton.interactable = true;

			}


		}

	}


}
