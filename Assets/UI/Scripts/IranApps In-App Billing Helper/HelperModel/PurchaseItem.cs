using System;
using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    public class PurchaseItem
    {
        /**
        * {
        *      "orderId":"Order ID" ,
        *      "packageName" : "your applications package name" ,
        *      "productId" : "ID of the product purchased" ,
        *      "purchaseTime" : "Time of the purchase" ,
        *      "purchaseToken" : "purchase token" ,
        *      "developerPayload" : "developer payload"
        * }
        */
        private string _orderId;
        private string _packageName;
        private string _productId;
        private string _purchaseTime;
        private string _purchaseToken;
        private string _developerPayload;


        private PurchaseItem()
        {
        }


        public static PurchaseItem FromJson(JSONNode node)
        {
            var item = new PurchaseItem();
            item._orderId = node["orderId"].Value;
            item._packageName = node["packageName"].Value;
            item._productId = node["productId"].Value;
            item._purchaseTime = node["purchaseTime"].Value;
            item._purchaseToken = node["purchaseToken"].Value;
            item._developerPayload = node["developerPayload"].Value;
            return item;
        }


        public string GetOrderId()
        {
            return _orderId;
        }

        public string GetPackageName()
        {
            return _packageName;
        }

        public string GetProductId()
        {
            return _productId;
        }

        public string GetPurchaseTime()
        {
            return _purchaseTime;
        }

        public string GetPurchaseToken()
        {
            return _purchaseToken;
        }

        public string GetDeveloperPayload()
        {
            return _developerPayload;
        }
    }
}
    


