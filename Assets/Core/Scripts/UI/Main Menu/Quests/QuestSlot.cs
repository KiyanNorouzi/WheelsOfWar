using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestSlot : MonoBehaviour 
{
    public delegate void questSlotClick(QuestSlot sender);
    public event questSlotClick OnSkip, OnClaim;



    public Image logoImage;
    public Text descText, rewardText, doneText, totalText;
    public Slider progressSlider;
    public GameObject disableOverlay;
    public GameObject[] stateGameObjects;



    Quest currentQuest;
    public Quest CurrentQuest
    {
        get { return currentQuest; }
    }


	public void SetQuest(Quest q)
    {
        currentQuest = q;

        int typeIndex = (int)q.type;
        logoImage.sprite = QuestSettings.Instance.logos[typeIndex];
        descText.text = QuestSettings.Instance.descText[typeIndex].Replace("*", MathHelper.GetStringWithComma(q.param));

        rewardText.text = q.rewardGold.ToString();
        doneText.text = MathHelper.GetStringWithComma(Mathf.FloorToInt(q.Progress));
        totalText.text = string.Concat("/", MathHelper.GetStringWithComma(q.param));

        progressSlider.value = q.ProgressPercent;

        int stateIndex = (int)q.State;
        for (int i = 0; i < stateGameObjects.Length; i++)
            stateGameObjects[i].SetActive(i == stateIndex);

        disableOverlay.SetActive(q.State == QuestState.Skipped || q.State == QuestState.Claimed);
    }

    public void SkipButton_Click()
    {
        if (OnSkip != null)
            OnSkip(this);
    }

    public void ClaimButton_Click()
    {
        if (OnClaim != null)
            OnClaim(this);
    }

    public void Refresh()
    {
        if (currentQuest != null)
            SetQuest(currentQuest);
        else
            Debug.Log("trying to refresh null quest slot, " + name, this);
    }

    public void Test_DoneQuest_Click()
    {
        if (Debug.isDebugBuild)
        {
            currentQuest.ProgressHappened(currentQuest.param);
            QuestManager.Instance.Save();

            Refresh();
        }
    }
}
