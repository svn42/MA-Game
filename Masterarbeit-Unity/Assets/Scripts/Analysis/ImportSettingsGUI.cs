/************************************************************************************

Dieses Skript generiert und aktualisiert GUI Elemente der Szene
Analysis_Settings

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImportSettingsGUI : MonoBehaviour {

    //GUI Elements
    public Text pathText;
    public Text metaText;
    //Toggles for Result
	public Toggle CompleteToggle;
    public Toggle InLeadToggle;
    public Toggle InTieToggle;
    public Toggle InDeficitToggle;
	//Toggles for Levels
	public Toggle Level1aToggle;
	public Toggle Level1bToggle;
	public Toggle Level1cToggle;
	public Toggle Level2aToggle;
	public Toggle Level2bToggle;
	public Toggle Level2cToggle;
	public Toggle Level3aToggle;
	public Toggle Level3bToggle;
	public Toggle Level3cToggle;


    //Content ist das Scrollfenster
    private GameObject content;

    //Prefabs
    public GameObject row;

    public List<Toggle> toggles;
    public List<GameObject> rows;

    void Awake()
    {
        content = GameObject.Find("Content");
        if (content == null)
        {
            Debug.LogError("There is no Content in your Scene!");
        }
    }

	// Use this for initialization
	void Start () {
        //Prüft ob Referenzen zu GUI Elemente in der Szene vorhanden sind
		if(pathText == null || metaText == null 
			|| Level1aToggle == null || Level1bToggle == null || Level1cToggle == null
			|| Level2aToggle == null || Level2bToggle == null || Level2cToggle == null
			|| Level3aToggle == null || Level3bToggle == null || Level3cToggle == null )
        {
            Debug.LogWarning("Some GUI elements are missing! Please check ImporterGUI references!");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Erzeugt für jeden Nutzer einen Eintrag in der Liste
    public void generateSubjectList()
    {
        //Alte Listenelemente müssen gelöscht werden, bevor neue generiert werden.
        foreach(GameObject gameObject in rows)
        {
            Destroy(gameObject);
        }
        rows.Clear();
        toggles.Clear();

        for (int i = 1; i < 61; i++)
        {
            GameObject rowInstance = (GameObject)Instantiate(row, new Vector3(0, 0, 0), Quaternion.identity);
            rowInstance.transform.Find("User_Name").GetComponent<Text>().text = "VP "+i;
            rowInstance.transform.SetParent(content.transform, false);
            rowInstance.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -80 * i, 0);

            rows.Add(rowInstance);
            toggles.Add(rowInstance.transform.Find("Toggle").GetComponent<Toggle>());
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, 60 * 80);
    }

    //Zeigt den Pfad der Versuchsdaten an
    public void setPathText(string path)
    {
        this.pathText.text = path;
    }

    //Zeigt an, wie viele Versuchspersonen geladen wurden
    public void setMetaText(int count)
    {
        this.metaText.text = count + " subject data were loaded";
    }

    //Aktiviert oder deaktivert alle Toggles in der Liste
    public void toggleAll(bool b)
    {
       foreach(Toggle t in toggles)
        {
            t.isOn = b;
        }
    }

    public void initializeSubjectToggles()
    {
        for (int i = 0; i < Subjects.instance.subjects.Count; i++)
        {
            toggles[i].isOn = Subjects.instance.subjects[i].selected;
        }
    }

    public void initializeMapToggles()
    {

     //   heatMapToggle.isOn = MetaSettings.instance.heatMap;
    //    traceMapToggle.isOn = MetaSettings.instance.traceMap;
      //  viewDirectionMapToggle.isOn = MetaSettings.instance.viewDirectionMap;
     //   relevantObjectsToggle.isOn = MetaSettings.instance.relevantObjects;
    }

    //Folgende Funktion werden ausgelöst, wenn die Toggles betätigt werden.

    public void heatMapToggled(bool newValue)
    {
   //     MetaSettings.instance.heatMap = newValue;
    }
    public void traceMapToggled(bool newValue)
    {
   //     MetaSettings.instance.traceMap = newValue;
    }
    public void viewDirectionMapToggled(bool newValue)
    {
   //     MetaSettings.instance.viewDirectionMap = newValue;
    }
    public void relevantObjectsToggled(bool newValue)
    {
    //    MetaSettings.instance.relevantObjects = newValue;
    }

}
