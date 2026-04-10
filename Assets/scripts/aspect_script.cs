using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aspect_script : MonoBehaviour
{
    public float sceneWidth = 10f;
    public float sceneHeight = 12f;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetComponent<Camera>();
        Debug.Log("Aspect: " + cam.aspect.ToString());

        if (cam.aspect < 1)
        {
            // portrait
            Debug.Log("portrait");
            float unitsPerPixel = sceneWidth / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            cam.orthographicSize = desiredHalfHeight;
        }
        else
        {
            // landscape
            Debug.Log("landscape");
            float unitsPerPixel = sceneHeight / Screen.height;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.width;

            cam.orthographicSize = desiredHalfHeight;
        }
    }
}
