using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeButton : MonoBehaviour {

	grid grid;
	menu menu;

	// Use this for initialization
	void Start () {
		menu = GameObject.Find("Canvas").GetComponent<menu>();
		grid = GameObject.Find("GameController").GetComponent<grid>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){
		// show home screen
		menu.hideMenu();
		grid.showHomeScreen();
		grid.changeState(GameState.MAINMENU);

		// general button sound
		grid.audioPlayer.playAudioClip(8);
		grid.audioPlayer.randSnapshot(1f);
	}
}
