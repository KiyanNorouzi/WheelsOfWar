using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainGarageCosmeticController : MonoBehaviour
{

    //*******************************Singleton**********************************
    #region Singlton

    static MainGarageCosmeticController _instance;

    public static MainGarageCosmeticController Instance
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





    //*********************************Variables*************************************
    #region Variables

    int currentCarIndex
    {
        get
        {
            return MainGarageCarController.Instance.CurrentCarIndex;
        }
    }

    int currentCarLevel
    {
        get
        {
            return MainGarageCarController.Instance.CurrentCarLevel;
        }

        set
        {
            MainGarageCarController.Instance.CurrentCarLevel = value;
        }
    }

    [Header("Cosmetic component for every car")]
    public CosmeticPerCars[] cosmeticPerCars;


    [Header("Camera Variables")]
    public Animator cameraAnimator;       //This variable included camera animator 
    public GarageCameraMover cameraMover;



    [Header("Content Manager Images")]
    public GameObject[] slotGameObjects;
    public Image[] contentImage; //This variavle included by UI Image type


    public string[] explainStrings;



    int[] cosmeticIndexes = new int[7];
    
    SideStates sideStates;
    public SideStates SideStates
    {
        get { return sideStates; }
    }
    #endregion



    //*********************************Properties**************************************
    #region Properties and Functions

    #region Camera Property

    public int cameraMove;
    int currentCameraIndex;
    public int CurrentCameraIndex
    {
        get
        {
            return currentCameraIndex;
        }
        set
        {
            int nextCameraIndex = Mathf.Clamp(value, 0, cameraMove);

            if (nextCameraIndex != currentCameraIndex)
            {
                currentCameraIndex = nextCameraIndex;
                cameraAnimator.SetInteger("CamMove", currentCameraIndex);
            }
        }
    }

    #endregion

    public void SetCosmeticIndex(SideStates side, int index)
    {
        //cosmeticIndexes[(int)side] = index;
        Accounting.Instance.currentUser.SetSelectedCosmetic((CarType)currentCarIndex, side, index);
    }

    public int GetCosmeticIndex(SideStates side)
    {
        //return cosmeticIndexes[(int)side];
        return Accounting.Instance.currentUser.GetSelectedCosmetic((CarType)currentCarIndex, side);
    }


    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        BackToMainCosmetics();
    }




    #endregion




    //*******************************Button Functions**********************************
    #region Button Functions

    public void CosmeticSectionButton(int _index)
    {
        CommonUI.Instance.PlayButtonClick();



        CurrentCameraIndex = _index;

        sideStates = (SideStates)_index;
        ContentSpriteManager();

        int selectedItemIndex = GetCosmeticIndex(sideStates);
        ButtonSelectedColorManager.Instance.ChangeCosmeticImageColor_Button(selectedItemIndex);

        TransactionButtonManager(selectedItemIndex);
        if (_index != 0) // color section
        {
            cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
            cameraMover.touchAgent.enabled = false;
            cameraMover.autoRotateEnabled = false;
        }
    }

    void CameraPositionInCosmeticSelect()
    {
        if (sideStates == SideStates.COLOR_SIDE)
        {
            cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
            cameraMover.touchAgent.enabled = false;
            cameraMover.autoRotateEnabled = false;
        }
        else
        {
            cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
            cameraMover.touchAgent.enabled = false;
            cameraMover.autoRotateEnabled = false;
        }

    }


    int tempItemIndex;
    public void NextItemCosmetic(int _index)
    {
        CommonUI.Instance.PlayButtonClick();
        
        //cosmeticPerCars[currentCarIndex].SetCosmetic(sideStates, _index);
        /*CosmeticItem item = CosmeticsSetting.Instance.carCosmetics[currentCarIndex].GetCosmetic(sideStates, _index);
        infoTablet.Activate(item.nameEN, item.descEN, item.price, CosmeticInfoTabletState.Select);*/
        
        //SetCosmeticIndex(sideStates, _index);

        tempItemIndex = _index;
        cosmeticPerCars[currentCarIndex].SetCosmetic(sideStates, _index);
        TransactionButtonManager(tempItemIndex);
    }

    public void ItemCosmeticBuy_Button()
    {
        //Accounting.Instance.currentUser.Buy(CosmeticsSetting.Instance.carCosmetics[currentCarIndex].colors[ GetCosmeticIndex(sideStates)].price, ConfirmBuy);
        Accounting.Instance.currentUser.Buy(CosmeticsSetting.Instance.carCosmetics[currentCarIndex].GetCosmetic(sideStates, tempItemIndex).price, ConfirmBuy);
    }

    void ConfirmBuy()
    {
        Accounting.Instance.currentUser.BuyCosmetic(Information.Instance.carInfo[currentCarIndex].carTag, (int)sideStates, tempItemIndex);
        Accounting.Instance.currentUser.SetSelectedCosmetic((CarType)currentCarIndex, sideStates, tempItemIndex);
        TransactionButtonManager(tempItemIndex);

        if (Accounting.Instance.currentUser.SelectedCarIndex == currentCarIndex)
            MainGarageCarController.Instance.SaveView();
    }

    public void ItemCosmeticSelect_Button()
    {
        Accounting.Instance.currentUser.SetSelectedCosmetic((CarType)currentCarIndex, sideStates, tempItemIndex);
        TransactionButtonManager(tempItemIndex);

        if (Accounting.Instance.currentUser.SelectedCarIndex == currentCarIndex)
        {
            MainGarageCarController.Instance.SaveView();
        }

    }








    #endregion

    #region Button Manager

    public CosmeticInfoTablet infoTablet;

    void TransactionButtonManager(int tempItem)
    {
        CosmeticItem item = CosmeticsSetting.Instance.carCosmetics[currentCarIndex].GetCosmetic(sideStates, tempItem);


        bool isSaled = Accounting.Instance.currentUser.GetCarCosmeticState((CarType)currentCarIndex, sideStates, tempItem);
        bool isSelected = Accounting.Instance.currentUser.GetSelectedCosmetic((CarType)currentCarIndex, sideStates) == tempItem;


        CosmeticInfoTabletState state;
        if (!item.price.IsFree() && !isSaled)
            state = CosmeticInfoTabletState.Buy;
        else
        {
            if (isSelected)
            {
                state = CosmeticInfoTabletState.Equip;
            }
            else
                state = CosmeticInfoTabletState.Select;
        }

        string descText = "";
        if (item.addingStatPercent != 0)
            descText = string.Format(explainStrings[(int)item.addingStatCategory], item.addingStatPercent);
        else
            descText = explainStrings[explainStrings.Length - 1];


        infoTablet.Activate(item.nameEN, descText, item.price, state);
        SetUpSliders(item.addingStatCategory, item.addingStatPercent, state == CosmeticInfoTabletState.Equip);
    }

    void SetUpSliders(UpgradeParts targetPart, int percent, bool selected)
    {
        for (int i = 0; i < 6; i++)
        {
            UpgradeParts part = (UpgradeParts)i;

            
            int defaultAmount = Information.Instance.GetPartValue(currentCarIndex, part, true, false);
            //float cosmeticPercent = Information.Instance.GetCosmeticAddingStatsPercent(currentCarIndex, part);
            //int cosmeticStats = Mathf.RoundToInt(defaultAmount * cosmeticPercent);

            /*if (part == UpgradeParts.Riffle)
                Debug.Log("default=" + defaultAmount + ", percent=" + cosmeticPercent + ", stat=" + cosmeticStats + ", selected=" + selected);*/

            if (targetPart == part &&  !selected) // show the value
            {
                UpgradeSliderController.Instance.statSliders[i].ShowPercent(defaultAmount, percent);
            }
            else
                UpgradeSliderController.Instance.statSliders[i].ShowPercent(defaultAmount, 0);
        }
    }

    #endregion


    #region Manager Functions



    void ContentSpriteManager()
    {
        CosmeticItem[] items = CosmeticsSetting.Instance.carCosmetics[currentCarIndex].GetCosmeticsArray(sideStates);


        for (int i = 0; i < items.Length; i++)
        {
            contentImage[i].sprite = items[i].iconSprite; // CosmeticsSetting.Instance.carCosmetics[currentCarIndex].colors[i].iconSprite;
            if (sideStates == SideStates.COLOR_SIDE)
            {
                contentImage[i].sprite = null;
                contentImage[i].color = CosmeticsSetting.Instance.carCosmetics[currentCarIndex].colors[i].color;
            }
            else
            {
                contentImage[i].color = Color.white;
                contentImage[i].sprite = items[i].iconSprite;
            }

            slotGameObjects[i].SetActive(true);
        }

        for (int i = items.Length; i < contentImage.Length; i++)
        {
            slotGameObjects[i].SetActive(false);
        }
    }

    public void BackToMainCosmetics()
    {
        CarType carType = (CarType)currentCarIndex;

        cosmeticPerCars[currentCarIndex].ColorIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.COLOR_SIDE);
        cosmeticPerCars[currentCarIndex].SideBackIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.BACK_SIDE);
        cosmeticPerCars[currentCarIndex].SideFrontIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.FRONT_SIDE);
        cosmeticPerCars[currentCarIndex].SideLeftIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.LEFT_SIDE);
        cosmeticPerCars[currentCarIndex].SideRightIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RIGHT_SIDE);
        cosmeticPerCars[currentCarIndex].SideTopIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.TOP_SIDE);
        cosmeticPerCars[currentCarIndex].TireIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RING_SIDE);
    }



    public void DeactivateInfoTablet()
    {
        infoTablet.Deactivate();
    }
    #endregion


}


#region Enum States

public enum SideStates
{
    COLOR_SIDE,
    FRONT_SIDE,
    LEFT_SIDE,
    BACK_SIDE,
    RIGHT_SIDE,
    TOP_SIDE,
    RING_SIDE
}

public enum CosmeticSelection
{
    AVAILABLE,
    LOCKED,
    EQUIPPED,
    NOT_EQUIPPED
}

#endregion

[System.Serializable]
public class TransactionButtonsClass
{
    public GameObject[] buyButtonGameObject;
    public GameObject[] buyTextGameObject;
    public GameObject[] selectGameObject;
    public GameObject[] equippedGameObject;
}