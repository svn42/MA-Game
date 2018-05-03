using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    PlayerLogging playerLogging;
    // Use this for initialization
    void Start()
    {
        playerLogging = transform.parent.GetComponent<PlayerLogging>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        playerLogging.AddZoneTime(other.gameObject.name);
    }
}
