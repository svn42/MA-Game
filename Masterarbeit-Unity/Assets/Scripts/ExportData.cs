/************************************************************************************

Dieses Skript bietet Funktionen um gesammelte Daten in .csv-Datein zu schreiben und
zu exportieren. Diese Funktionen werden vom ObjectManager genutzt.

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class ExportData : MonoBehaviour
{
    public string path;
    private string sceneName;
    private string subjectName;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        path = "ResearchData/csv/";
        //Erzeugt den Ordner, falls er noch nicht vorhanden
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    void Update()
    {

    }
}

    /*
    public string path;
    private string sceneName;
    private string subjectName;

    // Use this for initialization
    void Start()
    {

        sceneName = SceneManager.GetActiveScene().name;
        subjectName = MetaSettings.instance.subjectName;
        path = "ResearchData/csv/";

        //Erzeugt den Ordner, falls er noch nicht vorhanden
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Schreibt alle Cues in eine Spalte
    public void writeAllObjects(StreamWriter file, List<GameObject> registeredVisualCues, List<GameObject> registeredAuditiveCues)
    {
        for (int i = 0; i < registeredVisualCues.Count; i++)
        {
            if (i < registeredVisualCues.Count - 1)
            {
                file.Write(registeredVisualCues[i].name + "-" + registeredVisualCues[i].transform.position + ", ");
            }
            else
            {
                file.Write(registeredVisualCues[i].name + "-" + registeredVisualCues[i].transform.position);

                if (registeredAuditiveCues.Count > 0)
                {
                    file.Write(", ");
                }
            }
        }

        for (int i = 0; i < registeredAuditiveCues.Count; i++)
        {
            if (i < registeredAuditiveCues.Count - 1)
            {
                file.Write(registeredAuditiveCues[i].name + "-" + registeredAuditiveCues[i].transform.position + ", ");
            }
            else
            {
                file.Write(registeredAuditiveCues[i].name + "-" + registeredAuditiveCues[i].transform.position);
            }
        }

        file.Write(";");
    }

    //Schreibt alle gesehene Objekte in eine Spalte
    public void writeAllViewedObjects(StreamWriter file, List<string> viewedObjects)
    {
        viewedObjects = Helper.removeDuplicates(viewedObjects);

        for (int i = 0; i < viewedObjects.Count; i++)
        {
            if (i < viewedObjects.Count - 1)
            {
                file.Write(viewedObjects[i] + ", ");
            }
            else
            {
                file.Write(viewedObjects[i]);
            }
        }
        file.Write(";");
    }

    //Schreibt Informationen aller Visual Cues in mehreren Spalten
    public void writeVisualCues(StreamWriter file, List<GameObject> registeredVisualCues)
    {
        foreach (GameObject vc in registeredVisualCues)
        {
            file.Write(vc.tag + ";");
            file.Write(vc.GetComponent<VisualCueObject>().viewDuration + ";");
            file.Write(vc.GetComponent<VisualCueObject>().viewFrequency + ";");
            file.Write(vc.GetComponent<VisualCueTimer>().timeTillView + ";");
            file.Write(vc.GetComponent<VisualCueTimer>().timer + ";");
            file.Write(vc.name + ": Verwendete StartTrigger;"); //TODO Gibt es vers. Methoden um den Timer/Cue zu starten?
            file.Write(vc.GetComponent<VisualCueTimer>().stopTriggerMode + ";");
        }
    }

    //Schreibt Informationen aller Auditive Cues in mehreren Spalten
    public void writeAuditiveCues(StreamWriter file, List<GameObject> registeredAuditiveCues)
    {
        foreach (GameObject ac in registeredAuditiveCues)
        {
            file.Write(ac.tag + ";");
            file.Write(ac.GetComponent<AuditiveCueTimer>().timer + ";");
            file.Write(ac.GetComponent<AuditiveCueTimer>().mode + ";");
        }
    }

    // Schreibt das aktivierte End Objekt in eine Spalte
    public void writeEndObject(StreamWriter file, List<GameObject> registeredEndObjects)
    {
        foreach (GameObject eo in registeredEndObjects)
        {
            if (eo.GetComponent<EndLevel>().getIsEndObject())
            {
                file.Write(eo.name);
                break;
            }
        }
        file.Write(";");
    }

    // Exportiert Positionsdaten in eine .cvs-Datei
    public void exportPositionData(List<Vector3> positionData)
    {
        // Positionsdaten in Datei speichern.
        StreamWriter file = File.CreateText(path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + "_VP_" + subjectName + "_Position_" + sceneName + ".csv");

        file.WriteLine("Index; Position");

        for (int i = 0; i < positionData.Count; i++)
        {
            file.WriteLine(i + ";" + positionData[i]);
        }
        file.Close();
    }

    // Exportiert Rotationdatn in eine .cvs-Datei
    public void exportRotationData(List<Quaternion> rotationData)
    {
        StreamWriter file = File.CreateText(path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + "_VP_" + subjectName + "_Rotation_" + sceneName + ".csv");

        file.WriteLine("Index; Rotation");

        for (int i = 0; i < rotationData.Count; i++)
        {
            file.WriteLine(i + ";" + "(" + rotationData[i].x + ", " + rotationData[i].y + ", " + rotationData[i].z + ", " + rotationData[i].w + ")");
        }
        file.Close();
    }

    // Exportiert gesehene Objekte in eine .cvs-Datei
    public void exportViewedData(List<string> viewedObjects)
    {
        StreamWriter file = File.CreateText(path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + "_VP_" + subjectName + "_ViewedObjects_" + sceneName + ".csv");

        file.WriteLine("Index; Viewed Object");
        for (int i = 0; i < viewedObjects.Count; i++)
        {
            file.WriteLine(i + ";" + viewedObjects[i]);
        }
        file.Close();
    }

}
*/
  