using UnityEngine;
using System.Collections;

public class MainMenuTutorial : MonoBehaviour 
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Level = 0;
    }

    void OnEnable()
    {
        CommonUI.Instance.tutorial.OnFramePassed += tutorial_OnFramePassed;
    }

    void OnDisable()
    {
        CommonUI.Instance.tutorial.OnFramePassed -= tutorial_OnFramePassed;
    }

    int level;
    int Level
    {
        get { return level; }
        set 
        { 
            level = value;
            LevelChanged();
        }
    }


    void tutorial_OnFramePassed(string frameTag)
    {
        Level++;
        if (level == 5)
        {
            SceneManager.LoadScene(Scenes.Garage);
        }
    }

    public void LevelChanged()
    {
        switch (level)
        {
            case 0:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_01_Main menu");
                break;
            case 1:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_02_Main menu");
                break;
            case 2:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_03_Main menu");
                break;
            case 3:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_04_Main menu");
                break;
            case 4:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_05_Main menu");
                break;
        }
    }
}