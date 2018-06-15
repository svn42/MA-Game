using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlockPlacementZone : MonoBehaviour {


	private TutorialBlockPlacementChallenge tutorialBlockPlacementChallenge;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTutorialChallengeScript(TutorialBlockPlacementChallenge tt){
		tutorialBlockPlacementChallenge = tt;
	}

	//Bei Aktivierung des Triggers
	private void OnTriggerEnter2D(Collider2D other)
	{
		//sofern das andere Objekt eine Bande, ein anderer Block, ein Ball oder der andere Spieler ist
		if (other.gameObject.tag.Equals("Block"))
		{
			BlockTutorial block = other.gameObject.GetComponent<BlockTutorial> (); 
			tutorialBlockPlacementChallenge.RemoveZone (block);
		}

	}

}
