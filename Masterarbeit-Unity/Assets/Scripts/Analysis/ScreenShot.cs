/************************************************************************************

Dieses Skript steuert die Screenshot Generierungen.
Vor den Screenshots graut sie die Szene aus.

************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    //Referenzen
    public Camera screenshotCam;
    public HeatMap heatMap;
    
    //Prefabs
    public GameObject greyheatmap;
    
    //Variablen, die nicht im Inspektor geändert werden
    private int screenshotResolution;
    private List<Vector3> coordinates;

    private string subjectName;
    private string path;

	public bool screenshotsReady;
    private bool screenshotInProgress;

    public static ScreenShot instance;

    //Singleton pattern
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Initialize
        screenshotsReady = false;

        //Erzeugt den Screenshot Ordner falls noch nicht vorhanden
        path = "ResearchData/Screenshots";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //Prüft auf Fehlerhafte Einstellungen.
        if (heatMap.heatMapHeight >= screenshotCam.transform.position.y)
        {
            Debug.LogWarning("Heatmap position is bigger than Heatmap Camera position!");
        }

        screenshotResolution = (int)(Mathf.Round(screenshotCam.orthographicSize) / 45);
        if (screenshotResolution < 4)
        {
            screenshotResolution = 4;
        }
        if (screenshotResolution > 10)
        {
            screenshotResolution = 10;
        }

        //Start Screenshots
        if (Subjects.instance.subjects.Count>0)
        {
            StartCoroutine(screenshotQueueAnalysis());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
	 

    //Nimmt ein Screenshot auf und exportiert es
    private void TakeScreenShot(string screenshotPath)
    {
        int width = Screen.width * screenshotResolution;
        int height = Screen.height * screenshotResolution;

        //Quelle    14.01.16 23:00
        //http://gamedev.stackexchange.com/questions/79047/take-a-screenshot-from-camera-problem
        RenderTexture rt = new RenderTexture(width, height, 24);
        screenshotCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        screenshotCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotCam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToJPG();
        File.WriteAllBytes(screenshotPath, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", screenshotPath));
    }
   

    //Erzeugt Maps und nimmt Screenshots im Analyse Modus auf
    public IEnumerator screenshotQueueAnalysis()
    {
        greyOutScene();
     //   UI.instance.setInformationText("Start generating maps & screenshots, please wait...");
        yield return new WaitForSeconds(1f);
        
      
       //     UI.instance.setInformationText("Start Combined Heat Map, please wait...");
            yield return new WaitForSeconds(1f);
            heatMap.generateCombinedHeatmapCoordinates(Subjects.instance.subjects);
            heatMap.generateCombinedHeatmap();
            TakeScreenShot(generateScreenShotPath(subjectName, "Combined_Heat_Map"));
            heatMap.hideHeatMapObject();
     //      UI.instance.setInformationText("Combined Heat Map generated");
            yield return new WaitForSeconds(1f);
        
		      
    //    UI.instance.setInformationText("Finished!");
        yield return new WaitForSeconds(2f);
    //    UI.instance.setInformationText("");


    }
	   
    //Generiert den Speicherpfad
    private string generateScreenShotPath(string screenshotName, string screenshotType)
    {
       screenshotName = "_VP_" + screenshotName;
       return path + "/" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + screenshotName + "_" + screenshotType + "_" + SceneManager.GetActiveScene().name + ".jpg";
    }

    //Eingrauen der Spielwelt
    public void greyOutScene()
    {
        Terrain terrain = Terrain.activeTerrain;
        GameObject greyHeatmap = (GameObject)Instantiate(greyheatmap, new Vector3(terrain.transform.position.x + terrain.terrainData.size.x / 2, 35, terrain.transform.position.z + terrain.terrainData.size.z / 2), Quaternion.identity);
        greyHeatmap.transform.localScale = new Vector3(terrain.terrainData.size.x / 10, terrain.terrainData.size.y / 10, terrain.terrainData.size.z / 10);
    }




}
