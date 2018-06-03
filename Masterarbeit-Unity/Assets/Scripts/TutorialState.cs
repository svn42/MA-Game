using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialState : MonoBehaviour
{
    public GameObject gui;
    private Text ratingText;
    private Image greenCheck;
    public int rating;
    public int vpNummer;

    private void Start()
    {
        vpNummer = PlayerPrefs.GetInt("VP");
        greenCheck = gui.transform.Find("PauseScreen").gameObject.transform.Find("Spieler1").transform.Find("Spieler1_Check").GetComponent<Image>(); ;
        rating = PlayerPrefs.GetInt(vpNummer.ToString()+"Rating");
        if (SceneManager.GetActiveScene().name.Equals("TutorialEnd"))
        {
            ratingText = gui.transform.Find("PauseScreen").gameObject.transform.Find("TransparentScreen").transform.Find("RatingText").GetComponent<Text>(); ;
            ratingText.text = rating.ToString();
        }
    }

    void Update()
    {
        CheckInput();
    }


    public void CheckInput()
    {
        //wenn der Schuss-Button (A) losgelassen wird und der ShotTimer größer ist als die gewünschte Wartezeit zwischen zwei Schüssen 
        if (Input.GetButtonUp("ShootP1"))
        {
            if (SceneManager.GetActiveScene().name.Equals("TutorialEnd"))
            {
                PlayerPrefs.SetString(vpNummer.ToString() + "TutorialSolved", "Yes");
                PlayerPrefs.SetInt("CurrentVP", vpNummer);
                StartCoroutine(LoadNextScene("MainMenu"));

            }
            else
            {
                StartCoroutine(LoadNextScene());
            }
            PlayerPrefs.SetInt(vpNummer.ToString() + "Rating", rating);
        }

    }

    IEnumerator LoadNextScene()
    {
        greenCheck.enabled = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadNextScene(string sceneName)
    {
        greenCheck.enabled = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }
}