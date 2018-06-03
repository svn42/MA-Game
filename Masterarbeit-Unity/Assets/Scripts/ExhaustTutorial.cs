using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustTutorial : MonoBehaviour
{
    Color color; // Variable in der die Farbe verändert wird
    public float destroyTime; //Zeit bis zur Zerstörung des Abgaspartikels in Sekunden
    private Vector3 movementVector; //Der Bewegungsvektor des Partikels, der in jedem Update verwendet wird
    public float acceleration;  //der Beschleunigungswert des Partikels
    private TutorialGameState tutorialGameState;

    // Use this for initialization
    void Start()
    {
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
        //die Zerstörung des Objektes in "destroyTime" Sekunden wird in Auftrag gegeben
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialGameState.GetGamePaused())
        {
            Move();        //mit jedem Frame wird das Objekt bewegt
            Fade();        //und das Objekt durchsichtiger
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

    public void SetColor(Color col)
    {
        color = col;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }
}
