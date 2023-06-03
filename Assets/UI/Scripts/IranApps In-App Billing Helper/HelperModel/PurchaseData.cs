using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    public class PurchaseData
    {
        private string _sku;
        private PurchaseItem _purchaseItem;
        private string _signature;

        public static List<PurchaseData> FromJsonArray(JSONArray array)
        {
            List<PurchaseData> purchaseDatas = new List<PurchaseData>();

            for (int i = 0; i < array.Count; i++)
            {
                var dataNode = array[i];
                purchaseDatas.Add(new PurchaseData
                {
                    _sku = dataNode["sku"],
                    _purchaseItem = PurchaseItem.FromJson(dataNode["purchaseItem"].AsObject),
                    _signature = dataNode["signature"]
                });
            }
            return purchaseDatas;
        }


        public string GetSku()
        {
            return _sku;
        }

        public PurchaseItem GetPurchaseItem()
        {
            return _purchaseItem;
        }

        public string GetSignature()
        {
            return _signature;
        }
    }
}

