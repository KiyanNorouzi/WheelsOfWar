using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatViwer : MonoBehaviour
{
    #region Variables

    [Header("Pop Up Object")]
    public GameObject popUpObject; //This varible use for parent panel game object

    [Header("UI Objects")]
    public Button myButton; //This variable use for button types
    public Image myPanel; //This vaiable use for all image types
    public Text[] myText; //This variable countain all of text's in panel


    [Header("Fading Timer")]
    public float fadeTime = 2f; // this variable use for fading timer
    Color colorToFadeTo; //this varibale use for control the RGBA values 


    bool Popdown = false;

    #endregion

    void Start()
    {
        myButton = myButton.GetComponent<Button>();
        myPanel = myPanel.GetComponent<Image>();
        myButton.onClick.AddListener(() => { PopDown(); });
    }
    public void VisiblePanel()
    {
        if (Popdown)
        {
            popUpObject.SetActive(false);
        }
        else
        {
            popUpObject.SetActive(true);
            colorToFadeTo = new Color(1f, 1f, 1f, 0f);
            myPanel.CrossFadeColor(colorToFadeTo, fadeTime, true, true);
            for (int i = 0; i < myText.Length; i++)
            {
                myText[i].CrossFadeColor(colorToFadeTo, fadeTime, true, true);
            }
        }
    }
    public void PopDown()
    {
        if (!Popdown)
        {
            Popdown = true;
        }
        else
        {
            Popdown = false;
        }
    }
}