using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelContainer : MonoBehaviour
{
    // Start is called before the first frame update
    grid grid;

    public bool setted = false;

	// Use this for initialization
	void Start () {
		grid = GameObject.Find("GameController").GetComponent<grid>();

        Debug.Log("Screen Orientation: " + grid.screenOrientation.ToString());
        // if(grid.screenOrientation == 0){
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y + 10f,
                gameObject.transform.position.z);
            setted = true;
        // }
    }
}
