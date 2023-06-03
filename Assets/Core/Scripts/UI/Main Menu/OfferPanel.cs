using UnityEngine;
using System.Collections;

public class OfferPanel : MonoBehaviour
{
    public GameObject myGameObject;
    public GameObject[] icons1, icons2;
    public Animator myAnimator;
    public float changeTime;


    string carTag;
    public string CarTag
    {
        get { return carTag; }
    }

    int carIndex;
    public int CarIndex
    {
        get { return carIndex; }
    }



    int currentContent = 1;
    int currentPartIndex;



    public void Activate(int carIndex)
    {
        this.carIndex = carIndex;
        carTag = Information.Instance.carInfo[carIndex].carTag;

        ShowNextSuggestion();
        myGameObject.SetActive(true);
    }

    void ShowNextSuggestion()
    {
        int temp = 0;
        int loop = 0;
        do
        {
            loop++;
            currentPartIndex++;
            if (currentPartIndex >= 6)
                currentPartIndex = 0;


            int level = Accounting.Instance.currentUser.GetCarUpgrade(carTag, currentPartIndex, out temp);
            if (level < UpgradeInitializer.Instance.allUpgrades[currentPartIndex].upgrades.Length)
            {
                _Show(carIndex, currentPartIndex);
                Invoke("ShowNextSuggestion", changeTime);
                return;
            }
        } while (loop <= 6);

        Deactivate();
    }

    void _Show(int carIndex, int partIndex)
    {
        if (currentContent == 2)
        {
            for (int i = 0; i < icons1.Length; i++)
                icons2[i].SetActive(i == partIndex);
        }
        else
        {
            for (int i = 0; i < icons1.Length; i++)
                icons1[i].SetActive(i == partIndex);
        }

        currentContent++;
        if (currentContent == 3)
            currentContent = 1;

        myAnimator.SetInteger("n", currentContent);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Button_Click()
    {
        MainGarageUIController.sectionIndex = 0;
        MainGarageUIController.partIndex = currentPartIndex;

      //  MainGarageUIController.OfferSetState();

        SceneManager.LoadScene(Scenes.Garage);

    }

}