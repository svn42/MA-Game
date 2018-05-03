using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Range(1, 2)]
    public int playerTeam;    //Teamzugehörigkeit (1 oder 2)
    public int subjectNr;   //VersuchspersonenNummer aus nem Menü rausziehen
    public float brakingForce;  //Stärke des Abbremsens
    public float maxSpeed;
    public float speedX;    //Geschwindigkeit auf der X-Achse
    public float speedY;    //Geschwindigkeit auf der Y-Achse
    public float acceleration;  //Beschleunigungsvariable
    private Vector3 movementVector; //Bewegungsvektor

    public bool stunned;    //Wenn der Spieler betäubt wurde, wird die Variable true
    public float stunBlinkEffect;   //Zeitliches Intervall (in Sekunden), in dem das Blinken beim Stun stattfindet
    public float stunDurationBall;  //Die Zeit in Sekunden, die der Spieler gestunnt wird, soferne er den Ball berührt

    public GameObject exhaustPrefab; //das Prefab des Abgaspartikels wird über den Inspector bekannt gemacht   
    public GameObject exSpawner;    // der Spawner für die Abgaspartikel wird ebenfalls über den Inspektor bekannt gemacht
    public float exhaustTime;  //Die Zeit der aktuellen Bewegung in Frames. wird erhöht, sofern sich der Spieler bewegt und dient der Überprüfung, ob ein ABgaspartikel gespawnt werden soll
    public float exhaustSpawnTime;  //Die Zeit, die erreicht werden muss, bis ein Abgaspartikel gespawnt werden kann

    public BlockSpawner blockSpawn; //Der Blockspawner des Spielers wird über den Inspector mit dem Spieler verknüpft
    public ShotSpawner shotSpawn;   //Der Shotspawner des Spielers wird über den Inspector mit dem Spieler verknüpft
    public ZoneCollider zoneCollider;
    private PlayerLogging playerLogging;

    private Color teamColor;    //Die Farbe des Spielers, die anhand der Teamzugehörigkeit ermittelt wird


    // Use this for initialization
    void Start()
    {
        CheckTeamColor();   //zu Beginn bekommt der Spieler die richtige Farbe
        blockSpawn.GetComponent<BlockSpawner>().SetColor(teamColor);    //ebenso wird die Farbe dem Blockspawner und dem    
        shotSpawn.GetComponent<ShotSpawner>().SetColor(teamColor);      //ShotSpawner bekannt gemacht

        playerLogging = this.gameObject.GetComponent<PlayerLogging>();
        playerLogging.SetPlayerTeam(playerTeam);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned)
        {
            CheckInput();   //zunächst wird der Input überprüft
            Move(); //dann der Spieler bewegt
            CheckExhaust(); //sowie überprüft, ob das Abgas erzeugt werden soll
        }



    }

    //Sofern es zu einer Collision kommt
    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Boundary")  //und es sich um eine Bande handelt
        {
            speedX /= 6;    //Wird die Geschwindigkeit reduziert. 
            speedY /= 6;

            switch (coll.gameObject.name)
            {
                case "Boundary_Top":    //sofern es sich um die obere Bande handelt,
                    transform.Translate(new Vector3(0f, -50, 0f) * Time.deltaTime, Space.World);    //wird der Spieler nach unten "geschubst"
                    break;
                case "Boundary_Bottom": //bei der unteren Bande 
                    transform.Translate(new Vector3(0f, 50, 0f) * Time.deltaTime, Space.World); //nach oben
                    break;
                case "Boundary_Left":   //bei der linken Bande 
                    transform.Translate(new Vector3(50, 0f, 0f) * Time.deltaTime, Space.World); //nach rechts
                    break;
                case "Boundary_Right":  //und bei der rechten Bande 
                    transform.Translate(new Vector3(-50, 0, 0f) * Time.deltaTime, Space.World); //nach links
                    break;
            }
        }
        if (coll.gameObject.tag == "Ball")  //sofern das Objekt den Tag Ball hat
        {
            speedX /= 6;    //wird die Geschwindigkeit reduziert
            speedY /= 6;
            StartCoroutine(StunPlayer(stunDurationBall));  //und der Spieler für die Zeit "stunDurationBall" gestunnt
            blockSpawn.ResetBlockChargeTime();
        }

    }

    //public void OnCollisionExit2D(Collision2D coll)
    //{
    //    if (coll.gameObject.name == "Boundary_Top" || coll.gameObject.name == "Boundary_Bottom" || coll.gameObject.name == "Boundary_Left" || coll.gameObject.name == "Boundary_Right")
    //    {
    //    }

    //}

    public void CheckInput()
    {
        //sofern die Horizontale Achse betätigt wird (linke oder rechte Pfeiltaste sowie A oder D)
        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f))
        {
            //wird die Accelerate-Methode mit dem Argument X aufgerufen
            Accelerate("X");
        }
        else
        {
            //ansonsten wird die Brake-Methode mit dem Argument X verwendet
            Brake("X");
        }


        //das gleiche geschieht mit der Vertikalen Achse (hoch oder runter Pfeiltaste sowie W und S)
        if ((Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f))
        {
            Accelerate("Y");
        }
        else
        {
            Brake("Y");
        }

        //die BewegungsZeit wird erhöht, sofern mindestens eine der beiden Achsen eine Bewegung zurückliefern
        if ((Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f) || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
        {
            exhaustTime++;
        }
        else
        {
            //wenn die Figur nicht mehr bewegt wird, wird die BewegungsZeit auf 0 zurückgesetzt
            exhaustTime = 0;
        }

        //wenn der Block-Button (B) gedrückt wird
        if (Input.GetButton("Block"))
        {
            blockSpawn.AddBlockChargeTime();    //wird die Zeit zum Spawnen des Blocks hochgezählt
        }
        //wenn der Block-Button (B) losgelassen wird
        if (Input.GetButtonUp("Block"))
        {
            blockSpawn.SpawnBlock();    //wird überprüft, ob der Block gespawnt werden kann (wenn die Zeit groß genug ist)
        }

        //wenn der Schuss-Button (A) gedrückt wird
        if (Input.GetButton("Shoot"))
        {
            shotSpawn.AddShotChargeTime();  //wird die Zeit zum Aufladen des Schuss hochgezählt
        }
        //wenn der Schuss-Button (A) losgelassen wird
        if (Input.GetButtonUp("Shoot"))
        {
            shotSpawn.SpawnShot();  //wird der Schuss gespawnt 
        }

    }

    //Die Methode wird in jedem Update aufgerufen und regelt die Bewegung des Spielers
    public void Move()
    {
        //es wird ein Bewegungsvektor erstellt und dieser bekommt die Geschwindigkeit auf der X und Y Achse übertragen.
        movementVector = new Vector3(speedX, speedY, 0f);

        //der Spieler bewegt sich dann mit Hilfe deses Vektors auf dem Spielfeld. Die Bewegung ist immer relativ zur Spielwelt 
        transform.Translate(movementVector * Time.deltaTime, Space.World);
    }

    //Die Beschleunigen-Methode ermittelt die Geschwindigketi des Spielers bei einem Input
    public void Accelerate(string axis)
    {
        //Sofern das Argument "X" übergeben wird 
        if (axis.Equals("X"))
        {
            //erhöht sich die Geschwindigkeit auf der X-Achse um den Wert des Inputs, multipliziert mit der Beschleunigung
            speedX += Input.GetAxis("Horizontal") * acceleration;
        }
        //else if (axis.Equals("X") && (Mathf.Abs(speedX) < 25 && Mathf.Abs(speedX) >= 0))
        //{
        //    speedX += Input.GetAxis("Horizontal") * acceleration;
        //}

        //sofern die Geschwindigkeit doch außerhalb der Grenzen von -maxSpeed und maxSpeed liegt, wird der Wert an diese Grenzwerte angepasst
        if (speedX > maxSpeed)
        {
            speedX = maxSpeed;
        }
        else if (speedX < -maxSpeed)
        {
            speedX = -maxSpeed;
        }


        //Sofern das Argument Y übergeben wird, wird die Ermittlung der Geschwindigkeit genauso ermittelt wie für die X-Achse
        else if (axis.Equals("Y"))
        {
            speedY += Input.GetAxis("Vertical") * acceleration;
        }
        //else if (axis.Equals("Y") && (Mathf.Abs(speedY) < 25 && Mathf.Abs(speedY) >= 0))
        //{
        //    speedY += Input.GetAxis("Vertical") * acceleration;
        //}
        if (speedY > maxSpeed)
        {
            speedY = maxSpeed;
        }
        else if (speedY < -maxSpeed)
        {
            speedY = -maxSpeed;
        }

    }

    //Sofern kein Input seitens des Spielers kommt, wird die Brake-Methode zum Abbremsen der Figur verwendet
    public void Brake(string axis)
    {
        //sofern die Geschwindigkeit positiv ist
        if (axis.Equals("X") && speedX > 0)
        {
            //wird in die negative Richtung abgebremst
            speedX -= brakingForce;
        }
        //sofern die GEschwindigkeit negativ ist,
        else if (axis.Equals("X") && speedX < 0)
        {
            //wird in die positive Richtung abgebremst
            speedX += brakingForce;
        }


        //für die Y-Achse genauso
        if (axis.Equals("Y") && speedY > 0)
        {
            speedY -= brakingForce;
        }
        else if (axis.Equals("Y") && speedY < 0)
        {
            speedY += brakingForce;
        }

        //endgültiges abremsen, bei Geschwindigkeit um 0
        if (Mathf.Abs(speedX) >= -brakingForce / 2 && Mathf.Abs(speedX) <= brakingForce / 2)
        {
            speedX = 0;
        }
        if (Mathf.Abs(speedY) >= -brakingForce / 2 && Mathf.Abs(speedY) <= brakingForce / 2)
        {
            speedY = 0;
        }
    }

    //die Methode überprüft, ob ein Abgaspartikel erzeugt werden soll 
    public void CheckExhaust()
    {
        //sofern sich der Spieler eine bestimmte Zeit bewegt und diese Zeit über der festgelegten Zeit bis zum Spawnen eines Abgaspartikels liegt
        if (exhaustTime > exhaustSpawnTime * 60)
        {
            //wird ein Abgaspartikel an der Position des ExhaustSpawners erstellt
            GameObject exhaust = Instantiate(exhaustPrefab, exSpawner.transform.position, exSpawner.transform.rotation);
            exhaust.GetComponent<Exhaust>().SetColor(teamColor);    //das Partikel bekommt die Farbe des Spielers
            exhaust.GetComponent<Exhaust>().SetDirection(new Vector3(speedX, speedY, 0));
            //und die Zeit zum Spawnen eines Partikels auf null gesetzt
            exhaustTime = 0;
        }
    }

    //die Methode überprüft die Teamzugehörigkeit und ändert die Farbe des Spielers dementsprechend
    private void CheckTeamColor()
    {
        switch (playerTeam)
        {
            case 1: //Team 1 bekommt die rote Farbe
                teamColor = Color.red;
                break;
            case 2: //Team 2 die Blaue
                teamColor = Color.blue;
                break;
        }
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = teamColor;   //Die Farbe wird an das Sprite übergeben
    }

    //Methode zum Übermitteln der Betäubung an den Spieler. Als Argument wird die Zeit übergeben
    IEnumerator StunPlayer(float time)
    {
        StartCoroutine(StunEffect(time));   //Es wird eine Coroutine für den Blinkeffekt gestartet und die Zeit der Betäubung übergeben.
        stunned = true;                     //die Stun-Variable auf True gesetzt
        Debug.Log("stun true");
        yield return new WaitForSeconds(time);  //Die Zeit der Betäubung abgewartet 
        stunned = false;                        //und daraufhin die Stun-Variable auf false gesetzt

    }

    //Blinkeffekt des Stuns
    IEnumerator StunEffect(float time)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();  //der spriteRenderer Des Spielers wird der lokalen Variable zugewiesen
        int blinkAmount = (int)Mathf.Floor(time / stunBlinkEffect / 2);      //und die Anzahl der Blinkeffekte ermittelt. Die Anzahl ergibt sich aus der Zeit, dividiert durch die Dauer des Blinkeffektes / 2.

        for (int i = 0; i < blinkAmount; i++)   //solange die Anzahl der Blinkeffekte nicht erreicht wurde
        {
            spriteRenderer.color = Color.white;     //wird der Renderer im Wechsel weiß und daraufhin in der ursprünglichen Farbe des Spielers eingefärbt
            yield return new WaitForSeconds(stunBlinkEffect);
            spriteRenderer.color = teamColor;
            yield return new WaitForSeconds(stunBlinkEffect);

        }

    }

}