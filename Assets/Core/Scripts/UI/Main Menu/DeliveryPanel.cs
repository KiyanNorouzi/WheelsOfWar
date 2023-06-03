using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeliveryPanel : MonoBehaviour 
{
    public GameObject myGameObject;
    public Image iconImage;
    public Text timeText;
    public GameObject freeGameObject, fastForwardGameobect;
    public Sprite[] icons;


    int time;
    public int Time
    {
        get { return time; }
        set 
        { 
            time = value;
            timeText.text = MathHelper.GetTimeString(time);

            fastForwardGameobect.SetActive(time > 120);
            freeGameObject.SetActive(time <= 120);
        }
    }


    int partIndex;
    public int PartIndex
    {
        get { return partIndex; }
    }

    int carIndex;
    public int CarIndex
    {
        get { return carIndex; }
    }


	public void Activate(int carIndex, int partIndex, int timeRemaining)
    {
        this.partIndex = partIndex;
        this.carIndex = carIndex;

        iconImage.sprite = icons[partIndex];
        timeText.text = MathHelper.GetTimeString(timeRemaining);

        myGameObject.SetActive(true);
        _RefreshTime();
    }

    void _RefreshTime()
    {
        Time--;
        if (myGameObject.activeSelf)
            Invoke("_RefreshTime", 1);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }


    public void Button_Click()
    {
        MainGarageUIController.sectionIndex = 0;
        MainGarageUIController.partIndex = partIndex;

        SceneManager.LoadScene(Scenes.Garage);
    }
}
