using UnityEngine;
using System.Collections;

public class AdPlaySceneManager : MonoBehaviour 
{
    public static bool forcedAd;
    public static Scenes comebackScene;
    public static float lastAdShowTime;


    public GameObject notAd, waitForAd;
    public UnityEngine.UI.Text debugText;

    bool isAdPlayPlaying, isRequestLegimate;


	void Start()
    {
        debugText.text = "timescale=" + Time.timeScale;

        AdPlayManager.Instance.Init();
        _CheckForAd();
	}

    private void _CheckForAd()
    {
        StartCoroutine(_check());
    }

    IEnumerator _check()
    {
        notAd.SetActive(false);
        waitForAd.SetActive(false);

        //debugText.text += "is avaliable=" + AdPlayManager.Instance.IsAdAvailable.ToString() + ", timescale=" + Time.timeScale;

        yield return new WaitForSeconds(0.2f);


        //debugText.text += "is avaliable=" + AdPlayManager.Instance.IsAdAvailable.ToString();

        if (TapsellManager.Instance.IsAdAvailable)
        {
            notAd.SetActive(false);
            waitForAd.SetActive(true);

            lastAdShowTime = Time.time;

            TapsellManager.Instance.ShowAd(AdPlayDone);
            isRequestLegimate = true;
            isAdPlayPlaying = false;
        }
        else if (AdPlayManager.Instance.IsAdAvailable)
        {
            notAd.SetActive(false);
            waitForAd.SetActive(true);

            lastAdShowTime = Time.time;

            AdPlayManager.Instance.ShowAd(AdPlayDone);
            isRequestLegimate = true;
            isAdPlayPlaying = true;
        }
        else
        {
            if (forcedAd)
                SceneManager.LoadScene(Scenes.MainMenu);
            else
            {
                notAd.SetActive(true);
                waitForAd.SetActive(false);
            }
        }
    }

    int addingCoins;
    void AdPlayDone(AdShowResult result)
    {
        CommonUI.Instance.lockOverlay.Deactivate();
        isRequestLegimate = false;

        if (forcedAd)
        {
            SceneManager.LoadScene(comebackScene);
        }
        else
        {
            switch (result)
            {
                case AdShowResult.Showed:
                case AdShowResult.Installed:
                    addingCoins = CommonUI.Instance.buyCoinsMenu.freeCoinsMenu.freeCoins[CommonUI.Instance.buyCoinsMenu.freeCoinsMenu.freeCoins.Length - 1].rewardMoney;
                    CommonUI.Instance.messageBox.ShowMessage(Messages.ThanksForVideoAd, _GiveCoins, false);
                    break;

                case AdShowResult.Failed:
                    CommonUI.Instance.messageBox.ShowMessage(Messages.ErrorInVideoAd, ComeBack_Click, false);
                    break;
                case AdShowResult.NotAvailable:
                    CommonUI.Instance.messageBox.ShowMessage(Messages.AdNotAvailable, ComeBack_Click, false);
                    break;
            }
        }
    }

    void _GiveCoins()
    {
        //MoneyTablet.Instance.Add(addingCoins);
        ComeBack_Click();
    }

    public void RefreshButton_Click()
    {
        _CheckForAd();
    }

    public void ComeBack_Click()
    {
        SceneManager.LoadScene(comebackScene);
    }

    void OnApplicationPause(bool paused)
    {
        if (isAdPlayPlaying && !paused && isRequestLegimate)
        {
            AdPlayDone(AdShowResult.Showed);
            isRequestLegimate = false;
        }
    }
}