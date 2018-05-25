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
    private GlobalTimer globalTimer;
    PlayerLogging playerLoggingPlayer1;
    PlayerLogging playerLoggingPlayer2;
    public GameObject player;


    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        globalTimer = (GlobalTimer)FindObjectOfType(typeof(GlobalTimer));

        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].GetComponent<Player>().playerTeam == 1)
            {
                player = playerList[i];
                playerLoggingPlayer1 = player.GetComponent<PlayerLogging>();
            }
            else if (playerList[i].GetComponent<Player>().playerTeam == 2)
            {
                player = playerList[i];
                playerLoggingPlayer2 = player.GetComponent<PlayerLogging>();
            }
        }
    }
    void Update()
    {

    }

    public void ExportAllData()
    {
        //Erzeugt den Ordner, falls er noch nicht vorhanden
        if (!Directory.Exists("ResearchData/csv/"))
        {
            Directory.CreateDirectory("ResearchData/csv/");
        }
        path = "ResearchData/csv/testData" + sceneName + ".csv";

        //Falls noch keine .csv-Datei vorhanden ist
        if (!File.Exists(path))
        {
            //Erzeuge neue .csv-Datei und 
            using (StreamWriter newFile = File.CreateText(path))
            {
                //Schreibe alle Spaltennamen in erste Zeile
                newFile.Write("VP;");
                newFile.Write("Szene;");
                newFile.Write(sceneName + "_Startzeit;");
                newFile.Write(sceneName + "_Endzeit;");
                newFile.Write(sceneName + "_PlayTime;");
                newFile.Write(sceneName + "_TotalTime;");
                newFile.Write(sceneName + "_ZurueckgelegteDistanz;");
                //Resultat
                newFile.Write(sceneName + "Resultat;");
                newFile.Write(sceneName + "_Tore_erzielt;");
                newFile.Write(sceneName + "_Tore_kassiert;");
                //Art der Tore
                newFile.Write(sceneName + "_Richtige_Tore;");
                newFile.Write(sceneName + "_Eigentore;");
                //Zeiten
                //time per result
                newFile.Write(sceneName + "_Zeit_im_Vorsprung;");
                newFile.Write(sceneName + "_Zeit_im_Remis;");
                newFile.Write(sceneName + "_Zeit_im_Rueckstand;");
                //time per result percent
                newFile.Write(sceneName + "_Anteil_Zeit_im_Vorsprung;");
                newFile.Write(sceneName + "_Anteil_Zeit_im_Remis;");
                newFile.Write(sceneName + "_Anteil_Zeit_im_Rueckstand;");

                //ZoneTime
                newFile.Write(sceneName + "_Zeit_in_eigener_Tor_Zone;");
                newFile.Write(sceneName + "_Zeit_in_eigener_Zone;");
                newFile.Write(sceneName + "_Zeit_in_Mittelzone;");
                newFile.Write(sceneName + "_Zeit_in_gegnerischer_Zone;");
                newFile.Write(sceneName + "_Zeit_in_gegnerischer_Tor_Zone;");
                //ZoneTime Percent
                newFile.Write(sceneName + "_Anteil_Zeit_in_eigener_Tor_Zone;");
                newFile.Write(sceneName + "_Anteil_Zeit_in_eigener_Zone;");
                newFile.Write(sceneName + "_Anteil_Zeit_in_Mittelzone;");
                newFile.Write(sceneName + "_Anteil_Zeit_in_gegnerischer_Zone;");
                newFile.Write(sceneName + "_Anteil_Zeit_in_gegnerischer_Tor_Zone;");
                //shots
                newFile.Write(sceneName + "_Gesamtanzahl_an_Schuessen;");
                newFile.Write(sceneName + "_Normale_Schuesse;");
                newFile.Write(sceneName + "_Mittlere_Schuesse;");
                newFile.Write(sceneName + "_Grosse_Schuesse;");
                //shots percent
                newFile.Write(sceneName + "_Anteil_Normale_Schuesse;");
                newFile.Write(sceneName + "_Anteil_Mittlere_Schuesse;");
                newFile.Write(sceneName + "_Anteil_Grosse_Schuesse;");
                //accuracy
                newFile.Write(sceneName + "_Gesamtzahl_getroffener_Objekte;");
                newFile.Write(sceneName + "_Bloecke_getroffen;");
                newFile.Write(sceneName + "_Baelle_getroffen;");
                newFile.Write(sceneName + "_Gegner_getroffen;");
                newFile.Write(sceneName + "_gegnerischer_Schuss_zerstoert;");
                newFile.Write(sceneName + "_ohne_Ziel_zerstoert;");
                //accuracy_percent
                newFile.Write(sceneName + "_Anteil_Bloecke_getroffen;");
                newFile.Write(sceneName + "_Anteil_Baelle_getroffen;");
                newFile.Write(sceneName + "_Anteil_Gegner_getroffen;");
                newFile.Write(sceneName + "_Anteil_gegnerischer_Schuss_zerstoert;");
                newFile.Write(sceneName + "_Anteil_ohne_Ziel_zerstoert;");
                //Blocks
                newFile.Write(sceneName + "_Gesamtanzahl_platzierter_Bloecke;");
                newFile.Write(sceneName + "_Bloecke_eigene_Tor_Zone;");
                newFile.Write(sceneName + "_Bloecke_eigene_Zone;");
                newFile.Write(sceneName + "_Bloecke_Mittelzone;");
                newFile.Write(sceneName + "_Bloecke_gegnerische_Zone;");
                newFile.Write(sceneName + "_Bloecke_gegnerische_Tor_Zone;");
                //Blocks percent
                newFile.Write(sceneName + "_Anteil_Bloecke_eigene_Tor_Zone;");
                newFile.Write(sceneName + "_Anteil_Bloecke_eigene_Zone;");
                newFile.Write(sceneName + "_Anteil_Bloecke_Mittelzone;");
                newFile.Write(sceneName + "_Anteil_Bloecke_gegnerische_Zone;");
                newFile.Write(sceneName + "_Anteil_Bloecke_gegnerische_Tor_Zone;");
                //Opponent Stunned
                newFile.Write(sceneName + "_Gesamtanzahl_ausgefuehrte_Stuns;");
                newFile.Write(sceneName + "_Gesamtdauer_ausgefuehrte_Stuns;");
                newFile.Write(sceneName + "_ausgefuehrte_normale_Stuns;");
                newFile.Write(sceneName + "_ausgefuehrte_mittlere_Stuns;");
                newFile.Write(sceneName + "_ausgefuehrte_grosse_Stuns;");
                //Stunned By Opponent
                newFile.Write(sceneName + "_Gesamtanzahl_erhaltene_Stuns;");
                newFile.Write(sceneName + "_Gesamtdauer_erhaltene_Stuns;");
                newFile.Write(sceneName + "_erhaltene_normale_Stuns;");
                newFile.Write(sceneName + "_erhaltene_mittlere_Stuns;");
                newFile.Write(sceneName + "_erhaltene_grosse_Stuns;");
                //Stunned By Ball
                newFile.Write(sceneName + "_Stunned_durch_Ball;");
                //Emotes
                newFile.Write(sceneName + "_Gesamtanzahl_Emotes;");
                newFile.Write(sceneName + "_Emote_Nice;");
                newFile.Write(sceneName + "_Emote_Angry;");
                newFile.Write(sceneName + "_Emote_Cry;");
                newFile.Write(sceneName + "_Emote_Haha;");
                //Emotes Percent
                newFile.Write(sceneName + "_Anteil_Emote_Nice;");
                newFile.Write(sceneName + "_Anteil_Emote_Angry;");
                newFile.Write(sceneName + "_Anteil_Emote_Cry;");
                newFile.Write(sceneName + "_Anteil_Emote_Haha;");
                /*    
                
            */
            }
        }
        //Schreibt Daten in die .csv-Datei
        using (StreamWriter file = File.AppendText(path))
        {
            WritePlayerLoggingData(file, playerLoggingPlayer1);
            WritePlayerLoggingData(file, playerLoggingPlayer2);
        }

    }

    public void WritePlayerLoggingData(StreamWriter file, PlayerLogging pL)
    {
        file.Write("\n");
        file.Write(pL.playerTeam + ";");    
        file.Write(sceneName + ";");
        file.Write(globalTimer.startTime + ";");
        file.Write(globalTimer.endTime + ";");
        file.Write(globalTimer.playTime + ";");
        file.Write(globalTimer.totalTime + ";");
        file.Write(pL.distanceTravelled + ";");
        //Resultat
        file.Write(pL.finalResult + ";");
        file.Write(pL.goalsScored + ";");
        file.Write(pL.goalsConceded + ";");
        //Art der Tore
        file.Write(pL.correctGoalsScored + ";");
        file.Write(pL.ownGoalsScored + ";");
        //Zeit
        //time per result
        file.Write(pL.timeInLead + ";");
        file.Write(pL.timeTied + ";");
        file.Write(pL.timeInDeficit + ";");
        //time per result percent
        file.Write(pL.timeInLeadPercent + ";");
        file.Write(pL.timeTiedPercent + ";");
        file.Write(pL.timeInDeficitPercent + ";");
        //Zonetime
        file.Write(pL.timeOwnGoalZone + ";");
        file.Write(pL.timeOwnZone + ";");
        file.Write(pL.timeCenterZone + ";");
        file.Write(pL.timeOpponentZone + ";");
        file.Write(pL.timeOpponentGoalZone + ";");
        //Zone Percent
        file.Write(pL.timeOwnGoalZonePercent + ";");
        file.Write(pL.timeOwnZonePercent + ";");
        file.Write(pL.timeCenterZonePercent + ";");
        file.Write(pL.timeOpponentZonePercent + ";");
        file.Write(pL.timeOpponentGoalZonePercent + ";");
        //Schüsse
        file.Write(pL.totalShotsFired + ";");
        file.Write(pL.normalShotsFired + ";");
        file.Write(pL.mediumShotsFired + ";");
        file.Write(pL.largeShotsFired + ";");
        //Schüsse Percent
        file.Write(pL.normalShotsFiredPercent + ";");
        file.Write(pL.mediumShotsFiredPercent + ";");
        file.Write(pL.largeShotsFiredPercent + ";");
        //accuracy
        file.Write(pL.totalObjectsHit + ";");
        file.Write(pL.shotsHitBlock + ";");
        file.Write(pL.shotsHitBall + ";");
        file.Write(pL.shotsHitPlayer + ";");
        file.Write(pL.shotsHitEnemyShot + ";");
        file.Write(pL.shotsDestroyed + ";");
        //Accuracy Percent
        file.Write(pL.shotsHitBlockPercent + ";");
        file.Write(pL.shotsHitBallPercent + ";");
        file.Write(pL.shotsHitPlayerPercent + ";");
        file.Write(pL.shotsHitEnemyShotPercent + ";");
        file.Write(pL.shotsDestroyedPercent + ";");
        //Blocks
        file.Write(pL.totalBlocksPlaced + ";");
        file.Write(pL.blocksInOwnGoalZone + ";");
        file.Write(pL.blocksInOwnZone + ";");
        file.Write(pL.shotsHitPlayer + ";");
        file.Write(pL.blocksInCenterZone + ";");
        file.Write(pL.blocksInOpponentGoalZone + ";");
        //Blocks percent
        file.Write(pL.blocksInOwnZonePercent + ";");
        file.Write(pL.blocksInOwnGoalZonePercent + ";");
        file.Write(pL.blocksInCenterZonePercent + ";");
        file.Write(pL.blocksInOpponentZonePercent + ";");
        file.Write(pL.blocksInOpponentGoalZonePercent + ";");
        //Opponent Stunned
        file.Write(pL.totalEnemyStunned + ";");
        file.Write(pL.enemyStunnedTotalTime + ";");
        file.Write(pL.normalEnemyStunned + ";");
        file.Write(pL.mediumEnemyStunned + ";");
        file.Write(pL.largeEnemyStunned + ";");
        //Stunned by Opponent
        file.Write(pL.totalStunnedByEnemy + ";");
        file.Write(pL.stunnedByEnemyTotalTime + ";");
        file.Write(pL.normalStunnedByEnemy + ";");
        file.Write(pL.mediumStunnedByEnemy + ";");
        file.Write(pL.largeStunnedByEnemy + ";");

        //Stunned by ball
        file.Write(pL.stunnedByBall + ";");
        //Emotes
        file.Write(pL.totalEmotes + ";");
        file.Write(pL.emoteNice + ";");
        file.Write(pL.emoteAngry + ";");
        file.Write(pL.emoteCry + ";");
        file.Write(pL.emoteHaha + ";");
        //Emotes Percent
        file.Write(pL.emoteNicePercent + ";");
        file.Write(pL.emoteAngryPercent + ";");
        file.Write(pL.emoteCryPercent + ";");
        file.Write(pL.emoteHahaPercent + ";");
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
