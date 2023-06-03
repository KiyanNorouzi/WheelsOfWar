using UnityEngine;
using System.Collections;

public class MapSelectTutorial : MonoBehaviour
{
    public MapSelectController sceneController;

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
        if (level == 2)
        {
            //CommonUI.Instance.IsTutorial = false;
            sceneController.MapSelected(1);
        }
    }

    public void LevelChanged()
    {
        switch (level)
        {
            case 0:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_17_Map Select");
                break;
            case 1:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_18_Map Select");
                break;
            
        }
    }
}
