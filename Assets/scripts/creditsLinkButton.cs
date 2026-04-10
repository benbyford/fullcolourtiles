using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditsLinkButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void Clicked(){

		#if (UNITY_EDITOR)
		Debug.Log("Credits link clicked");
		#endif

		Application.OpenURL("http://fullcolourtiles.com");
	}
}
