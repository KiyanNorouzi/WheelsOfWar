using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatSlider : MonoBehaviour 
{
    public Slider mainSlider, upgradeSlider;
    public Text mainText, upgradePercentText, cosmeticText;
    public Image backSliderImage;



    public void ShowValue(float value, float addingValue)
    {
        if (addingValue >= 0)
        {
            backSliderImage.color = upgradePercentText.color = Color.green;

            mainSlider.value = value;
            upgradeSlider.value = value + addingValue;

            if (addingValue != 0)
                upgradePercentText.text = string.Concat(addingValue.ToString("0"), "♂");
            else
                upgradePercentText.text = "";
        }
        else
        {
            backSliderImage.color = upgradePercentText.color = Color.red;

            mainSlider.value = value + addingValue;
            upgradeSlider.value = value;
            upgradePercentText.text = string.Concat(addingValue.ToString("0"), "♀");
        }
        


        mainText.text = value.ToString();
        
    }

    public void ShowPercent(float value, float percent)
    {
        float addingValue = value * (percent / 100f);

        if (addingValue >= 0)
        {
            backSliderImage.color = upgradePercentText.color = Color.green;

            mainSlider.value = value;
            upgradeSlider.value = value + addingValue;

            if (percent != 0)
                upgradePercentText.text = string.Concat(percent.ToString("0"), "% ♂");
            else
                upgradePercentText.text = "";
        }
        else
        {
            backSliderImage.color = upgradePercentText.color = Color.red;

            mainSlider.value = value + addingValue;
            upgradeSlider.value = value;
            upgradePercentText.text = string.Concat(percent.ToString("0"), "% ♀");
        }


        mainText.text = value.ToString();
    }
}
