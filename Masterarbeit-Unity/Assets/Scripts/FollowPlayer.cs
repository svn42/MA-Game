using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.localPosition = player.transform.position + new Vector3(-17, 19, -0.01f);
        }
    }
	

    public void SetFollowPlayer(GameObject go)
    {
        player = go;
    }
}
