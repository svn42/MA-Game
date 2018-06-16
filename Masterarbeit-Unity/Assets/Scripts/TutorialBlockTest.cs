using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlockTest : MonoBehaviour {

	public int placedBlocks;
	private int blocksToPlace = 3;
	private TutorialGameState tutorialGameState;
	private TutorialLogging tutorialLogging;

	// Use this for initialization
	void Start () {
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		tutorialLogging = gameObject.GetComponent<TutorialLogging>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void AddPlacedBlock(){
		placedBlocks++;
		if (placedBlocks == blocksToPlace) {
			StartCoroutine (EndChallenge());
		}
	}


IEnumerator EndChallenge(){
	yield return new WaitForSeconds (1);		
	tutorialGameState.EndChallenge (0,0);
}
}
