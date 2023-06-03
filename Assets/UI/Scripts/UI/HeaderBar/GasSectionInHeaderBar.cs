using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GasSectionInHeaderBar : MonoBehaviour 
{
    public Slider slider;
    public Text gasText;
    public Image bar11, bar12, hiderImage;
    public RectTransform blinkingBarTransform;
    public BlinkingGasBar blinkngBar;



    public int MaxValue
    {
        get { return Accounting.Instance.currentUser.MaxGas; }
    }

    public int Value
    {
        get { return Accounting.Instance.currentUser.Gas; }
    }

    public bool IsFull
    {
        get { return Value >= MaxValue; }
    }




    void Start()
    {
        Accounting.Instance.currentUser.OnEnergyChanged += OnEnergyChanged;
        Accounting.Instance.currentUser.OnMaxEnergyChanged += OnEnergyChanged;

        blinkngBar.OnRecheckGasTime += blinkngBar_OnRecheckGasTime;

        OnEnergyChanged();
    }

    void blinkngBar_OnRecheckGasTime()
    {
        Accounting.Instance.currentUser.CheckForRefillGas();
    }

    void OnEnergyChanged()
    {
        Show(Accounting.Instance.currentUser.Gas, Accounting.Instance.currentUser.MaxGas);
    }




    public void Show(int value, int maxValue)
    {
        gasText.text = string.Format("{0}/{1}", value, maxValue);
        slider.value = value;
        //slider.maxValue = maxValue;

        hiderImage.enabled = maxValue <= 10;
        bar11.enabled = value >= 11;
        bar12.enabled = value >= 12;

        if (value < maxValue)
            blinkngBar.Activate();
        else
            blinkngBar.Deactivate();
    }

    public void Refresh()
    {
        OnEnergyChanged();
    }
}
