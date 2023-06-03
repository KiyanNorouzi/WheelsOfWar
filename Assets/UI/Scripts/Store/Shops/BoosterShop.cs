using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoosterShop : MonoBehaviour 
{
    public Transform[] itemTransforms;
    public float moveDuration;
    public int defaultItemIndex;


    public Vector3[] itemPositions;
    public Vector3[] itemScales;
    public Text englishText, persianText, descEnText, descFaText, priceText;
    public Image boosterSign;
    public AudioSource buyMoveSound;


    int[] places;
    int moveDirection, selectedItemIndex;
    float time, flow;


    void Start()
    {
        places = new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
        for (int i = 0; i < itemTransforms.Length; i++)
        {
            itemTransforms[i].localPosition = _GetPosition(places[i]);
            itemTransforms[i].localScale = _GetScale(places[i]);
        }

        _RoundUpPlaces(0);
    }

    public void Prev()
    {
        time = 0;
        moveDirection = 1;

        _RoundUpPlaces(moveDirection);
        buyMoveSound.Play();
    }

    public void Next()
    {
        time = 0;
        moveDirection = -1;

        _RoundUpPlaces(moveDirection);
        buyMoveSound.Play();
    }

    void _RoundUpPlaces(int moveDirection)
    {
        for (int i = 0; i < itemTransforms.Length; i++)
        {
            if (moveDirection == 1)
            {
                if (places[i] >= 2)
                    places[i] -= itemTransforms.Length;
            }
            else if (moveDirection == -1)
            {
                if (places[i] <= -2)
                    places[i] += itemTransforms.Length;
            }

            if (places[i] == 0)
                selectedItemIndex = i;
        }

        persianText.text = StoreData.Instance.packages[selectedItemIndex].packageNameFA;
        englishText.text = StoreData.Instance.packages[selectedItemIndex].packageNameEN;

        descFaText.text = StoreData.Instance.packages[selectedItemIndex].DescFA;
        descEnText.text = StoreData.Instance.packages[selectedItemIndex].DescEN;

        boosterSign.sprite = Inventory.Instance.slots[selectedItemIndex].myImage.sprite;

        priceText.text = StoreData.Instance.packages[selectedItemIndex].price.ToString();
    }

	void Update()
    {
        if (moveDirection != 0)
        {
            time += Time.deltaTime;
            flow = time / moveDuration;
            if (flow>=1)
            {
                for (int i = 0; i < itemTransforms.Length; i++)
                {
                    Vector3 nextPos = _GetPosition(places[i] + moveDirection);
                    Vector3 nextScale = _GetScale(places[i] + moveDirection);

                    itemTransforms[i].localPosition = nextPos;
                    itemTransforms[i].localScale = nextScale;

                    places[i] += moveDirection;
                }

                _RoundUpPlaces(moveDirection);
                moveDirection = 0;
            }
            else
            {
                for (int i = 0; i < itemTransforms.Length; i++)
                {
                    Vector3 currentPos = _GetPosition(places[i]);
                    Vector3 nextPos = _GetPosition(places[i] + moveDirection);

                    Vector3 currentScale = _GetScale(places[i]);
                    Vector3 nextScale = _GetScale(places[i] + moveDirection);

                    itemTransforms[i].localPosition = Vector3.Lerp(currentPos, nextPos, flow);
                    itemTransforms[i].localScale = Vector3.Lerp(currentScale, nextScale, flow);
                }
            }
        }
	}

    public void BuyButton_Click()
    {
        Accounting.Instance.currentUser.Buy(StoreData.Instance.packages[selectedItemIndex].price, _Buy);
    }

    void _Buy()
    {
        Inventory.Instance.AddItemWithAnimation(StoreData.Instance.packages[selectedItemIndex].type, 1, itemTransforms[selectedItemIndex].position, false);
    }

    void _OpenBuyCoinsMenu()
    {
        CommonUI.Instance.buyCoinsMenu.Activate();
    }

    Vector3 _GetPosition(int place)
    {
        if (place >= -2 && place <= 2)
            return itemPositions[place + 2];
        else
            return new Vector3(0, 0, 0);
    }

    Vector3 _GetScale(int place)
    {
        if (place >= -2 && place <= 2)
            return itemScales[place + 2];
        else
            return new Vector3(0, 0, 0);
    }
}