using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CosmeticInfoTablet : MonoBehaviour 
{
    public GameObject myGameObject;
    public Text titleText, descText, priceText;
    public GameObject[] layoutGameObjects;
    public GameObject goldBarGameObject, billsGameObkect;


	public void Activate(string name, string desc, PriceStructure price, CosmeticInfoTabletState state)
    {
        titleText.text = name;
        descText.text = desc;

        int index = (int)state;
        for (int i = 0; i < layoutGameObjects.Length; i++)
            layoutGameObjects[i].SetActive(i == index);

        if (price.Golds != 0)
        {
            goldBarGameObject.SetActive(true);
            billsGameObkect.SetActive(false);

            priceText.text = price.Golds.ToString();
        }
        else
        {
            goldBarGameObject.SetActive(false);
            billsGameObkect.SetActive(true);

            priceText.text = price.Bills.ToString();
        }

        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}

public enum CosmeticInfoTabletState
{
    Buy,
    Equip,
    Select
}