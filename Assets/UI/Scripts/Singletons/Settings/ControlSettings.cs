using UnityEngine;
using System.Collections;

public class ControlSettings : MonoBehaviour 
{
    #region Singleton

    static ControlSettings _instance;

    public static ControlSettings Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    #endregion 

    public float AccelerometerMinSensivity, AccelerometerMaxSensivity;
    public float JoystickMinSensivity, JoystickMaxSensivity;
}