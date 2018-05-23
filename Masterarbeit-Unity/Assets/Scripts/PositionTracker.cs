
/************************************************************************************
Übernommen aus dem Lost In Space Forschungsprojekt!
Dieses Skript speichert die Positionskoordinaten des Players
in regelmäßigen Intervallen.

************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{

    private PlayerLogging playerLogging;

    //Bestimmt wie oft die Position des Spielers gespeichert werden soll.
    public float trackingIntervall;
    public List<Vector3> positionList;
    public List<Vector3> positionListWithoutDuplicates;
    private float walkedDistance;

    // Use this for initialization
    void Start()
    {
        playerLogging = this.gameObject.GetComponent<PlayerLogging>();  //der PlayerLogger wird verknüpft
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddPosition()
    {
        positionList.Add(transform.position);
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
}
