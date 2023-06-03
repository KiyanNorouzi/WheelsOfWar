using UnityEngine;
using System.Collections;

public class GarageTutorial : MonoBehaviour 
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
        if (level == 12)
        {
            SceneManager.LoadScene(Scenes.MapSelect);
        }
    }

    public void LevelChanged()
    {
        switch (level)
        {
            case 0:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_06_Garaj");
                break;
            case 1:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_07_Garaj");
                break;
            case 2:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_08_Garaj");
                break;
            case 3:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_09_Garaj");
                break;
            case 4:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_10_Garaj");
                break;
            case 5:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_11_Garaj");
                break;
            case 6:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_12_Garaj");
                break;
            case 7:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_13_Garaj");
                break;
            case 8:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_14_Garaj");
                break;
            case 9:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_15_Garaj");
                break;
            case 10:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_16_Garaj");
                break;
            case 11:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_17_Garaj");
                break;
        }
    }

}
