using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{
	public InputField passwortField;
	public InputField VPInputField;
	public InputField ServerAddressInputField;
	public GameObject mainMenu;
	public GameObject VLPWMenu;
	public GameObject VLMenu;
	public Button tutorialButton;
	public Button lastTutorialButton;
	public Button localGameButton;
	public Button onlineGameButton;
	public Button lastOnlineButton;
	public Button disconnectButton;
	public GameObject waitForPlayer;

	private GameObject passwortErrorText;
	private GameObject VPErrorText;
	private GameObject IPChangedText;
	public int VPNummer;
	private string lastOnlineLevel = "StartOnline";

	public void Start ()
	{
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.sendRate = 80;
		PhotonNetwork.sendRateOnSerialize = 80;
		LoadData (PlayerPrefs.GetInt ("VP"));
		mainMenu.SetActive (true);
		waitForPlayer.GetComponent<Text> ().enabled = false;
		Cursor.visible = true;
		passwortErrorText = passwortField.gameObject.transform.Find ("Error-Text").gameObject;
		VPErrorText = VPInputField.gameObject.transform.Find ("Error-Text").gameObject;
		IPChangedText = ServerAddressInputField.gameObject.transform.Find ("Success-Text").gameObject;
		PhotonNetwork.PhotonServerSettings.ServerAddress = PlayerPrefs.GetString ("ServerAddress");
	}

	public void Update ()
	{
		//Debug.Log (PhotonNetwork.connectionStateDetailed.ToString ());
		CheckInput();
	}


	public void CheckInput(){
		if (Input.GetKeyUp (KeyCode.Keypad1)) {
			PlayerPrefs.SetInt ("RatingManipulated", 1453);
			Debug.Log ("RatingManipulated: "+ PlayerPrefs.GetInt("RatingManipulated"));
		} else if (Input.GetKeyUp (KeyCode.Keypad3)) {
			PlayerPrefs.SetInt ("RatingManipulated", 1753);
			Debug.Log ("RatingManipulated: "+ PlayerPrefs.GetInt("RatingManipulated"));
		}else if (Input.GetKeyUp (KeyCode.Keypad7)) {
			PlayerPrefs.SetInt ("RatingManipulated", 2053);
			Debug.Log ("RatingManipulated: "+ PlayerPrefs.GetInt("RatingManipulated"));
		}else if (Input.GetKeyUp (KeyCode.Keypad9)) {
			PlayerPrefs.SetInt ("RatingManipulated", 2453);
			Debug.Log ("RatingManipulated: "+ PlayerPrefs.GetInt("RatingManipulated"));
		}
	}

	public void StartTutorial ()
	{
		PlayerPrefs.SetString ("GameType", "Tutorial");
		SceneManager.LoadScene ("Tutorial_1");
		PlayerPrefs.SetInt(VPNummer.ToString() + "Rating", 0);

	}

	public void StartLocalGame1 ()
	{
		PlayerPrefs.SetString ("GameType", "Local");
		SceneManager.LoadScene ("Level 1_local");
		IPChangedText.SetActive (false);
	}

	public void StartLocalGame2 ()
	{
		PlayerPrefs.SetString ("GameType", "Local");
		SceneManager.LoadScene ("Level 2_local");
		IPChangedText.SetActive (false);
	}

	public void StartLocalGame3 ()
	{
		PlayerPrefs.SetString ("GameType", "Local");
		SceneManager.LoadScene ("Level 3_local");
		IPChangedText.SetActive (false);
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
				PhotonNetwork.LoadLevel (lastOnlineLevel);
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
		PlayerPrefs.SetInt ("RatingManipulated", 0);
		Debug.Log ("RatingManipulated: "+ PlayerPrefs.GetInt("RatingManipulated"));

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

	public void LoadLastOnlineScene(){
		PlayerPrefs.SetString ("GameType", "Online");
		PhotonNetwork.ConnectUsingSettings ("v01");
		PhotonNetwork.player.NickName = PlayerPrefs.GetInt ("VP").ToString();
		lastOnlineLevel = PlayerPrefs.GetString ("LastOnline");
	}

	public void LoadLastTutorial(){
		string lastTut = PlayerPrefs.GetString ("LastTutorial");
		SceneManager.LoadScene (lastTut);
	}

	public void ResetTutorial(){
		PlayerPrefs.SetString (VPNummer.ToString () + "TutorialSolved", "No");
		tutorialButton.interactable = true;
		lastTutorialButton.interactable = false;
		PlayerPrefs.SetString ("LastTutorial", "");
		PlayerPrefs.SetInt(VPNummer.ToString() + "Rating", 0);
		mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: ?";
	}

	public void ResetOnline(){
		PlayerPrefs.SetString ("LastOnline", "");
		if (PlayerPrefs.GetString (VPNummer.ToString () + "TutorialSolved").Equals ("Yes")) {
			onlineGameButton.interactable = true;
		} else {
			onlineGameButton.interactable = false;
		}
		lastOnlineButton.interactable = false;
		PlayerPrefs.SetInt ("RatingManipulated", 0);
		Debug.Log ("RatingManipulated: "+ PlayerPrefs.GetInt("RatingManipulated"));

	}

	public void SetServerIP(){
		string address;
		address = ServerAddressInputField.text;
		IPChangedText.SetActive (true);
		PlayerPrefs.SetString ("ServerAddress", address);
		PhotonNetwork.PhotonServerSettings.ServerAddress = PlayerPrefs.GetString ("ServerAddress");;
	}

	public void LoadData (int i)
	{
		VPNummer = i;
		if (VPNummer == 0) {

			mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: ?";
			VLMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: ?";
			mainMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: ?";
			tutorialButton.interactable = false;
			lastTutorialButton.interactable = false;
			lastOnlineButton.interactable = false;
			onlineGameButton.interactable = false;
		} else {

			VLMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: " + VPNummer;
			mainMenu.transform.Find ("VP-Text").gameObject.GetComponent<Text> ().text = "VP: " + VPNummer;

			if (PlayerPrefs.GetString (VPNummer.ToString () + "TutorialSolved").Equals ("Yes")) {
				mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: " + PlayerPrefs.GetInt (VPNummer.ToString () + "Rating");
				tutorialButton.interactable = false;
				lastTutorialButton.interactable = false;
				if (PlayerPrefs.GetString ("LastOnline") != "") {
					onlineGameButton.interactable = false;
					lastOnlineButton.interactable = true;
				} else {
					onlineGameButton.interactable = true;
					lastOnlineButton.interactable = false;
				}

				//Wenn das Tutorial noch nicht absolviert wurde und VP ausgewählt wurde
			} else {
					mainMenu.transform.Find ("Rating-Text").gameObject.GetComponent<Text> ().text = "Rating: ?";
				if (PlayerPrefs.GetString ("LastTutorial") != "") {
					tutorialButton.interactable = false;
					lastTutorialButton.interactable = true;

				} else {
					tutorialButton.interactable = true;
					lastTutorialButton.interactable = false;

				}
				onlineGameButton.interactable = false;
				lastOnlineButton.interactable = false;

			}


		}

	}


}
