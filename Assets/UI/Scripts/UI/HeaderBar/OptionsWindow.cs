using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsWindow : Window
{
    public Image[] buttonImages;
    public OptionContentPanel[] contentPanels;
    public Color buttonActiveColor, buttonDeactiveColor;


    int selectedTabIndex;
    public int SelectedTabIndex
    {
        get { return selectedTabIndex; }
        set
        {
            selectedTabIndex = value;
            for (int i = 0; i < buttonImages.Length; i++)
            {
                if (i == selectedTabIndex)
                {
                    buttonImages[i].color = buttonActiveColor;
                    contentPanels[i].Activate();
                }
                else
                {
                    buttonImages[i].color = buttonDeactiveColor;
                    contentPanels[i].Deactivate();
                }
            }
        }
    }



    public void TabButton_Click(int index)
    {
        SelectedTabIndex = index;
    }

    public override void Activate()
    {
        SelectedTabIndex = 0;
        base.Activate();
    }
}