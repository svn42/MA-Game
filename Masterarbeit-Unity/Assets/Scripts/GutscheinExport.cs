using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GutscheinExport : MonoBehaviour {

	public Button saveDateButton;
	public Text saveText;
	public InputField contactInputField;
	private string path;
	public string contact;
	string time;
	private TutorialScreenState tutorialScreenState;

	// Use this for initialization
	void Start () {
		tutorialScreenState = (TutorialScreenState)FindObjectOfType(typeof(TutorialScreenState));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadMainMenu(){
		SceneManager.LoadScene("MainMenu");
	}

	public void SaveData(){
		contact = contactInputField.text;
		time = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");



		saveText.enabled = true;
		saveDateButton.gameObject.SetActive (false);

		//Erzeugt den Ordner, falls er noch nicht vorhanden
		if (!Directory.Exists("ResearchData/GutscheinData/"))
		{
			Directory.CreateDirectory("ResearchData/GutscheinData/");
		}

		path = "ResearchData/GutscheinData/GutscheinDaten.csv";

		//Falls noch keine .csv-Datei vorhanden ist
		if (!File.Exists (path)) {
			//Erzeuge neue .csv-Datei und 
			using (StreamWriter newFile = File.CreateText (path)) {
				//Schreibe alle Spaltennamen in erste Zeile
				newFile.Write ("Rating;");
				newFile.Write ("Kontakt;");
				newFile.Write ("Datum;");
			}
		}

		//Schreibt Daten in die .csv-Datei
		using (StreamWriter file = File.AppendText(path))
		{
			WriteData(file);
		}

	}

	public void WriteData(StreamWriter file)
	{
		file.Write ("\n");
		file.Write (tutorialScreenState.rating + ";");
		file.Write (contact + ";");
		file.Write (time + ";");
	}
}
