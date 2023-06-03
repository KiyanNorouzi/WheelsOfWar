using UnityEngine;
using System.Collections;

public class InAppPurchases : MonoBehaviour
{
    #region Singleton

    static InAppPurchases _instance;

    public static InAppPurchases Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    public delegate void logged(string log);
    public event logged OnLog;


    public string[] IAP_Ids;

    public delegate void inappDone(int index, bool successful);

    string correctWaitText;
    inappDone doneMethod;
    int index;
    float time;

    public GameObject[] cafeGO, iranAppsGO, myketGO, googlePlayGO;

    
    IEnumerator Start()
    {
        switch(CommonUI.Instance.store)
        {
            case Stores.CafeBazaar:
                for (int i = 0; i < cafeGO.Length; i++)
                    cafeGO[i].SetActive(true);
                break;

            case Stores.IranApps:
                for (int i = 0; i < iranAppsGO.Length; i++)
                    iranAppsGO[i].SetActive(true);
                break;

            case Stores.Myket:
                for (int i = 0; i < myketGO.Length; i++)
                    myketGO[i].SetActive(true);
                break;

            case Stores.GooglePlay:
                for (int i = 0; i < googlePlayGO.Length; i++)
                    googlePlayGO[i].SetActive(true);
                break;
        }


        yield return new WaitForSeconds(5);

        
#if UNITY_ANDROID
        
        if (Application.platform == RuntimePlatform.Android)
        {
            switch (CommonUI.Instance.store)
            {
                case Stores.CafeBazaar:
                    CafeBazaarInAppHelper.Instance.Init();
                    break;
                case Stores.IranApps:

                    SampleUI.PrintText("iranapps setup");
                    IranAppsInAppBillingHelper.Instance.SetupInAppBilling();
                    break;
            }
        }
#endif
    }


    bool purchase;
    public void Request(int index, inappDone doneMethod)
    {
        Debug.Log("in-app item #" + index + " requested");

        this.index = index;
        //CommonUI.Instance.lockOverlay.Activate(LockOverlayMessages.ConnectingToStore);

        this.doneMethod = doneMethod;


        CommonUI.Instance.lockOverlay.Activate(LockOverlayMessages.ConnectingToStore);

        if (Debug.isDebugBuild)
        {
            StartCoroutine(SimulateSuccessfullShopping(1, index));
        }
        else
        {
            switch (CommonUI.Instance.store)
            {
                case Stores.CafeBazaar:
                    CafeBazaarInAppHelper.Instance.BuyProduct(index);
                    break;
                case Stores.IranApps:
                    purchase = true;
                    IranAppsInAppBillingHelper.Instance.BuyProduct(IAP_Ids[index], "ok_" + IAP_Ids[index] + "sfh", false);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator SimulateSuccessfullShopping(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        InAppPurchaseDone(index);
    }

    void OnApplicationPause(bool pause)
    {
        if (purchase && !pause && CommonUI.Instance.store == Stores.IranApps)
        {
            
            IranAppsInAppBillingHelper.Instance.GetPurchases();
            purchase = false;
        }
    }

    public void InAppPurchaseDone(string id)
    {
        for (int i = 0; i < IAP_Ids.Length; i++)
        {
            if (IAP_Ids[i].ToLower() == id.ToLower())
            {
                InAppPurchaseDone(i);
                return;
            }
        }
    }

    public void InAppPurchaseDone(int index)
    {
        //MoneyTablet.Instance.Add((int)(Prices.Instance.IAPCoins[index] * Prices.Instance.BillsMultiplyer));
        Accounting.Instance.currentUser.Bills += (int)(Prices.Instance.IAPItems[index].Bills * Prices.Instance.BillsMultiplyer);
        Accounting.Instance.currentUser.Golds += (int)(Prices.Instance.IAPItems[index].Golds * Prices.Instance.BillsMultiplyer);

        CommonUI.Instance.audioPlayer.Play(CommonUI.Instance.audioPlayer.addingMoney);
        CommonUI.Instance.lockOverlay.Deactivate();

        if (doneMethod != null)
            doneMethod(index, true);
    }

    public void InAppPurchaseFailed(string message)
    {
        CommonUI.Instance.lockOverlay.Deactivate();

        if (doneMethod != null)
            doneMethod(index, false);
    }

    public void Log(string s)
    {
        if (OnLog != null)
            OnLog(s);
    }
}