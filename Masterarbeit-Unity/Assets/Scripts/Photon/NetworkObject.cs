using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour {

	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation;
	private PhotonView pv;

	// Use this for initialization
	void Start () {
		pv = gameObject.GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!pv.isMine) {
			transform.position = realPosition;
			transform.rotation = realRotation;
		}

	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if (stream.isWriting) {

			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);

		} else {//this is the information you will recieve over the network
			realPosition = (Vector3)(stream.ReceiveNext ());
			realRotation = (Quaternion)(stream.ReceiveNext ());
		}
	}
}
