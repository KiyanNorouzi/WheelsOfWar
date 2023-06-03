using UnityEngine;
using System.Collections;

public class BoosterPanelTutorial : MonoBehaviour 
{
    public GameObject myGameObject;
    public GameObject[] frames;






    public bool IsActive
    {
        get { return myGameObject.activeSelf; }
    }


    int pageIndex;
    public int PageIndex
    {
        get { return pageIndex; }
        set 
        {
            pageIndex = value;

            for (int i = 0; i < frames.Length; i++)
                frames[i].SetActive(i == pageIndex);
        }
    }



    public void Activate()
    {
        PageIndex = 0;
        myGameObject.SetActive(true);
    }


    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void SkipFrame_Click()
    {
        if (PageIndex == frames.Length - 1)
            Deactivate();
        PageIndex++;
    }
}
