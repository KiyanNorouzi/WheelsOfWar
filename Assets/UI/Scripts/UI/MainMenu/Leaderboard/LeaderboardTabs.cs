using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaderboardTabs : MonoBehaviour 
{
    public Image[] tabButtonImages;
    public Color selectedTabColor, notSelectedTabColor;
    public Text[] persianTexts, englishTexts;
    public Color textSelectedTabColor, textNotSelectedTabColor;



    public void ItemSelected(int index)
    {
        for (int i = 0; i < tabButtonImages.Length; i++)
        {
            tabButtonImages[i].color = (i == index) ? selectedTabColor : notSelectedTabColor;
            persianTexts[i].color = (i == index) ? textSelectedTabColor : textNotSelectedTabColor;
            englishTexts[i].color = (i == index) ? textSelectedTabColor : textNotSelectedTabColor;
        }
    }
}