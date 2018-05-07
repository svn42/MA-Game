using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawner : MonoBehaviour
{


    public float spawnTimerMedium;    //Zeit des Aufladens, die benötigt wird, bis der mittlere Schuss spawnen soll in Sekunden
    public float spawnTimerLarge;    //Zeit des Aufladens, die benötigt wird, bis der mittlere Schuss spawnen soll in Sekunden
    public float spawnTimerLimit;   //maximale Zeit bis zum Resetten
    public float shotChargeTime;   //aktuelle Zeit des Aufladens 
    public GameObject normalShotPrefab;  //der normale Schuss wird als Prefab in den Inspector gezogen und hier referenziert
    public GameObject mediumShotPrefab;  //gleiches für den mittleren Schuss
    public GameObject largeShotPrefab;  //und den großen
    public bool spawnable = true;  //sofern ein Schuss spawnen darf (nicht kollidiert), ist die Variable true

    bool spawnNormalShot = false;
    bool spawnMediumShot = false;
    bool spawnLargeShot = false;

    public GameObject chargingShotSprite; //Sprite des charging-Shots (Child)
    private Player player;
    private int playerTeam;
    public int shotCount = 0;

    private Color shotColor;

    // Use this for initialization
    void Start()
    {
        spawnTimerMedium *= 60;               //die SpawnTimer werden in Frames umgerechnet (60 fps)
        spawnTimerLarge *= 60;
        spawnTimerLimit *= 60;
        chargingShotSprite.transform.localScale = new Vector3(0f, 0f, 0f);  //und die Visualisierung des ChargingShots "unsichtbar" gemacht
        player = transform.parent.GetComponent<Player>();
        playerTeam = player.playerTeam;

    }

    // Update is called once per frame
    void Update()
    {

    }

    //In der Methode wird die Transparenz des Blocks gesetzt. Übergeben wird ein Zeit Argument in fps
    public void SetShotType(GameObject prefab)
    {
    }

    //Die Methode wird beim Festhalten des A-Buttons in jedem Frame aufgerufen und erhöht die shotChargeTime.
    public void AddShotChargeTime()
    {
        shotChargeTime++;
        if (shotChargeTime != 0 && shotChargeTime < spawnTimerMedium)
        {
            spawnNormalShot = true;
            spawnMediumShot = false;
            spawnLargeShot = false;
            chargingShotSprite.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            chargingShotSprite.transform.localPosition = new Vector3(-1f, 0f, 0f);
        }
        //sofern die Zeit noch geringer ist als die zu erreichende SpawnZeit
        else if (shotChargeTime > spawnTimerMedium && shotChargeTime < spawnTimerLarge)
        {
            spawnNormalShot = false;
            spawnMediumShot = true;
            spawnLargeShot = false;
            chargingShotSprite.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            chargingShotSprite.transform.localPosition = new Vector3(-0.6f, 0f, 0f);
        }
        else if (shotChargeTime > spawnTimerLarge && shotChargeTime < spawnTimerLimit)
        {
            spawnNormalShot = false;
            spawnMediumShot = false;
            spawnLargeShot = true;
            chargingShotSprite.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
            chargingShotSprite.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else if (shotChargeTime > spawnTimerLimit)
        {
            spawnNormalShot = false;
            spawnMediumShot = false;
            spawnLargeShot = false;
            chargingShotSprite.transform.localScale = new Vector3(0f, 0f, 0f);

        }
    }

    //die Methode wird aufgerufen, sofern der A-Button losgelassen wird 
    public void SpawnShot()
    {
        bool shotFired = false;
        GameObject shot = new GameObject();
        //wenn das Ziel erreicht wurde und der ShotSpawner nicht kollidiert
        if (spawnNormalShot && spawnable)
        {
            shot = Instantiate(normalShotPrefab, chargingShotSprite.transform.position, this.transform.rotation);  //wird der Shot aus dem Prefab instanziiert
            shotFired = true;
            spawnNormalShot = false;
        }
        else if (spawnMediumShot && spawnable)
        {
            shot = Instantiate(mediumShotPrefab, chargingShotSprite.transform.position, this.transform.rotation);  //wird der Shot aus dem Prefab instanziiert
            shotFired = true;
            spawnMediumShot = false;
        }
        else if (spawnLargeShot && spawnable)
        {
            shot = Instantiate(largeShotPrefab, chargingShotSprite.transform.position, this.transform.rotation);  //wird der Shot aus dem Prefab instanziiert
            shotFired = true;
            spawnLargeShot = false;
        }

        if (shotFired)
        {
            shotCount++;
            shot.GetComponent<Shot>().SetDirection(this.transform.rotation);    //Der Schuss bekommt die Rotation des Spielers übergeben
            shot.GetComponent<Shot>().SetColor(shotColor);                      //dessen Farbe
            shot.GetComponent<Shot>().SetPlayerTeam(playerTeam);                //dessen Team
            shot.GetComponent<Shot>().SetShotID(shotCount);                     //sowie seine ID
            shot.name = "Shot_"+ shotCount + "_Player_" + playerTeam;       //der Name wird aus dem Count und der PlayerID gebaut.
        }
        ResetShotChargeTime();

    }

    //Methode, um die Farbe des Schusses zu setzen
    public void SetColor(Color col)
    {
        shotColor = col;    //Die Farbvariable für das Erstellen der neuen Schüsse bekommt die Farbe
    }

    public void ResetShotChargeTime()
    {
        shotChargeTime = 0;    //die Zeit des Aufladens wird zurückgesetzt
        player.SetShotTimer(0);
        chargingShotSprite.transform.localScale = new Vector3(0f, 0f, 0f); //und das Sprite zum aufladen auf 0 gesetzt
    }

}
