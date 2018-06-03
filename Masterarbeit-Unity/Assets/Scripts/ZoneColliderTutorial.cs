using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneColliderTutorial : MonoBehaviour
{
    GameObject player;
    TutorialGameState tutorialGameState;
    public bool collidingWithStopAndGoZone;
    // Use this for initialization
    void Start()
    {
        player = gameObject.transform.parent.gameObject;
        tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("StopAndGoZone"))
        {
            collidingWithStopAndGoZone = true;
            player.GetComponent<PlayerTutorial>().SetCollidingStopAndGoZone(true, other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("StopAndGoZone"))
        {
            collidingWithStopAndGoZone = false;
            player.GetComponent<PlayerTutorial>().SetCollidingStopAndGoZone(false,other.gameObject);
        }
    }

}
