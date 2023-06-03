using UnityEngine;
using System.Collections;

public class StoreData : MonoBehaviour 
{
    #region Singleton

    static StoreData _instance;

    public static StoreData Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    public Package[] packages;
    public BoosterSlotInfo[] slotsInfo;
}

[System.Serializable]
public class Package
{
    public string tag;
    public string packageNameEN, packageNameFA;
    public string DescEN, DescFA;
    public BoosterPackageType type;
    public PriceStructure price;
    public ItemData[] items;
	public int index;
}

[System.Serializable]
public class ItemData
{
    public ItemType type;
    public float count;
    public float[] parameters;
}

[System.Serializable]
public class BoosterSlotInfo
{
    public BoosterSlotOpenCondition condition;
    public int amount;
}



public enum ItemType
{
    Rocket,
    Mine,
    Health,
    Shield,
    MachinegunDamageMultiplyer,
    HealthRegeneration
}

public enum BoosterPackageType
{
    Minesx2,
    Rocketsx2,
    Rocketsx3,
    Shield100,
    Minesx2Rocketsx3Shield50,
    MachinegunDamagex120,
    HealthRegenerationOneSecond,
}

