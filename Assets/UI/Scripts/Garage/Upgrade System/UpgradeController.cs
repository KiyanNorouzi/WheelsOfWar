using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{

    //*******************************Singleton**********************************

    #region Singlton

    private static UpgradeController _instance;
    public static UpgradeController Instance
    {
        get { return _instance; }
    }
    public static void SetInstance(UpgradeController s_Instance)
    {
        _instance = s_Instance;
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    #endregion



    //*********************************Variables*************************************

    #region Variables

    [Header("Upgrade component for every car")]
    public UpgradePerCars[] upgradePerCars;

    public Color vipColor;


    [Header("Camera Variables")]
    public Animator cameraAnimator; //This variable included camera animator 
    public GarageCameraMover cameraMover;


    public CarUpgrade[] carUpgrade;

    int timeRem;


    UpgradeItem upgradeItemState;
    GeneralGarageStates generalGarageStates;   // Main States in Scene

    int currentCarIndex
    {
        get { return MainGarageCarController.Instance.CurrentCarIndex; }
    }


    #endregion


    void Start()
    {
        for (int i = 0; i < Accounting.Instance.currentUser.carUpgrades.Count; i++)
        {
            CarUpgrade u = Accounting.Instance.currentUser.carUpgrades[i];

            int carIndex = Information.Instance.GetCarIndex(u.CarTag);
            if (carIndex == -1)
                Debug.Log("car tag not found, " + u.CarTag);
            else
            {
                /*UpgradeState state = UpgradeState.BOUGHT;
                if (u.TimeRemaining > 0)
                    state = UpgradeState.BUY_NEXT_LEVEL_UP;

                upgradePerCars[carIndex].SetItem((UpgradeItem)u.PartIndex, u.Level, state);*/
            }
        }
    }


    int elementIndex;
    public void Element_Button(int _elementIndex)
    {
        elementIndex = _elementIndex;

        upgradeItemState = (UpgradeItem)elementIndex;
        UpgradeParts part = UpgradePerCars.ConvertUpgradeItemToPart(upgradeItemState);

        CarUpgrade upgrade = Accounting.Instance.currentUser.GetCarUpgrade(Information.Instance.carInfo[currentCarIndex].carTag, (int)part);

        


        if (upgrade != null && upgrade.TimeRemaining > 0)
        {
            int seconds = Mathf.RoundToInt(upgrade.TimeRemaining); // -Accounting.Instance.currentUser.CalculateSecondsFromLogin();
            infoTablet.ActivateAlreadyBought(upgradePerCars[currentCarIndex].GetItem(upgradeItemState) + 1, seconds);
        }
        else if (upgrade!=null && upgrade.Level >= UpgradeInitializer.Instance.allUpgrades[(int)part].upgrades.Length)
            infoTablet.ActivateMaxLevel(upgradePerCars[currentCarIndex].GetItem(upgradeItemState) + 1);
        else
        {
            float priceMultiplyer = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.UpgradeCost);
            int price = UpgradeInitializer.Instance.allUpgrades[(int)part].upgrades[upgradePerCars[currentCarIndex].GetItem(upgradeItemState)].upgradeCost.Bills;

            infoTablet.ActivateNormal(upgradePerCars[currentCarIndex].GetItem(upgradeItemState) + 1, Mathf.RoundToInt(price * priceMultiplyer));
        }
            


        


        
        CammeraState(8 + elementIndex);

        UpgradeSliderController.Instance.ShowUpgradeInfo(elementIndex);
    }


    public void RefreshInfoTablet()
    {
        Element_Button(elementIndex);
    }


    public UpgradeInfoTablet infoTablet;




    /*
    void UpgradeSliderEnable(GameObject _sliderGameObject, GameObject _textGameObject)
    {
        for (int i = 0; i < upgradeUIGameObjects.Length; i++)
        {
            upgradeUIGameObjects[i].SetActive(false);
        }

        _sliderGameObject.SetActive(true);
        _textGameObject.SetActive(true);

    }


    public void DisableUpgradeSliders()
    {

        for (int i = 0; i < upgradeUIGameObjects.Length; i++)
        {
            upgradeUIGameObjects[i].SetActive(false);
        }
    }

    public void DisableDowngradeImage()
    {
        for (int i = 0; i < downgradeUIImage.Length; i++)
        {
            downgradeUIImage[i].SetActive(false);
        }
    }
    */



    
    public void UpgradeBuy_Button()
    {
        if (Accounting.Instance.currentUser.CanUpgrade)
            Accounting.Instance.currentUser.Buy(UpgradeInitializer.Instance.allUpgrades[(int)upgradeItemState].upgrades[upgradePerCars[currentCarIndex].GetItem(upgradeItemState)].upgradeCost, ConfirmUpgradeBuy);
        else
            CommonUI.Instance.messageBox.ShowMessage(Messages.YouHaveAnotherUpgradeInProgress, _GoToUpgradingItem, true);
    }

    private void _GoToUpgradingItem()
    {
        int upgradingPartIndex  =Accounting.Instance.currentUser.UpgradingPartIndex;

        UpgradeParts part = (UpgradeParts)upgradingPartIndex;
        int index = (int)UpgradePerCars.ConvertUpgradePartToItem(part);

        Element_Button(index);
        ButtonSelectedColorManager.Instance.ChangeUpgradeImageColor_Button(index);
    }

    void ConfirmUpgradeBuy()
    {
        Accounting.Instance.currentUser.BuyCarUpgrade(Information.Instance.carInfo[currentCarIndex].carTag,
            (int)UpgradePerCars.ConvertUpgradeItemToPart(upgradeItemState), upgradePerCars[currentCarIndex].GetItem(upgradeItemState) + 1,
            (int)UpgradeInitializer.Instance.allUpgrades[(int)upgradeItemState].upgrades[upgradePerCars[currentCarIndex].GetItem(upgradeItemState)].upgradeTime);

        Element_Button(elementIndex);
    }

    public void DeliveryBuy_Button()
    {
        int seconds = Mathf.RoundToInt(Accounting.Instance.currentUser.UpgradingPart.TimeRemaining); // -Accounting.Instance.currentUser.CalculateSecondsFromLogin();
        int skipPrice = Accounting.Instance.currentUser.CalculateSkipUpgradeTimePrice(seconds);
        if (skipPrice == 0)
            _DeliverySkipDone();
        else
        {
            PriceStructure price = new PriceStructure();
            price.Golds = skipPrice;
            Accounting.Instance.currentUser.Buy(price, _DeliverySkipDone);
        }

        /*
        switch (upgradeItemState)
        {
            #region Machinegun Delivery Buy

            case UpgradeItem.RIFEL_UPGRADE:

                upgradePerCars[currentCarIndex].machinegunUpgradeState = UpgradeState.BOUGHT;

                upgradePerCars[currentCarIndex].machinegunUpgradeState = UpgradeState.TIMER_DONE_FOR_NEXT_LEVEL_UP;
                break;

            #endregion

            #region Rocket Delivery Buy

            case UpgradeItem.ROCKET_UPGRADE:

                upgradePerCars[currentCarIndex].rocketUpgradeState = UpgradeState.BOUGHT;

                upgradePerCars[currentCarIndex].rocketUpgradeState = UpgradeState.TIMER_DONE_FOR_NEXT_LEVEL_UP;

                break;

            #endregion

            #region Mine Delivery Buy

            case UpgradeItem.MINE_UPGRADE:

                upgradePerCars[currentCarIndex].mineUpgradeState = UpgradeState.BOUGHT;

                upgradePerCars[currentCarIndex].mineUpgradeState = UpgradeState.TIMER_DONE_FOR_NEXT_LEVEL_UP;

                break;

            #endregion

            #region Chassis Delivery Buy

            case UpgradeItem.CHASIS_UPGRADE:

                upgradePerCars[currentCarIndex].rocketUpgradeState = UpgradeState.BOUGHT;

                upgradePerCars[currentCarIndex].chassisUpgradeState = UpgradeState.TIMER_DONE_FOR_NEXT_LEVEL_UP;

                break;

            #endregion

            #region Engine Delivery Buy

            case UpgradeItem.ENGINE_UPGRADE:

                upgradePerCars[currentCarIndex].engineUpgradeState = UpgradeState.BOUGHT;

                upgradePerCars[currentCarIndex].engineUpgradeState = UpgradeState.TIMER_DONE_FOR_NEXT_LEVEL_UP;

                break;

            #endregion

            #region Guard Delivery Buy

            case UpgradeItem.GUARD_UPGRADE:

                upgradePerCars[currentCarIndex].guardUpgradeState = UpgradeState.BOUGHT;

                upgradePerCars[currentCarIndex].guardUpgradeState = UpgradeState.TIMER_DONE_FOR_NEXT_LEVEL_UP;

                break;

            #endregion
        }*/
    }

    private void _DeliverySkipDone()
    {
        Accounting.Instance.currentUser.SkipUpgrade();
        Element_Button(elementIndex);
    }

    public void DeliveryUpgrade_Button()
    {
        Debug.Log("delivery button 2");

        /*
        switch (upgradeItemState)
        {
            #region Machinegun Delivery Upgrade

            case UpgradeItem.RIFEL_UPGRADE:

                upgradePerCars[currentCarIndex].machinegunUpgradeState = UpgradeState.BOUGHT;

                break;

            #endregion

            #region Rocket Delivery Upgrade

            case UpgradeItem.ROCKET_UPGRADE:

                upgradePerCars[currentCarIndex].rocketUpgradeState = UpgradeState.BOUGHT;
                break;

            #endregion

            #region Mine Delivery Upgrade

            case UpgradeItem.MINE_UPGRADE:

                upgradePerCars[currentCarIndex].mineUpgradeState = UpgradeState.BOUGHT;
                break;

            #endregion

            #region Chassis Delivery Upgrade

            case UpgradeItem.CHASIS_UPGRADE:

                upgradePerCars[currentCarIndex].chassisUpgradeState = UpgradeState.BOUGHT;
                break;

            #endregion

            #region Engine Delivery Upgrade

            case UpgradeItem.ENGINE_UPGRADE:

                upgradePerCars[currentCarIndex].engineUpgradeState = UpgradeState.BOUGHT;
                break;

            #endregion

            #region Guard Delivery Upgrade

            case UpgradeItem.GUARD_UPGRADE:

                upgradePerCars[currentCarIndex].guardUpgradeState = UpgradeState.BOUGHT;
                break;

            #endregion
        }*/
    }


    void OnEnable()
    {
        Accounting.Instance.currentUser.OnVIPPackagesChanged += currentUser_OnVIPPackagesChanged;
    }

    void OnDisable()
    {
        Accounting.Instance.currentUser.OnVIPPackagesChanged -= currentUser_OnVIPPackagesChanged;
    }

    void currentUser_OnVIPPackagesChanged()
    {
        Element_Button(elementIndex);
    }

    void CammeraState(int _index)
    {
        cameraMover.SetCameraAngle(250, GeneralGarageStates.UPGRADE);
        cameraMover.touchAgent.enabled = false;
        cameraMover.autoRotateEnabled = false;
        cameraAnimator.SetInteger("CamMove", _index);
    }



    public void SetStepOne()
    {
        Element_Button(0);
    }



    #region Get Functions

    float _GetSpeed(int _carIndex)
    {
        return Information.Instance.carInfo[_carIndex].levels[0].speed;
    }

    float _GethealthValue(int _carIndex)
    {
        return Information.Instance.carInfo[_carIndex].levels[0].health;
    }

    float _GetArmor(int _carIndex)
    {
        return Information.Instance.carInfo[_carIndex].levels[0].armor;
    }

    float _GetRifleMaxDamage(int _carIndex)
    {
        return Information.Instance.carInfo[_carIndex].machineGunInfo.damageMax;
    }

    float _GetMineMaxDamage(int _carIndex)
    {
        return Information.Instance.carInfo[_carIndex].minerInfo.damageMax;
    }

    float _GetRocketMaxDamage(int _carIndex)
    {
        return Information.Instance.carInfo[_carIndex].rocketLauncherInfo.damageMax;
    }



    #endregion


}

#region Upgrade State

#endregion