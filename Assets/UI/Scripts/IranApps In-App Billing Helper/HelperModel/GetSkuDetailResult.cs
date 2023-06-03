using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    public class GetSkuDetailResult : MonoBehaviour
    {
        private bool _success;
        private List<ProductDetail> _products;
        private InAppError _error;

        public static GetSkuDetailResult FromJson(string json)
        {
            var o = JSON.Parse(json);
            var a = new GetSkuDetailResult(){_success = o["success"].AsBool};
            if (a._success)
            {
                var array = o["products"].AsArray;
                a._products = new List<ProductDetail>();
                for (int i = 0; i < array.Count; i++)
                {
                     a._products.Add(ProductDetail.FromJsonNode(array[i]));
                }
            }
            else
            {
                a._error = InAppError.Create(o["error"]);
            }
            return a;
        }


        public bool IsSuccess()
        {
            return _success;
        }

        public List<ProductDetail> GetProductsDetails()
        {
            return _products;
        }

        public InAppError GetError()
        {
            return _error;
        }
    }
}

