using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showScoreButton : MonoBehaviour {

	public GameObject scoreCancelButton;
	public GameObject scoreObj;
	public Color32 textColor;
	public Color32 textColorToggled;
	public Text showScoreTextButton;

	public grid gridScript;
	private bool toggled = false;
	private Text text;

	// Use this for initialization
	void Start () {

		// set button text component
		text = showScoreTextButton.GetComponent<Text>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){

		if(!toggled){

			// show clear data menu
			scoreObj.SetActive(true);

			// change text color
			text.color = textColorToggled;

			// show home screen
			gridScript.homeScreen.SetActive(false);
            gridScript.startButton.SetActive(false);
            gridScript.creditsButton.SetActive(false);
			gridScript.clearButton.SetActive(false);
			gridScript.langChangeButton.SetActive(false);

			// update scores
			gridScript.updateScores();

			gridScript.titleOverlay.SetActive(true);
			scoreCancelButton.SetActive(true);

			toggled = true;

			// general button sound
			gridScript.audioPlayer.playAudioClip(8);

		}else{

			// change text color
			text.color = textColor;

			// show home screen
			gridScript.homeScreen.SetActive(true);
            gridScript.startButton.SetActive(true);
            gridScript.creditsButton.SetActive(true);
			gridScript.clearButton.SetActive(true);
			gridScript.langChangeButton.SetActive(true);

			scoreObj.SetActive(false);
			scoreCancelButton.SetActive(false);

			gridScript.titleOverlay.SetActive(false);

			toggled = false;
		}
	}
}
