using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeSection : MonoBehaviour 
{
    public event Data.generalDelegate OnUpgrade;


    public GameObject myGameObject, priceSection;
    public Slider slider;
    public Text upgradeText, coinsText, currentLevelText;
    public Button upgradeButton;


    int currentCarIndex;
    int targetLevel;
    PriceStructure price;

    public void CarChanged(int carIndex)
    {
        currentCarIndex = carIndex;

        int currentLevel = Data.GetCarLevel(carIndex);

        if (currentLevel == Information.Instance.carInfo[carIndex].levels.Length - 1)
        {
            myGameObject.SetActive(false);
        }
        else
        {
            int nextLevel = currentLevel + 1;


            //Data.GetCarUseCount()
            /*int totalRoundsUsed = 0;
            for (int i = 0; i < currentLevel; i++)
            {
                totalRoundsUsed += Information.Instance.carInfo[carIndex].levels[i].RoundsRequired;
            }*/


            int roundsRequired = Information.Instance.carInfo[carIndex].levels[nextLevel].RoundsRequired;
            int roundsPlayed = Data.GetCarUseCountSinceUpgrade(carIndex);

            float progress = (float)roundsPlayed / (float)roundsRequired;
            slider.value = progress;

            if (progress >= GeneralSettings.Instance.ProgressNeededToOpenUpgradeCar)
            {
                upgradeButton.interactable = true;
                priceSection.SetActive(true);
            }
            else
            {
                upgradeButton.interactable = false;
                priceSection.SetActive(false);
            }
                

            //price = Information.Instance.carInfo[carIndex].levels[nextLevel].price;
            //price += Mathf.Max(0, roundsRequired - roundsPlayed) * Prices.Instance.EachRoundPrice;

            currentLevelText.text = string.Concat("Level ", currentLevel + 1);
            upgradeText.text = "Upgrade to Level " + (nextLevel + 1);
            coinsText.text = string.Concat("x", price);
            myGameObject.SetActive(true);
        }
    }

    /*
    public void UpgradeButton_Click()
    {
        targetLevel = Data.GetCarLevel(currentCarIndex) + 1;
        Accounting.Instance.currentUser.Buy()



        if (Data.Money >= price)
        {
            CommonUI.Instance.question.Ask(Messages.ConfirmBuyItem, _BuyConfirmed, null, false, price.ToString());
        }
        else
        {
            CommonUI.Instance.question.Ask(Messages.NotEnoughMoney, _BuyCash, null, false);
        }
    }*/


    void _BuyConfirmed()
    {
        //Data.SetCarLevel(currentCarIndex, targetLevel);
        Data.SetCarUseCountSinceUpgrade(currentCarIndex, 0);

        CarChanged(currentCarIndex);

        if (OnUpgrade != null)
            OnUpgrade();
    }

    void _BuyCash()
    {
        CommonUI.Instance.buyCoinsMenu.Activate();
    }


    public void Add5RoundsToCurrentCar()
    {
        Data.SetCarUseCountSinceUpgrade(currentCarIndex, Data.GetCarUseCountSinceUpgrade(currentCarIndex) + 5);
        CarChanged(currentCarIndex);
    }
}