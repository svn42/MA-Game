
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuickChatChallenge : MonoBehaviour
{
	public List<Sprite> emojis;
	private TutorialGameState tutorialGameState;
	private TutorialLogging tutorialLogging;

	public GameObject speechbubble;
	private SpriteRenderer emojiRenderer;
	// Use this for initialization
	void Start()
	{
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		tutorialLogging = gameObject.GetComponent<TutorialLogging>();
		emojiRenderer = speechbubble.transform.Find("Emoji").GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update()
	{
		SetUpZones();
	}

	public void SetUpZones()
	{
		if (emojis.Count > 0)
		{
			emojiRenderer.sprite = emojis[0];
		}

	}

	public void RemoveEmoji(string type)
	{
		if (type.Equals (emojis [0].name)) {
			emojis.RemoveAt (0);
			if (emojis.Count != 0) {
				emojiRenderer.sprite = emojis [0];
			} else {
				StartCoroutine (EndChallenge());
			}
		}
	}

	IEnumerator EndChallenge(){
		yield return new WaitForSeconds (2);		
		tutorialGameState.EndChallenge (0);
	}

}
