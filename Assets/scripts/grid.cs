using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public enum GameState
{
	MAINMENU,
	LEVELMENU,
	LEVEL,
	ENDOFLEVEL,
	ENDOFGAME,
	LANGMENU,
	DATAMENU,
	CREDITSMENU,
	SCOREMENU
}

public class grid : MonoBehaviour {

	[Header("Version")]
	public int releaseVersion;
	public GameState gameState = GameState.MAINMENU;

	[Header("Grid setup")]
	public float gridSize;
	public GameObject gridprefab;
	public GameObject gridprefab2;
	public GameObject nullTile;
	public GameObject gridParent;
	public GameObject EmptyTile;
	public GameObject tileParent;
	public GameObject tile;
	public GameObject tile2;
	public GameObject tile3;
	public GameObject tile4;
    public GameObject tile5;
    public GameObject tile6;
    public GameObject tile7;
    public GameObject tile8;
    public GameObject tile9;
    public GameObject tile10;
    public GameObject lastLevelObj;
	public particleScript lastLevelObjParticles;
	public GameObject hundredPrecentObj;
	public particleScript hundredPrecentObjParticles;
	public GameObject scoreObj;

	// rewards
	[Header("Rewards")]
	public GameObject reward1;
	public GameObject reward2;
	public GameObject reward3;
	public int lastRewardInt;
	public Color32 textCounterColorGold;
	public Color32 textCounterColorSilver;
	public Color32 textCounterColorBronze;

	[Header("Game status")]
	public int totalTilesInPlay;
	public int levelNo = 0;

	// GUI
	[Header("GUI")]
	public int currentClicks = 0;
	public int maxClicks = 30;
	public GameObject ClickCounter;
	public GameObject menuButton;
	public GameObject restartButton;
	public GameObject restartButtonEndLevel;
	public GameObject nextButton;
	public GameObject homeScreen;
	public GameObject startButton;
	public GameObject creditsButton;
	public GameObject creditsText;
	public GameObject creditsExitButton;
	public GameObject clearButton;
	public GameObject showScoreButton;
	public GameObject levelCounterText;
	public GameObject titleOverlay;
	public GameObject languageChangePanel;
    public GameObject moreLevelsPanel;
    public GameObject langChangeButton;
	public Text moreLevelsText;
	public GameObject quitButton;
	public GameObject levelStarTracker;
	public GameObject reloadProgress;
	public GameObject menuLevelProgress;


	[Header("GUI scores")]
	public GameObject goldCountText;
	public GameObject silverCountText;
	public GameObject bronzeCountText;

	// menu
	[Header("Menu Canvas")]
	public menu menu;

	[Header("Screen")]
	// screen vars
	public float screenWidth;
	public float screenHeight;
	public int screenOrientation = 1;

	[Header("Camera")]
	public float camShakeSize = 0.03f;

	[Header("audio setup")]
//	GameObject music;
	public AudioMixerSnapshot musicOnSnapshot;
	public AudioMixerSnapshot musicOffSnapshot;
	public float fadeTime = 1f;
	public GameObject speakerBtn;
	public audio_player audioPlayer;
	public bool audioPlaying = true;

	// level data
	GameObject newTile;
	levels levelsComp;

	// level data found in levels.cs
	int[,] levelGrid;
	int[,] levelStars;
	public int levelCount;

	// set grid dimensions
	int gridX = 4;
	int gridY = 4;

	float gridOffSetX;
	float gridOffSetY;

	// camera
	GameObject cam;
	float camOrth;

	RaycastHit hitObj;
	Ray ray;
	public bool inputAllowed = true;

	// input
	string lastButton;
	int keyInt;
	Vector3 mousePos;
	[SerializeField] UiController uiController;


	// level win particles
	GameObject particles;

	// tile colors
	colors tileColors;

	// stars
	starCount starStats;
	int hasSeen100percent = 0;

#if UNITY_SWITCH
    private nn.account.Uid userId;
    private const string mountName = "FullColourTiles";
    private const string fileName = "FCTSave";
    private static readonly string filePath = string.Format("{0}:/{1}", mountName, fileName);
#pragma warning disable 0414
    private nn.fs.FileHandle fileHandle = new();
#pragma warning restore 0414
#endif


	public bool isInScene = false;

    /*
	 * 
	 * START
	 * 
	 */

