using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FreeCoinSlot : MonoBehaviour 
{
    public delegate void freeCoinSlotClicked(FreeCoinSlot slot);
    public event freeCoinSlotClicked OnClick;

    public RectTransform myTransform;
    public Image itemImage, backImage;
    public Text itemText, rewardText;
    public Color activeColor, deactiveColor;

    public GameObject alreadyDoneGameObject;


    FreeCoinType type;
    bool isAvailable;
    int rewardCoins;




    public FreeCoinType Type
    {
        get { return type; }
    }

    public int RewardCoins
    {
        get { return rewardCoins; }
    }




    public void Activate(FreeCoinType type, Sprite itemSprite, string text, int rewardCoins)
    {
        this.type = type;
        this.rewardCoins = rewardCoins;

        itemImage.sprite = itemSprite;
        itemText.text = text;
        rewardText.text = MathHelper.GetStringWithComma(rewardCoins);


        Refresh();
    }

    public void Slot_Click()
    {
        if (OnClick != null)
            OnClick(this);
    }

    public void Refresh()
    {
        isAvailable = Information.Instance.IsFreeCoinAvailable(type); // ((int)type) % 2 == 1;

        if (isAvailable)
        {
            itemImage.color = backImage.color = activeColor;
            alreadyDoneGameObject.SetActive(false);
        }
        else
        {
            itemImage.color = backImage.color = deactiveColor;
            alreadyDoneGameObject.SetActive(true);
        }
    }
}

public enum FreeCoinType
{
    Facebook,
    Instagram,
    VideoAd
}