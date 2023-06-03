using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour 
{
    #region Singleton

    static Inventory _instance;
    public static Inventory Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion



    public delegate void inventoryItemClick(BoosterPackageType type);
    public event inventoryItemClick OnClick;


    public GameObject myGameObject;
    public Animator inventoryAnimator;
    public GameObject skipButtonGameObject, openButtonGameObject;
    public InventorySlot[] slots;
    public InventoryAddingItem[] addingItems;
    
    



    InventoryAddingItem FirstAvailableAddingItem
    {
        get
        {
            for (int i = 0; i < addingItems.Length; i++)
            {
                if (!addingItems[i].IsActive)
                    return addingItems[i];
            }

            return null;
        }
    }

    bool isIn;


    void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Count = 0;
            slots[i].OnClick += Inventory_OnClick;
        }

        for (int i = 0; i < addingItems.Length; i++)
        {
            addingItems[i].OnReached += addingItem_OnReached;
            addingItems[i].Deactivate();
        }


        skipButtonGameObject.SetActive(false);
        //FirstAvailableAddingItem.OnReached += addingItem_OnReached;
        //FirstAvailableAddingItem.Deactivate();
    }

    void Inventory_OnClick(BoosterPackageType type)
    {
        if (OnClick != null)
            OnClick(type);
    }

    void addingItem_OnReached(int addingSlotIndex, int addingCount, bool justForShow)
    {
        if (!justForShow)
            slots[addingSlotIndex].Add(addingCount);
        else
        {
            slots[addingSlotIndex].Refresh();
        }
    }

    public void SkipButton_Click()
    {
        //OpenButton_Click();
        Close();
    }

    public void OpenButton_Click()
    {
        isIn = !isIn;
        skipButtonGameObject.SetActive(isIn);
        inventoryAnimator.SetBool("in", isIn);

        _RefreshAllSlots();
    }

    public void Open()
    {
        isIn = true;
        skipButtonGameObject.SetActive(true);
        inventoryAnimator.SetBool("in", isIn);

        _RefreshAllSlots();
    }

    public void Close()
    {
        isIn = false;
        skipButtonGameObject.SetActive(false);
        inventoryAnimator.SetBool("in", isIn);
    }



    private void _RefreshAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Refresh();
            slots[i].myButton.interactable = true;
        }
    }

    public void AddItem(BoosterPackageType package, int count)
    {
        slots[(int)package].Add(count);
    }

    public void AddItemWithAnimation(BoosterPackageType package, int count, Vector3 sourcePosition, bool justForShow)
    {
        int slotIndex = (int)package;

        if (FirstAvailableAddingItem == null)
        {
            addingItem_OnReached(slotIndex, count, justForShow);
        }
        else
        {
            if (count > 0)
                FirstAvailableAddingItem.Activate(sourcePosition, slots[slotIndex].myTransform.position, slots[slotIndex].myImage.sprite, slotIndex, count, justForShow);
            else
                FirstAvailableAddingItem.Activate(slots[slotIndex].myTransform.position, sourcePosition, slots[slotIndex].myImage.sprite, slotIndex, count, justForShow);
        }
    }

    public int GetItem(BoosterPackageType package)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].type == package)
                return slots[i].Count;
        }

        return 0;
    }

    public void UseInUseItems()
    {
        if (GameplayUI.Instance != null)
        {
            for (int i = 0; i < GameplayUI.Instance.boosterPanel.EquippedPackages.Count; i++)
                AddItem(GameplayUI.Instance.boosterPanel.EquippedPackages[i], -1);

            GameplayUI.Instance.boosterPanel.ClearEquippedPackages();
        }
        else
            Debug.LogError("Why gameplayUI is null?");
    }

    public void SetOpenButtonEnable(bool enabled)
    {
        openButtonGameObject.SetActive(enabled);
    }

    public void Disable()
    {
        myGameObject.SetActive(false);
    }

    public void Enable()
    {
        myGameObject.SetActive(true);
    }

    public void EnableAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].myButton.interactable = true;
    }

    public void EnableSlot(BoosterPackageType type)
    {
        EnableSlot((int)type);
    }

    public void EnableSlot(int index)
    {
        slots[index].myButton.interactable = true;
    }


    public void DisableSlot(BoosterPackageType type)
    {
        DisableSlot((int)type);
    }

    public void DisableSlot(int index)
    {
        slots[index].myButton.interactable = false;
    }

    public void AddItemWithTag(string boosterTag, int count)
    {
        
    }

    public void SetItemCount(string boosterTag, int count)
    {
        for (int i = 0; i < StoreData.Instance.packages.Length; i++)
        {
            if (StoreData.Instance.packages[i].tag == boosterTag)
                slots[(int)StoreData.Instance.packages[i].type].SetNumber(count);
        }
    }
}