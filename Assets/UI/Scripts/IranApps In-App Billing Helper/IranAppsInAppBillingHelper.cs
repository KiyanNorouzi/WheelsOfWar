using SimpleJSON;
using UnityEngine;

public class IranAppsInAppBillingHelper : MonoBehaviour
{
    #region Singleton

    static IranAppsInAppBillingHelper _instance;
    public static IranAppsInAppBillingHelper Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion


    public const int ErrorCodePlatformNotSupported = -8;

#if UNITY_ANDROID    
    private AndroidJavaClass _pluginClass;
    private AndroidJavaObject _pluginInstance;
#endif

    void Start()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
            _pluginClass = new AndroidJavaClass("ir.tgbs.iranapps.billing.unityhelper.IranAppsUnityBillingHelper");
#endif
    }

    /// <summary>
    /// Call this method after instantiation to setup in app billing library internal clogs.
    /// </summary>
    public int SetupInAppBilling()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            _pluginInstance = _pluginClass.CallStatic<AndroidJavaObject>("getInstance");
            return _pluginInstance.Call<int>("setupInAppBilling");
        }
#endif
        return ErrorCodePlatformNotSupported;

    }

    /// <summary>
    /// Disposes the IAB plugin. After calling this method, you must call <see cref="Instantiate"/> and then <see cref="SetupInAppBilling"/>.
    /// </summary>
    public void Dispose()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            _pluginInstance.Call("dispose");
        }
#endif
    }


    /// <summary>
    /// Buys a product.
    /// </summary>
    /// <param name="productId">ID of the product</param>
    /// <param name="developerPayload">The developer payload.</param>
    /// <param name="consumable">if passed true, the product is purchased and consumed automatically.</param>
    /// <returns>-1 if API is not instantiated. -8 if wrong platform, 0 for okay.Get the final result in <see cref="OnIabBuyProductStatus"/></returns>
    public int BuyProduct(string productId, string developerPayload, bool consumable)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return _pluginInstance.Call<int>("buyProduct", productId, developerPayload, consumable);
        }
#endif
        return ErrorCodePlatformNotSupported;

    }

    /// <summary>
    /// Consumes a purchased item.
    /// </summary>
    /// <param name="purchaseToken"></param>
    /// <returns>-1 if API is not instantiated. -8 if wrong platform, 0 for okay.Get the final result in <see cref="OnIabConsumeStatus"/></returns>
    public int ConsumeProduct(string purchaseToken)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return _pluginInstance.Call<int>("consumeProduct", purchaseToken);
        }
#endif
        return ErrorCodePlatformNotSupported;
    }

    /// <summary>
    /// Gets purchases of the current user. (if the user is logged in)
    /// </summary>
    /// <returns>-1 if API is not instantiated. -8 if wrong platform, 0 for okay. Get the final result in <see cref="OnIabGetPurchasesStatus"/></returns>
    public int GetPurchases()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            SampleUI.PrintText("get purchases start");
            return _pluginInstance.Call<int>("getPurchases");
        }
#endif

        return ErrorCodePlatformNotSupported;
    }

    /// <summary>
    /// Gets details of the SKUs.
    /// </summary>
    /// <param name="skus"> Comma separated string of SKUs</param>
    /// <returns>-1 for when API is not instantiated, -8 for wrong platform and 0 for OKAY. Get the final result in <see cref="OnIabGetSkuStatus"/></returns>
    public int GetSkusDetails(string skusCsv)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return _pluginInstance.Call<int>("getSkuDetails", skusCsv);
        }
#endif

        return ErrorCodePlatformNotSupported;
    }
    /// <summary>
    /// </summary>
    /// <returns>-1 if API is not instantiated, -3 if AppStore is not supporting this method and 0 for OKAY</returns>
    public int LoginUser()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return _pluginInstance.Call<int>("loginUser");
        }
