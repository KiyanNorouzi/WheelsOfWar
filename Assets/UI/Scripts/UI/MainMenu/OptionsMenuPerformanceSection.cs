using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenuPerformanceSection : MonoBehaviour 
{
    public Text[] shadows, qualitySetting, vfx, colors;
    public Text[] bestText, mediumText, lowText;
    public Color buttonsSelectedColor, buttonsDeselectedColor;
    public Color featureSelectedColor, featureDeselectedColor;


    Color[] setColors;


    public bool LightsEnabled
    {
        get { return SettingData.Lights; }
        set
        {
            SettingData.Lights = value;
            if (value)
            {
                for (int i = 0; i < shadows.Length; i++)
                    shadows[i].color = featureSelectedColor;
            }
            else
            {
                for (int i = 0; i < shadows.Length; i++)
                    shadows[i].color = featureDeselectedColor;
            }
        }
    }

    public bool TextureQuality
    {
        get { return SettingData.TextureQuality; }
        set
        {
            SettingData.TextureQuality = value;
            if (value)
            {
                for (int i = 0; i < qualitySetting.Length; i++)
                    qualitySetting[i].color = featureSelectedColor;
            }
            else
            {
                for (int i = 0; i < qualitySetting.Length; i++)
                    qualitySetting[i].color = featureDeselectedColor;
            }
        }
    }

    public bool VFX
    {
        get { return SettingData.VFX; }
        set
        {
            SettingData.VFX = value;
            if (value)
            {
                for (int i = 0; i < vfx.Length; i++)
                    vfx[i].color = featureSelectedColor;
            }
            else
            {
                for (int i = 0; i < vfx.Length; i++)
                    vfx[i].color = featureDeselectedColor;
            }
        }
    }

    public bool Colors
    {
        get { return SettingData.Colors; }
        set
        {
            SettingData.Colors = value;
            if (value)
            {
                for (int i = 0; i < colors.Length; i++)
                    colors[i].color = featureSelectedColor;
            }
            else
            {
                for (int i = 0; i < colors.Length; i++)
                    colors[i].color = featureDeselectedColor;
            }
        }
    }


    int setSelected;
    public int Set
    {
        get{return setSelected;}
        set
        {
            setSelected = value;

            for (int i = 0; i < bestText.Length; i++)
                bestText[i].color = (3 == setSelected) ? setColors[2]:buttonsDeselectedColor;

            for (int i = 0; i < mediumText.Length; i++)
                mediumText[i].color = (2 == setSelected) ? setColors[1]:buttonsDeselectedColor;

            for (int i = 0; i < lowText.Length; i++)
                lowText[i].color = (1 == setSelected) ? setColors[0] : buttonsDeselectedColor;
        }
    }




    void Awake()
    {
        setColors = new Color[3];
        setColors[0] = lowText[0].color;
        setColors[1] = mediumText[0].color;
        setColors[2] = bestText[0].color;
    }


    void _Calculate()
    {
        int points = LightsEnabled ? 1 : 0;
        points += VFX ? 1 : 0;
        points += TextureQuality ? 1 : 0;
        points += Colors ? 1 : 0;

        if (points == 4)
            Set = 3;
        else if (points == 0)
            Set = 1;
        else
            Set = 2;
    }


    void OnEnable()
    {
        VFX = VFX;
        LightsEnabled = LightsEnabled;
        TextureQuality = TextureQuality;
        Colors = Colors;

        _Calculate();
    }

    public void Feature_Click(int index)
    {
        switch (index)
        {
            case 0: LightsEnabled = !LightsEnabled; break;
            case 1: TextureQuality = !TextureQuality; break;
            case 2: VFX = !VFX; break;
            case 3: Colors = !Colors; break;
        }

        _Calculate();
    }

    public void QualitySet_Click(int index)
    {
        switch (index)
        {
            case 1: // low
                LightsEnabled = TextureQuality = VFX = Colors = false;
                Set = 1;
                break;

            case 2: // medium
                LightsEnabled = TextureQuality = true;
                VFX = Colors = false;
                Set = 2;
                break;

            case 3: // best
                LightsEnabled = TextureQuality = VFX = Colors = true;
                Set = 3;
                break;
        }
    }

	
}