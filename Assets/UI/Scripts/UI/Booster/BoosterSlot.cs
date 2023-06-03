using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class BoosterSlot : MonoBehaviour , IDropHandler
{
    public delegate void boosterSlotClicked(int index);
    public event boosterSlotClicked OnClick, OnCardDrop;



    public GameObject itemParentGameObject, lockGameObject, selectLayoutGameObject;
    public GameObject readyToDropOverlayGameObject;
    public Image vipSlotImage;
    public Text conditionText;
	public int cardIndex;



    BoosterSlotOpenCondition condition;
    public BoosterSlotOpenCondition Condition
    {
        get { return condition; }
    }


    int amount;
    public int Amount
    {
        get { return amount; }
    }


    bool isLocked;
    public bool IsLocked
    {
        get { return isLocked; }
        set 
        {
            Debug.Log(name + " is locked=" + value);
            isLocked = value;
            if (isLocked)
            {
                itemParentGameObject.SetActive(false);
                lockGameObject.SetActive(true);

                /*if (IsConditionTrue())
                    readyToUnlockGameObject.SetActive(true);*/

                //backgroundImage.sprite = GameplayUI.Instance.boosterPanel.bgSprites[1];
            }
            else
            {

                itemParentGameObject.SetActive(true);
                lockGameObject.SetActive(false);

                //readyToUnlockGameObject.SetActive(false);
                //backgroundImage.sprite = GameplayUI.Instance.boosterPanel.bgSprites[0];
            }

            //selectLayoutGameObject.SetActive(false);
        }
    }


    BoosterCard card;
    public BoosterCard Card
    {
        get { return card; }
		set{card = value;}
    }



    public void OnEnter()
    {
        if (!isLocked && BoosterCard.draggingCard != null && !HasItem)
            readyToDropOverlayGameObject.SetActive(true);
    }

    public void OnExit()
    {
        if (!isLocked && BoosterCard.draggingCard != null && !HasItem)
            readyToDropOverlayGameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
		if( card != null ){
			return;
		}


        if (OnCardDrop != null)
			OnCardDrop(this.index);
	}


    public void PickUp()
    {
        readyToDropOverlayGameObject.SetActive(false);
        if (!isLocked && BoosterCard.draggingCard != null && !HasItem)
        {
            card = BoosterCard.draggingCard;
			cardIndex = Card.index;
            card.PickedUp(this);
            card.myTransform.SetParent(itemParentGameObject.transform);
            card.myTransform.anchoredPosition = new Vector2(0, 0);
			Debug.Log(card);
        }
    }

    public bool HasItem
    {
        get
        {
            return card != null;
        }
    }

    public BoosterPackageType PackageType
    {
        get { return card.type; }
    }

    public bool IsConditionTrue()
    {
        switch (condition)
        {
            case BoosterSlotOpenCondition.Level: return (Accounting.Instance.currentUser.Level >= amount);
            case BoosterSlotOpenCondition.HasVIPPower: return Accounting.Instance.currentUser.HasVIPPackage(0);
            case BoosterSlotOpenCondition.HasVIPResearch: return Accounting.Instance.currentUser.HasVIPPackage(1);
        }

        return false;
    }



    private int index;
    public void SetIndex(int index, BoosterSlotOpenCondition condition, int amount = 0)
    {
        this.index = index;
        this.condition = condition;
        this.amount = amount;


        itemParentGameObject.SetActive(true);

        //backgroundImage.sprite = GameplayUI.Instance.boosterPanel.bgSprites[index % GameplayUI.Instance.boosterPanel.bgSprites.Length];
        //numberImage.sprite = GameplayUI.Instance.boosterPanel.numberSprites[index % GameplayUI.Instance.boosterPanel.numberSprites.Length];

        switch (condition)
        {
            case BoosterSlotOpenCondition.Level:
                //vipSlotImage.enabled = false;
                conditionText.text = string.Concat("LEVEL ", amount);
                //itemText.text = string.Concat("LEVEL ", amount);
                break;
            case BoosterSlotOpenCondition.HasVIPPower:
                //vipSlotImage.enabled = true;
                conditionText.text = "VIP POWER OR RESEARCH";
                //itemText.text = string.Concat("", amount);
                break;
            case BoosterSlotOpenCondition.HasVIPResearch:
                //vipSlotImage.enabled = true;
                conditionText.text = "VIP PR";
                //itemText.text = string.Concat("", amount);
                break;
        }

        Refresh();
    }

    public void Select()
    {
        selectLayoutGameObject.SetActive(true);
    }

    public void Deselect()
    {
        selectLayoutGameObject.SetActive(false);
    }

    public void Clicked()
    {
        if (!isLocked && card != null)
        {
            card.BackInDeck();
        }

        if (OnClick != null)
            OnClick(index);
    }

    void _ItemPlacedAnimDone()
    {

    }

    public void Refresh()
    {
        IsLocked = !IsConditionTrue();
    }
}

public enum BoosterSlotState
{
    Locked,
    Open,
    Full
}

public enum BoosterSlotOpenCondition
{
    Level,
    HasVIPPower,
    HasVIPResearch
}