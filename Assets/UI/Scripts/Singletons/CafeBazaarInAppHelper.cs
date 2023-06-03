using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CafeBazaarInAppHelper : MonoBehaviour
{
    public static bool BuyCoins = false;
    public static string purchasingItemID = "";

#if UNITY_ANDROID
    
    private List<SkuDetails> skuDetailsList;
    private List<PurchasesData> purchasesData;
    List<string> skusList;
    bool successfullPurchase;

#endif


    #region Singleton

    static CafeBazaarInAppHelper _instance;
    public static CafeBazaarInAppHelper Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion




    public void Init()
    {
#if UNITY_ANDROID
        Bazaar_IABAndroid.Init();
#endif
    }

    /*void Update()
    {
#if UNITY_ANDROID
        if (BuyCoins == true)
        {
            BuyCoins = false;
            BuyCoinClass();
        }

        if (IsComeCoinPage)
            RunGetDetails();
#endif

    }*/

    public void BuyProduct(int index)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            purchasingItemID = InAppPurchases.Instance.IAP_Ids[index];
            Bazaar_IABAndroid.PurchaseSku(purchasingItemID);
        }
        else
            InAppPurchases.Instance.InAppPurchaseDone(index);
#else
        InAppPurchases.Instance.InAppPurchaseDone(index);
#endif
    }


#if UNITY_ANDROID


    public void RunGetDetails()
    {
        skusList = new List<string>();
        for (int i = 0; i < InAppPurchases.Instance.IAP_Ids.Length; i++)
        {
            skusList.Add(InAppPurchases.Instance.IAP_Ids[i]);
        }
        
        Bazaar_IABAndroid.RunGetSkuDetails(skusList);
    }

    private void OnEnable()
    {
        Bazaar_IABAndroidManager.GetSkuDetailsFinishedEvent += GetSkuDetailsFinishedEvent;
        Bazaar_IABAndroidManager.GetPurchasesFinishedEvent += GetPurchasesFinishedEvent;
        Bazaar_IABAndroidManager.PurchaseSucceededEvent += PurchaseSucceededEvent;
        Bazaar_IABAndroidManager.PurchaseCancelledEvent += PurchaseCancelledEvent;
        Bazaar_IABAndroidManager.PurchaseFailedEvent += PurchaseFailedEvent;
        Bazaar_IABAndroidManager.ConsumeSkuFinishedEvent += ConsumeSkuFinishedEvent;
    }

    private void OnDisable()
    {
        Bazaar_IABAndroidManager.GetSkuDetailsFinishedEvent -= GetSkuDetailsFinishedEvent;
        Bazaar_IABAndroidManager.GetPurchasesFinishedEvent -= GetPurchasesFinishedEvent;
        Bazaar_IABAndroidManager.PurchaseSucceededEvent -= PurchaseSucceededEvent;
        Bazaar_IABAndroidManager.PurchaseCancelledEvent -= PurchaseCancelledEvent;
        Bazaar_IABAndroidManager.PurchaseFailedEvent -= PurchaseFailedEvent;
        Bazaar_IABAndroidManager.ConsumeSkuFinishedEvent -= ConsumeSkuFinishedEvent;
    }


    

    private void GetSkuDetailsFinishedEvent(string flag)
    {
        if (flag == "true")
        {
            skuDetailsList = Bazaar_IABAndroid.GetSkuDetails();


            string s = "get sku details finished => ";
            for (int i = 0; i < skuDetailsList.Count; i++)
			{
                s += "productid=" + skuDetailsList[i].ProductId + ", Title=" + skuDetailsList[i].Title + "---";

                InAppPurchases.Instance.InAppPurchaseDone(skuDetailsList[i].ProductId);
                Bazaar_IABAndroid.ConsumeSku(skuDetailsList[i].ProductId);
			}

            InAppPurchases.Instance.Log(s);
        }
        else
            InAppPurchases.Instance.Log("get sku details finished no true => " + flag);
    }

    private void GetPurchasesFinishedEvent(string count)
    {
        //if (count != "0")
            //purchasesData = Bazaar_IABAndroid.GetPurchasesData();


        //string s = "get purchases finished => " + count;
        purchasesData = Bazaar_IABAndroid.GetPurchasesData();


        bool done = false;
        for (int i = 0; i < purchasesData.Count; i++)
        {
            //s += "prodct id=" + purchasesData[i].ProductId + ", token=" + purchasesData[i].PurchaseToken + ", payload=" + purchasesData[i].DeveloperPayload + "---";
            Bazaar_IABAndroid.ConsumeSku(purchasesData[i].ProductId);
            InAppPurchases.Instance.InAppPurchaseDone(purchasesData[i].ProductId);
            done = true;
        }

        //InAppPurchases.Instance.Log(s);

        if (!done)
        {
            InAppPurchases.Instance.InAppPurchaseFailed("");

            if (successfullPurchase)
            {
                Debug.Log("hack detected");
                CommonUI.Instance.messageBox.ShowMessage(Messages.HackDetected, null, true);
                //Accounting.Instance.SuspeciousActivityDetected(Data.UserName, null);
            }
                
        }
    }

    private void PurchaseSucceededEvent(string purchasedSkuToken)
    {
        //Bazaar_IABAndroid.ConsumeSku(purchasingItemID);
        //InAppPurchases.Instance.InAppPurchaseDone(purchasingItemID);

        //InAppPurchases.Instance.Log("purchase succeed, token=" + purchasedSkuToken);
        successfullPurchase = true;
        Bazaar_IABAndroid.RunGetPurchases();
        
    }

    private void PurchaseCancelledEvent(string productId)
    {
        //InAppPurchases.Instance.Log("purchase cancelled product id=" + productId);
        //InAppPurchases.Instance.InAppPurchaseFailed("Cancelled");

        successfullPurchase = false;
        Bazaar_IABAndroid.RunGetPurchases();
    }

    private void PurchaseFailedEvent(string productId)
    {
        //InAppPurchases.Instance.Log("purchase failed product id=" + productId);
        //InAppPurchases.Instance.InAppPurchaseFailed("PurchaseFailed");
        successfullPurchase = false;
        Bazaar_IABAndroid.RunGetPurchases();
    }

    private void ConsumeSkuFinishedEvent(string response)
    {
        if (response == "0")
        {
            //PlayerPrefs.SetInt((purchasingItemID + "Buy"), 0);
        }
    }

#endif
}