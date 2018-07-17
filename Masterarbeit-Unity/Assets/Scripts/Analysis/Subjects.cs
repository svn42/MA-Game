/************************************************************************************

Dieses Skript enthält alle Daten der Versuchspersonen

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class Subjects : MonoBehaviour {

    public string folder;
    public List<string> files;

    public List<Subject> subjects;
    public List<Vector3> averagePositions;

    public static Subjects instance;

    //Singleton pattern
    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

[System.Serializable]
public class Subject
{
    public bool selected;
    public string subjectName;

    public string positionPath;

    public List<Vector3> positions;
}