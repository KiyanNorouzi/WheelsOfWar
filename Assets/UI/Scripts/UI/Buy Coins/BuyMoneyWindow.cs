using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class BuyMoneyWindow : Window
{
    #region Singleton
    private static BuyMoneyWindow _instance;

    public static BuyMoneyWindow Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion

    public GameObject[] tabGameObjects;
    public FreeCoinsMenu freeCoinsMenu;
    public Image[] tabButtonImages;
    public Color activeColor, deactiveColor;
    public BuyCoinSlot[] billSlots, goldSlots;
    public Sprite[] billsSprites, goldsSprites;


    int inAppIndex;




    void Start()
    {
        
    }


    public void TabButton_Click(int tabIndex)
    {
        for (int i = 0; i < tabGameObjects.Length; i++)
        {
            if (i == tabIndex)
            {
                tabGameObjects[i].SetActive(true);
                tabButtonImages[i].color = activeColor;
            }
            else
            {
                tabGameObjects[i].SetActive(false);
                tabButtonImages[i].color = deactiveColor;
                
            }
        }
    }

    public void BuyCoinsMenu_OnPressed(int index)
    {
        inAppIndex = index;
        InAppPurchases.Instance.Request(index, InAppSuccessfullyDone);
    }

    void InAppSuccessfullyDone(int index, bool successfull)
    {
        
    }

    
    public override void Activate()
    {
        Activate(BuyCurrencySections.Golds);
    }

    public void Activate(BuyCurrencySections section)
    {
        float k = Prices.Instance.IAPItems[0].Bills / Prices.Instance.IAPPrices[0];
        for (int i = 0; i < billSlots.Length; i++)
        {
            if (i == 0)
                billSlots[i].Activate(Prices.Instance.IAPPrices[i], Prices.Instance.IAPItems[i].Bills, 0, billsSprites[i]);
            else
            {
                int count = Mathf.RoundToInt(Prices.Instance.IAPPrices[i] * k);
                int diff = Prices.Instance.IAPItems[i].Bills - count;
                float percent = (float)diff / count;

                billSlots[i].Activate(Prices.Instance.IAPPrices[i], Prices.Instance.IAPItems[i].Bills, Mathf.RoundToInt(percent * 100), billsSprites[i]);

                GameAnalytics.NewBusinessEvent("Bill", Prices.Instance.IAPItems[i].Bills,"Bills Pack","Bills_ID","");
                //GA.API.Business.NewEvent("BuyBills", "BuyBills", Prices.Instance.IAPItems[i].Bills);

            }
        }

        k = Prices.Instance.IAPItems[10].Golds / Prices.Instance.IAPPrices[10];
        for (int i = 0; i < goldSlots.Length; i++)
        {
            int count = Mathf.RoundToInt(Prices.Instance.IAPPrices[10 + i] * k);
            int diff = Prices.Instance.IAPItems[10 + i].Golds - count;
            float percent = (float)diff / count;

            if (i == 0)
                goldSlots[i].Activate(Prices.Instance.IAPPrices[10 + i], Prices.Instance.IAPItems[10 + i].Golds, 0, goldsSprites[0]);
            else
            {
              goldSlots[i].Activate(Prices.Instance.IAPPrices[10 + i], Prices.Instance.IAPItems[10 + i].Golds, Mathf.RoundToInt(percent * 100), goldsSprites[i]);

              GameAnalytics.NewBusinessEvent("Gold",Prices.Instance.IAPItems[10 + i].Golds,"Gold Pack","Gold_ID","");
             //GA.API.Business.NewEvent("BuyGolds", "BuyGolds", Prices.Instance.IAPItems[10 + i].Golds);
            
            }

        }

        TabButton_Click((int)section); // show buy coins menu by default
        base.Activate();
    }
}

public enum BuyCurrencySections
{
    Bills,
    Golds,
}