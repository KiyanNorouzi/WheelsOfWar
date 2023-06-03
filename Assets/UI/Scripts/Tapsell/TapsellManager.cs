using UnityEngine;
using System.Collections;

public class TapsellManager : MonoBehaviour 
{
    #region Singleton

    static TapsellManager _instance;

    public static TapsellManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion


    public UnityEngine.UI.Text logText;

    bool isAvailable;
    public bool IsAdAvailable
    {
        get 
        {
            _CheckAvailablity();
            return isAvailable; 
        }
    }

    IEnumerator Start()
    {
        //yield return new WaitForSeconds(6);

#if UNITY_ANDROID

        if (logText !=null)
            logText.text += "--- set key";

        if (Application.platform == RuntimePlatform.Android)
            TapsellDeveloper.getInstance().setKey("gtliplfrsntakqoqesopljigdtenblgidibjnhpdpanartocohcqjqkdehkjcomlltqoir");

        if (logText != null)
            logText.text += "--- after set key";
#endif

        yield return new WaitForSeconds(1);
        _CheckAvailablity();
    }

    public delegate void onAdDone(AdShowResult result);
    onAdDone doneMethod;

    int n;
    void _CheckAvailablity()
    {
        if (n == 0)
            n++;

#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            DeveloperCtaInterface.getInstance().checkCtaAvailability(3, 10, true, (bool connected, bool isAvailable) =>
            {
                this.isAvailable = connected && isAvailable;

                if (logText != null)
                    logText.text += "--- check available, connected=" + connected.ToString()+ ", avail=" + isAvailable.ToString();
            });
        }
#endif
    }

    void _ShowAd()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            DeveloperCtaInterface.getInstance().showNewCta(3, 1,
                (bool connected, bool isAvailable, int award) =>
                {
                    if (logText != null)
                        logText.text += "--- video done, connected=" + connected.ToString() + ", avail=" + isAvailable.ToString() + ", award=" + award.ToString();
                    _VideoCompleted(award);
                });


            _CheckAvailablity();
        }
#endif
    }


    public void TestShowAd()
    {
        ShowAd(null, false);
    }

    public void ShowAd(onAdDone doneMethod, bool forced = false)
    {
        if (IsAdAvailable)
        {
            this.doneMethod = doneMethod;
            /*if (forced)
                UI.Instance.LockEverything(AdPlayManager.Instance.showForcedAdString.Replace("*", Information.Instance.BroSisText));
            else
                UI.Instance.LockEverything(AdPlayManager.Instance.showAdString);*/



            _ShowAd();
        }
        else
        {
            if (logText != null)
                logText.text += "--- ad not available to show";
            Debug.Log("ad not available");
        }
    }

    void _VideoCompleted(int award)
    {
        //UI.Instance.UnlockEverything();

        if (doneMethod != null)
        {
            if (award > 0)
                doneMethod(AdShowResult.Showed);
            else
                doneMethod(AdShowResult.Failed);
        }
    }
}