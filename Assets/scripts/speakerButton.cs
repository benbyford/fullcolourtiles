using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speakerButton : MonoBehaviour {

	public grid grid;

	[Header("colors")]
	public Color32 activeColor;
	public Color32 inactiveColor;

	private Image img;

	// Use this for initialization
	void Start () {

		// find this image component
		img = gameObject.GetComponent<Image>();

		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);

	}
	
	public void Clicked(){

		if(grid.audioPlaying){

			grid.setSoundOff();

//			img.color = inactiveColor;

		}else{

			grid.setSoundOn();

			img.color = activeColor;

			// general button sound
			grid.audioPlayer.playAudioClip(8);
		}
	}
}
