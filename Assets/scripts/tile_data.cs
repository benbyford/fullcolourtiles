using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class tile_data : Selectable
{

	public int x;
	public int y;
	public int colourVal = 0; // 0 red 1 blue
	public Color color0;
	public Color color1;
    public Color currentColor;
    
    private bool selected;

    private EventSystem eventSystem;
    private SpriteRenderer sr;
    private int[,] currentLevelGrid; // for storing and changing the current level grid

#pragma warning disable CS0114
    void OnEnable()
#pragma warning restore CS0114
    {
        base.OnEnable();

        sr = gameObject.GetComponent<SpriteRenderer>();
    }

#pragma warning disable CS0114
    public void Select(){
        selected = true;
        sr.color = Color.Lerp(sr.color, Color.black, .2f); 
    }
#pragma warning restore CS0114

    public void Deselect(){
        selected = false;
        sr.color = currentColor; 
    }

    // change and store the color
    public void colourChange(int colour){

		if(colour == 1){
			
			colourVal = 0;
			sr.color = color0;

		}else{
			
			colourVal = 1;
			sr.color = color1;
		}
        currentColor = sr.color;
        
        if(selected) Select();
	}

	// do action for tile when clicked
	public int[,] doAction(int[,] levelGrid){

		currentLevelGrid = levelGrid;

		if(gameObject.GetComponent<plusAction>()){
            flipLeft(x, y);
            flipRight(x, y);
            flipUp(x, y);
            flipDown(x, y);
        }
        else if(gameObject.GetComponent<crossAction>()){
			flipCross(x, y);
		}else if(gameObject.GetComponent<circleAction>()){
            flipCross(x, y);
            flipLeft(x, y);
            flipRight(x, y);
            flipUp(x, y);
            flipDown(x, y);
        }
        else if (gameObject.GetComponent<TriRightAction>()){
            flipRight(x, y);
        }else if (gameObject.GetComponent<TriLeftAction>()){
            flipLeft(x, y);
        }else if (gameObject.GetComponent<TriUpAction>()){
            flipUp(x, y);
        }else if (gameObject.GetComponent<TriDownAction>()){
            flipDown(x, y);
        }else if (gameObject.GetComponent<HexLRAction>()){
			flipLeft(x, y);
            flipRight(x, y);
        }else if (gameObject.GetComponent<HexUDAction>()){
			flipUp(x,y);
            flipDown(x, y);
        }

        return currentLevelGrid;
	}

	void flipTile(int x, int y){

		GameObject flipObject = GameObject.Find("tile_"+x+"_"+y);

		if(flipObject){

			flipObject.GetComponent<tile_data>().colourChange(flipObject.GetComponent<tile_data>().colourVal);
			currentLevelGrid[x,y] = flipObject.GetComponent<tile_data>().colourVal + 1;
		}
	}

    // flip all tiles bordering clicked tile in a cross shape 
    void flipCross(int x, int y)
    {

        // flip tiles up, down, left, right
        flipTile(x - 1, y - 1);
        flipTile(x + 1, y + 1);
        flipTile(x + 1, y - 1);
        flipTile(x - 1, y + 1);

    }

    // flip ->
    void flipRight(int x, int y)
    {
        flipTile(x + 1, y);
    }
    // flip <-
    void flipLeft(int x, int y)
    {
        flipTile(x - 1, y);
    }
    // flip up
    void flipUp(int x, int y)
    {
        flipTile(x, y + 1);
    }
    // flip down
    void flipDown(int x, int y)
    {
        flipTile(x, y - 1);
    }
}
