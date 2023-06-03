using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditsMenu : MonoBehaviour 
{
    public GameObject myGameObject;
    public RectTransform myTransform;
    public float speed;
    public float max;

    bool isDragging;
    float y;
    bool initialized;

    void Start()
    {
        if (initialized)
            return;

        initialized = true;
        y = myTransform.anchoredPosition.y;
    }

    public void Activate()
    {
        if (!initialized)
            Start();

        myTransform.anchoredPosition = new Vector2(0, y);
        myGameObject.SetActive(true);
    }

    public void CloseButton_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        myGameObject.SetActive(false);
    }

    public void BeginDrag()
    {
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging)
        {
            myTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
            if (myTransform.anchoredPosition.y >= max)
                myTransform.anchoredPosition = new Vector2(0, y);
        }
    }
}