    // this for some reason unknown makes everything run smoothly
    void Awake(){
		Application.targetFrameRate = 60;
#if !UNITY_SWITCH // Switch seems to be unhappy about vsync settings?
		QualitySettings.vSyncCount = 0;
#endif
	}
	void Start () {

		// screen
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		// 0 = landscape, 1 = portrait
		if(screenWidth > screenHeight) screenOrientation = 0;

#if UNITY_SWITCH
        Debug.Log("[Switch] Initilizing account");
        nn.account.Account.Initialize();
        Debug.Log("[Switch] Setting user handle");
        nn.account.UserHandle userHandle = new();

        Debug.Log("[Switch] Checking if there's a preselected user");
        if (!nn.account.Account.TryOpenPreselectedUser(ref userHandle))
        {
           nn.Nn.Abort("Failed to open preselected user.");
        }
		
        Debug.Log("[Switch] Getting account ID");
        nn.Result result = nn.account.Account.GetUserId(ref userId, userHandle);
        result.abortUnlessSuccess();

        Debug.Log("Got account ID " + nn.account.Account.GetUserId(ref userId, userHandle) + " for user: " + userHandle);

        Debug.Log("[Switch] Mounting save data");
        result = nn.fs.SaveData.Mount(mountName, userId);
        result.abortUnlessSuccess();
#endif
        Debug.Log("[Switch] Initilizing/Loading save data");
        InitializeSave();
		Loading();
    }
	public void changeState(GameState state)
	{
        gameState = state;
        uiController.stateChange(gameState);
	}

	public void showHomeScreen(){

		menuButton.SetActive(false);
		restartButton.SetActive(false);
		homeScreen.SetActive(true);
		startButton.SetActive(true);
        changeState(GameState.MAINMENU);
        creditsButton.SetActive(true);
		clearButton.SetActive(true);
		langChangeButton.SetActive(true);
		showScoreButton.SetActive(true);
		ClickCounter.SetActive(false);
        levelStarTracker.SetActive(false);
        levelCounterText.SetActive(false);

#if UNITY_STANDALONE || UNITY_EDITOR
        quitButton.SetActive(true);
#endif

		homeScreen.GetComponent<Animator>().SetBool("fadeOut",false);
		homeScreen.GetComponent<Animator>().Play("titleIn");

		inputAllowed = false;
	}

	public void hideHomeScreen(){

		// fadeout homeScreen
		homeScreen.GetComponent<Animator>().SetBool("fadeOut", true);
		startButton.SetActive(false);
		creditsButton.SetActive(false);
		clearButton.SetActive(false);
		showScoreButton.SetActive(false);
		langChangeButton.SetActive(false);

#if UNITY_STANDALONE || UNITY_EDITOR
        quitButton.SetActive(false);
#endif

		StartCoroutine(setTitleInactive());
	}

