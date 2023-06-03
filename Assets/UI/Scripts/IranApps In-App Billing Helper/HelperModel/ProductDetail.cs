using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    public class ProductDetail
    {
        private string _description;
        private string _price;
        private string _title;
        private string _productId;

        public static ProductDetail FromJsonNode(JSONNode node)
        {
            return new ProductDetail()
            {
                _description = node["description"],
                _price = node["price"],
                _title = node["title"],
                _productId = node["productId"]
            };
        }

        public string GetDescription()
        {
            return _description;
        }

        public string GetPrice()
        {
            return _price;
        }

        public string GetTitle()
        {
            return _title;
        }

        public string GetProductId()
        {
            return _productId;
        }

    }

}

