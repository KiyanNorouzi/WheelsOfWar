using UnityEngine;
using System.Collections;

public class TutorialStartScene : MonoBehaviour 
{
    public static bool ServerError;
    public GameObject retryButtonGameObject, yesNoLayout;

	IEnumerator Start()
    {
        if (CommonUI.Instance == null)
            SceneManager.LoadGame(Scenes.TutorialStartScene);

        if (ServerError)
        {
            CommonUI.Instance.tutorial.LoadFrame("ServerError");
            retryButtonGameObject.SetActive(true);
        }
        else
        {
            CommonUI.Instance.tutorial.LoadFrame("Phase_01_Step_00_Main menu");
            retryButtonGameObject.SetActive(false);
        }

        yesNoLayout.SetActive(false);
        CommonUI.Instance.headerBar.Disable();

        yield return new WaitForSeconds(2);

        CommonUI.Instance.IsTutorial = true;
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
        if (level == 2)
            return;

        Level++;
        if (level == 4)
        {
            SceneManager.LoadMap(Maps.Tutorial);
        }
    }

    public void LevelChanged()
    {
        switch (level)
        {
            case 0:
                CommonUI.Instance.tutorial.LoadFrame("Phase_01_Step_01_Main menu");
                break;
            case 1:
                CommonUI.Instance.tutorial.LoadFrame("Phase_01_Step_02_Main menu");
                break;
            case 2:
                yesNoLayout.SetActive(true);
                CommonUI.Instance.tutorial.LoadFrame("Phase_01_Step_02.5_Main menu");
                break;
            case 3:
                
                yesNoLayout.SetActive(false);
                CommonUI.Instance.tutorial.LoadFrame("Phase_01_Step_03_Main menu");
                break;
        }
    }

    public void YesButton_Click()
    {
        CommonUI.Instance.tutorial.Deactivate();

        yesNoLayout.SetActive(false);
        StartCoroutine(_nextLevelIn(0.1f));
    }

    IEnumerator _nextLevelIn(float p)
    {
        yield return new WaitForSeconds(p);
        Level++;
    }

    public void NoButton_Click()
    {
        CommonUI.Instance.tutorial.Deactivate();
        yesNoLayout.SetActive(false);
        CommonUI.Instance.messageBox.Ask(Messages.SureToQuitTutorial, _quit, _quitCancelled, true);
    }

    void _quit()
    {
        CommonUI.Instance.tutorial.Deactivate();
        CommonUI.Instance.IsTutorial = false;

        SceneManager.LoadScene(Scenes.MainMenu);
    }

    void _quitCancelled()
    {
        Level = 2;
    }


    public void RetryButton_Click()
    {
        SceneManager.LoadMap(Maps.Tutorial);
    }

    public void SkipButton_Click()
    {
        CommonUI.Instance.tutorial.Deactivate();


        CommonUI.Instance.IsTutorial = false;
        PlayerPrefs.SetInt("skiptutorial", 1);

        CommonUI.Instance.IsTutorial = false;
        SceneManager.LoadScene(Scenes.MainMenu);
    }
}