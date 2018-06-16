using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialShotTest : MonoBehaviour {


	public int normalShotsFired;
	public int mediumShotsFired;
	public int largeShotsFired;
	private int shotsToDo = 3;
	private TutorialGameState tutorialGameState;
	public Text normalShots;
	public Text mediumShots;
	public Text largeShots;

	// Use this for initialization
	void Start () {
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
	}

	// Update is called once per frame
	void Update () {

	}


	public void AddShot(string type){
		switch (type) {
		case "normal":
			normalShotsFired++;
			normalShots.text = "Normale Schüsse: " + normalShotsFired;
			if (normalShotsFired >= 3) {
				normalShots.color = Color.green;
			}
			break;
		case "medium":
			mediumShotsFired++;
			mediumShots.text = "Mittlere Schüsse: " + mediumShotsFired;
			if (mediumShotsFired >= 3) {
				mediumShots.color = Color.green;
			}
			break;
		case "large":
			largeShotsFired++;
			largeShots.text = "Große Schüsse: " + largeShotsFired;
			if (largeShotsFired >= 3) {
				largeShots.color = Color.green;
			}
			break;
		}
		if (normalShotsFired >= shotsToDo && mediumShotsFired >= shotsToDo && largeShotsFired >= shotsToDo) {
			StartCoroutine (EndChallenge());
		}
	}


	IEnumerator EndChallenge(){
		yield return new WaitForSeconds (1);		
		tutorialGameState.EndChallenge (0,0);
	}
}
