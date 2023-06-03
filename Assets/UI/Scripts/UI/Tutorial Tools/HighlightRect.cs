using UnityEngine;
using System.Collections;

public class HighlightRect : MonoBehaviour 
{
    public event Data.generalDelegate OnClick;


    public GameObject myGameObject;
    public RectTransform myTransform;


    public bool IsActive
    {
        get { return myGameObject.activeSelf; }
    }


    public void Activate(Vector2 position, Vector2 size, bool acceptClicksOnlyOnHighlightedArea)
    {
        myTransform.anchoredPosition = position;
        myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

        //wholeAreaButton.interactable = !acceptClicksOnlyOnHighlightedArea;
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Click()
    {
        if (OnClick != null)
            OnClick();
    }
}