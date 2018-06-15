using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Regelt die Bewegung, die Animationen und die Wegpunkte des Gegners.
// Momentan auch noch Dinge wie z.B. damageValue oder killPointsValue, die noch in die einzelnen
//Entity-Scripts verlagert werden müssen
public class TargetMovement: MonoBehaviour
{

	//Liste  für die Wegpunkte. Im Inspector frei veränderbar
	public GameObject waypoint;
	private List<Vector3> waypointPositions = new List<Vector3>();
	//Geschwindigkeit des Gegners
	public float speed;

	int currentWaypoint = 0;
	Vector3 targetPositionDelta;
	Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
		SaveWaypoints ();
	}

	// Update is called once per frame
	void Update ()
	{
		WaypointWalk ();
		Move ();
	}

	void SaveWaypoints()
	{
		waypointPositions.Add (waypoint.transform.position);
		waypointPositions.Add (this.gameObject.transform.position);
	}

	// regelt die Wegpunkt-Steuerung
	void WaypointWalk ()
	{
		Vector3 targetPosition = waypointPositions[currentWaypoint];
		targetPositionDelta = targetPosition - transform.position;

		//wenn der Abstand zur targetPosition sehr gering ist, wird der näcshte Wegpunkt in der Liste angesteuert
		if (targetPositionDelta.sqrMagnitude <= 1) {
			currentWaypoint ++;

			//wenn der letzte Wegpunkt erreciht wurde, wird mit dem ersten Wegpunkt fortgefahren
			if (currentWaypoint >= waypointPositions.Count)
				currentWaypoint = 0;
		}
	}

	void Move ()
	{
		moveDirection = targetPositionDelta.normalized * speed;
		// regelt, dass die Bewegungsgeschwindigkeit auf allen Rechnern gleich ist 
		//(Da bin ich mir grad unsicher. Bitte korrigieren, wenn ich falsch liege!)
		transform.Translate (moveDirection * Time.deltaTime, Space.World);
	}

}
