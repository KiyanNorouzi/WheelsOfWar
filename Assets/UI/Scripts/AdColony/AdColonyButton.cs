using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdColonyButton : MonoBehaviour
{
    #region singleton

    static AdColonyButton _instance;

    public static AdColonyButton Instance
    {
        get { return _instance; }
    }


    void Awake()
    {
        _instance = this;
    }


    #endregion


    [HideInInspector]
    public string zoneId = "";

    public string zoneIdKey = "";


    void Start()
    {
        zoneId = ADCAdManager.GetZoneIdByKey(zoneIdKey);
    }


    public void PerformButtonPressLogic()
    {
        ADCAdManager.ShowVideoAdByZoneKey(zoneIdKey, true, false);
    }


}
