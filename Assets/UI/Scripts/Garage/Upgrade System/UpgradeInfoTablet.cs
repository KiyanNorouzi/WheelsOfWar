using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeInfoTablet : MonoBehaviour 
{
    public GameObject myGameObject;
    public GameObject[] stateGameObjects;
    public Text levelText, upgradePriceText, fastForwardPriceText, timeRemainingText;


    int secondsRemaining;
    UpgradeInfoTabletState currentState;

    public void ActivateNormal(int level, int price)
    {
        currentState = UpgradeInfoTabletState.Normal;
        levelText.text = string.Concat("Level ", level);

        int index = (int)currentState;
        for (int i = 0; i < stateGameObjects.Length; i++)
            stateGameObjects[i].SetActive(i == index);

        upgradePriceText.text = price.ToString();
        fastForwardPriceText.text = price.ToString();

        myGameObject.SetActive(true);
    }

    public void ActivateAlreadyBought(int level, int secondsRemaining)
    {
        this.secondsRemaining = secondsRemaining;

        currentState = UpgradeInfoTabletState.AlreadyBought;
        levelText.text = string.Concat("Level ", level);

        int index = (int)currentState;
        for (int i = 0; i < stateGameObjects.Length; i++)
            stateGameObjects[i].SetActive(i == index);


        _SetTimeAndSkipPrice();
        myGameObject.SetActive(true);
    }

    private void _SetTimeAndSkipPrice()
    {
        timeRemainingText.text = MathHelper.GetTimeString(secondsRemaining);

        int skipPrice = Accounting.Instance.currentUser.CalculateSkipUpgradeTimePrice(secondsRemaining);
        if (skipPrice == 0)
            fastForwardPriceText.text = "Free";
        else
            fastForwardPriceText.text = skipPrice.ToString();
    }

    public void ActivateMaxLevel(int level)
    {
        currentState = UpgradeInfoTabletState.MaxLevel;
        levelText.text = string.Concat("Level ", level);

        int index = (int)currentState;
        for (int i = 0; i < stateGameObjects.Length; i++)
            stateGameObjects[i].SetActive(i == index);


        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }


    float time;
    void Update()
    {
        if (currentState == UpgradeInfoTabletState.AlreadyBought)
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                time %= 1;
                secondsRemaining--;
                _SetTimeAndSkipPrice();

                if (secondsRemaining <= 0)
                    UpgradeController.Instance.RefreshInfoTablet();
            }
        }
    }
}

public enum  UpgradeInfoTabletState
{
    Normal,
    AlreadyBought,
    MaxLevel
}