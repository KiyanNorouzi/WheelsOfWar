using UnityEngine;
using System.Collections;

namespace Soomla.Store
{
    public class BuyGooglePlay : IStoreAssets
    {
        public int GetVersion()
        { // Get Current Version
            return 0;
        }

        public VirtualCurrency[] GetCurrencies()
        {
            return new VirtualCurrency[] { Gold_Currency, Bills_Currency };
        }

        public VirtualGood[] GetGoods()
        {
            return new VirtualGood[] { NO_ADS };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] { GOLD_PACK_1, GOLD_PACK_2, GOLD_PACK_3, GOLD_PACK_4, GOLD_PACK_5, GOLD_PACK_6, Bills_PACK_1, Bills_PACK_2, Bills_PACK_3, Bills_PACK_4, Bills_PACK_5, Bills_PACK_6 };
        }

        public VirtualCategory[] GetCategories()
        {
            return new VirtualCategory[] { };
        }



        public const string Gold_CURRENCY_ITEM_ID = "currency_golds";

        public const string Bills_CURRENCY_ITEM_ID = "currency_bills";


        public const string golds_10 = "golds_10";
        public const string golds_22 = "golds_22";
        public const string golds_48 = "golds_48";
        public const string golds_104 = "golds_104";
        public const string golds_140 = "golds_140";
        public const string golds_225 = "golds_225";


        public const string bills_400 = "bills_400";
        public const string bills_960 = "bills_960";
        public const string bills_2500 = "bills_2500";
        public const string bills_4200 = "bills_4200";
        public const string bills_7000 = "bills_7000";
        public const string bills_12000 = "bills_12000";



        #region  Virtual Currencies

        /** Virtual Currencies **/

        public static VirtualCurrency Gold_Currency = new VirtualCurrency
        (
        "Golds",                    // name
        "",                         // description
        Gold_CURRENCY_ITEM_ID      // item id
        );



        public static VirtualCurrency Bills_Currency = new VirtualCurrency
        (
        "Bills",                    // name
        "",                         // description
        Bills_CURRENCY_ITEM_ID      // item id
        );


        #endregion


        #region Virtual Currency Packs


        #region Gold Currency Packs

        /** Virtual Currency Packs **/

        public static VirtualCurrencyPack GOLD_PACK_1 = new VirtualCurrencyPack
            (
            "Gold pack 1",                    // name
            "",                           // description
            "Gold_1",                    // item id
            10,                           // number of currencies in the pack
            Gold_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(golds_10, 1.00)
            );


        public static VirtualCurrencyPack GOLD_PACK_2 = new VirtualCurrencyPack
            (
            "Gold pack 2",                    // name
            "",                           // description
            "Gold_2",                    // item id
            22,                           // number of currencies in the pack
            Gold_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(golds_22, 1.00)
            );



        public static VirtualCurrencyPack GOLD_PACK_3 = new VirtualCurrencyPack
            (
            "Gold pack 3",                    // name
            "",                           // description
            "Gold_3",                    // item id
            48,                           // number of currencies in the pack
            Gold_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(golds_48, 1.00)
            );


        public static VirtualCurrencyPack GOLD_PACK_4 = new VirtualCurrencyPack
            (
            "Gold pack 4",                    // name
            "",                           // description
            "Gold_4",                    // item id
            104,                           // number of currencies in the pack
            Gold_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(golds_104, 1.00)
            );



        public static VirtualCurrencyPack GOLD_PACK_5 = new VirtualCurrencyPack
            (
            "Gold pack 5",                    // name
            "",                           // description
            "Gold_5",                    // item id
            140,                           // number of currencies in the pack
            Gold_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(golds_140, 1.00)
            );



        public static VirtualCurrencyPack GOLD_PACK_6 = new VirtualCurrencyPack
            (
            "Gold pack 6",                    // name
            "",                           // description
            "Gold_6",                    // item id
            225,                           // number of currencies in the pack
            Gold_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(golds_225, 1.00)
            );

        #endregion

        #region Bills

        public static VirtualCurrencyPack Bills_PACK_1 = new VirtualCurrencyPack
            (
            "Bills pack 1",                    // name
            "",                           // description
            "Bills_1",                    // item id
            400,                           // number of currencies in the pack
            Bills_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(bills_400, 1.00)
            );


        public static VirtualCurrencyPack Bills_PACK_2 = new VirtualCurrencyPack
            (
            "Bills pack 2",                    // name
            "",                           // description
            "Bills_2",                    // item id
            960,                           // number of currencies in the pack
            Bills_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(bills_960, 1.00)
            );



        public static VirtualCurrencyPack Bills_PACK_3 = new VirtualCurrencyPack
            (
            "Bills pack 3",                    // name
            "",                           // description
            "Bills_3",                    // item id
            2500,                           // number of currencies in the pack
            Bills_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(bills_2500, 1.00)
            );


        public static VirtualCurrencyPack Bills_PACK_4 = new VirtualCurrencyPack
            (
            "Bills pack 4",                    // name
            "",                           // description
            "Bills_4",                    // item id
            4200,                           // number of currencies in the pack
            Bills_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(bills_4200, 1.00)
            );



        public static VirtualCurrencyPack Bills_PACK_5 = new VirtualCurrencyPack
            (
            "Bills pack 5",                    // name
            "",                           // description
            "Bills_5",                    // item id
            7000,                           // number of currencies in the pack
            Bills_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(bills_7000, 1.00)
            );



        public static VirtualCurrencyPack Bills_PACK_6 = new VirtualCurrencyPack
            (
            "Bills pack 6",                    // name
            "",                           // description
            "Bills_6",                    // item id
            12000,                           // number of currencies in the pack
            Bills_CURRENCY_ITEM_ID,                   // the currency associated with this pack
            new PurchaseWithMarket(bills_12000, 1.00)
            );

        #endregion

        #endregion






        public const string NO_ADS_PRODUCT_ID = "no_ads";

        public static VirtualGood NO_ADS = new LifetimeVG(
            "No more ads",
            "No more ads forever",
            "no_ads",
        new PurchaseWithMarket(NO_ADS_PRODUCT_ID, 0.99));
    }
}