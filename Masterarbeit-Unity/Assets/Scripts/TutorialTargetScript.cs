using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargetScript : MonoBehaviour {

	ShotChallenge shotChallenge;
	[Range (1, 3)]
	public int targetType;


	// Use this for initialization
	void Start () {
		shotChallenge = GameObject.FindObjectOfType<ShotChallenge> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		
		if (other.gameObject.tag.Equals("Shot")){
			ShotTutorial shot = other.gameObject.GetComponent<ShotTutorial> ();

			if (shot.strength == targetType) {
				shotChallenge.RemoveTarget ();
			}
		}

	}

}
