using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GasRefeelWindow : Window
{
    public Button goldButton, freeGasButton;
    public GameObject activateVIPGameObject, vipIsActiveGameObject;
    public GameObject gasIsFullMessageGameObject, nextRefillInGameObject;
    public Text priceText, nextRefillInText;
    public float goldGasRatio;
    public string nextRefillInFormat;

    float timeK;



    void OnEnable()
    {
        Accounting.Instance.currentUser.OnEnergyChanged += currentUser_OnEnergyChanged;
        Accounting.Instance.currentUser.OnMaxEnergyChanged += currentUser_OnEnergyChanged;
    }

    void currentUser_OnEnergyChanged()
    {
        int diff = Accounting.Instance.currentUser.MaxGas - Accounting.Instance.currentUser.Gas;

        if (diff == 0)
        {
            goldButton.interactable = freeGasButton.interactable = false;
            gasIsFullMessageGameObject.SetActive(true);
            nextRefillInGameObject.SetActive(false);

            enabled = false;
        }
        else
        {
            goldButton.interactable = freeGasButton.interactable = true;
            gasIsFullMessageGameObject.SetActive(false);
            nextRefillInGameObject.SetActive(true);

            timeK = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.GasRefillTime);
            _SetNextRefillText();

            enabled = true;
        }

        int price = Mathf.CeilToInt(diff * goldGasRatio);
        priceText.text = price.ToString();
    }


    public override void Activate()
    {
        if (Accounting.Instance.currentUser.IsGasTankFull)
        {
            goldButton.interactable = freeGasButton.interactable = false;
            gasIsFullMessageGameObject.SetActive(true);
        }
        else
        {
            goldButton.interactable = false;
            freeGasButton.interactable = AdPlayManager.Instance.IsAdAvailable;
            gasIsFullMessageGameObject.SetActive(false);
        }


        currentUser_OnEnergyChanged();


        bool isVIPActive = Accounting.Instance.currentUser.HasVIPPackage(VIPActionType.GasRefillTime);
        //activateVIPGameObject.SetActive(!isVIPActive);
        vipIsActiveGameObject.SetActive(isVIPActive);

        base.Activate();
    }

    private void _SetNextRefillText()
    {
        float time = GeneralSettings.Instance.GasRegenerateTime * timeK - Accounting.Instance.currentUser.LastRefillTimeInSeconds;
        nextRefillInText.text = string.Format(nextRefillInFormat, MathHelper.GetTimeString(time));
    }



    public void BuyGasButton_Click()
    {
        int diff = Accounting.Instance.currentUser.MaxGas - Accounting.Instance.currentUser.Gas;
        int priceInGolds = Mathf.CeilToInt(diff * goldGasRatio);

        PriceStructure price = new PriceStructure();
        price.Golds = priceInGolds;

        Accounting.Instance.currentUser.Buy(price, _GasBought);
    }

    private void _GasBought()
    {
        Accounting.Instance.currentUser.Gas = Accounting.Instance.currentUser.MaxGas;
    }


    public void FreeGasButton_Click()
    {
        AdPlayManager.Instance.ShowAd(_AdDone);
    }

    void _AdDone(AdShowResult result)
    {
        switch (result)
        {
            case AdShowResult.Failed:
            case AdShowResult.NotAvailable:
                break;
            case AdShowResult.Showed:
            case AdShowResult.Installed:
                Accounting.Instance.currentUser.Gas++;
                break;
        }
    }

    public void BuyVIPPackageButton_Click()
    {
        CloseButton_Click();
        CommonUI.Instance.headerBar.vipWindow.Activate();
    }


    float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time %= 1;
            _SetNextRefillText();
        }
    }
}
