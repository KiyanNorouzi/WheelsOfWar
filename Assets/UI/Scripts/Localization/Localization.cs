using UnityEngine;
using System.Collections;

public class Localization : MonoBehaviour
{
    #region Singleton

    void Awake()
    {
        //_instance = this;
        CurrentLanguage = (LanguageName)SettingData.LanguageIndex;
        SettingData.OnSwitchLanguage += LanguageSwitched;
    }

    void OnDestroy()
    {
        SettingData.OnSwitchLanguage -= LanguageSwitched;
    }


    #endregion


    public Language[] languages;

    private LanguageName currentLanguage;
    public LanguageName CurrentLanguage
    {
        get { return currentLanguage; }
        set 
        { 
            currentLanguage = value;

            int currentLanguageIndex = (int)currentLanguage;
            for (int i = 0; i < localizedObjects.Length; i++)
            {
                for (int j = 0; j < localizedObjects[i].gameobjects.Length; j++)
                {
                    if (localizedObjects[i].gameobjects[j] != null)
                        localizedObjects[i].gameobjects[j].SetActive(j == currentLanguageIndex);
                }
            }
        }
    }

    void LanguageSwitched()
    {
        CurrentLanguage = (LanguageName)SettingData.LanguageIndex;
    }

    public void Refresh()
    {
        CurrentLanguage = (LanguageName)SettingData.LanguageIndex;
    }


    public LocalizedObject[] localizedObjects;

}

[System.Serializable]
public class LocalizedObject
{
    public string desc;
    public GameObject[] gameobjects;
}


[System.Serializable]
public class Language
{
    public string shortString;
    public LanguageName title;
}