using UnityEngine;
using System.Collections;

public class PlayerData
{
    public static int GetInt(string key, int defaultValue = 0)
    {
		key = _GetAppropirateKey(key);
		return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static float GetFloat(string key, float defaultValue = 0)
    {
        key = _GetAppropirateKey(key);
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static string GetString(string key, string defaultValue = "")
    {
        key = _GetAppropirateKey(key);
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetInt(string key, int value)
    {
        key = _GetAppropirateKey(key);
        PlayerPrefs.SetInt(key, value);
    }

    public static void SetFloat(string key, float value)
    {
        key = _GetAppropirateKey(key);
        PlayerPrefs.SetFloat(key, value);
    }

    public static void SetString(string key, string value)
    {
        key = _GetAppropirateKey(key);
        PlayerPrefs.SetString(key, value);
    }




    static string _GetAppropirateKey(string key)
    {
        return string.Concat((int)Accounting.Instance.currentUser.UserType, Accounting.Instance.currentUser.Username, "-", key);
    }
}
