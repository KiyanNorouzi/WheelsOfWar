using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoosterPanel : MonoBehaviour
{
    #region Singleton
    /*
    static BoosterPanel _instance;
    public static BoosterPanel Instance
    {
        get { return BoosterPanel._instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }
    */
    #endregion



    public event Data.generalDelegate OnDone;

    public GameObject myGameObject;
    public BoosterSlot[] slots;
    public UnityEngine.UI.Text timerText;
    public float timeForAutoClose;
    public RectTransform myTransform, goButtonTransform;
    public AudioStruct audioStruct;
    public BoosterPanelTutorial tutorial;




    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip selectSlot, unlockSlot;
    }

    float screenWidthK;
    public float ScreenWidthK
    {
        get { return screenWidthK; }
    }

    float time, activeTimerTime;
    bool isTimerActive;
    BoosterPackageType buyingType;


    List<BoosterPackageType> equippedPackages = new List<BoosterPackageType>();
    public List<BoosterPackageType> EquippedPackages
    {
        get { return equippedPackages; }
    }



    int selectedItemIndex;
    public int SelectedItemIndex
    {
        get { return selectedItemIndex; }
        set
        {
            selectedItemIndex = value;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i == selectedItemIndex)
                {
                    slots[i].Select();
                    
                }
                else
                    slots[i].Deselect();
            }
        }
    }


	void Start()
    {
        /*float originalAspect = 16f / 9f;
        float currentAspect = 0;

        if (Screen.width > Screen.height)
            currentAspect = ((float)Screen.width) / ((float)Screen.height);
        else
            currentAspect = ((float)Screen.height) / ((float)Screen.width);

        float f = currentAspect / originalAspect;
        //Debug.Log("orig=" + originalAspect + ", curre=" + currentAspect + ", cam=" + Camera.main.aspect + ", f=" + f);

        myTransform.localScale = new Vector3(f, f, 1);
        goButtonTransform.localScale = new Vector3(1, 1f / f, 1);
        */



        screenWidthK = myTransform.rect.width / (float)Screen.width;
        Debug.Log("width=" + myTransform.rect.width + ", width=" + Screen.width);

        SelectedItemIndex = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].OnClick += Slot_OnClick;
            slots[i].OnCardDrop += BoosterPanel_OnCardDrop;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetIndex(i, StoreData.Instance.slotsInfo[i].condition, StoreData.Instance.slotsInfo[i].amount);
        }
        /*slots[0].SetIndex(0, BoosterSlotOpenCondition.Level, 1);
        slots[1].SetIndex(1, BoosterSlotOpenCondition.Level, 10);
        slots[2].SetIndex(2, BoosterSlotOpenCondition.Level, 25);
        slots[3].SetIndex(3, BoosterSlotOpenCondition.Money, 100);
        slots[4].SetIndex(4, BoosterSlotOpenCondition.Money, 250);*/


        for (int i = 0; i < 5; i++)
        {
            slots[i].Refresh();
        }
        /*slots[0].State = BoosterSlotState.Open;
        slots[1].State = BoosterSlotState.Open;

        slots[2].State = BoosterSlotState.Locked;
        //slots[3].State = BoosterSlotState.Locked;
        slots[3].State = BoosterSlotState.Open;
        slots[4].State = BoosterSlotState.Locked;*/


