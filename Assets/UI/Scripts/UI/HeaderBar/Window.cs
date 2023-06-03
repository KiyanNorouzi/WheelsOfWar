using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Window : MonoBehaviour 
{
    public static List<Window> openWindows = new List<Window>();


    void OnDestroy()
    {
        if (openWindows.Contains(this))
            openWindows.Remove(this);
    }


    public GameObject myGameObject;


    public bool IsActive
    {
        get
        { return myGameObject.activeSelf; }
    }



    public virtual void Activate()
    {
        if (!openWindows.Contains(this))
            openWindows.Add(this);

        myGameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        openWindows.Remove(this);
        myGameObject.SetActive(false);
    }

    public void CloseButton_Click()
    {
        Deactivate();
    }
}