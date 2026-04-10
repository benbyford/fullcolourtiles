using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class langButtonScript : MonoBehaviour
{

    public SystemLanguage lang;

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
        Debug.Log("lang changed to: " + lang.ToString());
#endif
        // this must reflect the order in the project settings
        switch (lang)
        {
            case SystemLanguage.English:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                break;
            case SystemLanguage.ChineseSimplified:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                break;
            case SystemLanguage.Russian:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
                break;
            case SystemLanguage.Spanish:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[3];
                break;
            case SystemLanguage.Portuguese:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[4];
                break;
            case SystemLanguage.German:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[5];
                break;
            case SystemLanguage.Japanese:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[6];
                break;
            case SystemLanguage.French:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[7];
                break;
            case SystemLanguage.Korean:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[8];
                break;
        }
    }
}
