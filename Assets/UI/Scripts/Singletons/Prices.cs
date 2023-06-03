using UnityEngine;
using System.Collections;

public class Prices : MonoBehaviour 
{
    #region Singleton

    static Prices _instance;

    public static Prices Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion 



    private static float priceMultiplyer = 1, billsMultiplyer = 1;

    public float BillsMultiplyer
    {
        get { return Prices.billsMultiplyer; }
    }

    public float PriceMultiplyer
    {
        get { return Prices.priceMultiplyer; }
    }




    public PriceStructure[] IAPItems;
    public int[] IAPPrices;
    public int EachRoundPrice;



    public void CheckForSaleOffer()
    {
        //Accounting.Instance.GetOfferSale(_SaleOfferStateFetched);
    }

    void _SaleOfferStateFetched(string decodedText)
    {
        decodedText = decodedText.ToLower();
        if (decodedText.StartsWith("ok"))
        {
            decodedText = decodedText.Substring(2);
            int commaIndex = decodedText.IndexOf(",");
            string priceString = decodedText.Substring(0, commaIndex);
            string billsString = decodedText.Substring(commaIndex + 1);

            if (!float.TryParse(priceString, out priceMultiplyer))
                priceMultiplyer = 1;

            if (!float.TryParse(billsString, out billsMultiplyer))
                billsMultiplyer = 1;

            //Debug.Log("Price Multiplyer=" + Prices.Instance.PriceMultiplyer + ", Bills Multiplyer=" + Prices.Instance.BillsMultiplyer);
        }
        else
            priceMultiplyer = billsMultiplyer = 1;


        //MoneyTablet.Instance.CheckForSale();
        //CommonUI.Instance.buyCoinsMenu.CheckForSale();
    }
}