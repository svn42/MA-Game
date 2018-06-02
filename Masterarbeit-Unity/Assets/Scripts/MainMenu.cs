using System.Collections;
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
    private GameObject passwortErrorText;
    private GameObject VPErrorText;
    public int VPNummer;

    public bool tutorialSolved;

    public void Start()
    {
        passwortErrorText = passwortField.gameObject.transform.Find("Error-Text").gameObject;
        VPErrorText = VPInputField.gameObject.transform.Find("Error-Text").gameObject;
    }

    public void StartTutorial()
    {

    }

    public void StartLocalGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void StartOnlineGame()
    {

    }

    public void OpenVLMenu()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CheckPassword()
    {
        string enteredPassword = "";
        enteredPassword = passwortField.text;

        if (enteredPassword.Equals("Apu"))
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
        string enteredVP = "";
        enteredVP = VPInputField.text;

        if (!enteredVP.Equals(""))
        {
            VPErrorText.SetActive(false);
            VLMenu.transform.Find("VP-Text").gameObject.GetComponent<Text>().text = "VP: "+ enteredVP;
            mainMenu.transform.Find("VP-Text").gameObject.GetComponent<Text>().text = "VP: " + enteredVP;
        }
        else
        {
            VPErrorText.SetActive(true);
        }
        VPInputField.text = "";

    }


}
