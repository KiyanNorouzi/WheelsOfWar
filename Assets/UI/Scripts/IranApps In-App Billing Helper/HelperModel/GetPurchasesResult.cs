using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    public class GetPurchasesResult
    {
        private bool _success;
        private List<PurchaseData> _data;
        private string _continuationToken;
        private InAppError _error;


        public static GetPurchasesResult FromJson(string json)
        {
            var o = JSON.Parse(json);
            var result = new GetPurchasesResult();
            result._success = o["success"].AsBool;
            if (result._success)
            {
                result._data = PurchaseData.FromJsonArray(o["data"].AsArray);
                result._continuationToken = o["continuation_token"];
            }
            else
            {
                result._error = InAppError.Create(o["error"]);
            }

            return result;
        }



        public List<PurchaseData> GetPurchaseDatas()
        {
            return _data;
        }

        public bool IsSuccess()
        {
            return _success;
        }

        public string GetContinuationToken()
        {
            return _continuationToken;
        }

        public InAppError GetError()
        {
            return _error;
        }
    }

}

