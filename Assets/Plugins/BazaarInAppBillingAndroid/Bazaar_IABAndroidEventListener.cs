using UnityEngine;

public class Bazaar_IABAndroidEventListener : MonoBehaviour
{

#if UNITY_ANDROID

    private void OnEnable()
    {
        // Listen to all events for illustration purposes
        Bazaar_IABAndroidManager.ServiceConnectionEvent += ServiceConnectionEvent;

        Bazaar_IABAndroidManager.PurchaseSucceededEvent += PurchaseSucceededEvent;
        Bazaar_IABAndroidManager.PurchaseCancelledEvent += PurchaseCancelledEvent;
        Bazaar_IABAndroidManager.PurchaseFailedEvent += PurchaseFailedEvent;

        Bazaar_IABAndroidManager.GetSkuDetailsFinishedEvent += GetSkuDetailsFinishedEvent;
        Bazaar_IABAndroidManager.GetPurchasesFinishedEvent += GetPurchasesFinishedEvent;

        Bazaar_IABAndroidManager.ConsumeSkuFinishedEvent += ConsumeSkuFinishedEvent;
    }

    private void OnDisable()
    {
        // Remove all event handlers
        Bazaar_IABAndroidManager.ServiceConnectionEvent -= ServiceConnectionEvent;

        Bazaar_IABAndroidManager.PurchaseSucceededEvent -= PurchaseSucceededEvent;
        Bazaar_IABAndroidManager.PurchaseCancelledEvent -= PurchaseCancelledEvent;
        Bazaar_IABAndroidManager.PurchaseFailedEvent -= PurchaseFailedEvent;

        Bazaar_IABAndroidManager.GetSkuDetailsFinishedEvent -= GetSkuDetailsFinishedEvent;
        Bazaar_IABAndroidManager.GetPurchasesFinishedEvent -= GetPurchasesFinishedEvent;

        Bazaar_IABAndroidManager.ConsumeSkuFinishedEvent -= ConsumeSkuFinishedEvent;
    }

    //===============================================================

    private void ServiceConnectionEvent(string isConnected)
    {
        Debug.Log("ServiceConnectionEvent : " + isConnected);
    }

    private void PurchaseSucceededEvent(string purchasedSkuToken)
    {
        Debug.Log("PurchaseSucceededEvent : " + purchasedSkuToken);
    }

    private void PurchaseCancelledEvent(string productId)
    {
        Debug.Log("PurchaseCancelledEvent : " + productId);
    }

    private void PurchaseFailedEvent(string productId)
    {
        Debug.Log("purchaseFailedEvent : " + productId);
    }

    private void GetSkuDetailsFinishedEvent(string flag)
    {
        Debug.Log("GetSkuDetailsFinishedEvent : " + flag);
    }

    private void GetPurchasesFinishedEvent(string count)
    {
        Debug.Log("GetPurchasesFinishedEvent : " + count);
    }

    private void ConsumeSkuFinishedEvent(string response)
    {
        Debug.Log("ConsumeSkuFinishedEvent : " + response);
    }

#endif
}