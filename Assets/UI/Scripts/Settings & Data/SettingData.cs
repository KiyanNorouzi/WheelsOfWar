using UnityEngine;
using System.Collections;

public class SettingData
{
    static bool soundInitialized;
    public static bool SoundInitialized
    {
        get { return SettingData.soundInitialized; }
    }




    public static float MusicVolume
    {
        get { return PlayerData.GetFloat("_musicv", GeneralSettings.Instance.DefaultSoundVolume); }
        set 
        {
            float vol = Mathf.Clamp01(value);
            PlayerData.SetFloat("_musicv", vol);

            if (IsMute)
                AudioListener.volume = 0;
            else
                AudioListener.volume = vol;
        }
    }

    public static bool IsMute
    {
        get { return PlayerData.GetInt("_ismute", 0) == 1; }
        set
        {
            PlayerData.SetInt("_ismute", value ? 1 : 0);
            if (value)
                AudioListener.volume = 0;
            else
                AudioListener.volume = MusicVolume;
        }
    }

    public static void SetSoundSettings()
    {
        soundInitialized = true;
        
        MusicVolume = MusicVolume;
        IsMute = IsMute;
    }

    public static float SFXVolume
    {
        get { return PlayerData.GetFloat("_sfxv", 1); }
        set { PlayerData.SetFloat("_sfxv", value); }
    }

    public static bool IsViberationOn
    {
        get { return (PlayerData.GetInt("_vib", 1) == 1); }
        set {PlayerData.SetInt("_vib", value?1:0); }
    }

    public static event Data.generalDelegate OnSwitchLanguage;

    public static int LanguageIndex
    {
        get 
        { 
            if (Accounting.Instance == null || Accounting.Instance.currentUser == null)
                return PlayerPrefs.GetInt("_lang", (int)GeneralSettings.Instance.DefaultLanguage);
            else
                return PlayerData.GetInt("_lang", (int)GeneralSettings.Instance.DefaultLanguage); 
        }
        set 
        { 
            PlayerData.SetInt("_lang", value);
            PlayerPrefs.SetInt("_lang", value);

            if (OnSwitchLanguage != null)
                OnSwitchLanguage();
        }
    }

    public static float AccelerometerControlSensivity
    {
        get { return PlayerData.GetFloat("controlsensivity", 0); }
        set { PlayerData.SetFloat("controlsensivity", value); }
    }

    public static float JoystickControlSensivity
    {
        get { return PlayerData.GetFloat("jcontrolsensivity", 0); }
        set { PlayerData.SetFloat("jcontrolsensivity", value); }
    }


    public static bool TextureQuality
    {

		get { return PlayerData.GetInt("texturequality", 1) == 1;}
        set { PlayerData.SetInt("texturequality", value ? 1 : 0);
			if(value){
				QualitySettings.SetQualityLevel(2);
			}
			else{
				QualitySettings.SetQualityLevel(0);
			}
		}
    }

    public static bool Lights
    {
        get { return PlayerData.GetInt("light", 1) == 1; }
        set { PlayerData.SetInt("light", value ? 1 : 0); }
    }

    public static bool VFX
    {
        get { return PlayerData.GetInt("VFX", 1) == 1; }
        set { PlayerData.SetInt("VFX", value ? 1 : 0);}
    }

    public static bool Colors
    {
        get { return PlayerData.GetInt("Colors", 1) == 1; }
        set { PlayerData.SetInt("Colors", value ? 1 : 0); }
    }
}

public enum LanguageName
{
    English,
    Persian
}