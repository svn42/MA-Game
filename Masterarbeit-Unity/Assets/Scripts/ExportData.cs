﻿/************************************************************************************

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
    public string logdataPath;
	public string completePositionPath;
	public string inLeadPositionPath;
	public string inTiePositionPath;
	public string inDeficitPositionPath;
    private string sceneName;
    private string sceneNameAbbreviation;
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

      public void StartUpExportData()
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
		case "Level 1.1":
			sceneNameAbbreviation = "L1-1";
			break;
		case "Level 1.2":
			sceneNameAbbreviation = "L1-2";
			break;
		case "Level 1.3":
			sceneNameAbbreviation = "L1-3";
			break;
		case "Level 2.1":
			sceneNameAbbreviation = "L2-1";
			break;
		case "Level 2.2":
			sceneNameAbbreviation = "L2-2";
			break;
		case "Level 2.3":
			sceneNameAbbreviation = "L2-3";
			break;
		case "Level 3.1":
			sceneNameAbbreviation = "L3-1";
			break;
		case "Level 3.2":
			sceneNameAbbreviation = "L3-2";
			break;
		case "Level 3.3":
			sceneNameAbbreviation = "L3-3";
			break;
        }

        globalTimer = (GlobalTimer)FindObjectOfType(typeof(GlobalTimer));

    }

	public void ExportAllData(PlayerLogging activePlayerLogging)
    {
        //Erzeugt den Ordner, falls er noch nicht vorhanden
        if (!Directory.Exists("ResearchData/csv/LogData/"))
        {
			Directory.CreateDirectory("ResearchData/csv/LogData/");
        }
		logdataPath = "ResearchData/csv/Logdata/LogData" + sceneNameAbbreviation + ".csv";

        //Falls noch keine .csv-Datei vorhanden ist
		if (!File.Exists(logdataPath))
        {
            //Erzeuge neue .csv-Datei und 
			using (StreamWriter newFile = File.CreateText(logdataPath))
            {
                //Schreibe alle Spaltennamen in erste Zeile
                newFile.Write("VP;");
                newFile.Write("VP_Gegner;");
                newFile.Write("Szene;");
                newFile.Write("Spielmodus;");

                newFile.Write(sceneNameAbbreviation + "_Startzeit;");
                newFile.Write(sceneNameAbbreviation + "_Endzeit;");

				newFile.Write(sceneNameAbbreviation + "_Rating;");
				newFile.Write(sceneNameAbbreviation + "_Rating_Gegner;");

				//Gesamtergebnis
				newFile.Write(sceneNameAbbreviation + "_Siege_Spieler1;");
				newFile.Write(sceneNameAbbreviation + "_Siege_Spieler2;");

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

                //ZoneTime
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Torzone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Zone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_Mittelzone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Zone;");
                newFile.Write(sceneNameAbbreviation + "_Zeit_gegnerische_Torzone;");
                //time per zone per result

                newFile.Write(sceneNameAbbreviation + "_Zeit_eigene_Torzone_Vorsprung;");
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

                //shots
                newFile.Write(sceneNameAbbreviation + "_Total_Schuesse;");
                newFile.Write(sceneNameAbbreviation + "_Normale_Schuesse;");
                newFile.Write(sceneNameAbbreviation + "_Mittlere_Schuesse;");
                newFile.Write(sceneNameAbbreviation + "_Grosse_Schuesse;");
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

                //Emotes
                newFile.Write(sceneNameAbbreviation + "_Total_Emotes;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Nice;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Angry;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Cry;");
                newFile.Write(sceneNameAbbreviation + "_Emote_Haha;");
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
		using (StreamWriter file = File.AppendText(logdataPath))
        {
			WritePlayerLoggingData(file, activePlayerLogging);
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

		//Gesamtergebnis
		file.Write(PlayerPrefs.GetInt("WinsP1") + ";");
		file.Write(PlayerPrefs.GetInt("WinsP2") + ";");

        //Bewegung
		file.Write(System.Math.Round(pL.distanceTravelled,2) + ";");
		file.Write(System.Math.Round(pL.distanceTravelledInLead,2) + ";");
		file.Write(System.Math.Round(pL.distanceTravelledInTie,2) + ";");
		file.Write(System.Math.Round(pL.distanceTravelledInDeficit,2) + ";");

        //Resultat
        file.Write(pL.finalResult + ";");
        file.Write(pL.endingCondition + ";");
        file.Write(pL.goalsScored + ";");
        file.Write(pL.goalsConceded + ";");
        //Art der Tore
        file.Write(pL.correctGoalsScored + ";");
        file.Write(pL.ownGoalsScored + ";");
        //Zeit
		file.Write(System.Math.Round(globalTimer.playTime,2) + ";");
		file.Write(System.Math.Round(globalTimer.totalTime,2) + ";");
        //time per result
        file.Write(System.Math.Round(pL.timeInLead,2) + ";");
        file.Write(System.Math.Round(pL.timeTied,2) + ";");
        file.Write(System.Math.Round(pL.timeInDeficit,2) + ";");
        //Zonetime
        file.Write(System.Math.Round(pL.timeOwnGoalZone,2) + ";");
        file.Write(System.Math.Round(pL.timeOwnZone,2) + ";");
        file.Write(System.Math.Round(pL.timeCenterZone,2) + ";");
        file.Write(System.Math.Round(pL.timeOpponentZone,2) + ";");
        file.Write(System.Math.Round(pL.timeOpponentGoalZone,2) + ";");
        //time per zone per result
        file.Write(System.Math.Round(pL.timeOwnGoalZoneInLead,2) + ";");
        file.Write(System.Math.Round(pL.timeOwnGoalZoneInTie,2) + ";");
        file.Write(System.Math.Round(pL.timeOwnGoalZoneInDeficit,2) + ";");

        file.Write(System.Math.Round(pL.timeOwnZoneInLead,2) + ";");
        file.Write(System.Math.Round(pL.timeOwnZoneInTie,2) + ";");
        file.Write(System.Math.Round(pL.timeOwnZoneInDeficit,2) + ";");

        file.Write(System.Math.Round(pL.timeCenterZoneInLead,2) + ";");
        file.Write(System.Math.Round(pL.timeCenterZoneInTie,2) + ";");
        file.Write(System.Math.Round(pL.timeCenterZoneInDeficit,2) + ";");

		file.Write(System.Math.Round(pL.timeOpponentZoneInLead,2) + ";");
		file.Write(System.Math.Round(pL.timeOpponentZoneInTie,2) + ";");
		file.Write(System.Math.Round(pL.timeOpponentZoneInDeficit,2) + ";");

		file.Write(System.Math.Round(pL.timeOpponentGoalZoneInLead,2) + ";");
		file.Write(System.Math.Round(pL.timeOpponentGoalZoneInTie ,2) + ";");
		file.Write(System.Math.Round(pL.timeOpponentGoalZoneInDeficit,2) + ";");
        //Schüsse
        file.Write(pL.totalShotsFired + ";");
        file.Write(pL.normalShotsFired + ";");
        file.Write(pL.mediumShotsFired + ";");
        file.Write(pL.largeShotsFired + ";");
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

		file.Write(System.Math.Round(pL.enemyStunnedTotalTime,2) + ";");
        file.Write(pL.normalEnemyStunned + ";");
        file.Write(pL.mediumEnemyStunned + ";");
        file.Write(pL.largeEnemyStunned + ";");

        file.Write(pL.totalEnemyStunnedInLead + ";");
		file.Write(System.Math.Round(pL.enemyStunnedTotalTimeInLead,2) + ";");
        file.Write(pL.normalEnemyStunnedInLead + ";");
        file.Write(pL.mediumEnemyStunnedInLead + ";");
        file.Write(pL.largeEnemyStunnedInLead + ";");

        file.Write(pL.totalEnemyStunnedInTie + ";");
		file.Write(System.Math.Round(pL.enemyStunnedTotalTimeInTie,2) + ";");
        file.Write(pL.normalEnemyStunnedInTie + ";");
        file.Write(pL.mediumEnemyStunnedInTie + ";");
        file.Write(pL.largeEnemyStunnedInTie + ";");

        file.Write(pL.totalEnemyStunnedInDeficit + ";");
		file.Write(System.Math.Round(pL.enemyStunnedTotalTimeInDeficit,2) + ";");
        file.Write(pL.normalEnemyStunnedInDeficit + ";");
        file.Write(pL.mediumEnemyStunnedInDeficit + ";");
        file.Write(pL.largeEnemyStunnedInDeficit + ";");

        //Stunned by Opponent
        file.Write(pL.totalStunnedByEnemy + ";");
		file.Write(System.Math.Round(pL.stunnedByEnemyTotalTime,2) + ";");
        file.Write(pL.normalStunnedByEnemy + ";");
        file.Write(pL.mediumStunnedByEnemy + ";");
        file.Write(pL.largeStunnedByEnemy + ";");
        //stunned by enemy per result
        file.Write(pL.totalStunnedByEnemyInLead + ";");
		file.Write(System.Math.Round(pL.stunnedByEnemyTotalTimeInLead,2) + ";");
        file.Write(pL.normalStunnedByEnemyInLead + ";");
        file.Write(pL.mediumStunnedByEnemyInLead + ";");
        file.Write(pL.largeStunnedByEnemyInLead + ";");

        file.Write(pL.totalStunnedByEnemyInTie + ";");
		file.Write(System.Math.Round(pL.stunnedByEnemyTotalTimeInTie,2) + ";");
        file.Write(pL.normalStunnedByEnemyInTie + ";");
        file.Write(pL.mediumStunnedByEnemyInTie + ";");
        file.Write(pL.largeStunnedByEnemyInTie + ";");

        file.Write(pL.totalStunnedByEnemyInDeficit + ";");
		file.Write(System.Math.Round(pL.stunnedByEnemyTotalTimeInDeficit ,2) + ";");
        file.Write(pL.normalStunnedByEnemyInDeficit + ";");
        file.Write(pL.mediumStunnedByEnemyInDeficit + ";");
        file.Write(pL.largeStunnedByEnemyInDeficit + ";");
        //Emotes
        file.Write(pL.totalEmotes + ";");
        file.Write(pL.emoteNice + ";");
        file.Write(pL.emoteAngry + ";");
        file.Write(pL.emoteCry + ";");
        file.Write(pL.emoteHaha + ";");
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
		file.Close();
    }

	public void WriteAllPositionData(PositionTracker posiTracker, PlayerLogging pL){
		exportPositionDataComplete (posiTracker, pL);
		exportPositionDataInLead (posiTracker, pL);
		exportPositionDataInTie (posiTracker, pL);
		exportPositionDataInDeficit (posiTracker, pL);
	}

	//Code aus dem FoPro Lost In Space!
	//PositionData
	// Exportiert die kompletten Positionsdaten in eine .csv-Datei
	public void exportPositionDataComplete(PositionTracker posiTracker, PlayerLogging pL)
	{
		completePositionPath = "ResearchData/csv/PositionData/"+ sceneNameAbbreviation+"/Complete/";
		List<Vector3> completePositionList = posiTracker.CompletePositionList;

		//Erzeugt den Ordner, falls er noch nicht vorhanden
		if (!Directory.Exists(completePositionPath))
		{
			Directory.CreateDirectory(completePositionPath);
		}

		// Positionsdaten in Datei speichern.
		StreamWriter file = File.CreateText(completePositionPath + "VP_" + pL.subjectNr+ "_" +globalTimer.startTime+ "_" + sceneNameAbbreviation + "_CompletePosition.csv");

		file.WriteLine("Index; Position");

		for (int i = 0; i < completePositionList.Count; i++)
		{
			file.WriteLine(i + ";" + completePositionList[i]);
		}
		file.Close();
	}

	// Exportiert die Positionsdaten in Führung in eine .csv-Datei
	public void exportPositionDataInLead(PositionTracker posiTracker, PlayerLogging pL)
	{
		inLeadPositionPath = "ResearchData/csv/PositionData/"+ sceneNameAbbreviation+"/InLead/";
		List<Vector3> inLeadPositionList = posiTracker.CompletePositionListInLead;

		//Erzeugt den Ordner, falls er noch nicht vorhanden
		if (!Directory.Exists(inLeadPositionPath))
		{
			Directory.CreateDirectory(inLeadPositionPath);
		}

		// Positionsdaten in Datei speichern.
		StreamWriter file = File.CreateText(inLeadPositionPath + "VP_" + pL.subjectNr+ "_" +globalTimer.startTime+ "_" + sceneNameAbbreviation + "_InLeadPosition.csv");

		file.WriteLine("Index; Position");

		for (int i = 0; i < inLeadPositionList.Count; i++)
		{
			file.WriteLine(i + ";" + inLeadPositionList[i]);
		}
		file.Close();
	}

	// Exportiert die Positionsdaten in Remis in eine .csv-Datei
	public void exportPositionDataInTie(PositionTracker posiTracker, PlayerLogging pL)
	{
		inTiePositionPath = "ResearchData/csv/PositionData/"+ sceneNameAbbreviation+"/InTie/";
		List<Vector3> inTiePositionList = posiTracker.CompletePositionListInTie;

		//Erzeugt den Ordner, falls er noch nicht vorhanden
		if (!Directory.Exists(inTiePositionPath))
		{
			Directory.CreateDirectory(inTiePositionPath);
		}

		// Positionsdaten in Datei speichern.
		StreamWriter file = File.CreateText(inTiePositionPath + "VP_" + pL.subjectNr+ "_" +globalTimer.startTime+ "_" + sceneNameAbbreviation + "_InTiePosition.csv");

		file.WriteLine("Index; Position");

		for (int i = 0; i < inTiePositionList.Count; i++)
		{
			file.WriteLine(i + ";" + inTiePositionList[i]);
		}
		file.Close();
	}

	// Exportiert die Positionsdaten in Rückstand in eine .csv-Datei
	public void exportPositionDataInDeficit(PositionTracker posiTracker, PlayerLogging pL)
	{
		inDeficitPositionPath = "ResearchData/csv/PositionData/"+ sceneNameAbbreviation+"/InDeficit/";
		List<Vector3> inDeficitPositionList = posiTracker.CompletePositionListInDeficit;

		//Erzeugt den Ordner, falls er noch nicht vorhanden
		if (!Directory.Exists(inDeficitPositionPath))
		{
			Directory.CreateDirectory(inDeficitPositionPath);
		}

		// Positionsdaten in Datei speichern.
		StreamWriter file = File.CreateText(inDeficitPositionPath + "VP_" + pL.subjectNr+ "_" +globalTimer.startTime+ "_" + sceneNameAbbreviation + "_InDeficitPosition.csv");

		file.WriteLine("Index; Position");

		for (int i = 0; i < inDeficitPositionList.Count; i++)
		{
			file.WriteLine(i + ";" + inDeficitPositionList[i]);
		}
		file.Close();
	}
}
