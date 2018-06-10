using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {


	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private PlayerPhoton playerScript;


	// Use this for initialization
	void Start () {
	
		playerScript = GetComponent<PlayerPhoton> ();

		if (photonView.isMine) {
			playerScript.enabled = true;
		}

	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if (stream.isWriting) {

			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			//stream.SendNext (playerScript.teamColor);

		} else {

			this.correctPlayerPos = (Vector3)stream.ReceiveNext ();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext ();
			//playerScript.SetColor((Color)stream.ReceiveNext());

		}
	}
}