//        Inventory.Instance.Open();
//        Inventory.Instance.SetOpenButtonEnable(false);

        equippedPackages = new List<BoosterPackageType>();
	}

    int droppingSlotIndex;
    void BoosterPanel_OnCardDrop(int index)
    {
		float priceMultiplyer = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.BoosterPrice);
		if( (StoreData.Instance.packages[slots[index].cardIndex].price.Bills * priceMultiplyer) > Accounting.Instance.currentUser.Bills ){
			CommonUI.Instance.messageBox.Ask(Messages.NotEnoughBills, _BuyBills, null, false);
			return;
		}

		droppingSlotIndex = index;
		_BuyCardConfirm ();

		if (slots [index].IsLocked)
			return;
		
		if (!slots [index].HasItem)
			return;




        Accounting.Instance.currentUser.Buy(StoreData.Instance.packages[slots[index].cardIndex].price * priceMultiplyer, _BuyCardConfirm);
	}

	void _BuyBills()
	{
		CommonUI.Instance.buyCoinsMenu.Activate(BuyCurrencySections.Bills);
	}


    private void _BuyCardConfirm()
    {
        if (BoosterCard.draggingCard != null && !slots[droppingSlotIndex].HasItem)
        {
            slots[droppingSlotIndex].PickUp();
        }
    }

    public void Activate()
    {
        CommonUI.Instance.headerBar.Enable();

        //MoneyTablet.Instance.State = MoneyTabletViewState.JustMoney;
        myGameObject.SetActive(true);

        if (equippedPackages == null)
            equippedPackages = new List<BoosterPackageType>();
        else
            equippedPackages.Clear();

        time = timeForAutoClose;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetIndex(i, StoreData.Instance.slotsInfo[i].condition, StoreData.Instance.slotsInfo[i].amount);
        }


        if (PlayerData.GetInt("boosterTSeen", 0) == 0)
        {
            tutorial.Activate();
            PlayerData.SetInt("boosterTSeen", 1);
        }
        else
            tutorial.Deactivate();
    }

    public void Deactivate()
    {
        //MoneyTablet.Instance.State = MoneyTabletViewState.Off;
        CommonUI.Instance.headerBar.Disable();
        myGameObject.SetActive(false);
    }

    /*
    void OnEnable()
    {
        Inventory.Instance.OnClick += InventoryItem_OnClick;
    }

    void OnDisable()
    {
        Inventory.Instance.OnClick -= InventoryItem_OnClick;
    }
    */


    void InventoryItem_OnClick(BoosterPackageType type)
    {
        TemporarilyStopTimer();

        if (selectedItemIndex == -1)
        {

        }
        else if (slots[selectedItemIndex].HasItem)
        {
            /*int index = (int)type;
            if (slots[selectedItemIndex].PackageType != type)
            {
                Inventory.Instance.AddItemWithAnimation(slots[selectedItemIndex].PackageType, 1, slots[selectedItemIndex].transform.position);
                slots[selectedItemIndex].SetNoItem();

                Inventory.Instance.AddItemWithAnimation(type, -1, slots[selectedItemIndex].transform.position);
                StartCoroutine(_SetItemSlotAfter(Inventory.Instance.addingItems[0].duration, selectedItemIndex, type));
            }
            else
            {
                Debug.Log("current slot already has the same item at " + Time.time);
            }*/
        }
        else
        {
            int itemRepositoryCount = Inventory.Instance.GetItem(type);
            if (itemRepositoryCount == 0)
            {
                buyingType = type;

                Accounting.Instance.currentUser.Buy(StoreData.Instance.packages[(int)type].price, _BuyConfrimed);
                return;
            }


            Inventory.Instance.DisableSlot(type);
            Inventory.Instance.AddItemWithAnimation(type, -1, slots[selectedItemIndex].transform.position, true);
            StartCoroutine(_SetItemSlotAfter(Inventory.Instance.addingItems[0].duration, selectedItemIndex, type));

            RefreshEquippedList();
            reservedIPlaces.Add(selectedItemIndex);

            _SelectNextFreeSlot();
        }
    }

    private void _SelectNextFreeSlot()
    {
        bool foundEmptySlot = false;
        for (int i = selectedItemIndex + 1; i < slots.Length; i++)
        {
            if (!slots[i].IsLocked && !slots[i].HasItem && !reservedIPlaces.Contains(i))
            {
                SelectedItemIndex = i;
                foundEmptySlot = true;
                break;
            }
        }

        if (!foundEmptySlot)
        {
            for (int i = 0; i < selectedItemIndex; i++)
            {
                if (!slots[i].IsLocked && !slots[i].HasItem && !reservedIPlaces.Contains(i))
                {
                    SelectedItemIndex = i;
                    foundEmptySlot = true;
                    break;
                }
            }

            if (!foundEmptySlot)
                SelectedItemIndex = -1;
        }
    }

    void _BuyConfrimed()
    {
        Inventory.Instance.AddItem(buyingType, 1);
        InventoryItem_OnClick(buyingType);
    }

    List<int> reservedIPlaces = new List<int>();

    IEnumerator _SetItemSlotAfter(float wait, int selectedItemIndex, BoosterPackageType type)
    {
        yield return new WaitForSeconds(wait);
        slots[selectedItemIndex].OnDrop(null);

        reservedIPlaces.Remove(selectedItemIndex);
    }

    void Slot_OnClick(int index)
    {
        if (slots[index].IsLocked)
        {
            Debug.Log("slot " + index + " is locked.");
            if (index == 3 || index == 4)
            {
                string param = (index == 3)?"Power or Research":"Power and Research";
                CommonUI.Instance.messageBox.ShowMessage(Messages.YouMustHaveVIPPackageToUseThisSlot, _OpenVIPWindow, true, param);
            }

            return;
        }

		if (!slots [index].HasItem)
			return;

		float priceMultiplyer = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.BoosterPrice);

		Accounting.Instance.currentUser.Bills += (int)(StoreData.Instance.packages [slots [index].cardIndex].price.Bills * priceMultiplyer);
		
        TemporarilyStopTimer();
        
        if (!slots[index].IsLocked)
        {
            SelectedItemIndex = index;
            audioStruct.Play(audioStruct.selectSlot);
			slots[index].Card = null;

            if (slots[index].HasItem)
                RefreshEquippedList();
        }
        else
        {
            switch (slots[index].Condition)
            {
                case BoosterSlotOpenCondition.Level:
                    if (slots[index].IsConditionTrue())
                    {
                        audioStruct.Play(audioStruct.unlockSlot);
                        Information.Instance.SetBoosterAvailablity(index, true);
                        
                        if (SelectedItemIndex == -1)
                            _SelectNextFreeSlot();
                    }
                    else
                        CommonUI.Instance.messageBox.ShowMessage(Messages.YouHavetoBeLevelXToOpenThisSlot, null, true, slots[index].Amount.ToString());
                    break;

                case BoosterSlotOpenCondition.HasVIPPower:
                    buyingSlotIndex = index;
                    _BuyConfirmed();
                    //CommonUI.Instance.messageBox.Ask(Messages.ConfirmBuyItem, _BuyConfirmed, null, false, slots[index].Amount.ToString());
                    break;

                case BoosterSlotOpenCondition.HasVIPResearch:
                    buyingSlotIndex = index;
                    _BuyConfirmed();
                    //CommonUI.Instance.messageBox.Ask(Messages.ConfirmBuyItem, _BuyConfirmed, null, false, slots[index].Amount.ToString());
                    break;
            }
        }
    }

    private void _OpenVIPWindow()
    {
        CommonUI.Instance.headerBar.vipWindow.Activate();
    }

    private void TemporarilyStopTimer()
    {
        isTimerActive = false;
        time = timeForAutoClose;
        timerText.text = time.ToString();

        activeTimerTime = 2;
    }

    int buyingSlotIndex;
    void _BuyConfirmed()
    {
        if (slots[buyingSlotIndex].IsConditionTrue())
        {
            Information.Instance.SetBoosterAvailablity(buyingSlotIndex, true);

            audioStruct.Play(audioStruct.unlockSlot);

            if (SelectedItemIndex == -1)
                SelectedItemIndex = buyingSlotIndex;
        }
        else
            CommonUI.Instance.buyCoinsMenu.Activate();
    }

    public void TutorialButton_Click()
    {
        tutorial.Activate();
    }

    public void GoButton_Click()
    {
        RefreshEquippedList();

        if (OnDone != null)
            OnDone();

        Deactivate();
    }

    public void RefreshEquippedList()
    {
        //equippedPackages = new List<BoosterPackageType>();
        equippedPackages.Clear();

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsLocked && slots[i].HasItem)
            {
                equippedPackages.Add(slots[i].PackageType);
            }
        }

        /*for (int i = 0; i < Inventory.Instance.addingItems.Length; i++)
        {
            if (Inventory.Instance.addingItems[i].IsActive && Inventory.Instance.addingItems[i].CarryingItemCount < 0)
                equippedPackages.Add(Inventory.Instance.addingItems[i].Type);
        }*/
    }

    void Update()
    {
        //if (!CommonUI.Instance.messageBox.IsActive && !CommonUI.Instance.buyCoinsMenu.IsActive && !tutorial.IsActive)
        {
            if (isTimerActive)
            {
                time -= Time.deltaTime;
                timerText.text = ((int)time).ToString();
                if (time <= 0)
                    GoButton_Click();
            }
            else
            {
                activeTimerTime -= Time.deltaTime;
                if (activeTimerTime <= 0)
                    isTimerActive = true;
            }
        }
        /*else
        {
            time = timeForAutoClose;
            timerText.text = ((int)time).ToString();
        }*/
    }

    public void ClearEquippedPackages()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Clicked();

        equippedPackages.Clear();
    }
}