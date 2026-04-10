using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* When a user leaves a level:
 * Save the previous currentlevel, set that level to whatever colour it should be (get its score from playerprefs), set that level the player just left to the current level colour. Call this script from the LongHoldButton menu button code.
 */

public class menu : MonoBehaviour {

	[Header("Game contollers")]
	public grid grid;
	public GameObject menuOuterContainer;
	public levels levels;

	[Header("Buttons")]
	public Button levelButton;
	public GameObject nullObj;
	//public Button moreLevelBtn;
	public GameObject menuButton;
	public GameObject restartButton;
	public GameObject speakerButton;
	public GameObject ClickCounter;
	public GameObject homeButton;
	public GameObject menuContainer; // empty gameObj to group menu buttons
	public GameObject LevelCounterText; // empty gameObj to group menu buttons

	[Header("Menu size")]
	public float maxMenuWidth = 1200f;

	[Header("Colors")]
	public Color32 rewardColor1;
	public Color32 rewardColor2;
	public Color32 rewardColor3;
	public Color32 currentLevelColor;
	public Color32 defaultColor;

	public Sprite[] levelSprites;

	[Header("Overlay")]
	public GameObject overlay;
	public Color32 overlayColor1;
	public Color32 overlayColor2;
	public float overlayTime = 2;


	RectTransform menuPos;
	Button button;
	int levelsCount;
	int lastLevelNo = 0;

	[SerializeField] List<GameObject> levelTiles;

	GameObject updateLastLevelButton;
	GameObject updateButton;

	//scrollHelper scrolling;


	void Start () { // The start

		Debug.Log(Application.systemLanguage.ToString());
		
		overlay.GetComponent<SpriteRenderer>().color = overlayColor1;
		overlay.SetActive(false);

		levelsCount = levels.getLevelsCount();

		#if (UNITY_EDITOR)
		Debug.Log("There are " + levelsCount + " levels");
		Debug.Log("Last played level pre-close: " + PlayerPrefs.GetInt("lastLevel"));
		#endif

		menuPos = menuContainer.GetComponent<RectTransform>();

		// add level buttons to menu
		setupMenu();

		// move menu off screen by default
		menuOuterContainer.transform.localPosition = new Vector3(-3000f,menuPos.transform.localPosition.y,0);

		SetWidths();
    }

