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
    private string gameType;
    public string path;
    private string sceneName;
    private string sceneNameAbbreviation;
    private string subjectName;
    private GlobalTimer globalTimer;
    PlayerLogging playerLoggingPlayer1;
    PlayerLogging playerLoggingPlayer2;
    public GameObject player;


    void Start()
    {
        
    }
    void Update()
    {

    }

    public void FindPlayerLogging(string type)
    {
       
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

    public void StartUpExportData()
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Level 1":
                sceneNameAbbreviation = "L1";
                break;
            case "Level 2":
                sceneNameAbbreviation = "L2";
                break;
            case "Level 3":
                sceneNameAbbreviation = "L3";
                break;
        }

        globalTimer = (GlobalTimer)FindObjectOfType(typeof(GlobalTimer));

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
                newFile.Write("VP_Gegner;");
                newFile.Write("Szene;");
                newFile.Write("Spielmodus;");

                newFile.Write(sceneNameAbbreviation + "_Startzeit;");
                newFile.Write(sceneNameAbbreviation + "_Endzeit;");

                newFile.Write("Rating;");
                newFile.Write("Rating_Gegner;");

                //distance
                newFile.Write(sceneNameAbbreviation + "_Totale_Bewegung;");
                newFile.Write(sceneNameAbbreviation + "_Totale_Bewegung_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Totale_Bewegung_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Totale_Bewegung_Rueckstand;");

                //Resultat
                newFile.Write(sceneNameAbbreviation + "_Resultat;");
                newFile.Write(sceneNameAbbreviation + "_Endbedingung;");
                newFile.Write(sceneNameAbbreviation + "_Tore;");
                newFile.Write(sceneNameAbbreviation + "_Gegentore;");
                //Art der Tore
                newFile.Write(sceneNameAbbreviation + "_Richtige_Tore;");
                newFile.Write(sceneNameAbbreviation + "_Eigentore;");
                //Zeiten
                newFile.Write(sceneNameAbbreviation + "_PlayTime;");
                newFile.Write(sceneNameAbbreviation + "_TotalTime;");
                //time per result
                newFile.Write(sceneNameAbbreviation + "_Zeit_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_Rueckstand;");
                ////time per result percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_im_Vorsprung;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_im_Remis;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_im_Rueckstand;");

                //ZoneTime
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Torzone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Zone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_Mittelzone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Zone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Torzone;");
                ////ZoneTime Percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Tor_Zone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Zone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_Mittelzone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Zone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Tor_Zone;");

                //time per zone per result

                newFile.Write(sceneNameAbbreviation + "_Zeit_in_eigene_Torzone_Vorsprung;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Torzone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Torzone_Rueckstand;");

                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Zone_Vorsprung;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Zone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Zone_Rueckstand;");

                newFile.Write(sceneNameAbbreviation + "_Zeit_Mittelzone_Vorsprung;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_Mittelzone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_Mittelzone_Rueckstand;");

                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Zone_Vorsprung;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Zone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Zone_Rueckstand;");

                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Torzone_Vorsprung;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Torzone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Torzone_Rueckstand;");

                ////time per zone per result Percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Tor_Zone_im_Vorsprung;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Zone_im_Vorsprung;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_Mittelzone_im_Vorsprung;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Zone_im_Vorsprung;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Tor_Zone_im_Vorsprung;");

                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Tor_Zone_im_Remis;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Zone_im_Remis;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_Mittelzone_im_Remis;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Zone_im_Remis;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Tor_Zone_im_Remis;");

                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Tor_Zone_im_Rueckstand;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_eigener_Zone_im_Rueckstand;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_Mittelzone_im_Rueckstand;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Zone_im_Rueckstand;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Zeit_in_gegnerischer_Tor_Zone_im_Rueckstand;");

                //shots
                newFile.Write(sceneNameAbbreviation + "_Total_Schuesse;");
                newFile.Write(sceneNameAbbreviation + "_Normale_Schuesse;");
                newFile.Write(sceneNameAbbreviation + "_Mittlere_Schuesse;");
                newFile.Write(sceneNameAbbreviation + "_Grosse_Schuesse;");
                ////shots percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Normale_Schuesse;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Mittlere_Schuesse;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Grosse_Schuesse;");
                //shots per Result
                newFile.Write(sceneNameAbbreviation + "_Total_Schuesse_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Normale_Schuesse_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Mittlere_Schuesse_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Grosse_Schuesse_Fuehrung;");

                newFile.Write(sceneNameAbbreviation + "_Total_Schuesse_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Normale_Schuesse_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Mittlere_Schuesse_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Grosse_Schuesse_Remis;");

                newFile.Write(sceneNameAbbreviation + "_Total_Schuesse_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Normale_Schuesse_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Mittlere_Schuesse_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Grosse_Schuesse_Rueckstand;");



                //accuracy
                newFile.Write(sceneNameAbbreviation + "_Total_getroffene_Objekte;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Bloecke;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Baelle;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Gegner;");
                newFile.Write(sceneNameAbbreviation + "_gegnerischer_Schuss_zerstoert;");
                newFile.Write(sceneNameAbbreviation + "_Ziel_verfehlt;");
                ////accuracy_percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_getroffen_Bloecke_getroffen;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_getroffen_Baelle_getroffen;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_getroffen_Gegner_getroffen;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_getroffen__gegnerischer_Schuss_zerstoert;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Ziel_verfehlt;");
                //Accuracy per result
                newFile.Write(sceneNameAbbreviation + "_Total_getroffene_Objekte_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Bloecke_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Baelle_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Gegner_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_gegnerischer_Schuss_zerstoert_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Ziel_verfehlt_Fuehrung;");

                newFile.Write(sceneNameAbbreviation + "_Total_getroffene_Objekte_Remis;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Bloecke_Remis;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Baelle_Remis;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Gegner_Remis;");
                newFile.Write(sceneNameAbbreviation + "_gegnerischer_Schuss_zerstoert_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Ziel_verfehlt_Remis;");

                newFile.Write(sceneNameAbbreviation + "_Total_getroffene_Objekte_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Bloecke_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Baelle_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_getroffen_Gegner_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_gegnerischer_Schuss_zerstoert_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Ziel_verfehlt_Rueckstand;");

                //Blocks
                newFile.Write(sceneNameAbbreviation + "_Total_Bloecke;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Torzone;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Zone;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_Mittelzone;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Zone;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Torzone;");
                ////Blocks percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Bloecke_eigene_Torzone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Bloecke_eigene_Zone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Bloecke_Mittelzone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Bloecke_gegnerische_Zone;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Bloecke_gegnerische_Torzone;");
                //blocks per result
                newFile.Write(sceneNameAbbreviation + "_Total_Bloecke_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Torzone_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Zone_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_Mittelzone_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Zone_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Torzone_Fuehrung;");

                newFile.Write(sceneNameAbbreviation + "_Total_Bloecke_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Torzone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Zone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_Mittelzone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Zone_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Torzone_Remis;");

                newFile.Write(sceneNameAbbreviation + "_Total_Bloecke_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Torzone_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_eigene_Zone_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_Mittelzone_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Zone_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Bloecke_gegnerische_Torzone_Rueckstand;");

                //Opponent Stunned
                newFile.Write(sceneNameAbbreviation + "_Total_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_normale_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_mittlere_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_grosse_Stuns;");
                //enemy stunned per result
                newFile.Write(sceneNameAbbreviation + "_Total_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_normale_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_mittlere_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_grosse_Stuns_Fuehrung;");

                newFile.Write(sceneNameAbbreviation + "_Total_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_normale_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_mittlere_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_grosse_Stuns_Remis;");

                newFile.Write(sceneNameAbbreviation + "_Total_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_normale_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_mittlere_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_grosse_Stuns_Rueckstand;");

                //Stunned By Opponent
                newFile.Write(sceneNameAbbreviation + "_Total_erhaltene_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_erhaltene_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_normale_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_mittlere_Stuns;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_grosse_Stuns;");

                newFile.Write(sceneNameAbbreviation + "_Total_erhaltene_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_erhaltene_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_normale_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_mittlere_Stuns_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_grosse_Stuns_Fuehrung;");

                newFile.Write(sceneNameAbbreviation + "_Total_erhaltene_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_erhaltene_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_normale_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_mittlere_Stuns_Remis;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_grosse_Stuns_Remis;");

                newFile.Write(sceneNameAbbreviation + "_Total_erhaltene_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Dauer_erhaltene_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_normale_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_mittlere_Stuns_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_erhaltene_grosse_Stuns_Rueckstand;");

                ////Stunned By Ball
                //newFile.Write(sceneNameAbbreviation + "_Stunned_durch_Ball;");
                //newFile.Write(sceneNameAbbreviation + "_Stunned_durch_Ball_Fuehrung;");
                //newFile.Write(sceneNameAbbreviation + "_Stunned_durch_Ball_Remis;");
                //newFile.Write(sceneNameAbbreviation + "_Stunned_durch_Ball_Deficit;");

                //Emotes
                newFile.Write(sceneNameAbbreviation + "_Total_Emotes;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Nice;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Angry;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Cry;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Haha;");
                ////Emotes Percent
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Emote_Nice;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Emote_Angry;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Emote_Cry;");
                //newFile.Write(sceneNameAbbreviation + "_Anteil_Emote_Haha;");
                //Emotes per result
                newFile.Write(sceneNameAbbreviation + "_Total_Emotes_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Nice_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Angry_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Cry_Fuehrung;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Haha_Fuehrung;");

                newFile.Write(sceneNameAbbreviation + "_Total_Emotes_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Nice_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Angry_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Cry_Remis;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Haha_Remis;");

                newFile.Write(sceneNameAbbreviation + "_Total_Emotes_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Nice_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Angry_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Cry_Rueckstand;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Haha_Rueckstand;");
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
        file.Write(pL.subjectNr + ";");
        file.Write(pL.subjectNrEnemy + ";");

        file.Write(sceneName + ";");
        file.Write(pL.gameType + ";");
        file.Write(globalTimer.startTime + ";");
        file.Write(globalTimer.endTime + ";");

        file.Write(pL.rating + ";");
        file.Write(pL.ratingEnemy + ";");

        //Bewegung
        file.Write(pL.distanceTravelled + ";");
        file.Write(pL.distanceTravelledInLead + ";");
        file.Write(pL.distanceTravelledInTie + ";");
        file.Write(pL.distanceTravelledInDeficit + ";");

        //Resultat
        file.Write(pL.finalResult + ";");
        file.Write(pL.endingCondition + ";");
        file.Write(pL.goalsScored + ";");
        file.Write(pL.goalsConceded + ";");
        //Art der Tore
        file.Write(pL.correctGoalsScored + ";");
        file.Write(pL.ownGoalsScored + ";");
        //Zeit
        file.Write(globalTimer.playTime + ";");
        file.Write(globalTimer.totalTime + ";");
        //time per result
        file.Write(pL.timeInLead + ";");
        file.Write(pL.timeTied + ";");
        file.Write(pL.timeInDeficit + ";");
        //time per result percent
        //file.Write(pL.timeInLeadPercent + ";");
        //file.Write(pL.timeTiedPercent + ";");
        //file.Write(pL.timeInDeficitPercent + ";");
        //Zonetime
        file.Write(pL.timeOwnGoalZone + ";");
        file.Write(pL.timeOwnZone + ";");
        file.Write(pL.timeCenterZone + ";");
        file.Write(pL.timeOpponentZone + ";");
        file.Write(pL.timeOpponentGoalZone + ";");
        //Zone Percent
        //file.Write(pL.timeOwnGoalZonePercent + ";");
        //file.Write(pL.timeOwnZonePercent + ";");
        //file.Write(pL.timeCenterZonePercent + ";");
        //file.Write(pL.timeOpponentZonePercent + ";");
        //file.Write(pL.timeOpponentGoalZonePercent + ";");
        //time per zone per result
        file.Write(pL.timeOwnGoalZoneInLead + ";");
        file.Write(pL.timeOwnGoalZoneInTie + ";");
        file.Write(pL.timeOwnGoalZoneInDeficit + ";");

        file.Write(pL.timeOwnZoneInLead + ";");
        file.Write(pL.timeOwnZoneInTie + ";");
        file.Write(pL.timeOwnZoneInDeficit + ";");

        file.Write(pL.timeCenterZoneInLead + ";");
        file.Write(pL.timeCenterZoneInTie + ";");
        file.Write(pL.timeCenterZoneInDeficit + ";");

        file.Write(pL.timeOpponentZoneInLead + ";");
        file.Write(pL.timeOpponentZoneInTie + ";");
        file.Write(pL.timeOpponentZoneInDeficit + ";");

        file.Write(pL.timeOpponentGoalZoneInLead + ";");
        file.Write(pL.timeOpponentGoalZoneInTie + ";");
        file.Write(pL.timeOpponentGoalZoneInDeficit + ";");
        //time per zone per result Percent
        //file.Write(pL.timeOwnGoalZoneInLeadPercent + ";");
        //file.Write(pL.timeOwnZoneInLeadPercent + ";");
        //file.Write(pL.timeCenterZoneInLeadPercent + ";");
        //file.Write(pL.timeOpponentZoneInLeadPercent + ";");
        //file.Write(pL.timeOpponentGoalZoneInLeadPercent + ";");

        //file.Write(pL.timeOwnGoalZoneInTiePercent + ";");
        //file.Write(pL.timeOwnZoneInTiePercent + ";");
        //file.Write(pL.timeCenterZoneInTiePercent + ";");
        //file.Write(pL.timeOpponentZoneInTiePercent + ";");
        //file.Write(pL.timeOpponentGoalZoneInTiePercent + ";");

        //file.Write(pL.timeOwnGoalZoneInDeficitPercent + ";");
        //file.Write(pL.timeOwnZoneInDeficitPercent + ";");
        //file.Write(pL.timeCenterZoneInDeficitPercent + ";");
        //file.Write(pL.timeOpponentZoneInDeficitPercent + ";");
        //file.Write(pL.timeOpponentGoalZoneInDeficitPercent + ";");

        //Schüsse
        file.Write(pL.totalShotsFired + ";");
        file.Write(pL.normalShotsFired + ";");
        file.Write(pL.mediumShotsFired + ";");
        file.Write(pL.largeShotsFired + ";");
        //Schüsse Percent
        //file.Write(pL.normalShotsFiredPercent + ";");
        //file.Write(pL.mediumShotsFiredPercent + ";");
        //file.Write(pL.largeShotsFiredPercent + ";");
        file.Write(pL.totalShotsFiredInLead + ";");
        file.Write(pL.normalShotsFiredInLead + ";");
        file.Write(pL.mediumShotsFiredInLead + ";");
        file.Write(pL.largeShotsFiredInLead + ";");

        file.Write(pL.totalShotsFiredInTie + ";");
        file.Write(pL.normalShotsFiredInTie + ";");
        file.Write(pL.mediumShotsFiredInTie + ";");
        file.Write(pL.largeShotsFiredInTie + ";");

        file.Write(pL.totalShotsFiredInDeficit + ";");
        file.Write(pL.normalShotsFiredInDeficit + ";");
        file.Write(pL.mediumShotsFiredInDeficit + ";");
        file.Write(pL.largeShotsFiredInDeficit + ";");

        //accuracy
        file.Write(pL.totalObjectsHit + ";");
        file.Write(pL.shotsHitBlock + ";");
        file.Write(pL.shotsHitBall + ";");
        file.Write(pL.shotsHitPlayer + ";");
        file.Write(pL.shotsHitEnemyShot + ";");
        file.Write(pL.shotsDestroyed + ";");
        //Accuracy Percent
        //file.Write(pL.shotsHitBlockPercent + ";");
        //file.Write(pL.shotsHitBallPercent + ";");
        //file.Write(pL.shotsHitPlayerPercent + ";");
        //file.Write(pL.shotsHitEnemyShotPercent + ";");
        //file.Write(pL.shotsDestroyedPercent + ";");

        file.Write(pL.totalObjectsHitInLead + ";");
        file.Write(pL.shotsHitBlockInLead + ";");
        file.Write(pL.shotsHitBallInLead + ";");
        file.Write(pL.shotsHitPlayerInLead + ";");
        file.Write(pL.shotsHitEnemyShotInLead + ";");
        file.Write(pL.shotsDestroyedInLead + ";");

        file.Write(pL.totalObjectsHitInTie + ";");
        file.Write(pL.shotsHitBlockInTie + ";");
        file.Write(pL.shotsHitBallInTie + ";");
        file.Write(pL.shotsHitPlayerInTie + ";");
        file.Write(pL.shotsHitEnemyShotInTie + ";");
        file.Write(pL.shotsDestroyedInTie + ";");

        file.Write(pL.totalObjectsHitInDeficit + ";");
        file.Write(pL.shotsHitBlockInDeficit + ";");
        file.Write(pL.shotsHitBallInDeficit + ";");
        file.Write(pL.shotsHitPlayerInDeficit + ";");
        file.Write(pL.shotsHitEnemyShotInDeficit + ";");
        file.Write(pL.shotsDestroyedInDeficit + ";");

        //Blocks
        file.Write(pL.totalBlocksPlaced + ";");
        file.Write(pL.blocksInOwnGoalZone + ";");
        file.Write(pL.blocksInOwnZone + ";");
        file.Write(pL.blocksInCenterZone + ";");
        file.Write(pL.blocksInOpponentZone + ";");
        file.Write(pL.blocksInOpponentGoalZone + ";");
        //Blocks percent
        //file.Write(pL.blocksInOwnZonePercent + ";");
        //file.Write(pL.blocksInOwnGoalZonePercent + ";");
        //file.Write(pL.blocksInCenterZonePercent + ";");
        //file.Write(pL.blocksInOpponentZonePercent + ";");
        //file.Write(pL.blocksInOpponentGoalZonePercent + ";");

        file.Write(pL.totalBlocksPlacedInLead + ";");
        file.Write(pL.blocksInOwnGoalZoneInLead + ";");
        file.Write(pL.blocksInOwnZoneInLead + ";");
        file.Write(pL.blocksInCenterZoneInLead + ";");
        file.Write(pL.blocksInOpponentZoneInLead + ";");
        file.Write(pL.blocksInOpponentGoalZoneInLead + ";");

        file.Write(pL.totalBlocksPlacedInTie + ";");
        file.Write(pL.blocksInOwnGoalZoneInTie + ";");
        file.Write(pL.blocksInOwnZoneInTie + ";");
        file.Write(pL.blocksInCenterZoneInTie + ";");
        file.Write(pL.blocksInOpponentZoneInTie + ";");
        file.Write(pL.blocksInOpponentGoalZoneInTie + ";");

        file.Write(pL.totalBlocksPlacedInDeficit + ";");
        file.Write(pL.blocksInOwnGoalZoneInDeficit + ";");
        file.Write(pL.blocksInOwnZoneInDeficit + ";");
        file.Write(pL.blocksInCenterZoneInDeficit + ";");
        file.Write(pL.blocksInOpponentZoneInDeficit + ";");
        file.Write(pL.blocksInOpponentGoalZoneInDeficit + ";");

        //Opponent Stunned
        file.Write(pL.totalEnemyStunned + ";");
        file.Write(pL.enemyStunnedTotalTime + ";");
        file.Write(pL.normalEnemyStunned + ";");
        file.Write(pL.mediumEnemyStunned + ";");
        file.Write(pL.largeEnemyStunned + ";");

        file.Write(pL.totalEnemyStunnedInLead + ";");
        file.Write(pL.enemyStunnedTotalTimeInLead + ";");
        file.Write(pL.normalEnemyStunnedInLead + ";");
        file.Write(pL.mediumEnemyStunnedInLead + ";");
        file.Write(pL.largeEnemyStunnedInLead + ";");

        file.Write(pL.totalEnemyStunnedInTie + ";");
        file.Write(pL.enemyStunnedTotalTimeInTie + ";");
        file.Write(pL.normalEnemyStunnedInTie + ";");
        file.Write(pL.mediumEnemyStunnedInTie + ";");
        file.Write(pL.largeEnemyStunnedInTie + ";");

        file.Write(pL.totalEnemyStunnedInDeficit + ";");
        file.Write(pL.enemyStunnedTotalTimeInDeficit + ";");
        file.Write(pL.normalEnemyStunnedInDeficit + ";");
        file.Write(pL.mediumEnemyStunnedInDeficit + ";");
        file.Write(pL.largeEnemyStunnedInDeficit + ";");

        //Stunned by Opponent
        file.Write(pL.totalStunnedByEnemy + ";");
        file.Write(pL.stunnedByEnemyTotalTime + ";");
        file.Write(pL.normalStunnedByEnemy + ";");
        file.Write(pL.mediumStunnedByEnemy + ";");
        file.Write(pL.largeStunnedByEnemy + ";");
        //stunned by enemy per result
        file.Write(pL.totalStunnedByEnemyInLead + ";");
        file.Write(pL.stunnedByEnemyTotalTimeInLead + ";");
        file.Write(pL.normalStunnedByEnemyInLead + ";");
        file.Write(pL.mediumStunnedByEnemyInLead + ";");
        file.Write(pL.largeStunnedByEnemyInLead + ";");

        file.Write(pL.totalStunnedByEnemyInTie + ";");
        file.Write(pL.stunnedByEnemyTotalTimeInTie + ";");
        file.Write(pL.normalStunnedByEnemyInTie + ";");
        file.Write(pL.mediumStunnedByEnemyInTie + ";");
        file.Write(pL.largeStunnedByEnemyInTie + ";");

        file.Write(pL.totalStunnedByEnemyInDeficit + ";");
        file.Write(pL.stunnedByEnemyTotalTimeInDeficit + ";");
        file.Write(pL.normalStunnedByEnemyInDeficit + ";");
        file.Write(pL.mediumStunnedByEnemyInDeficit + ";");
        file.Write(pL.largeStunnedByEnemyInDeficit + ";");


        ////Stunned by ball
        //file.Write(pL.stunnedByBall + ";");

        //file.Write(pL.stunnedByBallInLead + ";");
        //file.Write(pL.stunnedByBallInTie + ";");
        //file.Write(pL.stunnedByBallInDeficit + ";");


        //Emotes
        file.Write(pL.totalEmotes + ";");
        file.Write(pL.emoteNice + ";");
        file.Write(pL.emoteAngry + ";");
        file.Write(pL.emoteCry + ";");
        file.Write(pL.emoteHaha + ";");
        //Emotes Percent
        //file.Write(pL.emoteNicePercent + ";");
        //file.Write(pL.emoteAngryPercent + ";");
        //file.Write(pL.emoteCryPercent + ";");
        //file.Write(pL.emoteHahaPercent + ";");
        //Emotes per Result
        file.Write(pL.totalEmotesInLead + ";");
        file.Write(pL.emoteNiceInLead + ";");
        file.Write(pL.emoteAngryInLead + ";");
        file.Write(pL.emoteCryInLead + ";");
        file.Write(pL.emoteHahaInLead + ";");

        file.Write(pL.totalEmotesInTie + ";");
        file.Write(pL.emoteNiceInTie + ";");
        file.Write(pL.emoteAngryInTie + ";");
        file.Write(pL.emoteCryInTie + ";");
        file.Write(pL.emoteHahaInTie + ";");

        file.Write(pL.totalEmotesInDeficit + ";");
        file.Write(pL.emoteNiceInDeficit + ";");
        file.Write(pL.emoteAngryInDeficit + ";");
        file.Write(pL.emoteCryInDeficit + ";");
        file.Write(pL.emoteHahaInDeficit + ";");
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
