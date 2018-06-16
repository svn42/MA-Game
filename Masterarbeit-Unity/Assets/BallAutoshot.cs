using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAutoshot : MonoBehaviour {

	public GameObject target;
	public int speed;
	private AudioSource audioSource;
	private AudioClip ballspawn;
	private AudioClip ballhit;
	private AudioClip soundInflateBall;


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		ballspawn = Resources.Load<AudioClip>("Sounds/ball_spawn");
		ballhit = Resources.Load<AudioClip>("Sounds/Sounds/ball_hit");
		soundInflateBall = Resources.Load<AudioClip>("Sounds/Sounds/inflate_ball");
		StartCoroutine(ChargeShot ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ChargeShot(){
		yield return new WaitForSeconds (0.1f);

		PlaySound (ballspawn, 0.25f);
		yield return new WaitForSeconds (0.1f);
		PlaySound(soundInflateBall, 0.8f);

		yield return new WaitForSeconds (2.8f);
		PlaySound (ballhit , 0.8f);

		Vector3 force = transform.position - target.transform.position;
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce (force * -speed);

	}

	public void PlaySound(AudioClip ac, float volume)
	{
		float lastTimeScale = Time.timeScale;
		Time.timeScale = 1f;
		audioSource.PlayOneShot(ac, volume);
		Time.timeScale = lastTimeScale;
	}

}
