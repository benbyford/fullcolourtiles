using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeLangButton : MonoBehaviour
{

    public Color32 textColor;
    public Color32 textColorToggled;
    public Text moreLevelsText;
    public grid GameController;
    public langChangeCancel cancelBtn;

    public bool toggled = false;
    private Text text;

    // Use this for initialization
    void Awake()
    {

        // set event listener for clickevent
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
#if (UNITY_EDITOR)
        Debug.Log("lang Clicked");
#endif

        if (toggled)
        {
            toggled = false;
            GameController.homeScreen.SetActive(true);
            GameController.clearButton.SetActive(true);
            GameController.showScoreButton.SetActive(true);
            GameController.creditsButton.SetActive(true);

            GameController.languageChangePanel.SetActive(false);
            GameController.titleOverlay.SetActive(false);

            // change text color
            moreLevelsText.color = textColor;

        }
        else
        {
            toggled = true;
            GameController.homeScreen.SetActive(false);
            GameController.clearButton.SetActive(false);
            GameController.showScoreButton.SetActive(false);
            GameController.creditsButton.SetActive(false);

            // show panel, do web request, do anayltics
            GameController.languageChangePanel.SetActive(true);
            GameController.titleOverlay.SetActive(true);

            // change text color
            moreLevelsText.color = textColorToggled;

            // general button sound
            GameController.audioPlayer.playAudioClip(8);
        }
    }
}
