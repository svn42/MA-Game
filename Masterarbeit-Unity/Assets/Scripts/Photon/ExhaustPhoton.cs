using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustPhoton : MonoBehaviour
{
    Color color; // Variable in der die Farbe verändert wird
    public float destroyTime; //Zeit bis zur Zerstörung des Abgaspartikels in Sekunden
    private Vector3 movementVector; //Der Bewegungsvektor des Partikels, der in jedem Update verwendet wird
    public float acceleration;  //der Beschleunigungswert des Partikels
	private GameStatePhoton gameState;
	private PhotonView pv;
	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation;

    // Use this for initialization
    void Start()
    {
		pv = gameObject.GetComponent<PhotonView> ();
		gameState = (GameStatePhoton)FindObjectOfType(typeof(GameStatePhoton));
        //die Zerstörung des Objektes in "destroyTime" Sekunden wird in Auftrag gegeben
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
		if (pv.isMine) {
			if (!gameState.GetGamePaused ()) {
				Move ();        //mit jedem Frame wird das Objekt bewegt
				Fade ();        //und das Objekt durchsichtiger
			}
		}
		else if (!pv.isMine) {
			transform.position = realPosition;
			transform.rotation = realRotation;
		}

    }

    //Methode zum Bewegen
    public void Move()
    {
        transform.Translate(movementVector * Time.deltaTime, Space.World);
    }

    //Die Transparenz des Partikels wird mit jedem Frame so reduziert, dass das Partikel zum Zeitpunkt der Zerstörung komplet transparent ist
    public void Fade()
    {
        color.a -= 1 / (destroyTime * 60);
        GetComponent<SpriteRenderer>().material.color = color;
    }

    //das Partikel bekommt in der Methode den Inputvektor des Spielers übergeben
    public void SetDirection(Vector3 input)
    {
        //dieser wird mit der Beschleunigung des Partikels sowie mit -1 multipliziert, damit das Partikel in die andere Richtung fliegt
        movementVector = input * acceleration * -1;
    }

	[PunRPC]
    public void SetColor(Vector3 colVector)
    {
		Color colorNew = new Color(colVector.x, colVector.y, colVector.z, 1);
		color = colorNew;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if (stream.isWriting) {

			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);

		} else {
			realPosition = (Vector3)(stream.ReceiveNext ());
			realRotation = (Quaternion)(stream.ReceiveNext ());
		}
	}

}
