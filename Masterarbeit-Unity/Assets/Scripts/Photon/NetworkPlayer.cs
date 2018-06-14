using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {


	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private PlayerPhoton playerScript;
	private MovementPhoton movement;


	// Use this for initialization
	void Start () {
	
		playerScript = GetComponent<PlayerPhoton> ();
		movement = GetComponent<MovementPhoton> ();

		if (photonView.isMine) {
			//playerScript.enabled = true;
			movement.enabled = true;
		}

	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if (stream.isWriting) {

			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);

		} else {

			this.correctPlayerPos = (Vector3)stream.ReceiveNext ();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext ();

		}
	}
}
