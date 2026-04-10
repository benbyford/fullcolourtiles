using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clearMenuButton : MonoBehaviour {

	public GameObject clearMenu;
	public Color32 textColor;
	public Color32 textColorToggled;
	public Text ClearTextButton;

	public grid gridScript;
	private bool toggled = false;
	private Text text;

	// Use this for initialization
	void Start () {

		// set button text component
		text = ClearTextButton.GetComponent<Text>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){

		if(!toggled){

			// show clear data menu
			clearMenu.SetActive(true);

			// change text color
			text.color = textColorToggled;

			// show home screen
			gridScript.homeScreen.SetActive(false);
			gridScript.creditsButton.SetActive(false);
			gridScript.showScoreButton.SetActive(false);
			gridScript.langChangeButton.SetActive(false);

			gridScript.titleOverlay.SetActive(true);

			toggled = true;

			// general button sound
			gridScript.audioPlayer.playAudioClip(8);

		}else{
			
			clearMenu.SetActive(false);
			gridScript.changeState(GameState.MAINMENU);

			// change text color
			text.color = textColor;

			// show home screen
			gridScript.homeScreen.SetActive(true);
			gridScript.creditsButton.SetActive(true);
			gridScript.showScoreButton.SetActive(true);
			gridScript.langChangeButton.SetActive(true);

			gridScript.titleOverlay.SetActive(false);

			toggled = false;
		}
	}
}
