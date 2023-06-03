using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CosmeticsSetting : MonoBehaviour
{
    #region Singleton

    static CosmeticsSetting _instance;

    public static CosmeticsSetting Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion

    public CarCosmetics[] carCosmetics;


    public int GetCosmeticStatsPercent(CarType carType, UpgradeParts part)
    {
        int addingStats = 0;
        for (int i = 0; i < 7; i++)
        {
            SideStates side = (SideStates)i;

            int selectedIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, side);
            if (selectedIndex != -1)
            {
                CosmeticItem item = carCosmetics[(int)carType].GetCosmetic(side, selectedIndex);
                if (item.addingStatCategory == part)
                {
                    //Debug.Log("--- side " + side + ", selected=" + selectedIndex + ", adding cat=" + item.addingStatCategory + ", percent=" + item.addingStatPercent);
                    addingStats += item.addingStatPercent;
                }
            }
        }

        return addingStats;
    }
}

[System.Serializable]
public class CarCosmetics
{
    public string carName;

    public CosmeticColor[] colors;
    public CosmeticItem[] sideFront;
    public CosmeticItem[] sideBack;
    public CosmeticItem[] sideLeft;
    public CosmeticItem[] sideRight;
    public CosmeticItem[] sideTop;
    public CosmeticItem[] rings;


    public int GetGetCosmeticsCount(SideStates side)
    {
        return GetCosmeticsArray(side).Length;
    }

    public CosmeticItem[] GetCosmeticsArray(SideStates side)
    {
        switch (side)
        {
            default:
            case SideStates.COLOR_SIDE: return colors;
            case SideStates.FRONT_SIDE: return sideFront;
            case SideStates.LEFT_SIDE: return sideLeft;
            case SideStates.BACK_SIDE: return sideBack;
            case SideStates.RIGHT_SIDE: return sideRight;
            case SideStates.TOP_SIDE: return sideTop;
            case SideStates.RING_SIDE: return rings;
        }
    }

    public CosmeticItem GetCosmetic(SideStates side, int index)
    {
        CosmeticItem[] array = GetCosmeticsArray(side);
        //Debug.Log("side state=" + side + ", count=" + array.Length + ", index=" + index);
        return array[index];
    }


}

[System.Serializable]
public class CosmeticItem
{
    public string nameEN, nameFA;
    [Multiline]
    public string descFA, descEN;

    public int addingStatPercent;
    public UpgradeParts addingStatCategory;

    public PriceStructure price;
    public Sprite iconSprite;

    public bool used;

    public string GetCosmeticNameInCurrentLanguage()
    {
        if (SettingData.LanguageIndex == 0)
            return nameEN;
        else
            return nameFA;
    }

    public string GetCosmeticDescriptionInCurrentLanguage()
    {
        if (SettingData.LanguageIndex == 0)
            return descEN;
        else
            return descFA;
    }

}

[System.Serializable]
public class CosmeticColor:CosmeticItem
{
    public Color color;
}