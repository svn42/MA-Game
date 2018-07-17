/************************************************************************************

Dieses Skript öffnet einen Folderbrowser und importiert aus allen .csv-Datein im Ordner
die Positions- und Rotationsdaten pro Versuchsperson. Extrahiert den Namen aus den Dateinamen.
Mittelt die Positionen.

************************************************************************************/

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Windows.Forms; //Get the DLL from C:\Program Files\Unity\Editor\Data\Mono\lib\mono\2.0 and add it in Asset/Plugin!
using UnityEngine.SceneManagement;
using System.Linq;

public class ImportSettings : MonoBehaviour {


    public ImportSettingsGUI importerGUI;

    // Use this for initialization
    void Start () {

        //Prüft auf fehlende Referenzen/Komponente
        importerGUI = GetComponent<ImportSettingsGUI>();
        if(importerGUI == null)
        {
            Debug.LogError("There is no ImporterGUI script attached to this gameObject!");
        }

        //Initialisiert GUI
        importerGUI.initializeSubjectToggles();
        importerGUI.initializeMapToggles();
        importerGUI.generateSubjectList();
        importerGUI.setMetaText(Subjects.instance.subjects.Count);
        importerGUI.setPathText(Subjects.instance.folder);

        //loadFiles();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Start_Scene");
        }
    }
    //Liest die csv Datein aus dem angegebenen Pfad
    public void loadFiles()
    {
        FolderBrowserDialog objDialog = new FolderBrowserDialog();
        objDialog.RootFolder = Environment.SpecialFolder.MyComputer;
        objDialog.SelectedPath = UnityEngine.Application.dataPath;

        DialogResult result = objDialog.ShowDialog();
        if (result == DialogResult.OK)
        {
            Subjects.instance.folder = objDialog.SelectedPath;
            Subjects.instance.files = Directory.GetFiles(Subjects.instance.folder, "*.csv").ToList<string>();
        }
        Subjects.instance.subjects = generateSubjects(ref Subjects.instance.files);

        //Initialize GUI
        importerGUI.generateSubjectList();
        importerGUI.setPathText(Subjects.instance.folder);
        importerGUI.setMetaText(Subjects.instance.subjects.Count);
    }

    public void readFiles(List<Subject> subjects)
    {
        //Iterates through all subjects
        for (int i = 0; i < subjects.Count ; i++)
        {
            List<Vector3> positions = new List<Vector3>();

			//Read position vectors
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(subjects[i].positionPath))
                {
                    string line;

                    //skip first Line
                    line = sr.ReadLine();

                    //Read line by line
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Removes the Index and "("
                        line = line.Substring(line.IndexOf('(') + 1);
                        line = line.Substring(0, line.IndexOf(")"));
                        //Split remaining string by ",", which are the coordinates
                        string[] position = line.Split(',');

                        //Add read coordinates to the list
                        positions.Add(new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])));
                    }
                }
           }
           catch (Exception e)
            {
                // Let the user know what went wrong.
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
            }

            subjects[i].positions = positions;
        }
    }

    //Erzeugt aus den geladenen Daten Versuchspersonen
    public List<Subject> generateSubjects(ref List<string> files)
    {
        List<Subject> tempList = new List<Subject>();
        for (int i = 0; i < files.Count; i++)
        {
            if(!files[i].Contains("Position"))
            {
                    files.RemoveAt(i);
                    i--;
            }
            else
            {
                string subjectName = extractSubjectName(files[i]);
                Subject tempSubject = tempList.Find(subject => subject.subjectName == subjectName);
                //Subject not generated yet, so generate new subject
                if (tempSubject == null)
                {
                    tempSubject = new Subject();
                    tempSubject.subjectName = subjectName;
                    tempList.Add(tempSubject);

                }
                //Add Path
                if (files[i].Contains("Position"))
                {
                    tempSubject.positionPath = files[i];
                }

            }
        }
        return tempList;
    }

    public void startScene()
    {
        setSelectedSubjects();
        readFiles(Subjects.instance.subjects);
        Subjects.instance.averagePositions = calculateAveragePositions(Subjects.instance.subjects);
        
		//if  toggle level 1

		//SceneManager.LoadScene(Level1_Analysis);
    }

    //Extrahiert den Namen der Versuchsperson
    //Unsicher! Name must noch contain "_", "Rotation", etc.
    public string extractSubjectName(string file)
    {
        file = file.Substring(file.IndexOf("VP_") + 3);
        file = file.Substring(0, file.IndexOf("_"));
        return file;
    }

    //Berechnet die durschnittliche Position aller Versuchspersonen
    public List<Vector3> calculateAveragePositions(List<Subject> subjects)
    {
        List<Vector3> averagePositions = new List<Vector3>();

        int biggestList = 0;

        foreach(Subject s in subjects)
        {
            if(s.positions.Count>biggestList)
            {
                biggestList = s.positions.Count;
            }
        }

        for(int i = 0; i < biggestList; i++)
        {

            Vector3 point = new Vector3(0, 0, 0);

            //Counts how much coordinates were added
            int counter = 0; 

            foreach (Subject subject in subjects)
            {
                //Only add if the list of positions has elements
                if(i<subject.positions.Count)
                {
                    point = point + subject.positions[i];
                    counter++;
                }
            }
            //calculates average point
            point = point / counter;
            averagePositions.Add(point);
        }

        return averagePositions;
    }

 
    //Durchläuft alle Toggles und aktiviert/deaktiviert die Daten der Versuchspersonen
    public void setSelectedSubjects()
    {
        for(int i = 0; i < importerGUI.toggles.Count; i++)
        {
           Subjects.instance.subjects[i].selected = importerGUI.toggles[i].isOn;
        }
    }
}