	private void SetWidths() { // Sets the width of the scroll rect
		// change menu container size
		// limit max width size
		float canvasWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x - 190;
		if(canvasWidth > maxMenuWidth) canvasWidth = maxMenuWidth;
		menuContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(
			canvasWidth, 
			gameObject.GetComponent<RectTransform>().sizeDelta.y
		);

		// set width of scroll box
		menuOuterContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, gameObject.GetComponent<RectTransform>().sizeDelta.y);
	}
		
	int levelStar = 0;
	public void setupMenu(){ // Inital setup to create the tiles on the level select - only called once at start
		
		levelStar = 0;
		for (int i = 0; i < levelsCount; i++){

			button = (Button) Instantiate(levelButton, new Vector3(0, 0, 0), Quaternion.identity); // These are the level select tiles being instantiated
			button.transform.SetParent(menuContainer.transform);
			button.name = "level"+i;
			button.transform.localPosition = new Vector3(0f,0f,0f);
			button.transform.localScale = new Vector3(1f,1f,1f);

			levelStar = PlayerPrefs.GetInt("level" + Convert.ToString(i));

			button.GetComponent<buttonClick>().levelNo = i;

			// add roman numeral text to button
			button.GetComponentInChildren<Text>().text = utility.ToRomanNumber(i + 1);

			// update look of button
			updatebuttonSprite(button.gameObject, levelStar, i);

			if(grid.levelNo == i) button.GetComponent<Image>().color = currentLevelColor;

            levelTiles.Add(button.gameObject);
        }

        // more level button
        // Button moreLevel = (Button) Instantiate(moreLevelBtn, new Vector3(0, 0, 0), Quaternion.identity);
        // moreLevel.name = "moreButton";
        // moreLevel.transform.SetParent(menuContainer.transform);
        // moreLevel.transform.localPosition = new Vector3(0f,0f,0f);
        // moreLevel.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
    }
		
	public void resetMenuButtons(){ // Reset all level select tiles to default colour

		// get all level buttons
		//GameObject[] levelTiles = GameObject.FindGameObjectsWithTag("levelTile");

		// change color to white
		for(var i = 0; i < levelTiles.Count; i ++){
			levelTiles[i].GetComponent<Image>().color = defaultColor;
		}
	}
		
	public void updateMenu(int levelNo, int rewardNum = 0){

		// levelStar = PlayerPrefs.GetInt("level" + Convert.ToString(levelNo));
		GameObject updateButton = GameObject.Find("level"+ Convert.ToString(levelNo));

		if(updateButton){
			// update look of button
			updatebuttonSprite(updateButton, rewardNum, levelNo);
		}
	}

	void updatebuttonSprite(GameObject button, int stars, int levelNo){ // Update level select tiles on instantiation

		// change colors
		switch (stars) {
			case 1:
				button.GetComponent<Image>().color = rewardColor1;
				break;
			case 2:
				button.GetComponent<Image>().color = rewardColor2;
				break;
			case 3:
				button.GetComponent<Image>().color = rewardColor3;
				break;
			default:
				button.GetComponent<Image>().color = defaultColor;
				break;
		}

		// change icon 
		button.GetComponent<Image>().sprite = levelSprites[0];
	}


	public void showMenu(){ // Shows the menu

        // show menu in middle
        menuOuterContainer.SetActive(true);

		RectTransform menuRect = menuOuterContainer.GetComponent<RectTransform>();
		menuRect.anchoredPosition = new Vector3(0,100,0);

        menuButton.SetActive(false);
		restartButton.SetActive(false);
		ClickCounter.SetActive(false);
		LevelCounterText.SetActive(false);

		// show overlay
		overlay.SetActive(true);
		homeButton.SetActive(true);
		speakerButton.SetActive(true);
		overlay.GetComponent<Animator>().Play("fadeIn");
		menuContainer.GetComponent<Animator>().Play("popinmenu");

		// disable input
		grid.inputAllowed = false;
		grid.destroyLastLevel();

		SetWidths();
	}
		
	public void hideMenu(){ // Hides the menu

		menuOuterContainer.transform.localPosition = new Vector3(-3000f,menuOuterContainer.transform.localPosition.y,0);
		menuOuterContainer.SetActive(false);


        menuButton.SetActive(true);
		restartButton.SetActive(true);
		ClickCounter.SetActive(true);
		LevelCounterText.SetActive(true);

		speakerButton.SetActive(false);
		homeButton.SetActive(false);

		// hide overlay
		overlay.GetComponent<Animator>().SetTrigger("fadeOut");

		// enable input
		StartCoroutine(allowInputAfterHalfS());
	}

	public void fadeInOut(){ // Fades in and out something I guess
		overlay.GetComponent<Animator>().Play("fadeInOut");
	}

	public void SetLastLevelButton() // this is hardcoded to the current amount of levels, if more get added then this will need to be changed
	{
		Debug.Log("Setting level 70's medal colour");

		for (int i = 0; i < levelTiles.Count; i++)
		{
			if (levelTiles[i].name == "level69")
			{
				updateLastLevelButton = levelTiles[i];
			}
		}

        int lastLevelScore = PlayerPrefs.GetInt("level69");

        if (updateLastLevelButton)
        {
            switch (lastLevelScore)
            {
                case 1:
                    updateLastLevelButton.GetComponent<Image>().color = rewardColor1;
                    Debug.Log("Setting level number 70 to bronze");
                    break;
                case 2:
                    updateLastLevelButton.GetComponent<Image>().color = rewardColor2;
                    Debug.Log("Setting level number 70 to silver");
                    break;
                case 3:
                    updateLastLevelButton.GetComponent<Image>().color = rewardColor3;
                    Debug.Log("Setting level number 70 to gold");
                    break;
                default:
                    updateLastLevelButton.GetComponent<Image>().color = defaultColor;
                    Debug.Log("Setting level number 70 to default");
                    break;
            }
        }
    }
		
	public void setCurrentLevelButton(int levelNo){ // Sets the colours for the level tiles on level select

		// last level button
		//GameObject updateLastLevelButton = GameObject.Find("level"+ Convert.ToString(lastLevelNo)); // change to iterate through array. Failing due to objects not being active when going to next level

		Debug.Log("Setting current level");

		for (int i = 0; i < levelTiles.Count; i++)
		{
			if (levelTiles[i].name == "level" + lastLevelNo)
			{
				updateLastLevelButton = levelTiles[i];
			}
			else if (levelTiles[i].name == "level" + levelNo)
			{
				updateButton = levelTiles[i];

            }
				
		}

		int lastLevelScore = PlayerPrefs.GetInt("level" + Convert.ToString(lastLevelNo));
		 
		//Debug.Log("Previous Level: " + (lastLevelNo+1) + " | Current Level: " + (levelNo+1));
		//Debug.Log("Previous level button: " + updateLastLevelButton);

#if (UNITY_EDITOR)
		//Debug.Log("last level:" +lastLevelScore);
#endif

		// add default color if not completed yet, else rewarded color
		if(updateLastLevelButton && lastLevelNo != 70)
        {
			switch (lastLevelScore) {
				case 1:
					updateLastLevelButton.GetComponent<Image>().color = rewardColor1;
					Debug.Log("Setting level number " + (lastLevelNo + 1) + " to bronze");
					break;
				case 2:
					updateLastLevelButton.GetComponent<Image>().color = rewardColor2;
                    Debug.Log("Setting level number " + (lastLevelNo + 1) + " to silver");
                    break;
				case 3:
					updateLastLevelButton.GetComponent<Image>().color = rewardColor3;
                    Debug.Log("Setting level number " + (lastLevelNo + 1) + " to gold");
                    break;
				default:
					updateLastLevelButton.GetComponent<Image>().color = defaultColor;
                    Debug.Log("Setting level number " + (lastLevelNo + 1) + " to default");
                    break;
			}
		}

		// new level button
		//GameObject updateButton = GameObject.Find("level"+ Convert.ToString(levelNo));

		if(updateButton && levelNo != 70)
        {
			updateButton.GetComponent<Image>().color = currentLevelColor;
			Debug.Log("Updating level number " + (levelNo + 1) + " to be current level");
		}

		lastLevelNo = levelNo;
	}


	IEnumerator allowInputAfterHalfS() {
		yield return new WaitForSeconds(0.5f);

		grid.inputAllowed = true;
	}
}
