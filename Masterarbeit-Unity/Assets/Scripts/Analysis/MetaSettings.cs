/************************************************************************************

This script saves the view directions of a gameobject on a
regular intervall and exports the data to a file.

************************************************************************************/

using UnityEngine;
using System.Collections;

public  class MetaSettings : MonoBehaviour {

    public string sceneName = "Default";
    public string subjectName = "Mustermann";

    public bool completeData;
    public bool inLeadData;
    public bool inTieData;
    public bool InDeficitData;

    public static MetaSettings instance;

    //Singleton Pattern
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
