using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startButtonEnd : MonoBehaviour {

	grid GameController;


	// Use this for initialization
	void Start () {

		// get the main grid script
		GameController = GameObject.Find("GameController").GetComponent<grid>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}
	public void Clicked(){
		#if (UNITY_EDITOR)
		Debug.Log("next level");
		#endif

		GameController.levelNo++;
        GameController.restartLevel();
		GameController.changeState(GameState.LEVEL);

		GameController.restartButtonEndLevel.SetActive(false);
		GameController.nextButton.SetActive(false);

        // level start sound
        GameController.audioPlayer.playAudioClip(1);
	}
}
