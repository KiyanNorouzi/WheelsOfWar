using UnityEngine;
using System.Collections;

public class Information : MonoBehaviour
{
    #region Singleton

    static Information _instance;

    public static Information Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        flags = new BitwiseFlag();
        flags.FromInt(PlayerPrefs.GetInt("flags", 0));

        boosterSlots = new BitwiseFlag();
    }

    #endregion

    public CarInfo[] carInfo;

    BitwiseFlag flags, boosterSlots;
   

    public bool IsFreeCoinAvailable(FreeCoinType type)
    {
        switch (type)
        {
            case FreeCoinType.Facebook: return !flags[(int)FlagsValues.FacebookCoinsCollected];
            case FreeCoinType.Instagram: return !flags[(int)FlagsValues.InstagramCoinsCollected];
            case FreeCoinType.VideoAd: return true; // AdPlayManager.Instance.IsAdAvailable;
        }

        return false;
    }

    public void FreeCoinCollected(FreeCoinType type)
    {
        switch (type)
        {
            case FreeCoinType.Facebook: _SetFlag((int)FlagsValues.FacebookCoinsCollected, true); break;
            case FreeCoinType.Instagram: _SetFlag((int)FlagsValues.InstagramCoinsCollected, true); break;
            case FreeCoinType.VideoAd:
                break;
        }
    }





    void _SetFlag(int index, bool value)
    {
        flags[index] = value;
        PlayerPrefs.SetInt("flags", flags.ToInt());
    }



    public bool GetBoosterAvailablity(int i)
    {
        return boosterSlots[i];
    }


    public void SetBoosterAvailablity(int i, bool isAvailable, bool sync = true)
    {
        boosterSlots[i] = isAvailable;
        //PlayerData.SetInt("_bbst", boosterSlots.ToInt());

        //if (sync)
            //Accounting.Instance.SetBoosterSlotsStateTillSuccess();
    }



    public void LoadPersonalData()
    {
        //boosterSlots.FromInt(PlayerData.GetInt("_bbst"));
    }

    public int GetCarIndex(string carTag)
    {
        carTag = carTag.ToLower();
        for (int i = 0; i < carInfo.Length; i++)
        {
            if (carInfo[i].carTag.ToLower() == carTag)
                return i;
        }

        return -1;
    }




    public int GetPartValue(int carIndex, UpgradeParts part, bool addUpgrades, bool addCosmetics)
    {
        int defaultValue = Mathf.RoundToInt(Information.Instance.carInfo[carIndex].GetUpgradePartDefaultValue(part));

        if (part == UpgradeParts.Engine)
            Debug.Log("*** part" + part + ", default value=" + defaultValue);

        if (addUpgrades)
            defaultValue += GetUpgradeAddingStats(carIndex, part);

        if (part == UpgradeParts.Engine)
            Debug.Log("*** part" + part + ", default value after upgrades=" + defaultValue);

        if (addCosmetics)
            defaultValue += Mathf.RoundToInt(defaultValue * GetCosmeticAddingStatsPercent(carIndex, part));

        if (part == UpgradeParts.Engine)
            Debug.Log("*** part" + part + ", default value after cosmetics=" + defaultValue);

        return defaultValue;
    }

    public int GetUpgradeAddingStats(int carIndex, UpgradeParts part)
    {
        int upgradeLevel = 0;
        CarUpgrade upgrade = Accounting.Instance.currentUser.GetCarUpgrade(Information.Instance.carInfo[carIndex].carTag, (int)part);
        if (upgrade != null)
            upgradeLevel = upgrade.Level;

        return UpgradeInitializer.Instance.GetPreviousLevelsUpgradeAmountFor(part, upgradeLevel);
    }

    public float GetCosmeticAddingStatsPercent(int carIndex, UpgradeParts part)
    {
        float cosmeticPercent = CosmeticsSetting.Instance.GetCosmeticStatsPercent((CarType)carIndex, part) / 100f;
        return cosmeticPercent;
    }
}



public enum FlagsValues
{
    FacebookCoinsCollected,
    InstagramCoinsCollected,
}

[System.Serializable()]
public class CarInfo
{
    public string carTag;
    public string carName;
    public string carFaName;
    public int requiredLevel;

    public GunInfo minerInfo, rocketLauncherInfo;
    public MachineGunInfo machineGunInfo;

    public CarClass Class;
    public string carPrefabName, carBurnedPrefabName;
    public GameObject carBurnedPrefab;
    [Multiline]
    public string carHistory, carHistoryFA;
    public int burnedInstanceCount;
    public CarData[] levels;
    public float cameraDistance;
    public float cameraHeight;
    public float cameraTargetHeight;
    public float targetSignHeight;
    public bool commingSoon;
    public bool isAvailable;

    public bool enoughRequired
    {
        get { return Accounting.Instance.currentUser.Level >= requiredLevel; }
    }

    //public bool selected = false;

    public string GetCarNameInCurrentLanguage()
    {
        if (SettingData.LanguageIndex == 0)
            return carName;
        else
            return carFaName;
    }

    public string GetCarHistoryInCurrentLanguage()
    {
        if (SettingData.LanguageIndex == 0)
            return carHistory;
        else
            return carHistoryFA;
    }


    public float GetUpgradePartDefaultValue(UpgradeParts part)
    {
        switch (part)
        {
            default:
            case UpgradeParts.Riffle: return machineGunInfo.damageMax;
            case UpgradeParts.Mine: return minerInfo.damageMax;
            case UpgradeParts.Rocket: return rocketLauncherInfo.damageMax;
            case UpgradeParts.Engine: return levels[0].speed;
            case UpgradeParts.Armor: return levels[0].armor;
            case UpgradeParts.Chasis: return levels[0].health;
        }
    }
}

[System.Serializable()]
public class CarData
{
    public int health;
    public int speed;
    public int handling;
    public int attack;
    public int armor;
    //public float fireRate;
    //public int coolDownTime;

    public int RoundsRequired;
    public PriceStructure price;

    public float blockingDamage
    {
        get
        {
            return (armor * 0.06f) / (1 + armor * 0.06f);
        }
    }

    public string armorNameEN, armorNameFA;
    public string classShield;


}

public class AudioStructBase
{
    public AudioSource player;
    public void Play(AudioClip ac)
    {
        Play(ac, false);
    }

    public void Play(AudioClip ac, bool loop)
    {
        Play(ac, loop, 1);
    }

    public void Play(AudioClip ac, bool loop, float pitch)
    {
        if (ac == null)
            return;

        player.clip = ac;
        player.loop = loop;
        player.pitch = pitch;
        player.Play();
    }
}

public enum CarClass
{
    A,
    B,
    C
}