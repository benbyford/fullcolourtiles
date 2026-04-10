using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cancelButtonScores : MonoBehaviour {

	public showScoreButton scoresButton;
	public grid grid;

	// Use this for initialization
	void Start () {

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){
		// show home screen
		gameObject.transform.parent.gameObject.SetActive(false);
        grid.changeState(GameState.MAINMENU);

        // toggle clear data button
        scoresButton.Clicked();

		// general button sound
		grid.audioPlayer.playAudioClip(8, 0.6674f);
	}
}