	// init background grid
	void setupGrid(){

		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {

				GameObject gridP;

				if(Convert.ToBoolean(y%2)){
					if(Convert.ToBoolean(x%2)){
						gridP = (GameObject) Instantiate(gridprefab, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
					}else{
						gridP = (GameObject) Instantiate(gridprefab2, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
					}
				}else{
					if(Convert.ToBoolean(x%2)){
						gridP = (GameObject) Instantiate(gridprefab2, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
					}else{
						gridP = (GameObject) Instantiate(gridprefab, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
					}
				}

				gridP.transform.SetParent(gridParent.transform);
				gridP.name = "grid_X"+x+"Y"+y;

				gridP.GetComponent<storePosition>().x = x;
				gridP.GetComponent<storePosition>().y = y;
			}
		}
		var m = gridSize * gridX;

		Debug.Log("Screen Height: " + Screen.height.ToString());
        Debug.Log("Screen Width: " + Screen.width.ToString());
	}

	// add level tiles
	public void setupLevel(int newLevelNo){

		// change colors
		tileColors.changeColors();

		levelNo = newLevelNo; // make sure grid global var leveNo is correct

		// save last level loaded
		PlayerPrefs.SetInt("lastLevel", newLevelNo);
        Saving();

        // check to see number of medals
        starStats = getPlayerStarStats();

        // if first time playing then show menu
        if (PlayerPrefs.GetInt("firstTime") <= 0)
        {
			Debug.Log("Setting first time experience");

            menuButton.SetActive(true);
            restartButton.SetActive(true);

            PlayerPrefs.SetInt("firstTime", 1);
            Saving();
        }

        // if all medals are gold then 100% done
        if (starStats.goldCount >= levelCount && hasSeen100percent == 0){

			Debug.Log("Showing 100% screen");

			// turn off menu and restart buttons
			restartButton.SetActive(false);
			menuButton.SetActive(false);

			// set next level to last level screen
			newLevelNo = levelCount;

			// send 100% analytics
			sendAnalytics("100PercentGold", starStats.goldCount, starStats.silverCount, starStats.bronzeCount);

			// show 100% screens and animations
			hundredPrecentObj.SetActive(true);
			hundredPrecentObjParticles.StartEmitting();

			StartCoroutine(hundredPercent());

			hasSeen100percent = 1;
			PlayerPrefs.SetInt("hasSeen100percent", hasSeen100percent);
            Saving();
        }
		// If beaten the last level
		else if(newLevelNo >= levelCount)
		{
			// On hitting play from the main menu, the last level goes from being blue coloured (current level) to default. It is not set to be gold until restarting.

			Debug.Log("Showing end level beaten");

            PlayerPrefs.SetInt("firstTime", 0); // Make it act like first time user next boot if player exits game after beating the final level.
            PlayerPrefs.SetInt("lastLevel", 0);
            Saving();

			menu.SetLastLevelButton();

            isInScene = true;

            // last level reached
            // send analytics event with current score
            if (PlayerPrefs.GetInt("lastLevelReached") == 0){
				Debug.Log("sending analytics");
				sendAnalytics("lastLevelReached", starStats.goldCount, starStats.silverCount, starStats.bronzeCount);
            }

			// last level
			// show end text
#if (UNITY_EDITOR)
			Debug.Log("last level");
#endif

			lastLevelObj.SetActive(true);
			scoreObj.SetActive(true);

			lastLevelObjParticles.StartEmitting();

			// hide menu button and restart
			restartButton.SetActive(false);
			menuButton.SetActive(false);

            Image pointImg = uiController.pointer.GetComponent<Image>();
            pointImg.enabled = false;

            // Update score percentages
            updateScores();

			StartCoroutine(finishedGameRestartGame());
			StartCoroutine(finishHide());

		}else{
				
			hasSeen100percent = 0;

			// any level
			levelGrid = levelsComp.getLevel(levelNo);
			initGrid(gridOffSetX,gridOffSetY);

			lastLevelObj.SetActive(false);
	
			// change color of buttons
			changeButtonColour();

			// check number of tiles
			checkTotalTiles();

			levelCounterText.GetComponent<Text>().text = utility.ToRomanNumber(levelNo + 1);
			levelCounterText.GetComponent<Animator>().Play("bounceText");
		}

		// reset clicks and counter
		currentClicks = 0;
		ClickCounter.GetComponent<Text>().text = Convert.ToString(currentClicks) + " / 30";
		ClickCounter.GetComponent<Text>().color = textCounterColorGold;
        levelStarTracker.GetComponent<Image>().color = textCounterColorGold;

        // change the menu colors for current level button
        Debug.Log("Calling setCurrentLevelButton from grid");
        menu.setCurrentLevelButton(newLevelNo);
		inputAllowed = true;
	}

	public void updateScores()
	{
		goldCountText.GetComponent<Text>().text = Convert.ToString(starStats.goldCount + " / " + levelCount);
		silverCountText.GetComponent<Text>().text = Convert.ToString(starStats.silverCount + " / " + levelCount);
		bronzeCountText.GetComponent<Text>().text = Convert.ToString(starStats.bronzeCount + " / " + levelCount);
	}

	void changeButtonColour()
	{
		restartButton.GetComponent<Image>().color = tileColors.getColor1();
		reloadProgress.GetComponent<Image>().color = tileColors.getColor1();
		menuButton.GetComponent<Image>().color = tileColors.getColor1();
		menuLevelProgress.GetComponent<Image>().color = tileColors.getColor1();
		restartButtonEndLevel.GetComponent<Image>().color = tileColors.getColor0();
		nextButton.GetComponent<Image>().color = tileColors.getColor0();
	}


	// start game on start button
	public void startLevel(int num){

		// start game on level num
		setupLevel(num);

		hideHomeScreen();

		// show buttons 
		menuButton.SetActive(true);
		restartButton.SetActive(true);
		ClickCounter.SetActive(true);
        levelStarTracker.SetActive(true);
        levelCounterText.SetActive(true);

		// allow input after 1 sec
		StartCoroutine(startscreenAfterFade());
	}


	// destroy all current tiles
	public void destroyLastLevel(){

		// kill last level
		GameObject[] destroyTiles1 = GameObject.FindGameObjectsWithTag("tile");

		// destroy all last level tiles
		for(var i = 0; i < destroyTiles1.Length; i ++){ Destroy(destroyTiles1[i]); }
	}

	// set up level tiles
	void initGrid(float gridOffSetX, float gridOffSetY){
		//bool _firstTile = true;

		for (int x = 0; x < gridX; x++) {

			for (int y = 0; y < gridY; y++) {

				int colour = 1; // 0 for red 1 for blue
				tile_data tileData;
				GameObject newTile = null;

                // title 0
                switch (levelGrid[x,y]) {
					case 0:
						// newTile = (GameObject) Instantiate(EmptyTile, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
						break;
					case 1:
						colour = 0;
						goto case 2; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 2:
						newTile = (GameObject) Instantiate(tile, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
						break;
					case 3:
						colour = 0;
						goto case 4; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 4:
						newTile = (GameObject) Instantiate(tile2, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
						break;
					case 5:
						colour = 0;
						goto case 6; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 6:
						newTile = (GameObject) Instantiate(tile3, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
						break;
						// circle title
					case 7:
						colour = 0;
						goto case 8; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 8:
						newTile = (GameObject) Instantiate(tile4, new Vector3((gridSize*x)-gridOffSetX, (gridSize*y)-gridOffSetY, 0), Quaternion.identity);
						break;
					// triangle right
					case 9:
						colour = 0;
						goto case 10; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 10:
						newTile = (GameObject)Instantiate(tile5, new Vector3((gridSize * x) - gridOffSetX, (gridSize * y) - gridOffSetY, 0), Quaternion.identity);
						break;
						// triangle left
					case 11:
						colour = 0;
						goto case 12; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 12:
						newTile = (GameObject)Instantiate(tile6, new Vector3((gridSize * x) - gridOffSetX, (gridSize * y) - gridOffSetY, 0), Quaternion.identity);
						break;
					// triangle up
					case 13:
						colour = 0;
						goto case 14; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 14:
						newTile = (GameObject)Instantiate(tile7, new Vector3((gridSize * x) - gridOffSetX, (gridSize * y) - gridOffSetY, 0), Quaternion.identity);
						break;
					// triangle down
					case 15:
						colour = 0;
						goto case 16; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
					case 16:
						newTile = (GameObject)Instantiate(tile8, new Vector3((gridSize * x) - gridOffSetX, (gridSize * y) - gridOffSetY, 0), Quaternion.identity);
						break;
                    // hexigon left right
                    case 17:
                        colour = 0;
                        goto case 18; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
                    case 18:
                        newTile = (GameObject)Instantiate(tile9, new Vector3((gridSize * x) - gridOffSetX, (gridSize * y) - gridOffSetY, 0), Quaternion.identity);
                        break;
                    // hexigon left right
                    case 19:
                        colour = 0;
                        goto case 20; // this goto sucks, C# doesnt allow switch statements to flow on with code inbetween
                    case 20:
                        newTile = (GameObject)Instantiate(tile10, new Vector3((gridSize * x) - gridOffSetX, (gridSize * y) - gridOffSetY, 0), Quaternion.identity);
                        break;
                    default:
						break;
				}

				if (newTile != null)
				{
                    newTile.transform.SetParent(tileParent.transform);
                    newTile.name = "tile_" + x + "_" + y;

                    tileData = newTile.GetComponent<tile_data>();
                    tileData.color0 = tileColors.getColor0();
                    tileData.color1 = tileColors.getColor1();

                    tileData.x = x;
                    tileData.y = y;
                    tileData.colourChange(colour);
                }
            }
		}
	}


	bool currentlyMouse = true;
	public void TitleHit(GameObject obj, Vector3 pos,  bool mouse)
    {
		// to be used by particle system
        mousePos = pos;
		currentlyMouse = mouse;

        // send a message to the object that got hit
        hit(obj);
    }


    // restart current level
    public void restartLevel(){

		destroyLastLevel();

		particles.GetComponent<particleScript>().StopEmitting();

		inputAllowed = true;

		// enable buttons to be selectable
		menuButton.GetComponent<Button>().interactable = true;
		restartButton.GetComponent<Button>().interactable = true;

		// setup level
		setupLevel(levelNo);
    }


	// Something has been hit
	Color32 hitTileColor;
	public void hit(GameObject hitObj){

		//Debug.Log("Hit Triggered. Object: " + hitObj);
		if( hitObj.tag == "tile" && inputAllowed){

			// increment clicks
			currentClicks = currentClicks + 1;
            ClickCounter.GetComponent<Text>().text = Convert.ToString(currentClicks) + " / 30";

			// tile data
			tile_data hitData = hitObj.GetComponent<tile_data>();
			hitObj.GetComponent<Animator>().Play("tileClick");
			hitTileColor = hitObj.GetComponent<SpriteRenderer>().color;

			hitData.colourChange(hitData.colourVal);

#if (UNITY_EDITOR)
			//Debug.Log("Tile hit with value: "+hitData.colourVal);
#endif

			levelGrid[hitData.x,hitData.y] = hitData.colourVal + 1;

			// do main action 
			levelGrid = hitData.doAction(levelGrid);

			// show click animation
			// move camera forward a touch when tile clicked 
			cam.transform.GetComponent<Camera>().orthographicSize = camOrth + camShakeSize;

			StartCoroutine(waitForOneMill());

			// play tile click sound
			audioPlayer.playAudioClip(0);

			// check if game completed
        	checkLevelComplete();

		}else{
#if (UNITY_EDITOR)
			Debug.Log("not a tile clicked");
#endif
		}
	}

	/*
	 * 
	 * Get total Tiles
	 * 
	 * total number of tiles in play
	 * 
	 **/
	void setLevelScore(){

		Debug.Log("Setting score for level " + levelNo);

		lastRewardInt = getStarLevel(currentClicks);
		Debug.Log("Score for level " + levelNo + " = " + lastRewardInt);

		// get current level score
		int currentLevelScore = PlayerPrefs.GetInt("level"+Convert.ToString(levelNo));
		Debug.Log("Current level score: " + currentLevelScore);

		if(currentLevelScore <= lastRewardInt){

			// set level score in memory
			PlayerPrefs.SetInt("level"+Convert.ToString(levelNo), lastRewardInt);
            Saving();

            // update menu
            menu.updateMenu(levelNo,lastRewardInt);

            // if gold then vibrate
            // only vibrate if on mobile
            if (lastRewardInt == 3){
				StartCoroutine(vibrateAfterSecs(2f));
			}
		}
	}

	int getStarLevel(int check){

		int stars = 1; // private stars variable

		// compare level clicks and current clicks
		if(check >= levelStars[levelNo,1]){
			stars = 1;
		}else if(check > levelStars[levelNo,0]){
			stars = 2;
		}else if(check <= levelStars[levelNo,0]){
			stars = 3; // best score
		}else{
			stars = 0;
		}
		return stars;
	}

	public void flushLevelScores(){
		levelNo = 0;

		PlayerPrefs.SetInt("lastLevel",0);
		PlayerPrefs.DeleteAll();
		DeleteSave();
	}

	/*
	 * 
	 * Get total Tiles
	 * 
	 * total number of tiles in play
	 * 
	 **/ 

	void checkTotalTiles(){
		totalTilesInPlay = 0;

		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				if(levelGrid[x,y] > 0) totalTilesInPlay++;
			}
		}
#if (UNITY_EDITOR)
		Debug.Log("Tiles in play: "+totalTilesInPlay);
#endif
	}


	/*
	 * 
	 * Check Level End
	 * 
	 * compare number of one colour with total number of tiles
	 * 
	 **/

	 void checkLevelComplete(){

		int one = 0;
		int two = 0;

		GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");

		// destroy all last level tiles
		for(var i = 0; i < tiles.Length; i ++){
			int val = tiles[i].GetComponent<tile_data>().colourVal;

			if(val==0){
				one++;
			}else if(val==1){
				two++;
			}
		}

#if (UNITY_EDITOR)
		Debug.Log("Tile colour count: " + one +" & "+ two);
		Debug.Log("Total tiles: " + totalTilesInPlay);
#endif

		// level finished
		if(	one == totalTilesInPlay || two == totalTilesInPlay)
		{
			Debug.Log("Won level");

            for (var i = 0; i < tiles.Length; i++)
			{
				Debug.Log("Setting Tile: " + tiles[i] + " to false from " + tiles[i].GetComponent<tile_data>().interactable);
				tiles[i].GetComponent<tile_data>().interactable = false;
				if (tiles[i].GetComponent<Button>() != null)
				{
                    tiles[i].GetComponent<Button>().interactable = false;
                }
            }
            // disallow input for success animation
            inputAllowed = false;

            // set level score
            setLevelScore();

			// start particles and move to mouse
			particles.SetActive(true);
			ParticleSystem.MainModule settings = particles.GetComponent<ParticleSystem>().main;
			settings.startColor = new ParticleSystem.MinMaxGradient(hitTileColor);

			// move particles to position or mouse position depending on input
			particles.transform.position = mousePos;
			if(currentlyMouse) particles.GetComponent<particleScript>().moveToMouse(mousePos);
			particles.GetComponent<particleScript>().StartEmitting();

			// animate rewards on level finish
			animateEventRewards();

			// show overlay
			menu.overlay.SetActive(true);

			// if new section make a level gap
			int l = levelNo + 1;
			bool sectionLevel = levelsComp.sectionMarkers(l);
			
			if(sectionLevel){
				menu.overlay.GetComponent<Animator>().Play("fadeInNewSection");
			}else{
				menu.fadeInOut();
			}

			// play level win sound
			audioPlayer.playAudioClip(3);

			// start new level

			var menuImage = menuButton.GetComponent<Image>();
			menuImage.color = new Color(menuImage.color.r, menuImage.color.g, menuImage.color.b, 0.2f);
			
			var reloadImage = restartButton.GetComponent<Image>();
			reloadImage.color = new Color(reloadImage.color.r, reloadImage.color.g, reloadImage.color.b, 0.2f);

			if(hitTileColor == restartButtonEndLevel.GetComponent<Image>().color) restartButtonEndLevel.GetComponent<Image>().color = tileColors.getColor1();
			if(hitTileColor == nextButton.GetComponent<Image>().color) nextButton.GetComponent<Image>().color = tileColors.getColor1();

			restartButtonEndLevel.SetActive(true);
			nextButton.SetActive(true);

			changeState(GameState.ENDOFLEVEL);

			menuButton.GetComponent<Button>().interactable = false;
			restartButton.GetComponent<Button>().interactable = false;

			for(var i = 0; i < tiles.Length; i ++){
				tiles[i].GetComponent<tile_data>().interactable = false;
			}

		}else{

			// check whether max clicks reached
			if(currentClicks == maxClicks){

				restartButton.GetComponent<LongHoldButton>().Reload();
			}

			// check level score min max for medals
			int starScore = getStarLevel(currentClicks);
			switch (starScore) {
			case 3:
				ClickCounter.GetComponent<Text>().color = textCounterColorGold;
				levelStarTracker.GetComponent<Image>().color = textCounterColorGold;
                break;
			case 2:
				ClickCounter.GetComponent<Text>().color = textCounterColorSilver;
				levelStarTracker.GetComponent<Image>().color = textCounterColorSilver;
				break;
			case 1:
				ClickCounter.GetComponent<Text>().color = textCounterColorBronze;
				levelStarTracker.GetComponent<Image>().color = textCounterColorBronze;
				break;
			default:
				break;
			}
		}
	}

	int rewardI = 1;
	float waitTime = 0;
	int audioClipNum = 5;
	void animateEventRewards(){

		waitTime = 0.25f;

		GameObject reward = reward1;

		if(rewardI <= lastRewardInt){

			if(rewardI == 2){
				reward = reward2;
				audioClipNum = 6;
				waitTime = waitTime * 1.5f;
			}
			if(rewardI == 3){
				audioClipNum = 7;
				reward = reward3;
				waitTime = waitTime * 3f;
			}

			StartCoroutine(waitFor(waitTime, reward));
			StartCoroutine(playsoundWaitFor(waitTime*1.7f, audioClipNum));

			rewardI++;

		}else{
			// reset for next time
			rewardI = 1;
			audioClipNum = 5;
		}
	}

	/*
	 * 
	 * Sound
	 * 
	 */

	public void setSoundOn(){

		audioPlayer.playAudio();

		// set audio pref
		PlayerPrefs.SetInt("sound", 1);
        Saving();

        audioPlaying = true;
	}
	public void setSoundOff(){

		audioPlayer.pauseAudio();

		speakerBtn.GetComponent<Image>().color = speakerBtn.GetComponent<speakerButton>().inactiveColor;

		// set audio pref
		PlayerPrefs.SetInt("sound", 2);
		Saving();


        audioPlaying = false;
	}

	/*
	 * 
	 * ANAYLTICS
	 * 
	 */

	struct starCount {  
		public int goldCount;  
		public int silverCount;
		public int bronzeCount;
	}

	starCount getPlayerStarStats()
	{

		int gold = 0;
		int silver = 0;
		int bronze = 0;

		for (int i = 0; i < levelCount; i++)
		{

			int star = PlayerPrefs.GetInt("level" + Convert.ToString(i));

			if (star == 3) { gold++; }
			if (star == 2) { silver++; }
			if (star == 1) { bronze++; }
		}

		starCount starStatsCount = new starCount();
		starStatsCount.goldCount = gold;
		starStatsCount.silverCount = silver;
		starStatsCount.bronzeCount = bronze;

		return starStatsCount;
	}
	void sendAnalytics(String eventName, int goldCount, int silverCount, int bronzeCount){
#if !UNITY_SWITCH && !UNITY_PS5 && !UNITY_GAMECORE_XBOXSERIES && !UNITY_GAMECORE
		Analytics.CustomEvent(eventName, new Dictionary<string, object>
			{
				{ "goldCount", goldCount },
				{ "silverCount", silverCount },
				{ "bronzeCount", bronzeCount }
			}
		);
#endif
	}

	void InitializeSave()
	{

#if UNITY_SWITCH
        nn.fs.EntryType entryType = 0;
        nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
        if (result.IsSuccess())
        {
            return;
        }
        if (!nn.fs.FileSystem.ResultPathNotFound.Includes(result))
        {
            result.abortUnlessSuccess();
        }

        byte[] data = UnityEngine.Switch.PlayerPrefsHelper.rawData;
        long saveDataSize = data.LongLength;

        UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();

        result = nn.fs.File.Create(filePath, saveDataSize);
        result.abortUnlessSuccess();

        result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Write);
        result.abortUnlessSuccess();

        const int offset = 0;
        result = nn.fs.File.Write(fileHandle, offset, data, data.LongLength, nn.fs.WriteOption.Flush);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);
        result = nn.fs.FileSystem.Commit(mountName);
        result.abortUnlessSuccess();

        UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
#endif
    }

	void Saving()
	{
		Debug.Log("Saving data");

        PlayerPrefs.Save();

#if UNITY_SWITCH
		byte[] data = UnityEngine.Switch.PlayerPrefsHelper.rawData;
        long saveDataSize = data.LongLength;

        UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();

        nn.Result result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Write);
        result.abortUnlessSuccess();

		result = nn.fs.File.SetSize(fileHandle, data.LongLength);
        result.abortUnlessSuccess();

        const int offset = 0;
        result = nn.fs.File.Write(fileHandle, offset, data, data.LongLength, nn.fs.WriteOption.Flush);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);
        result = nn.fs.FileSystem.Commit(mountName);
        result.abortUnlessSuccess();

        UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
#endif
    }

