
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialQuickChatChallenge : MonoBehaviour
{
	public List<Sprite> emojis;
	private TutorialGameState tutorialGameState;

	public GameObject speechbubble;
	private Image emojiRenderer;
	// Use this for initialization
	void Start()
	{
		tutorialGameState = (TutorialGameState)FindObjectOfType(typeof(TutorialGameState));
		emojiRenderer = speechbubble.transform.Find("Emoji").GetComponent<Image> ();
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
		tutorialGameState.EndChallenge (0,0);
	}

}
