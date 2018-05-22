using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Logging : MonoBehaviour {

    public GlobalTimer globalTimer;
    public ExportData exportData;
    public string sceneName;
    
    // Use this for initialization
    void Start () {
        sceneName = SceneManager.GetActiveScene().name;
        globalTimer = GetComponent<GlobalTimer>();
        exportData = GetComponent<ExportData>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
