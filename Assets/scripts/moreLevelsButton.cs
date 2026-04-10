using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class moreLevelsButton : MonoBehaviour {

//	public menu menu;
	grid grid;
	cancelMorelevelsButton cancelBtn;

	// Use this for initialization
	void Start () {
		//grid = GameObject.Find("GameController").GetComponent<grid>();

		//// set event listener for clickevent
		//Button btn = gameObject.GetComponent<Button>();
		//btn.onClick.AddListener(Clicked);
	}

	//void Clicked(){
	//	#if (UNITY_EDITOR)
	//	Debug.Log("more levels clicked");
	//	#endif

	//	grid.showMoreLevelsPanel();

	//	cancelBtn = GameObject.Find("cancelMoreLevelsBtn").GetComponent<cancelMorelevelsButton>();
	//	cancelBtn.returnHome = false;

	//	// general button sound
	//	grid.audioPlayer.playAudioClip(8);
	//}
}
