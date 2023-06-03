using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VIPSlot : MonoBehaviour 
{
    public GameObject selectionOverlay;
    public Text priceText, remainingTimeText;
    public PriceStructure price;
    public GameObject buyButton;
    public Button myButton;


    float timeRemaining;


    public void ActivateLocked()
    {
        selectionOverlay.SetActive(false);
        priceText.text = price.Golds.ToString();
        myButton.interactable = true;
        buyButton.SetActive(true);
    }

    public void ActivateAvailable(float timeRemaining)
    {
        selectionOverlay.SetActive(true);
        buyButton.SetActive(false);
        myButton.interactable = false;

        this.timeRemaining = timeRemaining;
        remainingTimeText.text = MathHelper.GetTimeString(timeRemaining);
        time = 0;
    }

    float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time %= 1;

            timeRemaining--;
            remainingTimeText.text = MathHelper.GetTimeString(timeRemaining);
        }
    }
}