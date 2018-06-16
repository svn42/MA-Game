using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

	private PlayerPhoton playerScript;
	private MovementPhoton movementScript;

	private Vector3 lastPosition;
	private Vector3 velocity;
	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation;
	private Vector3 sendPosition = Vector3.zero;
	private Quaternion sendRotation;
	[Range(0.0f, 1.0f)]
	public float predictionCoeff = 1.0f; //How much the game should predict an observed object's velocity: between 0 and 1


	// Use this for initialization
	void Start () {
	
		playerScript = GetComponent<PlayerPhoton> ();
		movementScript = GetComponent<MovementPhoton> ();

		realPosition = playerScript.transform.position;
		realRotation = playerScript.transform.rotation;

		if (photonView.isMine) {
			movementScript.enabled = true;
		}

	}

	// Update is called once per frame
	void Update () {
			if (!photonView.isMine) {
			realPosition = playerScript.transform.position;
			realRotation = playerScript.transform.rotation;

			lastPosition = realPosition;

		//	transform.position = Vector3.Lerp(transform.position, realPosition + (predictionCoeff * velocity * Time.deltaTime), Time.deltaTime);
		//	transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, Time.deltaTime);
		//	transform.position = Vector3.Lerp(this.transform.position, realPosition, Time.deltaTime * 100);
		//	transform.rotation = Quaternion.Lerp(this.transform.rotation, realRotation, Time.deltaTime * 100);
				}

	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if (stream.isWriting && photonView.isMine) {

			stream.SendNext ( transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext((realPosition - lastPosition) / Time.deltaTime);

		} else 
			{//this is the information you will recieve over the network
			realPosition = (Vector3) (stream.ReceiveNext());
			realRotation = (Quaternion) (stream.ReceiveNext());
			velocity = (Vector3)(stream.ReceiveNext());					
		}
	}
}
