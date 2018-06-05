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
    public Button loacalGameButton;
    public Button onlineGameButton;

    private GameObject passwortErrorText;
    private GameObject VPErrorText;
    public int VPNummer;

    public void Start()
    {
        LoadData(PlayerPrefs.GetInt("VP"));

        passwortErrorText = passwortField.gameObject.transform.Find("Error-Text").gameObject;
        VPErrorText = VPInputField.gameObject.transform.Find("Error-Text").gameObject;


        if (PlayerPrefs.GetString(VPNummer.ToString()+"TutorialSolved").Equals("Yes"))
        {
            loacalGameButton.interactable = true;
            onlineGameButton.interactable = true;
            mainMenu.transform.Find("Rating-Text").gameObject.GetComponent<Text>().text = "Rating: " + PlayerPrefs.GetInt(VPNummer.ToString()+"Rating");
        }
    }

    public void StartTutorial()
    {
        PlayerPrefs.SetString("GameType", "Tutorial");
        SceneManager.LoadScene("Tutorial_1");
    }

    public void StartLocalGame()
    {
        PlayerPrefs.SetString("GameType","Local");
        SceneManager.LoadScene("Level 1");
    }

    public void StartOnlineGame()
    {
        PlayerPrefs.SetString("GameType", "Online");
        SceneManager.LoadScene("Level 1");

    }
    
    public void QuitGame()
    {
        Application.Quit();
        PlayerPrefs.SetInt("VP",0);
    }

    public void CheckPassword()
    {
        string enteredPassword = "";
        enteredPassword = passwortField.text;

        if (enteredPassword.Equals("apu"))
        {
            passwortErrorText.SetActive(false);
            VLMenu.SetActive(true);
            VLPWMenu.SetActive(false);
        }
        else
        {
            passwortErrorText.SetActive(true);
        }
        passwortField.text = "";

    }

    public void CheckVP()
    {
        string enteredVPStr = "";
        enteredVPStr = VPInputField.text;
        int enteredVPInt;
        if (int.TryParse(enteredVPStr, out enteredVPInt)){
            if (!enteredVPStr.Equals(""))
            {
                VPErrorText.SetActive(false);
                LoadData(enteredVPInt);
                PlayerPrefs.SetInt("VP",enteredVPInt);
            }
            else
            {
                VPErrorText.SetActive(true);
            }
            VPInputField.text = "";
        }
        
    }

    public void LoadData(int i)
    {
        VPNummer = i;
        if (VPNummer == 0)
        {

            mainMenu.transform.Find("Rating-Text").gameObject.GetComponent<Text>().text = "Rating: ?";
            VLMenu.transform.Find("VP-Text").gameObject.GetComponent<Text>().text = "VP: ?";
            mainMenu.transform.Find("VP-Text").gameObject.GetComponent<Text>().text = "VP: ?";

            tutorialButton.interactable = false;
            loacalGameButton.interactable = false;
            onlineGameButton.interactable = false;
        } else
        {

            VLMenu.transform.Find("VP-Text").gameObject.GetComponent<Text>().text = "VP: "+ VPNummer;
            mainMenu.transform.Find("VP-Text").gameObject.GetComponent<Text>().text = "VP: "+ VPNummer;

            if (PlayerPrefs.GetString(VPNummer.ToString() + "TutorialSolved").Equals("Yes"))
            {
                mainMenu.transform.Find("Rating-Text").gameObject.GetComponent<Text>().text = "Rating: " + PlayerPrefs.GetInt(VPNummer.ToString() + "Rating");
                tutorialButton.interactable = false;
                loacalGameButton.interactable = true;
                onlineGameButton.interactable = true;
            }
            else
            {
                mainMenu.transform.Find("Rating-Text").gameObject.GetComponent<Text>().text = "Rating: ?";

                tutorialButton.interactable = true;
                loacalGameButton.interactable = false;
                onlineGameButton.interactable = false;

            }


        }

    }


}
