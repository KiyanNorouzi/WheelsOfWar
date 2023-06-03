using UnityEngine;
using System.Collections;

public class EnvironmentSpecificSettings : MonoBehaviour 
{
    #region Singleton

    static EnvironmentSpecificSettings _instance;

    public static EnvironmentSpecificSettings Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion 


    public EnvironmentSettings[] settings;
}

[System.Serializable]
public class EnvironmentSettings
{
    public int gasConsume;
    public int unlockLevel;
}
