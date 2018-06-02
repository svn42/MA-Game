using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallExplosion : MonoBehaviour
{

    private GameState gameState;
    private float destroyTime;
    // Use this for initialization
    void Start()
    {

        gameState = (GameState)FindObjectOfType(typeof(GameState));
        destroyTime = gameState.goalFreezeTime;
        //die Zerstörung des Objektes in "destroyTime" Sekunden wird in Auftrag gegeben
        StartCoroutine(DestroyWait());

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += transform.localScale * Time.deltaTime * 5;
    }

    IEnumerator DestroyWait()
    {
        yield return new WaitForSecondsRealtime(destroyTime);
        Destroy(gameObject);
    }

}
