/************************************************************************************

Dieses Skript rundet Positionsdaten und schreibt sie in eine Dictaionary.
Aus den gerundeteten Daten werden Heatmap Marker erzeugt und je nach Häufigkeit
unterschiedlich gefärbt.

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HeatMap : MonoBehaviour {

    //Settings
    public float heatMapHeight;

    //Werte von 1 - 20. Legt fest, wie sensibel sich die Heatmap verfärben soll. 
    [Range(1, 20)]
    public int generalHeatmapMarkerSensitivity;
    [Range(0.1f, 0.9f)]
    public float joinedHeatmapMarkerSensitivity;

    public Dictionary<Vector3, float> heatmapData;
    private GameObject heatMapObject;

    //Prefabs
    public GameObject heatmapmarker;

    public Gradient gradient;

    // Use this for initialization
    void Start () {

        //Erzeugt ein leeres GameObject, welches alle Heatmap Marker beinhalten wird.
        heatMapObject = new GameObject("Heat_Map");
        heatMapObject.transform.SetParent(gameObject.transform);

        heatmapData = new Dictionary<Vector3, float>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Berechnet Heatmap-Koordinaten für alle geladene Versuchspersonen
    public void generateCombinedHeatmapCoordinates(List<Subject> subjects)
    {
        Debug.Log("Start: " + System.DateTime.Now.ToString("HH-mm-ss-fff"));
        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i].selected)
            {
                generateHeatmapCoordinates(subjects[i].positions);
            }
        }
        Debug.Log("End: " + System.DateTime.Now.ToString("HH-mm-ss-fff"));
    }

    //Erzeugt Heatmap-Koordinaten aus einer 
    public Dictionary<Vector3, float> generateHeatmapCoordinates(List<Vector3> coordinates)
    {
        
        // Heatmap aufbauen
        // Positionsdaten durchgehen
        for (int i = 0; i < coordinates.Count; i++)
        {

            float valueData; // aktuelle Vector3 Position aus der HashMap

            // Aktuelle Position auf ganze Zahlen runden 
            // Koordinaten x und z werden auf ganze Zahlen gerundet. 
            // Y Koordinate wird auf 'heatmapHight' gesetzt, damit diese über der Map schwebt.
            Vector3 roundedData = new Vector3(Mathf.Round(coordinates[i].x), heatMapHeight, Mathf.Round(coordinates[i].z));

            // Liste der an die aktuelle Position angrenzenden Flächen
            List<Vector3> joinedMarker = new List<Vector3>();

            //Füllen des Nachbarfeld-Arrays
            joinedMarker.Add(new Vector3(roundedData.x + 1, heatMapHeight, roundedData.z - 1));
            joinedMarker.Add(new Vector3(roundedData.x, heatMapHeight, roundedData.z - 1));
            joinedMarker.Add(new Vector3(roundedData.x - 1, heatMapHeight, roundedData.z - 1));
            joinedMarker.Add(new Vector3(roundedData.x - 1, heatMapHeight, roundedData.z));
            joinedMarker.Add(new Vector3(roundedData.x - 1, heatMapHeight, roundedData.z + 1));
            joinedMarker.Add(new Vector3(roundedData.x, heatMapHeight, roundedData.z + 1));
            joinedMarker.Add(new Vector3(roundedData.x + 1, heatMapHeight, roundedData.z + 1));
            joinedMarker.Add(new Vector3(roundedData.x + 1, heatMapHeight, roundedData.z));


            if (!heatmapData.ContainsKey(roundedData))
            {
                // Position ist noch NICHT HashMap gespeichert ist
                // Häufigkeit auf 1 setzen
                heatmapData.Add(roundedData, 1);

                // Benachbarten Flächen hinzufügen
                // Häufigkeit jeweils um 0.5 hochsetzen
                for (int j = 0; j < joinedMarker.Count; j++)
                {
                    //TODO Denkfehler? neue benachbarte können auch schon existieren!
                    /*
                    heatmapData.TryGetValue(joinedMarker[j], out valueData);
                    heatmapData.Remove(joinedMarker[j]);
                    heatmapData.Add(joinedMarker[j], valueData += joinedHeatmapMarkerSensitivity);
                    */
                    
                    if (heatmapData.ContainsKey(joinedMarker[j]))
                    {
                        heatmapData[joinedMarker[j]] = heatmapData[joinedMarker[j]] + joinedHeatmapMarkerSensitivity;
                    }
                    else
                    {
                        heatmapData.Add(joinedMarker[j], joinedHeatmapMarkerSensitivity);
                    }
                    
                }

            }
            else
            {
                // Position ist bereits in HashMap gespeichert
                // Position aus HashMap holen und Häufigkeit in 'valueData' zwischenspeichern
                // Position mit neuer Häufigkeit in HashMap speichern 

                //schneller
               heatmapData[roundedData] = heatmapData[roundedData] + 1;
                
                /*
                heatmapData.TryGetValue(roundedData, out valueData);
                heatmapData.Remove(roundedData);
                heatmapData.Add(roundedData, valueData += 1);
                */

                // Häufigkeit für benachbarte Positionen ebenfalls hochzählen
                // Falls benachbarte Position noch nicht vorhanden ist, benachbarten Flächen hinzufügen
                for (int j = 0; j < joinedMarker.Count; j++)
                {
                    if(heatmapData.ContainsKey(joinedMarker[j]))
                    {
                        heatmapData[joinedMarker[j]] = heatmapData[joinedMarker[j]] + joinedHeatmapMarkerSensitivity;
                    }
                    else
                    {
                        heatmapData.Add(joinedMarker[j], joinedHeatmapMarkerSensitivity);
                    }
                    /*
                    heatmapData.TryGetValue(joinedMarker[j], out valueData);
                    heatmapData.Remove(joinedMarker[j]);
                    heatmapData.Add(joinedMarker[j], valueData += joinedHeatmapMarkerSensitivity);
                    */
                }

            }
        }
        return heatmapData;
    }

    //Erstellt für jede Heatmap-Koordinate einen Heatmap-Marker in der Szene
    //Mit einer dynamischen Farbverteilung
    public void generateCombinedHeatmap()
    {
        float min = heatmapData.First().Value;
        float max = 0;
        float total = 0;
        foreach (KeyValuePair<Vector3, float> kvp in heatmapData)
        {
            if (kvp.Value > max)
            {
                max = kvp.Value;
            }

            if (kvp.Value < min)
            {
                min = kvp.Value;
            }
            total = total + kvp.Value;
        }

        //float step = (max - min) / 9;
        float average = total / heatmapData.Count;
        float step = 1f / (average * 4 );
        
        //red ab 90% des max werts
        //blau ist ab min wert

        Debug.Log("Min: " + min);
        Debug.Log("Max: " + max);
        Debug.Log("Step: " + step);
        Debug.Log("Average: " + average);

        // Positionsdaten aus HashMap instanziieren
        foreach (KeyValuePair<Vector3, float> kvp in heatmapData)
        {
           // Debug.Log(kvp.Value);
            // Prefab für Heatmapmarker
            GameObject heatmapMarker = (GameObject)Instantiate(heatmapmarker, new Vector3(kvp.Key.x, kvp.Key.y, kvp.Key.z), Quaternion.identity);
            heatmapMarker.transform.SetParent(heatMapObject.transform);
            heatmapMarker.GetComponent<Renderer>().material.color = gradient.Evaluate(kvp.Value * step);
        }
    }

    //Erstellt für jede Heatmap-Koordinate einen Heatmap-Marker in der Szene
    //Mit einer statischen Farbverteilung
    public void generateSingleHeatmap(Dictionary<Vector3, float> heatmpaData)
    {
        // Positionsdaten aus HashMap instanziieren
        foreach (KeyValuePair<Vector3, float> kvp in heatmapData)
        {

            // Prefab für Heatmapmarker
            GameObject heatmapMarker = (GameObject)Instantiate(heatmapmarker, new Vector3(kvp.Key.x, kvp.Key.y, kvp.Key.z), Quaternion.identity);
            heatmapMarker.transform.SetParent(heatMapObject.transform);

            // HeatmapMarker je nach Häufigkeit Farben zuweisen
            // darkblue -> lightblue -> darkTurqoise-> green -> yellowGreen -> yellow -> lightOrange -> darkOrange -> red 

            if (kvp.Value <= 30 / generalHeatmapMarkerSensitivity) //darkblue
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(0, 0, 0.5f, 0.5f);
            }
            if (kvp.Value > 30 / generalHeatmapMarkerSensitivity && kvp.Value <= 45 / generalHeatmapMarkerSensitivity) //lightblue
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(0, 0.5f, 1f, 0.5f);
            }
            if (kvp.Value > 45 / generalHeatmapMarkerSensitivity && kvp.Value <= 60 / generalHeatmapMarkerSensitivity) //darkTurqoise
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(0, 0.8f, 0.8f, 0.5f);
            }
            if (kvp.Value > 60 / generalHeatmapMarkerSensitivity && kvp.Value <= 75 / generalHeatmapMarkerSensitivity) //green
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);
            }
            if (kvp.Value > 75 / generalHeatmapMarkerSensitivity && kvp.Value <= 90 / generalHeatmapMarkerSensitivity) //yellowGreen
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(0.6f, 0.8f, 0, 0.5f);
            }
            if (kvp.Value > 90 / generalHeatmapMarkerSensitivity && kvp.Value <= 105 / generalHeatmapMarkerSensitivity) //yellow
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 0.5f);
            }
            if (kvp.Value > 105 / generalHeatmapMarkerSensitivity && kvp.Value <= 120 / generalHeatmapMarkerSensitivity) //lightOrange
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(1, 0.33f, 0f, 0.5f);
            }
            if (kvp.Value > 120 / generalHeatmapMarkerSensitivity && kvp.Value <= 135 / generalHeatmapMarkerSensitivity) //darkOrange
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(1, 0.66f, 0f, 0.5f);
            }
            if (kvp.Value > 135 / generalHeatmapMarkerSensitivity) //red
            {
                heatmapMarker.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
            }
            Debug.Log(kvp.Value);
        }
    }

    //Löscht alle Heatmap-Marker aus der Szene
    public void deleteHeatmap()
    {
        Destroy(heatMapObject);
        heatMapObject = new GameObject("Heat_Map");
        heatMapObject.transform.SetParent(gameObject.transform);
    }

    //Deaktiviert alle Heatmap-Marker in der Szene
    public void hideHeatMapObject()
    {
        heatMapObject.SetActive(false);
    }
    //Aaktiviert alle Heatmap-Marker in der Szene
    public void showHeatMapObject()
    {
        heatMapObject.SetActive(true);
    }
}