    void Loading()
	{
		Debug.Log("Loading save data");

#if UNITY_SWITCH
        nn.fs.EntryType entryType = 0;
        nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
        if (nn.fs.FileSystem.ResultPathNotFound.Includes(result)) { return; }
        result.abortUnlessSuccess();

        result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Read);
        result.abortUnlessSuccess();

        long fileSize = 0;
        result = nn.fs.File.GetSize(ref fileSize, fileHandle);
        result.abortUnlessSuccess();

        byte[] data = new byte[fileSize];
        result = nn.fs.File.Read(fileHandle, 0, data, fileSize);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);

        UnityEngine.Switch.PlayerPrefsHelper.rawData = data;
#endif

        // get levels
        levelsComp = gameObject.GetComponent<levels>();
        levelCount = levelsComp.getLevelsCount();
        levelStars = levelsComp.getStars();


        // ***** testing only DO NOT include in production
        //		if(PlayerPrefs.GetInt("version") < releaseVersion){
        //
        //			flushLevelScores();
        //
        //			PlayerPrefs.SetInt("version", releaseVersion);
        //
        //			#if (UNITY_EDITOR)
        //			Debug.Log("Running old version, flushed level data");
        //			#endif
        //
        //		}else{
        //			// edge case
        //			// if player version great or equal then set version to current version
        //			PlayerPrefs.SetInt("version", releaseVersion);
        //		}

        // colors
        tileColors = gameObject.GetComponent<colors>();

        // set grid
        gridOffSetX = (gridSize * gridX) / 2;
        gridOffSetY = (gridSize * gridY) / 2;

        // camera
        cam = GameObject.Find("Main Camera");
        camOrth = cam.transform.GetComponent<Camera>().orthographicSize;

        // get particles
        particles = GameObject.Find("Win particles");
        particles.gameObject.SetActive(false);

        //set up background grid
        setupGrid();


		// ---------------------

        hasSeen100percent = PlayerPrefs.GetInt("hasSeen100percent");

        // if played before
        if (PlayerPrefs.GetInt("firstTime") > 0)
        {
            creditsButton.SetActive(false);

            // load last level
            levelNo = PlayerPrefs.GetInt("lastLevel");
			Debug.Log("Loading level: " + levelNo);

            // setup level tiles
            setupLevel(levelNo);

            gameState = GameState.LEVEL;
        }
        else
        { // if not played before then hide menu and show on second level
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            ClickCounter.SetActive(false);
            levelStarTracker.SetActive(false);
            homeScreen.SetActive(true);
            startButton.SetActive(true);
            //			creditsButton.SetActive(true);

            inputAllowed = false;

            gameState = GameState.MAINMENU;
        }

        uiController.stateChange(gameState);

        // find music object
        //		music = GameObject.Find("music");

        // set snapshot to off before fading in
        //		audioPlayer.noSoundSnapshot.TransitionTo(0f);

        // set audio on or off
        if (PlayerPrefs.GetInt("sound") == 2)
        {
            setSoundOff();
        }
        else
        {
            // set sound on if sound off not set
            setSoundOn();
        }

        // all levels gold then add particles to home screen
        // check to see number of medals
        starStats = getPlayerStarStats();
    }

    public void DeleteSave()
	{
        PlayerPrefs.DeleteAll();
#if UNITY_SWITCH
		Saving();
#endif
	}

