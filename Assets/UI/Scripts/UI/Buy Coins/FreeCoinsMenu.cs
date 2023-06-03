using UnityEngine;
using System.Collections;

public class FreeCoinsMenu : MonoBehaviour
{
    #region Singleton

    static FreeCoinsMenu _instance;
    public static FreeCoinsMenu Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion

    public Transform myTransform;
    public FreeCoinStructure[] freeCoins;
    public GameObject freeCoinSlotPrefab;
    public FreeCoinSlot sampleSlot;
    public RectTransform contentTransform;
    public float distance;


    FreeCoinSlot[] slots;
    float defaultContentSize;

    public void  Activate()
    {
        if (slots == null)
        {
            slots = new FreeCoinSlot[freeCoins.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                if (i == 0)
                    slots[i] = sampleSlot;
                else
                {
                    slots[i] = ((GameObject)Instantiate(freeCoinSlotPrefab)).GetComponent<FreeCoinSlot>();
                    slots[i].myTransform.SetParent(sampleSlot.myTransform.parent);
                    slots[i].myTransform.localScale = Vector3.one;
                    slots[i].name = string.Concat(sampleSlot.name, i);
                    slots[i].myTransform.anchoredPosition = new Vector2(sampleSlot.myTransform.anchoredPosition.x + i * distance, sampleSlot.myTransform.anchoredPosition.y);
                }

                slots[i].OnClick += FreeCoinsMenu_OnClick;
            }

            if (defaultContentSize == 0)
                defaultContentSize = contentTransform.rect.width;

            //float targetSize = freeCoins.Length * 160 + (sampleSlot.myTransform.anchoredPosition.x - (sampleSlot.myTransform.rect.width / 2f)) * 2;

            float targetWidth = slots[slots.Length - 1].myTransform.anchoredPosition.x + slots[0].myTransform.anchoredPosition.x;

            //Debug.Log("count=" + freeCoins.Length + ", margin=" + ((sampleSlot.myTransform.anchoredPosition.x - (sampleSlot.myTransform.rect.width / 2f)) * 2) + ", total=" + targetSize + ", default size=" + defaultContentSize);
            float w = Mathf.Max(targetWidth, defaultContentSize);
            contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        }

        contentTransform.anchoredPosition = new Vector2(contentTransform.rect.width / 2f, 0);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Activate(freeCoins[i].type, freeCoins[i].itemImage, freeCoins[i].desc, freeCoins[i].rewardMoney);
        }
    }

    FreeCoinSlot clickedSlot;
    void FreeCoinsMenu_OnClick(FreeCoinSlot slot)
    {
        this.clickedSlot = slot;
        switch (clickedSlot.Type)
        {
            case FreeCoinType.Facebook:
                Application.OpenURL(GeneralSettings.Instance.facebookUrl);
                break;
            case FreeCoinType.Instagram:
                Application.OpenURL(GeneralSettings.Instance.instagramUrl);
                break;
            case FreeCoinType.VideoAd:
                /*if (AdPlayManager.Instance.IsAdAvailable)
                {
                    CommonUI.Instance.lockOverlay.Activate(LockOverlayMessages.WaitingForVideoAd);
                    AdPlayManager.Instance.ShowAd(AdPlayDone);
                }
                else
                    CommonUI.Instance.question.ShowMessage(Messages.AdNotAvailable, null, false);*/

                Scenes comebackScene = Scenes.MainMenu;
                if (SceneManager.CurrentSceneType == Scenes.Garage || SceneManager.CurrentSceneType == Scenes.MapSelect || SceneManager.CurrentSceneType == Scenes.Store)
                    comebackScene = SceneManager.CurrentSceneType;

                SceneManager.LoadAdScene(comebackScene, false);
                CommonUI.Instance.buyCoinsMenu.Deactivate();
                break;
        }
    }

    void AdPlayDone(AdShowResult result)
    {
        CommonUI.Instance.lockOverlay.Deactivate();

        switch (result)
        {
            case AdShowResult.Showed:
            case AdShowResult.Installed:
                addingCoins = clickedSlot.RewardCoins;
                CommonUI.Instance.messageBox.ShowMessage(Messages.ThanksForVideoAd, _GiveCoins, false);
                break;

            case AdShowResult.Failed:
                CommonUI.Instance.messageBox.ShowMessage(Messages.ErrorInVideoAd, null, false);
                break;
            case AdShowResult.NotAvailable:
                CommonUI.Instance.messageBox.ShowMessage(Messages.AdNotAvailable, null, false);
                break;
        }

        clickedSlot.Refresh();
        clickedSlot = null;
    }

    int addingCoins;
    void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            if (clickedSlot != null && clickedSlot.Type != FreeCoinType.VideoAd && Information.Instance.IsFreeCoinAvailable(clickedSlot.Type))
            {
                Information.Instance.FreeCoinCollected(clickedSlot.Type);
                addingCoins = clickedSlot.RewardCoins;

                if (clickedSlot.Type == FreeCoinType.Facebook)
                    CommonUI.Instance.messageBox.ShowMessage(Messages.ThanksForFacebook, _GiveCoins, false);
                else if (clickedSlot.Type == FreeCoinType.Instagram)
                    CommonUI.Instance.messageBox.ShowMessage(Messages.ThanksForInstagram, _GiveCoins, false);
                else
                    Debug.LogError("New Type of free coins without appropirate message");

                clickedSlot.Refresh();
                clickedSlot = null;
            }
        }
    }

    void _GiveCoins()
    {
        //MoneyTablet.Instance.Add(addingCoins);
        //Accounting.Instance.SyncScoreTillSuccess(Data.UserName, Data.TotalScore, Data.Money, "");
    }
}

[System.Serializable]
public class FreeCoinStructure
{
    public FreeCoinType type;
    public Sprite itemImage;
    public string desc;
    public int rewardMoney;
}