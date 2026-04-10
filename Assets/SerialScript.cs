using UnityEngine;
using UnityEngine.UI;

public class SerialScript : MonoBehaviour
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = Application.version;
    }
}
