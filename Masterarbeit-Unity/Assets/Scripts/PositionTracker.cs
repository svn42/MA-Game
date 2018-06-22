
/************************************************************************************
Übernommen aus dem Lost In Space Forschungsprojekt!
Dieses Skript speichert die Positionskoordinaten des Players
in regelmäßigen Intervallen.

************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;



public class PositionTracker : MonoBehaviour
{

    private PlayerLogging playerLogging;

    //Bestimmt wie oft die Position des Spielers gespeichert werden soll.
    public float trackingIntervall;
    public List<Vector3> positionList;
    public List<Vector3> positionListInLead;
    public List<Vector3> positionListInTie;
    public List<Vector3> positionListInDeficit;
    public List<Vector3> CompletePositionList;
    public List<Vector3> CompletePositionListInLead;
    public List<Vector3> CompletePositionListInTie;
    public List<Vector3> CompletePositionListInDeficit;

    public List<Vector3> positionListWithoutDuplicates;

    private float walkedDistance;
    private float walkedDistanceInLead;
    private float walkedDistanceInTie;
    private float walkedDistanceInDeficit;

    public string formerResult;
    public string currentResult;

    // Use this for initialization
    void Start()
    {
        playerLogging = this.gameObject.GetComponent<PlayerLogging>();  //der PlayerLogger wird verknüpft
        formerResult = "in_tie";
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddPosition()
    {

        switch (currentResult)
        {
            case "in_lead":
                positionListInLead.Add(transform.position);
                CompletePositionListInLead.Add(transform.position);
                break;
            case "in_tie":
                positionListInTie.Add(transform.position);
                CompletePositionListInTie.Add(transform.position);
                break;
            case "in_deficit":
                positionListInDeficit.Add(transform.position);
                CompletePositionListInDeficit.Add(transform.position);
                break;
        }
        positionList.Add(transform.position);
        CompletePositionList.Add(transform.position);

    }

    public void StartTracking()
    {
        InvokeRepeating("AddPosition", 0f, trackingIntervall);
    }

    public void StopTracking()
    {
        CancelInvoke("AddPosition");
    }

    //Berechnet die zurückgelegte Distanz der Versuchsperson
    public void CalculateWalkedDistance()
    {
        walkedDistance = 0;

        positionListWithoutDuplicates = RemoveDuplicates(GetComponent<PositionTracker>().positionList);

        for (int i = 0; i < positionListWithoutDuplicates.Count - 1; i++)
        {
            walkedDistance = walkedDistance + Vector3.Distance(positionListWithoutDuplicates[i], positionListWithoutDuplicates[i + 1]);
        }
        playerLogging.AddWalkedDistance(walkedDistance);
    }

    public void CalculateWalkedDistancePerResult(string currentResult)
    {
        switch (currentResult)
        {
            case "in_lead":

                walkedDistanceInLead = 0;

                positionListWithoutDuplicates = RemoveDuplicates(GetComponent<PositionTracker>().positionListInLead);

                for (int i = 0; i < positionListWithoutDuplicates.Count - 1; i++)
                {
                    walkedDistanceInLead = walkedDistanceInLead + Vector3.Distance(positionListWithoutDuplicates[i], positionListWithoutDuplicates[i + 1]);
                }
                playerLogging.AddWalkedDistance(currentResult, walkedDistanceInLead);
                positionListInLead = new List<Vector3>();
                break;

            case "in_tie":

                walkedDistanceInTie = 0;

                positionListWithoutDuplicates = RemoveDuplicates(GetComponent<PositionTracker>().positionListInTie);

                for (int i = 0; i < positionListWithoutDuplicates.Count - 1; i++)
                {
                    walkedDistanceInTie = walkedDistanceInTie + Vector3.Distance(positionListWithoutDuplicates[i], positionListWithoutDuplicates[i + 1]);
                }
                playerLogging.AddWalkedDistance(currentResult, walkedDistanceInTie);
                positionListInTie = new List<Vector3>();
                break;

            case "in_deficit":

                walkedDistanceInDeficit = 0;

                positionListWithoutDuplicates = RemoveDuplicates(GetComponent<PositionTracker>().positionListInDeficit);

                for (int i = 0; i < positionListWithoutDuplicates.Count - 1; i++)
                {
                    walkedDistanceInDeficit = walkedDistanceInDeficit + Vector3.Distance(positionListWithoutDuplicates[i], positionListWithoutDuplicates[i + 1]);
                }
                playerLogging.AddWalkedDistance(currentResult, walkedDistanceInDeficit);
                positionListInDeficit = new List<Vector3>();
                break;
        }

        walkedDistance = 0;

        positionListWithoutDuplicates = RemoveDuplicates(GetComponent<PositionTracker>().positionList);

        for (int i = 0; i < positionListWithoutDuplicates.Count - 1; i++)
        {
            walkedDistance = walkedDistance + Vector3.Distance(positionListWithoutDuplicates[i], positionListWithoutDuplicates[i + 1]);
        }
        playerLogging.AddWalkedDistance(walkedDistance);
        positionList = new List<Vector3>();

    }



    public static List<Vector3> RemoveDuplicates(List<Vector3> toRemove)
    {
        List<Vector3> temp = new List<Vector3>();

        for (int i = 0; i < toRemove.Count; i++)
        {
            if (!temp.Contains(toRemove[i]))
            {
                temp.Add(toRemove[i]);
            }
        }
        return temp;
    }

    public void ChangeResult(string result)
    {
        formerResult = currentResult;
        CalculateWalkedDistancePerResult(formerResult);
        currentResult = result;
    }

}
