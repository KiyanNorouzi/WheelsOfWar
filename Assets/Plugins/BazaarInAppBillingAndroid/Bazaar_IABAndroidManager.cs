using System;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

public class Bazaar_IABAndroidManager : MonoBehaviour
{
#if UNITY_ANDROID

    // Fired when service connected to Bazaar
    public static event Action<string> ServiceConnectionEvent;

    // Fired when a product is successfully purchased
    public static event Action<string> PurchaseSucceededEvent;

    // Fired when a purchase is cancelled
    public static event Action<string> PurchaseCancelledEvent;

    // Fired when a purchase fails
    public static event Action<string> PurchaseFailedEvent;

    // Fired when purchasable skus got from bazaar
    public static event Action<string> GetSkuDetailsFinishedEvent;

    // Fired when owned items got from bazaar
    public static event Action<string> GetPurchasesFinishedEvent;

    // Fired when a sku consumed
    public static event Action<string> ConsumeSkuFinishedEvent;

    //================================================================================

    private void Awake()
    {
        // Set the GameObject name to the class name for easy access from java
        gameObject.name = this.GetType().ToString();
        DontDestroyOnLoad(this);
    }

    public void ServiceConnection(string isConnected)
    {
        if (ServiceConnectionEvent != null)
            ServiceConnectionEvent(isConnected);
    }

    public void PurchaseSucceeded(string purchasedSkuToken)
    {
        if (PurchaseSucceededEvent != null)
            PurchaseSucceededEvent(purchasedSkuToken);
    }

    public void PurchaseCancelled(string productId)
    {
        if (PurchaseCancelledEvent != null)
            PurchaseCancelledEvent(productId);
    }

    public void PurchaseFailed(string productId)
    {
        if (PurchaseFailedEvent != null)
            PurchaseFailedEvent(productId);
    }

    public void GetSkuDetailsFinished(string flag)
    {
        if (GetSkuDetailsFinishedEvent != null)
            GetSkuDetailsFinishedEvent(flag);
    }

    public void GetPurchasesFinished(string count)
    {
        if (GetPurchasesFinishedEvent != null)
            GetPurchasesFinishedEvent(count);
    }

    public void ConsumeSkuFinished(string response)
    {
        if (ConsumeSkuFinishedEvent != null)
            ConsumeSkuFinishedEvent(response);
    }
#endif
}
