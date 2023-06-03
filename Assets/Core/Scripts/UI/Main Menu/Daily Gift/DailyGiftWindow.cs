using UnityEngine;
using System.Collections;

public class DailyGiftWindow : Window
{
    public GameObject slotPrefab, claimButtonGameObject, nextGameObject;
    public DailyGiftSlot sample;
    public RectTransform slotsParentTransform;
    public Color[] slotBackColors;
    public UnityEngine.UI.Text userDaysText, totalDaysText, timeRemainingText;


    DailyGiftSlot[] slots;

    void Initialize()
    {
        slots = new DailyGiftSlot[DailyGifts.Instance.items.Length];


        int rowCapacity = (int)(slotsParentTransform.rect.width / sample.myTransform.rect.width);
        int consecutiveRows = 0;
        int lines = 1;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i == 0)
                slots[i] = sample;
            else
            {
                slots[i] = ((GameObject)Instantiate(slotPrefab)).GetComponent<DailyGiftSlot>();
                slots[i].myTransform.SetParent(sample.myTransform.parent);
                slots[i].myTransform.localScale = Vector3.one;

                slots[i].name = string.Concat(sample.name, i);

                float x = slots[i - 1].myTransform.anchoredPosition.x + slots[i-1].myTransform.rect.width;
                float y = slots[i - 1].myTransform.anchoredPosition.y;
                consecutiveRows++;
                if (consecutiveRows >= rowCapacity)
                //if (x +  slots[i].myTransform.rect.width> slotsParentTransform.rect.width)
                {
                    consecutiveRows = 0;
                    lines++;

                    x = sample.myTransform.anchoredPosition.x;
                    y -= sample.myTransform.rect.height;
                }

                slots[i].myTransform.anchoredPosition = new Vector2(x, y);
                slots[i].OnClick += DailyGiftWindow_OnClick;
            }
        }

        slotsParentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lines * sample.myTransform.rect.height + 20);
    }


    void _RefreshTime()
    {
        RefreshTimeText();

        if (IsActive)
            Invoke("_RefreshTime", 1);
    }

    void RefreshTimeText()
    {
        ServerTime.Instance.RefreshTimeLocally();

        int hoursRemaining = 23 - ServerTime.Instance.Hour;
        int minutesRemaining = 59 - ServerTime.Instance.Minute;
        int secondsRemaining = 60 - ServerTime.Instance.Seconds;

        timeRemainingText.text = string.Format("{0:00}:{1:00}':{2:00}\"", hoursRemaining, minutesRemaining, secondsRemaining);
    }

    void DailyGiftWindow_OnClick(int dayIndex, DailyGiftSlotState slotType, DailyGiftRewardType rewardType, int amount)
    {
        if (slotType == DailyGiftSlotState.Active)
            ClaimButton_Click();
    }




    public override void Activate()
    {
        if (slots == null)
            Initialize();

        _Refresh();
        base.Activate();

        _RefreshTime();
    }

    int lastActiveSlotIndex, activeSlotsCount;

    void _Refresh()
    {
        //int consecutiveDays = 3; //Accounting.Instance.currentUser.ConsecutiveDays;
        //int consecutiveAwardsClaimed = 3; //Accounting.Instance.currentUser.ConsecutiveDaysRewardClaimed;

        int consecutiveDays = Accounting.Instance.currentUser.ConsecutiveDays;
        int consecutiveAwardsClaimed = Accounting.Instance.currentUser.ConsecutiveDaysRewardClaimed;

        activeSlotsCount = 0;
        bool hasAReward = false;
        for (int i = 0; i < slots.Length; i++)
        {
            DailyGiftSlotState state;

            if (i + 1 == consecutiveDays && i + 1 == consecutiveAwardsClaimed)
                state = DailyGiftSlotState.Passed;
            else if (i + 1 > consecutiveDays)
                state = DailyGiftSlotState.Future;
            else if (i + 1 < consecutiveAwardsClaimed)
                state = DailyGiftSlotState.Passed;
            else // if (i == consecutiveDays)
            {
                activeSlotsCount++;
                lastActiveSlotIndex = i;
                state = DailyGiftSlotState.Active;
                hasAReward = true;
            }


            if (DailyGifts.Instance.items[i].type == DailyGiftRewardType.VIP)
            {

            }

            
            slots[i].Activate(i + 1, state, DailyGifts.Instance.items[i].type, DailyGifts.Instance.items[i].amount, slotBackColors[(int)state]);
        }

        claimButtonGameObject.SetActive(hasAReward);
        nextGameObject.SetActive(!hasAReward);

        userDaysText.text = consecutiveDays.ToString();
        totalDaysText.text = string.Concat("/", DailyGifts.Instance.items.Length);
    }

    public void ClaimButton_Click()
    {
        StartCoroutine(_CheckForRewards());
    }

    IEnumerator _CheckForRewards()
    {
        float animationSpeed = 1;
        if (activeSlotsCount > 1)
            animationSpeed = 2;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].SlotType == DailyGiftSlotState.Active)
            {
                string desc = "";
                DailyGiftRewardType rewardType = slots[i].RewardType;
                int rewardAmount = slots[i].Amount;

                switch (slots[i].RewardType)
                {
                    case DailyGiftRewardType.Bills:
                        Accounting.Instance.currentUser.Bills += slots[i].Amount;
                        break;
                    case DailyGiftRewardType.Golds:
                        Accounting.Instance.currentUser.Golds += slots[i].Amount;
                        break;
                    case DailyGiftRewardType.VIP:
                        if (Accounting.Instance.currentUser.HasVIPPackage(slots[i].Amount))
                        {
                            string rewardText ="";
                            if (CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Golds != 0)
                            {
                                Accounting.Instance.currentUser.Golds += CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Golds;
                                rewardText = CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Golds + " GOLD BARS";

                                rewardType = DailyGiftRewardType.Golds;
                                rewardAmount = CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Golds;
                            }
                            else
	                        {
                                Accounting.Instance.currentUser.Bills += CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Bills;
                                rewardText = CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Bills + " BILLS";

                                rewardType = DailyGiftRewardType.Bills;
                                rewardAmount = CommonUI.Instance.headerBar.vipWindow.slots[slots[i].Amount].price.Bills;
	                        }
                            
                            desc = string.Concat("YOU ALREADY HAVE VIP PACKAGE ", VIPPackagesSettings.Instance.packages[slots[i].Amount].name, System.Environment.NewLine, 
                                "YOU'LL GET " , rewardText, " INSTEAD");
                        }
                        else
                        {
                            Accounting.Instance.currentUser.BuyVIPPackage(slots[i].Amount);
                        }
                        break;
                    case DailyGiftRewardType.Score:
                        Accounting.Instance.currentUser.TotalScore += slots[i].Amount;
                        break;
                }


                DailyGiftSlotState state = DailyGiftSlotState.Passed;
                slots[i].Activate(i + 1, state, DailyGifts.Instance.items[i].type, DailyGifts.Instance.items[i].amount, slotBackColors[(int)state]);
                MainMenuController.Instance.rewardWindow.ActivateReward(rewardType, rewardAmount, desc, animationSpeed);


                yield return new WaitForSeconds(4f / animationSpeed);
            }
        }

        Accounting.Instance.currentUser.ConsecutiveDaysRewardClaimed = Accounting.Instance.currentUser.ConsecutiveDays;
        

        _Refresh();
        //Deactivate();
    }
}