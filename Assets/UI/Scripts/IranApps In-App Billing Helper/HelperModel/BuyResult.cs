using UnityEngine;
using SimpleJSON;

namespace SimpleJSON
{
    public class BuyResult
    {
        private bool _success;
        private PurchaseItem _purchaseItem;
        private InAppError _error;

        public static BuyResult FromJson(string json)
        {
            var buyResult = new BuyResult();
            var rootNode = JSON.Parse(json);
            buyResult._success = rootNode["success"].AsBool;
            if (buyResult._success)
            {
                buyResult._purchaseItem = PurchaseItem.FromJson(rootNode["item"].AsObject);
            }
            else
            {
                buyResult._error = InAppError.Create(rootNode["error"]);
            }
            return buyResult;
        }


        public InAppError GetError()
        {
            return _error;
        }

        public bool IsSuccess()
        {
            return _success;
        }

        public PurchaseItem GetPurchaseItem()
        {
            return _purchaseItem;
        }
    }


}
