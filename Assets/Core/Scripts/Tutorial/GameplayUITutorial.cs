using UnityEngine;
using System.Collections;

public class GameplayUITutorial : MonoBehaviour
{
    public PlayMenu playMenu;

    IEnumerator Start()
    {
        playMenu.backButton.interactable = false;

        yield return new WaitForSeconds(2);
        if (level == 0)
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
        if (level == 0)
            return;

        Debug.Log("level " + level);

        if (level == 2)
            playMenu.ShowPage(ServerMenuPages.ServersListPage);

        if (level == 4)
        {
            if (thereIsAServerToConnect)
                CommonUI.Instance.tutorial.Deactivate();
            else
                playMenu.ShowPage(ServerMenuPages.StartJoinServerPage);
        }
            

        if (level == 5 && !thereIsAServerToConnect)
            playMenu.ShowPage(ServerMenuPages.StartServerPage);

        Level++;
    }

    bool thereIsAServerToConnect;
    public void LevelChanged()
    {
        switch (level)
        {
            case 0:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_19_Server Menu");
                break;
            case 1:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_20_Server Menu");
                break;
            case 2:
                CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_21_Server Menu");
                break;
            case 3:
                GoToNextLevelAfter(0.25f);
                break;
            case 4:
                thereIsAServerToConnect = playMenu.ServersFound > 0;
                if (thereIsAServerToConnect)
                    CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_22_Server Menu");
                else
                    CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_24_Server Menu");
                break;
            case 5:
                if (thereIsAServerToConnect)
                {
                    //CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_22_Server Menu");

                }
                else
                    CommonUI.Instance.tutorial.LoadFrame("Phase_04_Step_25_Server Menu");
                break;

            case 6:
                CommonUI.Instance.tutorial.LoadFrameASide("Phase_04_Step_26_Server Menu", float.MaxValue);
                break;
            case 7:
                CommonUI.Instance.tutorial.Deactivate();
                // waiting to create the room
                break;
            case 8:
                CommonUI.Instance.tutorial.LoadFrameASide("Phase_04_Step_23_Server Menu_Re", 3);
                GoToNextLevelAfter(5);
                break;
            case 9:
                CommonUI.Instance.IsTutorial = false;
                break;
        }
    }

    void GoToNextLevelAfter(float wait)
    {
        StartCoroutine(_GoToNextLevelAfter(wait));
    }

    IEnumerator _GoToNextLevelAfter(float wait)
    {
        yield return new WaitForSeconds(wait);
        Level++;
    }


    void Update()
    {
        switch (level)
        {
            case 0:
                if (playMenu.PageIndex == 1)
                    Level = 1;
                break;
            case 5:
                if (thereIsAServerToConnect && !playMenu.myGameObject.activeSelf)
                    Level = 8;
                break;
            case 6:
                if (playMenu.loadingOverlayGameObject.activeSelf)
                    Level = 7;
                break;
            case 7:
                if (!playMenu.myGameObject.activeSelf)
                    Level = 8;
                break;
        }
    }
}
