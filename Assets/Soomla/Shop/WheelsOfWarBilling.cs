using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Soomla.Store;
using Soomla;


public class WheelsOfWarBilling : MonoBehaviour
{
  

    void Start()
    {

        SoomlaStore.Initialize(new WheelsOfWarAssets());
        StoreEvents.OnItemPurchaseStarted += OnItemPurchaseStarted;
        StoreEvents.OnMarketPurchaseStarted += OnMarketPurchaseStarted;
        StoreEvents.OnMarketPurchaseCancelled += OnMarketPurchaseCancelled;
        StoreEvents.OnMarketPurchase += (PurchasableVirtualItem pvi, string str, Dictionary<string, string> dic) => { };
        StoreEvents.OnItemPurchased += OnItemPurchased;
        StoreEvents.OnSoomlaStoreInitialized += OnSoomlaStoreInitialized;


        StoreEvents.OnMarketRefund += OnMarketRefund;
        StoreEvents.OnGoodEquipped += OnGoodEquipped;
        StoreEvents.OnGoodUnEquipped += OnGoodUnequipped;
        StoreEvents.OnGoodUpgrade += OnGoodUpgrade;
        StoreEvents.OnBillingSupported += OnBillingSupported;
        StoreEvents.OnBillingNotSupported += OnBillingNotSupported;
        StoreEvents.OnCurrencyBalanceChanged += OnCurrencyBalanceChanged;
        StoreEvents.OnGoodBalanceChanged += OnGoodBalanceChanged;
        StoreEvents.OnMarketPurchaseDeferred += OnMarketPurchaseDeferred;
        StoreEvents.OnRestoreTransactionsStarted += OnRestoreTransactionsStarted;
        StoreEvents.OnRestoreTransactionsFinished += OnRestoreTransactionsFinished;
        StoreEvents.OnUnexpectedStoreError += OnUnexpectedStoreError;
        StoreEvents.OnVerificationStarted += OnVerificationStarted;

    #if UNITY_ANDROID && !UNITY_EDITOR
			    StoreEvents.OnIabServiceStarted += OnIabServiceStarted;
			    StoreEvents.OnIabServiceStopped += OnIabServiceStopped;
    #endif

    }


    public void OnItemPurchaseStarted(PurchasableVirtualItem pvi) { }
    public void OnMarketPurchaseStarted(PurchasableVirtualItem pvi) { }
    public void OnMarketPurchase(PurchasableVirtualItem pvi, string purchaseToken, string payload, string str) { }
    public void OnItemPurchased(PurchasableVirtualItem pvi, string payload)
    {
        switch (pvi.ID)
        {
            case WheelsOfWarAssets.golds_10:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.golds_10);
                break;

            case WheelsOfWarAssets.golds_22:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.golds_22);
                break;

            case WheelsOfWarAssets.golds_48:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.golds_48);
                break;

            case WheelsOfWarAssets.golds_104:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.golds_104);
                break;

            case WheelsOfWarAssets.golds_140:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.golds_140);
                break;

            case WheelsOfWarAssets.golds_225:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.golds_225);
                break;


                //////////////////////////

            case WheelsOfWarAssets.bills_400:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.bills_400);
                break;

            case WheelsOfWarAssets.bills_960:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.bills_960);
                break;

            case WheelsOfWarAssets.bills_2500:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.bills_2500);
                break;

            case WheelsOfWarAssets.bills_4200:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.bills_4200);
                break;

            case WheelsOfWarAssets.bills_7000:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.bills_7000);
                break;

            case WheelsOfWarAssets.bills_12000:
                Debug.Log("This item is bought");
                StoreInventory.BuyItem(WheelsOfWarAssets.bills_12000);
                break;
        }

    }


    public void OnSoomlaStoreInitialized() { }

    public void OnMarketPurchaseCancelled(PurchasableVirtualItem item) { }



    /// <summary>
    /// Handles unexpected errors with error code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    public void OnUnexpectedStoreError(int errorCode)
    {
        SoomlaUtils.LogError("ExampleEventHandler", "error with code: " + errorCode);
    }


    /// <summary>
    /// Handles a market refund event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void OnMarketRefund(PurchasableVirtualItem pvi)
    {

    }


    /// <summary>
    /// Handles a good equipped event.
    /// </summary>
    /// <param name="good">Equippable virtual good.</param>
    public void OnGoodEquipped(EquippableVG good)
    {

    }

    /// <summary>
    /// Handles a good unequipped event.
    /// </summary>
    /// <param name="good">Equippable virtual good.</param>
    public void OnGoodUnequipped(EquippableVG good)
    {

    }

    /// <summary>
    /// Handles a good upgraded event.
    /// </summary>
    /// <param name="good">Virtual good that is being upgraded.</param>
    /// <param name="currentUpgrade">The current upgrade that the given virtual
    /// good is being upgraded to.</param>
    public void OnGoodUpgrade(VirtualGood good, UpgradeVG currentUpgrade)
    {

    }

    /// <summary>
    /// Handles a billing supported event.
    /// </summary>
    public void OnBillingSupported()
    {

    }

    /// <summary>
    /// Handles a billing NOT supported event.
    /// </summary>
    public void OnBillingNotSupported()
    {

    }


    /// <summary>
    /// Handles an item purchase deferred event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    /// <param name="payload">Developer supplied payload.</param>
    public void OnMarketPurchaseDeferred(PurchasableVirtualItem pvi, string payload)
    {
    }

    /// <summary>
    /// Handles a currency balance changed event.
    /// </summary>
    /// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
    /// <param name="balance">Balance of the given virtual currency.</param>
    /// <param name="amountAdded">Amount added to the balance.</param>
    public void OnCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded)
    {

    }

    /// <summary>
    /// Handles a good balance changed event.
    /// </summary>
    /// <param name="good">Virtual good whose balance has changed.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="amountAdded">Amount added.</param>
    public void OnGoodBalanceChanged(VirtualGood good, int balance, int amountAdded)
    {

    }

    /// <summary>
    /// Handles a restore Transactions process started event.
    /// </summary>
    public void OnRestoreTransactionsStarted()
    {

    }

    /// <summary>
    /// Handles a restore transactions process finished event.
    /// </summary>
    /// <param name="success">If set to <c>true</c> success.</param>
    public void OnRestoreTransactionsFinished(bool success)
    {

    }

    /// <summary>
    /// Handles a market purchase verification started event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void OnVerificationStarted(PurchasableVirtualItem pvi)
    {

    }


#if UNITY_ANDROID && !UNITY_EDITOR
		public void OnIabServiceStarted() {

		}
		public void OnIabServiceStopped() {

		}
#endif



}