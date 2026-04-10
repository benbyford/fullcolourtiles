using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colors : MonoBehaviour {
	public int colorCount = 0;
	public Color[] colorArray;

	Color currentColor0;
	Color currentColor1;

	void Start () {
		currentColor0 = colorArray[0];
		currentColor1 = colorArray[1];	
	}

	public void changeColors(){
		
		if(colorCount == colorArray.Length-1){

			currentColor0 = colorArray[colorCount];
			colorCount = 0;
			currentColor1 = colorArray[colorCount];

		}else{
			
			currentColor0 = colorArray[colorCount];
			currentColor1 = colorArray[colorCount+1];
			colorCount++;
		}
	}

	public Color getColor0(){
		return currentColor0;
	}
	public Color getColor1(){
		return currentColor1;
	}
}
