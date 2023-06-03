using UnityEngine;
using System.Collections;

public class RateUsWindow : MonoBehaviour 
{
    public GameObject myGameObject;

    public void Activate()
    {
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }


    public void OKButton_Click()
    {
        Deactivate();
        CommonUI.Instance.ShowRateWindow();
    }

    public void CancelButton_Click()
    {
        Deactivate();
    }
}
