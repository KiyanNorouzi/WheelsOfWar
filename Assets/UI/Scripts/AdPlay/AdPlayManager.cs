using UnityEngine;
using System.Collections;
using ir.adPlay.plugin;

public class AdPlayManager : MonoBehaviour 
{
    #region Singleton

    static AdPlayManager _instance;

    public static AdPlayManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion


    public string appID, devID;
    public bool isTestMode;

    public bool IsAdAvailable
    {
        get { return adPlay.checkAdAvailibility(); }
    }

    bool installed;
    public bool Installed
    {
        get
        {
            return installed;
        }
    }



	void Start()
    {
	}

    public delegate void onAdDone(AdShowResult result);
    onAdDone doneMethod;

    static bool initialized;

    public void Init()
    {
        if (!initialized)
        {
            adPlay.init(appID, devID, OnVideoComplete, OnInstallComplete, OnAddAvailable, OnAddFailed);
            adPlay.setTestMode(isTestMode);
            adPlay.setAutoDialogueDisplay(false);
            //adPlay.setAutoDownlaodContents(false);

            initialized = true;
        }
    }

    public void ShowAd(onAdDone doneMethod)
    {
        if (IsAdAvailable)
        {
            CommonUI.Instance.DebugText("ad available, show");

            this.doneMethod = doneMethod;
            adPlay.showAdIfAvailable(false);
        }
        else
        {
            CommonUI.Instance.DebugText("ad not available, show");

            if (doneMethod != null)
                doneMethod(AdShowResult.NotAvailable);
        }
    }


    void OnVideoComplete()
    {
        CommonUI.Instance.DebugText("on video complete");

        installed = false;
        if (doneMethod != null)
            doneMethod(AdShowResult.Showed);
    }

    void OnInstallComplete()
    {
        CommonUI.Instance.DebugText("on video installed");
        installed = true;
        if (doneMethod != null)
            doneMethod(AdShowResult.Installed);
    }

    void OnAddAvailable()
    {
        CommonUI.Instance.DebugText("on ad available");
    }

    void OnAddFailed()
    {
        CommonUI.Instance.DebugText("on video failed");
        if (doneMethod != null)
            doneMethod(AdShowResult.Failed);
    }


}

public enum AdShowResult
{
    Failed,
    Showed,
    Installed,
    NotAvailable
}