#endif
        return ErrorCodePlatformNotSupported;
    }

    /// <summary>
    /// Checks whether there is a user logged in in IranApps or not.
    /// </summary>
    /// <returns>-1 means that the helper is not instantiated, 0 means the user is logged in, 2 means there is no user logged in</returns>
    public int IsUserLoggedIn()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return _pluginInstance.Call<int>("isUserLoggedIn");
        }
#endif
        return ErrorCodePlatformNotSupported;
    }
    /// <summary>
    /// Callback method to know that IAB service connection status is changed.
    /// </summary>
    /// <param name="input"> a JSON</param>
    public void OnIabServiceConnectionStatus(string input)
    {
        var result = ServiceConnectionResult.FromJson(input);
        if (result.IsSuccess())
        {
            SampleUI.PrintText("Connected to IranApps in-app billing service");
        }
        else
        {
            if (result.GetStatus() == ServiceConnectionResult.StatusConnectionLost)
            {
                SampleUI.PrintText("An Error Happened while connecting to IranApps in-app billing servicer: " + result.GetError().GetErrorName() + " Code: " + result.GetError().GetErrorCode());
            }
            else
            {
                SampleUI.PrintText("Connection to IranApps in-app billing service is lost.");
            }
        }
    }

    /// <summary>
    /// Callback method to know the buy product status.
    /// </summary>
    /// <param name="input"> A JSON string representing the <see cref="BuyResult"/> class. Use <see cref="BuyResult.FromJson"/> to decode the JSON string.</param>
    public void OnIabBuyProductStatus(string input)
    {
        SampleUI.PrintText("buy product done.");

        BuyResult buyResult = BuyResult.FromJson(input);
        if (buyResult.IsSuccess())
        {
            SampleUI.PrintText("Success Purchasing Item:\n" 
                + "Developer Payload: " + buyResult.GetPurchaseItem().GetDeveloperPayload() + "\n" 
                + "Order Id: " + buyResult.GetPurchaseItem().GetOrderId() + "\n" 
                + "Package Name: " + buyResult.GetPurchaseItem().GetPackageName() + "\n" 
                + "Product ID: " + buyResult.GetPurchaseItem().GetProductId() + "\n" 
                + "Purchase Time: " + buyResult.GetPurchaseItem().GetPurchaseTime() + "\n"
                + "Purchase Token: " + buyResult.GetPurchaseItem().GetPurchaseToken());

            /*string payload = buyResult.GetPurchaseItem().GetDeveloperPayload();
            if (payload.StartsWith("ok") && payload.EndsWith("sfh"))
            {
                ConsumeProduct(buyResult.GetPurchaseItem().GetPurchaseToken());
                InAppPurchases.Instance.InAppPurchaseDone(buyResult.GetPurchaseItem().GetProductId());
            }
            else
                InAppPurchases.Instance.InAppPurchaseFailed("probably cheat");*/

            
            //InAppPurchases.Instance.InAppPurchaseDone(buyResult.GetPurchaseItem().GetProductId());
            GetPurchases();
        }
        else
        {
            SampleUI.PrintText("Failure Purchasing Item:\n Error: " + buyResult.GetError().GetErrorName() + " Error Code: " + buyResult.GetError().GetErrorCode());
            //InAppPurchases.Instance.InAppPurchaseFailed(buyResult.GetError().GetErrorName());

            GetPurchases();
        }
    }
    /// <summary>
    /// Callback method to know the login flow status. (specific when you instantiate IranApps)
    /// </summary>
    /// <param name="input"> A JSON representing <see cref="LoginResult"/>. Use <see cref="LoginResult.FromJson"/> to decode received JSON to a <see cref="LoginResult"/> instance.</param>
    public void OnIabLoginStatus(string input)
    {
        var loginResult = LoginResult.FromJson(input);
        if (loginResult.IsSuccess())
        {
            SampleUI.PrintText("User Logged In Successfully.");
            
        }
        else
        {
            SampleUI.PrintText("Logging in user caught an error. " + loginResult.GetError().GetErrorName() + " Code: " + loginResult.GetError().GetErrorCode());
        }
    }

    /// <summary>
    /// Callback method to know the consume status.
    /// </summary>
    /// <param name="input">A JSON representing <see cref="ConsumeResult"/>. Use <see cref="ConsumeResult.FromJson"/> to decode received JSON to a <see cref="ConsumeResult"/> instance.</param>
    public void OnIabConsumeStatus(string input)
    {
        var consumeResult = ConsumeResult.FromJson(input);
        if (consumeResult.IsSuccess())
        {
            SampleUI.PrintText("Item Consumed successfully.");
        }
        else
        {
            if (consumeResult.GetStatus()==ConsumeResult.StatusNotOwned)
            {
                SampleUI.PrintText("Item not owned to be able to consume.");
            }
            else
            {
                SampleUI.PrintText("Consuming Failed. Error: " + consumeResult.GetError().GetErrorName() + " Code: " + consumeResult.GetError().GetErrorCode());
            }
        }
    }


    /// <summary>
    /// Callback method to know the get purchases status.
    /// </summary>
    /// <param name="input"> a JSON</param>
    public void OnIabGetPurchasesStatus(string input)
    {
        /**
         * 
         * { success: asd ,
         *   data : [ { sku: asd , purchaseItem : { orderId : , packageName: , productId: , purchaseTime: , purchaseToken: , developerPayload: } , signature: } , ... ]
         *   continuation_token : , 
         *   error : 
         * }
         * */

        var result = GetPurchasesResult.FromJson(input);

        if (result.IsSuccess())
        {
            string line = "Received Purchases for user. " + result.GetPurchaseDatas().Count +"  \n Purchases are: \n";

            bool isDoneSuccessfully = false;
            for (int i = 0; i < result.GetPurchaseDatas().Count; i++)
            {
                line = line + result.GetPurchaseDatas()[i].GetPurchaseItem().GetProductId() + " " +
                       result.GetPurchaseDatas()[i].GetPurchaseItem().GetPurchaseToken() + " " +
                       result.GetPurchaseDatas()[i].GetPurchaseItem().GetDeveloperPayload() + "\n";

                InAppPurchases.Instance.InAppPurchaseDone(result.GetPurchaseDatas()[i].GetPurchaseItem().GetProductId());
                ConsumeProduct(result.GetPurchaseDatas()[i].GetPurchaseItem().GetPurchaseToken());

                isDoneSuccessfully = true;
            }

            if (!isDoneSuccessfully)
                InAppPurchases.Instance.InAppPurchaseFailed("");

            SampleUI.PrintText(line);
        }
        else
        {
            InAppPurchases.Instance.InAppPurchaseFailed("");
            SampleUI.PrintText("Getting PurchCaught error. " + result.GetError().GetErrorName() + " Code: " + result.GetError().GetErrorCode());
        }
    }
    /// <summary>
    /// Callback method to know the getSkus status.
    /// </summary>
    /// <param name="input"> a JSON</param>
    public void OnIabGetSkuStatus(string input)
    {
        var result = GetSkuDetailResult.FromJson(input);

        if (result.IsSuccess())
        {
            string line = "Total of " + result.GetProductsDetails().Count + " SKU details received.\n";
            for (int i = 0; i < result.GetProductsDetails().Count; i++)
            {
                var d = result.GetProductsDetails()[i];
                line = line + d.GetProductId() + " " + d.GetTitle() +" "+ d.GetDescription() + " " + d.GetPrice() + "\n";
            }
            SampleUI.PrintText(line);
        }
        else
        {
            SampleUI.PrintText("Caught error while getting SKUs details. " + result.GetError().GetErrorName() + " " + result.GetError().GetErrorCode());
        }
    }
}
