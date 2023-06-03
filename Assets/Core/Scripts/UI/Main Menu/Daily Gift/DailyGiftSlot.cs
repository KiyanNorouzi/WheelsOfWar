using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DailyGiftSlot : MonoBehaviour 
{
    public delegate void dailyGiftSlotClick(int dayIndex, DailyGiftSlotState slotType, DailyGiftRewardType rewardType, int amount);
    public event dailyGiftSlotClick OnClick;


    public GameObject myGameObject;
    public RectTransform myTransform; 
    public Image backImage;
    public GameObject[] signsGameObjects;
    public GameObject claimGameObject;
    public Text dayIndexText, rewardAmountText;


    int dayIndex, amount;
    DailyGiftSlotState slotType;
    DailyGiftRewardType rewardType;



    public int DayIndex
    {
        get { return dayIndex; }
    }

    public DailyGiftSlotState SlotType
    {
        get { return slotType; }
    }
    
    public DailyGiftRewardType RewardType
    {
        get { return rewardType; }
    }

    public int Amount
    {
        get { return amount; }
    }





    public void Activate(int dayIndex, DailyGiftSlotState slotType, DailyGiftRewardType rewardType, int amount, Color backColor)
    {
        this.dayIndex = dayIndex;
        this.amount = amount;
        this.slotType = slotType;
        this.rewardType = rewardType;


        dayIndexText.text = dayIndex.ToString();

        int rewardTypeIndex = (int)rewardType;
        for (int i = 0; i < signsGameObjects.Length; i++)
            signsGameObjects[i].SetActive(i == rewardTypeIndex);

        if (rewardType != DailyGiftRewardType.VIP)
        {
            rewardAmountText.text = amount.ToString();
        }
        else
        {
            rewardAmountText.text = VIPPackagesSettings.Instance.packages[amount].name;
        }


        backImage.color = backColor;
        claimGameObject.SetActive(slotType == DailyGiftSlotState.Passed);

        myGameObject.SetActive(true);
    }

    public void Button_Click()
    {
        if (OnClick != null)
            OnClick(dayIndex, slotType, rewardType, amount);
    }
}

public enum DailyGiftSlotState
{
    Passed,
    Active,
    Future
}

public enum DailyGiftRewardType
{
    Bills,
    Golds,
    VIP,
    Score
}