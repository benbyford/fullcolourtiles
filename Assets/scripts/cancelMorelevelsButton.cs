using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cancelMorelevelsButton : MonoBehaviour {

	public grid grid;
	public moreLevelsButtonHomeScreen moreLevelsButtonHome;
	public bool returnHome = false;

	// Use this for initialization
	void Start () {

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){
		// show home screen
		gameObject.transform.parent.gameObject.SetActive(false);
		grid.titleOverlay.SetActive(false);

		// got back to home
		if(returnHome){
			grid.showHomeScreen();
            grid.changeState(GameState.MAINMENU);
            moreLevelsButtonHome.moreLevelsText.color = moreLevelsButtonHome.textColor;
		}else{
			// got back to menu
			grid.menu.showMenu();
            grid.changeState(GameState.LEVELMENU);
        }

		// general button sound
		grid.audioPlayer.playAudioClip(8, 0.6674f);
	}
}
