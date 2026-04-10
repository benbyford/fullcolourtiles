using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditsButton : MonoBehaviour {

	public Color32 textColor;
	public Color32 textColorToggled;
	public Text creditsTextButton;
	public grid GameController;
	public UiController uiCon;

	public bool toggled = false;
	private Text text;

	// Use this for initialization
	void Awake () {

		// set button text component
		text = creditsTextButton.GetComponent<Text>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){
		#if (UNITY_EDITOR)
		Debug.Log("Credits Clicked");
		#endif

		if(toggled){
			Debug.Log("Toggling menu to show");
			toggled = false;
			GameController.homeScreen.SetActive(true);
			GameController.startButton.SetActive(true);
			GameController.clearButton.SetActive(true);
			GameController.showScoreButton.SetActive(true);
			GameController.langChangeButton.SetActive(true);
			GameController.startButton.SetActive(true);
            gameObject.GetComponent<Button>().interactable = true;
            //uiCon.stateChange(GameState.MAINMENU);

            GameController.creditsExitButton.SetActive(false);
            GameController.creditsText.SetActive(false);
			GameController.titleOverlay.SetActive(false);

			// change text color
			text.color = textColor;

		}else{
            Debug.Log("Toggling credits to show");
            toggled = true;
			GameController.homeScreen.SetActive(false);
            GameController.startButton.SetActive(false);
            GameController.clearButton.SetActive(false);
			GameController.showScoreButton.SetActive(false);
			GameController.langChangeButton.SetActive(false);
			gameObject.GetComponent<Button>().interactable = false;
            uiCon.stateChange(GameState.CREDITSMENU);

			GameController.creditsExitButton.SetActive(true);
            GameController.creditsText.SetActive(true);
			GameController.titleOverlay.SetActive(true);

			// change text color
			text.color = textColorToggled;

			// general button sound
			GameController.audioPlayer.playAudioClip(8);
		}
	}
}
