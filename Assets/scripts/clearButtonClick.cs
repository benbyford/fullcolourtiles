using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clearButtonClick : MonoBehaviour {

	GameObject canvas;
	menu menuScript;
	grid gridScript;
	public clearMenuButton clearMenuBtn;
	public Animator homeScreenAnimator;


	// Use this for initialization
	void Start () {
			
		// get the main grid script
		canvas = GameObject.Find("Canvas");
		menuScript = canvas.GetComponent<menu>();

		// get the main grid script
		gridScript = GameObject.Find("GameController").GetComponent<grid>();

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	void Clicked(){
		#if (UNITY_EDITOR)
		Debug.Log("Clear button clicked");
#endif

		gridScript.DeleteSave();

		gridScript.destroyLastLevel();
		menuScript.resetMenuButtons();

		gridScript.flushLevelScores();

		// hide the cancel menu
		gameObject.transform.parent.gameObject.SetActive(false);

		// toggle clear data button
		clearMenuBtn.Clicked();

		// show home screen
		gridScript.homeScreen.SetActive(true);
		gridScript.creditsButton.SetActive(true);

		// general button sound
		gridScript.audioPlayer.playAudioClip(0,1f);
	}
}
