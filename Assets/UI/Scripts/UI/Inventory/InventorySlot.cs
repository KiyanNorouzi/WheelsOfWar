using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour 
{
    public delegate void slotClicked(BoosterPackageType type);
    public event slotClicked OnClick;

    public RectTransform myTransform;
    public Image myImage;
    public Text numberText;
    public InventorySlotGraphicItem[] graphicalItems;
    public Button myButton;
    public BoosterPackageType type;

    int count;
    public int Count
    {
        get
        {
            if (GameplayUI.Instance == null || GameplayUI.Instance.boosterPanel == null || GameplayUI.Instance.boosterPanel.EquippedPackages == null)
            {
                return count;
            }
            else
            {
                bool isThisItemInUse = false;
                for (int i = 0; i < GameplayUI.Instance.boosterPanel.EquippedPackages.Count; i++)
			    {
                    if (GameplayUI.Instance.boosterPanel.EquippedPackages[i] == type)
                    {
                        isThisItemInUse = true;
                        break;
                    }
			    }

                if (isThisItemInUse)
                    return count - 1;
                else
                    return count;
            }
        }

        set
        {
            count = Mathf.Max(0, value);
            _SetSettings(count);
        }
            
    }

    private void _SetSettings(int count)
    {
        numberText.text = count.ToString();

        if (count <= 0)
        {
            for (int i = 0; i < graphicalItems.Length; i++)
            {
                if (graphicalItems[i].image != null)
                    graphicalItems[i].image.color = graphicalItems[i].deactiveState;

                if (graphicalItems[i].text != null)
                    graphicalItems[i].text.color = graphicalItems[i].deactiveState;
            }
        }
        else
        {
            for (int i = 0; i < graphicalItems.Length; i++)
            {
                if (graphicalItems[i].image != null)
                    graphicalItems[i].image.color = graphicalItems[i].activeState;

                if (graphicalItems[i].text != null)
                    graphicalItems[i].text.color = graphicalItems[i].activeState;
            }
        }
    }


    public void Slot_Click()
    {
        if (OnClick != null)
            OnClick(type);
    }

    public void Add(int count)
    {
        Debug.Log(name + " add " + count);
        Count = this.count + count;
    }

    public void SetNumber(int count)
    {
        Count = count;
    }

    public void Refresh()
    {
        _SetSettings(Count);
    }
}




[System.Serializable]
public class InventorySlotGraphicItem
{
    public Image image;
    public Text text;
    public Color activeState, deactiveState;
}