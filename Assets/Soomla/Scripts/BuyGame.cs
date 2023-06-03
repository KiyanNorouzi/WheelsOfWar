using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;


namespace Soomla.Store
{
    public class BuyGame : MonoBehaviour
    {
        private float secTime = 1.0f, totTime = 0.0f;
        private bool adsBought, add;
        private static bool soomlaInitialized;

        void Start()
        {

            PlayGamesPlatform.Activate();

            if (gameObject.name == "Restore Purchase")
                StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;

            if (!soomlaInitialized)
                SoomlaStore.Initialize(new BuyGooglePlay());
            if (PlayerPrefs.GetString("NoAds") == "yes")
                gameObject.SetActive(false);
            soomlaInitialized = true;
        }

        void Update()
        {
            if (gameObject.name != "Restore Purchase")
            {
                if (Time.timeSinceLevelLoad > totTime)
                {
                    CheckIAP_PurchaseStatus();
                    totTime = Time.timeSinceLevelLoad + secTime;
                }
                if (adsBought && !add)
                {
                    PlayerPrefs.SetString("NoAds", "yes");
                    add = true;
                }
            }
        }

        public void onRestoreTransactionsFinished(bool success)
        {
            // success - true if the restore transactions operation has succeeded
            if (success)
                adsBought = true;
        }

        void CheckIAP_PurchaseStatus()
        {
            if (StoreInventory.GetItemBalance("no_ads") >= 1)
                adsBought = true;
        }

        void OnMouseUpAsButton()
        {
            try
            {
                if (gameObject.name != "Restore Purchase")
                    StoreInventory.BuyItem("no_ads");
                else
                    SoomlaStore.RestoreTransactions();
            }
            catch (Exception e)
            {
                Debug.Log("SOOMLA/UNITY" + e.Message);
            }
        }

    }

}