#if UNITY_SWITCH
    private void OnDestroy()
    {
		nn.fs.FileSystem.Unmount(mountName);
    }
#endif

    /*
	 *
	 * Animation delays
	 *
	 */
    IEnumerator waitFor(float wait, GameObject reward) {

		yield return new WaitForSeconds(wait);

		reward.SetActive(true);
		reward.GetComponent<Animator>().Play("show-reward");

		animateEventRewards();
	}

	IEnumerator playsoundWaitFor(float wait, int sound = 0) {

		yield return new WaitForSeconds(wait);

		audioPlayer.playAudioClip(sound);
	}


	IEnumerator vibrateAfterSecs(float wait){

		yield return new WaitForSeconds(wait);

		if (SystemInfo.supportsVibration && !audioPlayer.paused){
#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
#endif

#if (UNITY_EDITOR)
			Debug.Log("vibrate");
#endif
		}
	}

	IEnumerator waitForOneMill() {
		yield return new WaitForSeconds(0.1f);

		cam.transform.GetComponent<Camera>().orthographicSize = camOrth;
	}


	IEnumerator waitForNewLevel() {

		yield return new WaitForSeconds(3f);

		// goto next level
		levelNo += 1;

		destroyLastLevel();

		// start new level
		setupLevel(levelNo);
	}

	IEnumerator waitForTwoSec() {
		yield return new WaitForSeconds(2f);

		particles.GetComponent<particleScript>().StopEmitting();
		inputAllowed = true;
	}
		
	IEnumerator startscreenAfterFade() {
		yield return new WaitForSeconds(1.5f);

//		homeScreen.SetActive(false);
		inputAllowed = true;
	}

	IEnumerator setTitleInactive() {
		yield return new WaitForSeconds(2f);

		homeScreen.SetActive(false);
	}

	// show home screen after last level finished
	IEnumerator finishedGameRestartGame() {
        yield return new WaitForSeconds(5f);

		lastLevelObj.GetComponent<Animator>().Play("fadeOutFinish");
		scoreObj.GetComponent<Animator>().Play("fadeOutFinish");

		showHomeScreen();
	}

	// hide last level after its animated down
	IEnumerator finishHide() {
		yield return new WaitForSeconds(8f);

		lastLevelObj.SetActive(false);
		scoreObj.SetActive(false);
		isInScene = false;
	}

	// show home screen after last level finished
	IEnumerator hundredPercent() {
		yield return new WaitForSeconds(8f);

		hundredPrecentObj.SetActive(false);

		Debug.Log("Setting 100% screen to false");

		creditsText.SetActive(true);
		Debug.Log("Enabled credits text");

		titleOverlay.SetActive(true);
		Debug.Log("Enabled title overlay");

		showHomeScreen();
	}
}
