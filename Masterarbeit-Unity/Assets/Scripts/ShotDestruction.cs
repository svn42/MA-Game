using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDestruction : MonoBehaviour
{

    private float destroyTime;
    // Use this for initialization
    void Start()
    {

        //die Zerstörung des Objektes in "destroyTime" Sekunden wird in Auftrag gegeben
        StartCoroutine(DestroyWait());
        transform.localScale /= 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += transform.localScale * Time.deltaTime * 5;
    }

    IEnumerator DestroyWait()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Destroy(gameObject);
    }

    public void SetColor(Color col)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;
    }

}
