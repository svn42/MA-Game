using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallExplosionPhoton : MonoBehaviour
{

	private GameStatePhoton gameState;
    private float destroyTime;
    // Use this for initialization
    void Start()
    {

		gameState = (GameStatePhoton)FindObjectOfType(typeof(GameStatePhoton));
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
