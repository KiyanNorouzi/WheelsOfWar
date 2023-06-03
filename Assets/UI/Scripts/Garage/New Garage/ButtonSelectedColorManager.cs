using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonSelectedColorManager : MonoBehaviour
{
    #region

    static ButtonSelectedColorManager _instance;

    public static ButtonSelectedColorManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion


    [Header("Upgrade Buttons")]
    public Image[] upgradeFrame;
    public Image[] upgradeImage;
    public Text[] upgradeText;

    [Header("Cosmetic Buttons")]
    public Image[] cosmeticFrame;
    public Image[] cosmeticImage;
    public Outline[] cosmeticOutline;

    [Header("Colors")]
    public Color normalColor;
    public Color highlightColor;


    public void ChangeUpgradeImageColor_Button(int _index)
    {
        for (int i = 0; i < upgradeFrame.Length; i++)
        {
            upgradeFrame[i].color = normalColor;
            upgradeImage[i].color = normalColor;
            upgradeText[i].color = normalColor;
        }

        upgradeFrame[_index].color = highlightColor;
        upgradeImage[_index].color = highlightColor;
        upgradeText[_index].color = highlightColor;
    }



    public void ChangeCosmeticImageColor_Button(int _index)
    {
        bool colorMode = MainGarageCosmeticController.Instance.SideStates == SideStates.COLOR_SIDE;
        for (int i = 0; i < cosmeticFrame.Length; i++)
        {
            if (colorMode)
            {
                if (i == _index)
                {
                    cosmeticFrame[_index].color = highlightColor;
                    cosmeticOutline[i].enabled = true;
                }
                else
                {
                    cosmeticFrame[i].color = normalColor;
                    cosmeticOutline[i].enabled = false;
                }
            }
            else
            {
                cosmeticOutline[i].enabled = false; 

                if (i == _index)
                {
                    cosmeticFrame[_index].color = highlightColor;
                    cosmeticImage[_index].color = highlightColor;
                }
                else
                {
                    cosmeticFrame[i].color = normalColor;
                    cosmeticImage[i].color = normalColor;
                }
            }
        }
    }



    public void DisableImageColor()
    {
        for (int i = 0; i < upgradeFrame.Length; i++)
        {
            upgradeFrame[i].color = normalColor;
            upgradeImage[i].color = normalColor;
            upgradeText[i].color = normalColor;
        }

        for (int i = 0; i < cosmeticFrame.Length; i++)
        {
            cosmeticFrame[i].color = normalColor;
            cosmeticImage[i].color = normalColor;
        }
    }


}
