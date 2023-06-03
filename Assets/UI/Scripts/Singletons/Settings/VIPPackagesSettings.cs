using UnityEngine;
using System.Collections;

public class VIPPackagesSettings : MonoBehaviour 
{
    #region Singleton

    static VIPPackagesSettings _instance;

    public static VIPPackagesSettings Instance
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



    public VIPPackageContent[] packages;





}



[System.Serializable]
public class VIPPackageContent
{
    public string name;
    public VIPAction[] actions;
}


[System.Serializable]
public class VIPAction
{
    public VIPActionType type;
    public float multiplyer;
}

public enum VIPActionType
{
    ExtraRewardBills,
    BoosterPrice,
    BoosterSlotOpen,
    UpgradeDeliveryTime,
    UpgradeCost,
    GasSlotsOpen,
    GasRefillTime,
}