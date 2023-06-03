using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameAnalyticsSDK;

public class MapSelectController : SceneControllerBase 
{
    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip selected;
        public AudioClip nextprevButton;
    }

    public float musicVolume;
    public AudioStruct audioPlayer;

    public Animator buttonsAnimator;
    public Button prevButton, nextButton;
    public MapButton[] mapButtons;


    public GameObject uiButtonsGameObject;
    public GameObject mapSelectTutorial;

	public Button teamButton;
	public Button FFaButton;
	public Image teamButtonImage;
	public Image FFaButtonImage;
    public Color EnableColor; 

    int index;

    void Start()
    {
		SetGameToFFA ();

        if (CommonUI.Instance == null)
            SceneManager.LoadGame(Scenes.MapSelect);

        //Inventory.Instance.Disable();

        index = 1;
        
        prevButton.interactable = false;
        nextButton.interactable = true;

        CommonUI.Instance.menuMusicManager.SetVolume(0, musicVolume);

        mapSelectTutorial.SetActive(CommonUI.Instance.IsTutorial);

        for (int i = 0; i < mapButtons.Length; i++)
        {
            mapButtons[i].SetLock(Accounting.Instance.currentUser.Level < EnvironmentSpecificSettings.Instance.settings[i].unlockLevel, EnvironmentSpecificSettings.Instance.settings[i].unlockLevel,
                EnvironmentSpecificSettings.Instance.settings[i].gasConsume);
        }
    }

    
    public void MapSelected(int index)
    {
        if (Accounting.Instance.currentUser.Level < EnvironmentSpecificSettings.Instance.settings[index].unlockLevel)
        {
            CommonUI.Instance.messageBox.ShowMessage(Messages.MapLocked, null, true, EnvironmentSpecificSettings.Instance.settings[index].unlockLevel.ToString());
            return;
        }

        this.index = index;
        PlayButton_Click();


        GameAnalytics.NewDesignEvent("Map" + index.ToString() + "Played number is:", 1f);
    }

	public void SetGameToTeam(){
		teamButtonImage.color = EnableColor;
		FFaButtonImage.color = Color.white;
		GameplayDefaultSettings.Instance.isTeamMatch = true;

	}

	public void SetGameToFFA(){
		teamButtonImage.color = Color.white;
		FFaButtonImage.color = EnableColor;
		GameplayDefaultSettings.Instance.isTeamMatch = false;
	}

	public void PlayButton_Click()
    {
        if (!CommonUI.Instance.IsTutorial && Accounting.Instance.currentUser.Level < EnvironmentSpecificSettings.Instance.settings[index - 1].unlockLevel)
        {
            CommonUI.Instance.messageBox.ShowMessage(Messages.MapLocked, null, true, EnvironmentSpecificSettings.Instance.settings[index - 1].unlockLevel.ToString());
            return;
        }




        if (!CommonUI.Instance.IsTutorial && Accounting.Instance.currentUser.Gas < EnvironmentSpecificSettings.Instance.settings[index - 1].gasConsume)
        {
            CommonUI.Instance.messageBox.ShowMessage(Messages.NotEnoughGas, null, true, EnvironmentSpecificSettings.Instance.settings[index - 1].gasConsume.ToString());
            return;
        }

        /*if (index == 2)
        {
            CommonUI.Instance.question.ShowMessage(Messages.MapNotAvailableYet, null, true);
        }
        else*/
        {
            uiButtonsGameObject.SetActive(false);
            StartCoroutine(LoadMapAfter(1.1f, index));
        }
    }

    IEnumerator LoadMapAfter(float wait, int index)
    {
        CommonUI.Instance.menuMusicManager.SetVolume(0, 0, 0.5f);
        yield return new WaitForSeconds(0.2f);

        if (index == 0)
            mapButtons[0].animator.SetTrigger("select");
        else
            mapButtons[index - 1].animator.SetTrigger("select");

        audioPlayer.Play(audioPlayer.selected);

        yield return new WaitForSeconds(wait);
        SceneManager.LoadMap((Maps)index);
    }

    public override void BackButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();
        SceneManager.LoadScene(Scenes.Garage);
    }

    public void Prev_Click()
    {
        audioPlayer.Play(audioPlayer.nextprevButton);
        if (index == 2)
        {
            index = 1;
            buttonsAnimator.SetInteger("map", index);

            prevButton.interactable = false;
            nextButton.interactable = true;
        }
    }

    public void Next_Click()
    {
        audioPlayer.Play(audioPlayer.nextprevButton);
        if (index == 1)
        {
            index = 2;
            buttonsAnimator.SetInteger("map", index);

            nextButton.interactable = false;
            prevButton.interactable = true;
        }
    }
}
