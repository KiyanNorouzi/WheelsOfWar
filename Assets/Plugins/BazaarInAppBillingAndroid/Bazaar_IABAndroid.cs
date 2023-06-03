using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
public class Bazaar_IABAndroid : MonoBehaviour
{

    #region private

    private static AndroidJavaClass plugin;

    #endregion

    //==================================================

    // first call this method before connectiong to Bazaar - IMPORTANT ---> call this one time!
    public static void Init()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        plugin = new AndroidJavaClass("com.zanagames.BazaarIAB.IABPlugin");
        plugin.CallStatic("ResetAll");
        if (plugin != null)
            plugin.CallStatic("Init");
    }

    // purchase the product with the given productId
    public static void PurchaseSku(string sku)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            plugin.CallStatic("SetPayLoad", "");
            plugin.CallStatic("PurchaseSku", sku);
        }
#endif
    }

    // purchases the product with the given productId and developerPayload
    public static void PurchaseSku(string sku, string developerPayload)
    {
        plugin.CallStatic("SetPayLoad", developerPayload);
        plugin.CallStatic("Buy", sku);
    }

    // call GetSkusDetails thread
    public static void RunGetSkuDetails(string[] skus)
    {
        string skusStr = "";
        if (skus.Length > 1)
        {
            for (int i = 0; i < skus.Length; i++)
            {
                skusStr += skus[i] + ";";
            }
        }
        else
        {
            skusStr = skus[0];
        }
        plugin.CallStatic("RunGetSkuDetails", skusStr);
    }

    // call GetSkusDetails thread
    public static void RunGetSkuDetails(List<string> skus)
    {
        string skusStr = "";
        if (skus.Count > 1)
        {
            for (int i = 0; i < skus.Count; i++)
            {
                skusStr += skus[i] + ";";
            }
        }
        else
        {
            skusStr = skus[0];
        }

        plugin.CallStatic("RunGetSkuDetails", skusStr);
    }

    // get skudetails list
    public static List<SkuDetails> GetSkuDetails()
    {
        string[] jsonArray = plugin.CallStatic<string[]>("GetSkuDetails");

        List<SkuDetails> skuDetailsList = new List<SkuDetails>();
        foreach (string json in jsonArray)
        {
            var j = new JSON(json);
            SkuDetails skuDetails = new SkuDetails();
            skuDetails.ProductId = j.ToString("productId");
            skuDetails.Price = getEnglishNumber(j.ToString("price"));
            skuDetails.Title = j.ToString("title");
            skuDetails.Description = j.ToString("description");
            skuDetailsList.Add(skuDetails);
        }

        return skuDetailsList;
    }

    // call GetPurchases thread
    public static void RunGetPurchases()
    {
        //plugin.CallStatic("RunGetPurchases", "");
        plugin.CallStatic("RunGetPurchases");
    }

    // get purchases data list
    public static List<PurchasesData> GetPurchasesData()
    {
        string[] jSonArray = plugin.CallStatic<string[]>("GetPurchasesData");

        List<PurchasesData> ownedItemsList = new List<PurchasesData>();
        foreach (string json in jSonArray)
        {
            var j = new JSON(json);
            PurchasesData purchasesData = new PurchasesData();
            purchasesData.OrderId = j.ToString("orderId");
            purchasesData.PackageName = j.ToString("packageName");
            purchasesData.ProductId = j.ToString("productId");
            purchasesData.PurchaseTime = j.ToLong("purchaseTime");
            purchasesData.PurchaseState = j.ToInt("purchaseState");
            purchasesData.DeveloperPayload = j.ToString("developerPayload");
            purchasesData.PurchaseToken = j.ToString("purchaseToken");
            ownedItemsList.Add(purchasesData);
        }

        return ownedItemsList;
    }

    // consume purchased sku if necessary
    public static void ConsumeSku(string productId)
    {
        plugin.CallStatic("ConsumePurchase", productId);
    }

    public static int GetState()
    {
        return plugin.CallStatic<int>("GetState");
    }

    // Get error messages from plugin - good for debugging
    public static string GetErrorMsg()
    {
        return plugin.CallStatic<string>("GetErrorMsg");
    }

    //for debug
    public static int GetAddedSkusArraySize()
    {
        return plugin.CallStatic<int>("GetAddedSkusArraySize");
    }

    // convert english number to persian
    private static string getEnglishNumber(string persianNumber)
    {
        string eNumber = "";

        eNumber = persianNumber.Replace("ریال", "")
            .Replace("،", "")
            .Replace(",", "")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            .Replace("۰", "0")
            .Replace(" ", "").Trim();
        return eNumber;
    }
}
#endif