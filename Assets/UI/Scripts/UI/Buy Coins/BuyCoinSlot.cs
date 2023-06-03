using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyCoinSlot : MonoBehaviour
{
    public Text priceText, billsText;
    public GameObject priceOffGameObject, moreBillsGameObject, extraSectionGameObject;
    public Text priceOffFAText, priceOffENText, priceOffAmountText, moreBillsText, extraText;
    public Image image;
    public string priceOffFaString, priceOffEnString;
    public string priceFormat;

    public void Activate(int price, int bills, int extraPercent, Sprite sprite)
    {
        if (Prices.Instance.PriceMultiplyer != 1)
        {
            int offPercent = Mathf.RoundToInt((1f - Prices.Instance.PriceMultiplyer) * 100);

            priceOffFAText.text = string.Format(priceOffFaString, offPercent);
            priceOffENText.text = string.Format(priceOffEnString, offPercent);

            priceOffAmountText.text = string.Format(priceFormat, MathHelper.GetStringWithComma(Mathf.RoundToInt(price * Prices.Instance.PriceMultiplyer)));
            priceOffGameObject.SetActive(true);
        }
        else
            priceOffGameObject.SetActive(false);

        if (Prices.Instance.BillsMultiplyer != 1)
        {
            moreBillsText.text = Mathf.RoundToInt(bills * (Prices.Instance.BillsMultiplyer - 1f)).ToString();
            moreBillsGameObject.SetActive(true);
        }
        else
            moreBillsGameObject.SetActive(false);

        if (extraPercent == 0)
        {
            extraText.text = string.Concat(extraPercent, "%");
            //extraSectionGameObject.SetActive(false);
        }
        else
        {
            extraText.text = string.Concat(extraPercent, "%");
            extraSectionGameObject.SetActive(true);
        }

        image.sprite = sprite;
        priceText.text = string.Format(priceFormat, MathHelper.GetStringWithComma(price)); // string.Format(priceFormat, (int)(price * Prices.Instance.priceMultiplyer));
        billsText.text = MathHelper.GetStringWithComma(bills); // ((int)(bills * Prices.Instance.billsMultiplyer)).ToString();
    }
}