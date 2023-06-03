using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeSliderController : MonoBehaviour
{

    #region Singleton

    static UpgradeSliderController _instance;

    public static UpgradeSliderController Instance
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

    #region Variables

    public UpgradePerCars[] upgradePerCars;
    public StatSlider[] statSliders;

    int currentCarIndex
    {
        get { return MainGarageCarController.Instance.CurrentCarIndex; }
    }

    #endregion

    public void ShowUpgradeInfo(int selectedPartIndex)
    {
        UpgradeParts selectedPart = UpgradePerCars.ConvertUpgradeItemToPart((UpgradeItem)selectedPartIndex);
        selectedPartIndex = (int)selectedPart;

        for (int i = 0; i < 6; i++)
        {
            UpgradeParts part = (UpgradeParts)i;
            int level = upgradePerCars[currentCarIndex].GetItem(part);
            //int prevUpgrades = UpgradeInitializer.Instance.GetPreviousLevelsUpgradeAmountFor(part, level);

            int defaultValue = Information.Instance.GetPartValue(currentCarIndex, part, true, false);

            if (part == selectedPart)
            {
                /*statSliders[i].ShowValue(Information.Instance.carInfo[currentCarIndex].GetUpgradePartDefaultValue(part) + prevUpgrades,
                   UpgradeInitializer.Instance.allUpgrades[i].upgrades[level].stat);*/
                statSliders[i].ShowValue(defaultValue, UpgradeInitializer.Instance.allUpgrades[i].upgrades[level].stat);
            }
            else if (i == (int)UpgradeInitializer.Instance.allUpgrades[selectedPartIndex].upgrades[level].affectingPart 
                && UpgradeInitializer.Instance.allUpgrades[selectedPartIndex].upgrades[level].decreaseValue!=0)
            {
                /*statSliders[i].ShowValue(Information.Instance.carInfo[currentCarIndex].GetUpgradePartDefaultValue(part) + prevUpgrades,
                    -UpgradeInitializer.Instance.allUpgrades[selectedPartIndex].upgrades[level].decreaseValue);*/
                statSliders[i].ShowValue(defaultValue, -UpgradeInitializer.Instance.allUpgrades[selectedPartIndex].upgrades[level].decreaseValue);
            }
            else
            {
                //statSliders[i].ShowValue(Information.Instance.carInfo[currentCarIndex].GetUpgradePartDefaultValue(part) + prevUpgrades, 0);
                statSliders[i].ShowValue(defaultValue, 0);
            }
        }
    }

    public void ShowValues()
    {
        for (int i = 0; i < UpgradeSliderController.Instance.statSliders.Length; i++)
        {
            int defaultValue = Information.Instance.GetPartValue(MainGarageCarController.Instance.CurrentCarIndex, (UpgradeParts)i, true, false);
            UpgradeSliderController.Instance.statSliders[i].ShowValue(defaultValue, 0);

            /*
            if (((UpgradeParts)i) == UpgradeParts.Engine)
                Debug.Log("---- speed=" + defaultValue);
             */
        }
    }
}