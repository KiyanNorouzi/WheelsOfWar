using UnityEngine;
using System.Collections;

public class QuestsWindow : Window
{
    public QuestSlot[] slots;
    public PriceStructure skipPrice;
    public UnityEngine.UI.Text remainingText;



    void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].OnSkip += QuestsWindow_OnSkip;
            slots[i].OnClaim += QuestsWindow_OnClaim;
        }
    }

    void _Refresh()
    {
        RefreshTimeText();

        if (IsActive)
            Invoke("_Refresh", 1);
    }

    void RefreshTimeText()
    {
        ServerTime.Instance.RefreshTimeLocally();

        int hoursRemaining = 23 - ServerTime.Instance.Hour;
        int minutesRemaining = 59 - ServerTime.Instance.Minute;
        int secondsRemaining = 60 - ServerTime.Instance.Seconds;

        remainingText.text = string.Format("{0:00}:{1:00}':{2:00}\"", hoursRemaining, minutesRemaining, secondsRemaining);
    }

    void QuestsWindow_OnClaim(QuestSlot sender)
    {
        MainMenuController.Instance.rewardWindow.ActivateReward(DailyGiftRewardType.Golds, sender.CurrentQuest.rewardGold, "");
        Accounting.Instance.currentUser.Golds += sender.CurrentQuest.rewardGold;

        sender.CurrentQuest.Claimed();
        sender.Refresh();
        QuestManager.Instance.Save();

        MainMenuController.Instance.RefreshQuestsText();
    }

    QuestSlot skippingSlot;
    void QuestsWindow_OnSkip(QuestSlot sender)
    {
        skippingSlot = sender;

        Accounting.Instance.currentUser.Buy(skipPrice, _Skip);
        MainMenuController.Instance.RefreshQuestsText();
    }

    private void _Skip()
    {
        skippingSlot.CurrentQuest.Skip();
        skippingSlot.Refresh();
    }


    public override void Activate()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log("Quest " + i.ToString() + " state=" + QuestManager.Instance[i].State + ", progress=" + QuestManager.Instance[i].ProgressPercent);
            slots[i].SetQuest(QuestManager.Instance[i]);
        }

        base.Activate();

        
        RefreshTimeText();
        Invoke("_Refresh", 1);
    }
}
