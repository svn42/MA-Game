﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPhoton : MonoBehaviour
{

    public float acceleration;  //der Beschleunigungswert des Partikels
    [Range(1, 3)]
    public int strength;    //Stärke des Schusses. Normaler Schuss =1, Mittlerer Schuss = 2, Großer Schuss = 3.
    private string shotType;    //Typ des Schusses
    public int ballImpact;     //Wirkung des Schusses auf den Ball bei einer Kollision
    private int playerTeam;
    private int shotID;
    public float stunDuration;
    public GameObject player;
    public PlayerLogging playerLogging;
    private TrailRenderer trailRenderer;
    public GameObject shotDestructionPrefab;
	private GameStatePhoton gameState;
	private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
		gameState = (GameStatePhoton)FindObjectOfType(typeof(GameStatePhoton));
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //zuweisen des shotTypes in abhängigkeit zur Stärke
        switch (strength)
        {
            case 1:
                shotType = "normal";
                break;
            case 2:
                shotType = "medium";
                break;
            case 3:
                shotType = "large";
                break;
        }
        //zuweisen des Spielers, der den Schuss abgegeben hat
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerList.Length; i++)
        {
			if (playerList[i].GetComponent<PlayerPhoton>().playerTeam == playerTeam)
            {
                player = playerList[i];
                playerLogging = player.GetComponent<PlayerLogging>();
            }
        }

        if (shotType.Equals("medium") || shotType.Equals("large"))
        {
            trailRenderer = gameObject.GetComponent<TrailRenderer>();
            SetupTrailRenderer();
        }

    }


    // Update is called once per frame
    void Update()
    {
        Move();        //mit jedem Frame wird das Objekt bewegt
    }

    //Methode zum Bewegen
    public void Move()
    {
        transform.Translate(transform.right * acceleration * Time.deltaTime, Space.World);
    }

    //das Partikel bekommt in der Methode den Inputvektor des Spielers übergeben
    public void SetDirection(Quaternion rotation)
    {
        //dieser wird mit der Beschleunigung des Partikels multipliziert
        transform.rotation = rotation;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject collidingObject = coll.gameObject;

        switch (collidingObject.tag)
        {
            case "Block":
                collidingObject.GetComponent<BlockPhoton>().ReduceHealth(strength);
                DestroyShot();
                playerLogging.AddAccuracy("block");
                break;
            case "Player":
                if (collidingObject.GetComponent<PlayerPhoton>().playerTeam != playerTeam)   //wenn der Schuss den gegnerischen Spieler trifft
                {
                    // reference: https://answers.unity.com/questions/1100879/push-object-in-opposite-direction-of-collision.html
                    // calculate force vector
                    Vector3 force = coll.transform.position - transform.position;
                    // normalize force vector to get direction only and trim magnitude
                    force.Normalize();
                    coll.rigidbody.AddForce(force * ballImpact);
                    DestroyShot();
                    playerLogging.AddAccuracy("player");
                }
                break;
            case "Boundary":
                DestroyShot();
                playerLogging.AddAccuracy("destroy");
                gameState.PlaySound("soundSlap", 0.15f);
                break;
            case "Shot":
			if (collidingObject.GetComponent<ShotPhoton>().GetPlayerTeam() != playerTeam)
                {
                    gameState.PlaySound("soundSlap", 0.15f);

				if (strength < collidingObject.GetComponent<ShotPhoton>().strength)
                    {
                        DestroyShot();
                    }
				else if (strength == collidingObject.GetComponent<ShotPhoton>().strength)
                    {
                        playerLogging.AddAccuracy("shot");
                        DestroyShot();
                    }
				else if (strength >= collidingObject.GetComponent<ShotPhoton>().strength)
                    {
                        playerLogging.AddAccuracy("shot");
                    }
                }
                else
                {
				if (shotID > collidingObject.GetComponent<ShotPhoton>().GetShotID())
                    {
                        DestroyShot();
                        playerLogging.AddAccuracy("destroy");
                    }
                }
                break;
            case "Ball":
                // reference: https://answers.unity.com/questions/1100879/push-object-in-opposite-direction-of-collision.html
                // calculate force vector
                Vector3 forceBall = coll.transform.position - transform.position;
                // normalize force vector to get direction only and trim magnitude
                forceBall.Normalize();
                coll.rigidbody.AddForce(forceBall * ballImpact);
			collidingObject.GetComponent<BallPhoton>().SetLastHitBy(playerTeam);
                switch (strength)
                {
                    case 1:
				collidingObject.GetComponent<BallPhoton>().PlayHitSound(0.3f);
                        break;
                    case 2:
				collidingObject.GetComponent<BallPhoton>().PlayHitSound(0.5f);
                        break;
                    case 3:
				collidingObject.GetComponent<BallPhoton>().PlayHitSound(0.8f);
                        break;
                }
                DestroyShot();
                playerLogging.AddAccuracy("ball");
                break;
        }

    }

    public void SetupTrailRenderer()
    {
        if (playerTeam == 1)
        {
            if (shotType.Equals("large"))
            {
                trailRenderer.material = Resources.Load<Material>("Materials/TrailRendererLargeShotRed");
            }
            else if (shotType.Equals("medium"))
            {
                trailRenderer.material = Resources.Load<Material>("Materials/TrailRendererMediumShotRed");
            }
        }
        else if (playerTeam == 2)
        {
            if (shotType.Equals("large"))
            {
                trailRenderer.material = Resources.Load<Material>("Materials/TrailRendererLargeShotBlue");
            }
            else if (shotType.Equals("medium"))
            {
                trailRenderer.material = Resources.Load<Material>("Materials/TrailRendererMediumShotBlue");
            }
        }
    }


    public void DestroyShot()
    {
		GameObject go = Instantiate(shotDestructionPrefab, transform.position, transform.rotation);  //Die Zerstörungsanimation des Shots wird  instanziiert
        go.GetComponent<ShotDestruction>().SetColor(spriteRenderer.color);
        Destroy(gameObject);
    }

	[PunRPC]
	public void SetColor(Vector3 colVector)
    {
		Color colorNew = new Color(colVector.x, colVector.y, colVector.z, 1);
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.color = colorNew;
    }

    public Color GetColor()
    {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        return spriteRenderer.color;
    }

	[PunRPC]
	public void SetPlayerTeam(int i)
	{
		playerTeam = i;
	}

	[PunRPC]
	public void SetShotName(string name)
	{
		gameObject.name = name;
	}

    public int GetPlayerTeam()
    {
        return playerTeam;
    }

	[PunRPC]
    public void SetShotID(int i)
    {
        shotID = i;
    }

    public int GetShotID()
    {
        return shotID;
    }

    public float GetStunDuration()
    {
        return stunDuration;
    }

    public string GetShotType()
    {
        return shotType;
    }

}
