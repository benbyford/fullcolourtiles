using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startButton : MonoBehaviour {

	grid GameController;


	// Use this for initialization
	void Start () {

		// get the main grid script
		GameController = GameObject.Find("GameController").GetComponent<grid>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	int levelNum = 0;
	public void Clicked(){
		#if (UNITY_EDITOR)
		Debug.Log("start game");
		#endif

		if(GameController.levelNo >= GameController.levelCount){
			
		}else if(GameController.levelNo > 0){
			levelNum = GameController.levelNo;
		}
		GameController.startLevel(levelNum);
        GameController.changeState(GameState.LEVEL);

        // level start sound
        GameController.audioPlayer.playAudioClip(1);
	}
}
