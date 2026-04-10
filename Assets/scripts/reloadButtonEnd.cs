using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reloadButtonEnd : MonoBehaviour {

	grid GameController;

	// Use this for initialization
	void Start () {

		// get the main grid script
		GameController = GameObject.Find("GameController").GetComponent<grid>();

		// gameObject.GetComponent<Animator>().StopPlayback();

		// set event listener for clickevent
		gameObject.GetComponent<Button>().onClick.AddListener(Clicked);
	}

	public void Clicked(){
		#if (UNITY_EDITOR)
		Debug.Log("reload level");
		#endif

		// only allow click if input allowed
		// if(GameController.inputAllowed){
        GameController.restartLevel();

		GameController.restartButtonEndLevel.SetActive(false);
		GameController.nextButton.SetActive(false);
        GameController.changeState(GameState.LEVEL);

        // level start sound
        GameController.audioPlayer.playAudioClip(2);
	}
}
