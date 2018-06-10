using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTutorial : MonoBehaviour
{

	public float acceleration;  //der Beschleunigungswert des Partikels
	[Range(1, 3)]
	public int strength;    //Stärke des Schusses. Normaler Schuss =1, Mittlerer Schuss = 2, Großer Schuss = 3.
	private string shotType;    //Typ des Schusses
	public int ballImpact;     //Wirkung des Schusses auf den Ball bei einer Kollision
	public int playerTeam;
	private int shotID;
	public float stunDuration;
	public GameObject player;
	private TrailRenderer trailRenderer;
	public GameObject shotDestructionPrefab;
	private TutorialGameState tutorialGameState;
	SpriteRenderer spriteRenderer;
	public TutorialLogging tutorialLogging;

	// Use this for initialization
	void Start()
	{
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));

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
		player = GameObject.FindGameObjectWithTag("Player");
		tutorialLogging = player.GetComponent<TutorialLogging>();
		//playerTeam = player.GetComponent<PlayerTutorial>().playerTeam;

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
			collidingObject.GetComponent<BlockTutorial>().ReduceHealth(strength);
			DestroyShot();
			if (playerTeam != 55)   //wenn der schuss nicht von einer Kanone kam
			{
				tutorialLogging.AddAccuracy("block");
			}

			break;
		case "Player":
			if (collidingObject.GetComponent<PlayerTutorial>().playerTeam != playerTeam)   //wenn der Schuss den gegnerischen Spieler trifft
			{
				// reference: https://answers.unity.com/questions/1100879/push-object-in-opposite-direction-of-collision.html
				// calculate force vector
				Vector3 force = coll.transform.position - transform.position;
				// normalize force vector to get direction only and trim magnitude
				force.Normalize();
				coll.rigidbody.AddForce(force * ballImpact);
				DestroyShot();
				if (playerTeam != 55)
				{
					tutorialLogging.AddAccuracy("player");
				}
			}
			break;
		case "Boundary":
			DestroyShot();
			tutorialGameState.PlaySound("soundSlap", 0.05f);
			if (playerTeam != 55)
			{
				tutorialLogging.AddAccuracy("destroy");
			}
			break;
		case "Shot":
			if (collidingObject.GetComponent<ShotTutorial>().GetPlayerTeam() != playerTeam)
			{
				tutorialGameState.PlaySound("soundSlap", 0.05f);

				if (strength < collidingObject.GetComponent<ShotTutorial>().strength)
				{
					DestroyShot();

				}
				else if (strength == collidingObject.GetComponent<ShotTutorial>().strength)
				{
					DestroyShot();
					if (playerTeam != 55)
					{
						tutorialLogging.AddAccuracy("shot");
					}
				}
				else if (strength >= collidingObject.GetComponent<ShotTutorial>().strength)
				{
					if (playerTeam != 55)
					{
						tutorialLogging.AddAccuracy("shot");
					}
				}
			}
			else
			{
				if (shotID > collidingObject.GetComponent<ShotTutorial>().GetShotID())
				{
					DestroyShot();
					if (playerTeam != 55)
					{
						tutorialLogging.AddAccuracy("destroy");
					}
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
			collidingObject.GetComponent<BallTutorial>().SetLastHitBy(playerTeam);
			switch (strength)
			{
			case 1:
				collidingObject.GetComponent<BallTutorial>().PlayHitSound(0.3f);
				break;
			case 2:
				collidingObject.GetComponent<BallTutorial>().PlayHitSound(0.5f);
				break;
			case 3:
				collidingObject.GetComponent<BallTutorial>().PlayHitSound(0.8f);
				break;
			}
			DestroyShot();
			if (playerTeam != 55)
			{
				tutorialLogging.AddAccuracy("ball");
			}
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
		else if (playerTeam == 55)
		{
			if (shotType.Equals("large"))
			{
				trailRenderer.material = Resources.Load<Material>("Materials/TrailRendererLargeShotCannon");
			}
			else if (shotType.Equals("medium"))
			{
				trailRenderer.material = Resources.Load<Material>("Materials/TrailRendererMediumShotCannon");
			}
		}
	}


	public void DestroyShot()
	{
		GameObject go = Instantiate(shotDestructionPrefab, transform.position, transform.rotation);  //Die Zerstörungsanimation des Shots wird  instanziiert
		go.GetComponent<ShotDestruction>().SetColor(spriteRenderer.color);
		Destroy(gameObject);
	}

	public void SetColor(Color col)
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.color = col;
	}

	public Color GetColor()
	{
		return spriteRenderer.color;
	}


	public void SetPlayerTeam(int i)
	{
		playerTeam = i;
	}

	public int GetPlayerTeam()
	{
		return playerTeam;
	}

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
