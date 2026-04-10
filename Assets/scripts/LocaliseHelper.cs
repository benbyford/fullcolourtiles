using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

// top ten steam languages
public enum LangEnum
{
    EN,     // english
    ZH_CH,  // simplified chinese
    RU,     // russian
    ES,     // spanish
    PT,     // portuguese
    DE,     // german
    JA,     // japanese
    FR,     // french
    KO      // korean
}

public class LocaliseHelper : MonoBehaviour
{
    // default is enlgish so dont include in switch statement
    public LangEnum currentLang = LangEnum.EN;
    public SystemLanguage systemLang = SystemLanguage.English;

    void Start()
    {
        //This checks if your computer's operating system is in a particularlanguage
        switch (Application.systemLanguage)
        {
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.ChineseTraditional:
            case SystemLanguage.Chinese:
                currentLang = LangEnum.ZH_CH;
                systemLang = SystemLanguage.ChineseSimplified;
                break;
            case SystemLanguage.Russian:
                currentLang = LangEnum.RU;
                systemLang = SystemLanguage.Russian;
                break;
            case SystemLanguage.Spanish:
                currentLang = LangEnum.ES;
                systemLang = SystemLanguage.Spanish;
                break;
            case SystemLanguage.Portuguese:
                currentLang = LangEnum.PT;
                systemLang = SystemLanguage.Portuguese;
                break;
            case SystemLanguage.German:
                currentLang = LangEnum.DE;
                systemLang = SystemLanguage.German;
                break;
            case SystemLanguage.Japanese:
                currentLang = LangEnum.JA;
                systemLang = SystemLanguage.Japanese;
                break;
            case SystemLanguage.French:
                currentLang = LangEnum.FR;
                systemLang = SystemLanguage.French;
                break;
            case SystemLanguage.Korean:
                currentLang = LangEnum.KO;
                systemLang = SystemLanguage.Korean;
                break;
        }
        #if ENABLE_UNITYEVENTS
        Debug.Log("Current lang: " + currentLang.ToString());
#endif

        SetLang(systemLang);
    }

    public void SetLang(SystemLanguage lang)
    {
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
