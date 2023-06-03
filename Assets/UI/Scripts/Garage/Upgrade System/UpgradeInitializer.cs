using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeInitializer : MonoBehaviour
{
    #region singlton

    private static UpgradeInitializer _instance;
    public static UpgradeInitializer Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion


    public CarUpgrades[] allUpgrades;



    public int GetPreviousLevelsUpgradeAmountFor(UpgradeParts part, int level)
    {
        int index = (int)part;

        int sum = 0;
        for (int i = 0; i < allUpgrades.Length; i++)
        {
            if (i == index)
            {
                sum += allUpgrades[i].GetPreviousLevesUpgrade(level);
                if (part == UpgradeParts.Engine)
                    Debug.Log("added " + allUpgrades[i].GetPreviousLevesUpgrade(level));
            }
            else
            {
                //for (int j = 0; j < allUpgrades[i].upgrades.Length; j++)
                for (int j = 0; j < level; j++)
                {
                    if (allUpgrades[i].upgrades[j].affectingPart == part)
                    {
                        sum -= allUpgrades[i].upgrades[j].decreaseValue;
                        if (allUpgrades[i].upgrades[j].decreaseValue != 0)
                            Debug.Log("*-*-*-*-*-*- subtracted " + ((UpgradeParts)i) + ", amount=" + allUpgrades[i].upgrades[j].decreaseValue);
                    }
                }
            }
        }

        return sum;
    }
}

public enum UpgradeParts
{
    Riffle,
    Mine,
    Rocket,
    Engine,
    Armor,
    Chasis
}

public enum UpgradeItem
{
    RIFEL_UPGRADE,
    ROCKET_UPGRADE,
    MINE_UPGRADE,
    CHASIS_UPGRADE,
    ENGINE_UPGRADE,
    GUARD_UPGRADE
}


[System.Serializable]
public class CarUpgrades
{
    public ItemUpgrade[] upgrades;

    public int GetPreviousLevesUpgrade(int level)
    {
        int sum = 0;



        for (int i = 0; i < level; i++)
            sum += upgrades[i].stat;

        return sum;
    }
}


[System.Serializable]
public class ItemUpgrade
{
    public int stat;

    public UpgradeParts affectingPart;
    public int decreaseValue;

    public PriceStructure upgradeCost;
    public float upgradeTime;
}