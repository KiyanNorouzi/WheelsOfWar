using UnityEngine;
using System.Collections;

public class ScrServersMenuController : MonoBehaviour 
{
    public GameObject[] layouts;
    public GUIText serverNameLabel;

    int layoutIndex = 0;
    public int LayoutIndex
    {
        get { return layoutIndex; }
        set 
        { 
            layoutIndex = value;
            for (int i = 0; i < layouts.Length; i++)
                layouts[i].SetActive(i == layoutIndex);
        }
    }

    public void BackButton_Clicked()
    {
        switch (layoutIndex)
        {
            case 0:
                SceneManager.LoadPreviousScene();
                break;
            case 1:
                LayoutIndex = 0;
                break;
            case 2:
            case 3:
                LayoutIndex = 1;
                break;
        }

        
    }


    public void Map0_Clicked()
    {
        MapClicked(0);
    }

    public void Map1_Clicked()
    {
        MapClicked(1);
    }

    void MapClicked(int mapIndex)
    {
        Debug.Log("map clicked=" + mapIndex);
        LayoutIndex = 1;
    }


    public void StartServerButton_Clicked()
    {
        LayoutIndex = 2;
    }

    public void JoinServerButton_Clicked()
    {
        LayoutIndex = 3;
    }

    public void CreateServer_Clicked()
    {
        Debug.Log("creating server. name=" + serverNameLabel.text);

        ScrGarageController.BackToMainMenuAfterSelect = false;
        SceneManager.LoadScene(Scenes.Garage);
    }

    public void JoinServer_Clicked()
    {
        Debug.Log("join server");

        ScrGarageController.BackToMainMenuAfterSelect = false;
        SceneManager.LoadScene(Scenes.Garage);
    }
}
