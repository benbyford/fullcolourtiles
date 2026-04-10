using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour, ISelectHandler, IPointerDownHandler
{

	public int levelNo = 0;

	menu canvas;

	scrollHelper sh;
	EventSystem eventSystem;

	grid gridScript;
	UiController uiController;

	// Use this for initialization
	void Start () {

		// get the main grid script
		canvas = GameObject.Find("Canvas").GetComponent<menu>();

		sh = GameObject.Find("MenuContainer").GetComponent<scrollHelper>();

		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

		// get the main grid script
		gridScript = GameObject.Find("GameController").GetComponent<grid>();

		uiController = GameObject.FindGameObjectWithTag("eventsystem").GetComponent<UiController>();

        // set event listener for clickevent
        Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//uiController.ChangeCurrentBtn(eventData.lastPress);

		uiController.controllerUsed = false;

		return;
	}


    public void OnSelect(BaseEventData eventData)
	{
		//Debug.Log("Selected grid tile");

		if (uiController.controllerUsed)
			sh.SnapTo(eventSystem.currentSelectedGameObject);
    }
	
	void Clicked(){
#if (UNITY_EDITOR)
		//Debug.Log("I was clicked, loading level " + levelNo);
#endif

		gridScript.inputAllowed = false;
	
		gridScript.destroyLastLevel();
		gridScript.setupLevel(levelNo);
		gridScript.changeState(GameState.LEVEL);
		uiController.SetCurrentLevelBtn(transform.gameObject);


        canvas.hideMenu();

		// level start sound
		gridScript.audioPlayer.playAudioClip(1);
		gridScript.audioPlayer.randSnapshot(1f);
	}
}
