using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExportTutorialData : MonoBehaviour {

	private string path;
	string time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveData(){
		time = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

		//Erzeugt den Ordner, falls er noch nicht vorhanden
		if (!Directory.Exists("ResearchData/TutorialData/"))
		{
			Directory.CreateDirectory("ResearchData/TutorialData/");
		}

		path = "ResearchData/TutorialData/TutorialData.csv";

		//Falls noch keine .csv-Datei vorhanden ist
		if (!File.Exists (path)) {
			//Erzeuge neue .csv-Datei und 
			using (StreamWriter newFile = File.CreateText (path)) {
				//Schreibe alle Spaltennamen in erste Zeile
				newFile.Write ("VP;");
				newFile.Write ("Datum;");
				newFile.Write ("TotalRating;");
				newFile.Write ("ZoneStopAndGo;");
				newFile.Write ("StunChallenge;");
				newFile.Write ("ShotStatic;");
				newFile.Write ("ShotDynamic;");
				newFile.Write ("BlockPlacement;");
				newFile.Write ("BlockDestroy;");
				newFile.Write ("ScoreGoals;");
				newFile.Write ("ScoreGoalsHard;");
				newFile.Write ("Goalkeeper;");

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
		int vp = PlayerPrefs.GetInt ("VP");
		file.Write ("\n");
		file.Write (vp+";");
		file.Write (time + ";");
		file.Write (PlayerPrefs.GetInt(vp+"Rating")+";");
		file.Write (PlayerPrefs.GetInt(vp+"ZoneStopAndGo")+";");
		file.Write (PlayerPrefs.GetInt(vp+"StunChallenge")+";");
		file.Write (PlayerPrefs.GetInt(vp+"ShotStatic")+";");
		file.Write (PlayerPrefs.GetInt(vp+"ShotDynamic")+";");
		file.Write (PlayerPrefs.GetInt(vp+"BlockPlacement")+";");
		file.Write (PlayerPrefs.GetInt(vp+"BlockDestroy")+";");
		file.Write (PlayerPrefs.GetInt(vp+"ScoreGoals")+";");
		file.Write (PlayerPrefs.GetInt(vp+"ScoreGoalsHard")+";");
		file.Write (PlayerPrefs.GetInt(vp+"Goalkeeper")+";");
		file.Close();
	}

}

