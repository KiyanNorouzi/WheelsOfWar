using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour 
{
    public GameObject rootGameObject;
    public Toggle[] muteToggles;
    public Button resumeButton;

    public Image[] inputMethodImages;
    public Button[] respawnButtons;
    public Slider accelerometerSensivitySlider, joystickSensivitySlider;
    public Text roomNameText;


    void Start()
    {
        isRespawnButtonEnable = true;

        int selectedInputMode = (int)GameplayUI.Instance.ControlLayout;

        for (int i = 0; i < inputMethodImages.Length; i++)
            inputMethodImages[i].enabled = (i == selectedInputMode);

        Activate();
    }
    
    public void ResumeButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();
        GameplayUI.Instance.SetHUDActive(true);
        Deactivate();
    }

    public GameObject[] pagesGameObject;
    public void ControlButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        pagesGameObject[0].SetActive(false);
        pagesGameObject[1].SetActive(true);
    }

	public void RespawnButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();
        GameplayUI.Instance.SetHUDActive(true);
        if (ScrCarController.Instance == null)
            GameplayUI.Instance.SpawnPlayer();
        else
            GameplayUI.Instance.RespawnTheCarFromPauseMenu();
            //ScrCarController.Instance.Respawn();
        
        Deactivate();
    }

    public void BackToMainMenu_Click()
    {
        CommonUI.Instance.PlayButtonClick();
        CommonUI.Instance.messageBox.Ask(Messages.Quit, _BackToMainMenu, null, true);
    }

    void _BackToMainMenu()
    {
        CommonUI.Instance.PlayButtonClick();

        //GameplayUI.Instance.newsWall.SubmitText("* " + Data.UserName, ExtraSigns.Left);
        ScrCarController.Instance.LeaveRoom();

        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void MusicButton_Click(bool enabled)
    {
        CommonUI.Instance.PlayButtonClick();

        bool isMute = false;
        if (SettingData.LanguageIndex == 0)
        {
            isMute = muteToggles[1].isOn = muteToggles[0].isOn;
        }
        else if (SettingData.LanguageIndex == 1)
        {
            isMute = muteToggles[0].isOn = muteToggles[1].isOn;
        }

        SettingData.IsMute = isMute;
    }

    public void InputMethod_Click(int index)
    {
        CommonUI.Instance.PlayButtonClick();

        if (inputMethodImages[index].enabled)
            return;

        switch (index)
        {
            case 0: GameplayUI.Instance.ControlLayout = ControlLayout.Joystick; break;
            case 1: GameplayUI.Instance.ControlLayout = ControlLayout.Accelerometer; break;
        }

        for (int i = 0; i < inputMethodImages.Length; i++)
            inputMethodImages[i].enabled = (i == index);

        //GameplayUI.Instance.SetHUDActive(true);
        //Deactivate();
    }

    public void Activate()
    {
        if (PhotonNetwork.room != null)
            roomNameText.text = string.Format("[{0}]", PhotonNetwork.room.name);
        else
            roomNameText.text = "[-]";

        if (ScrCarController.Instance == null)
        {
            resumeButton.interactable = false;
        }
        else
        {
            resumeButton.interactable = true;
        }

        muteToggles[0].isOn = muteToggles[1].isOn = SettingData.IsMute; // (AudioListener.volume == 0);

        accelerometerSensivitySlider.value = SettingData.AccelerometerControlSensivity;
        joystickSensivitySlider.value = SettingData.JoystickControlSensivity;

        GameplayUI.Instance.SetHUDActive(false);

        pagesGameObject[0].SetActive(true);
        pagesGameObject[1].SetActive(false);

        rootGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        rootGameObject.SetActive(false);
    }

    bool isRespawnButtonEnable;
    void Update()
    {
        if (ScrCarController.Instance != null)
        {
            if (isRespawnButtonEnable != ScrCarController.Instance.IsAlive)
            {
                isRespawnButtonEnable = ScrCarController.Instance.IsAlive;
                for (int i = 0; i < respawnButtons.Length; i++)
                    respawnButtons[i].interactable = isRespawnButtonEnable;
            }
        }

        //onlineUsersText.text = "count of players: " + PhotonNetwork.countOfPlayers + ", players in rooms:" + PhotonNetwork.countOfPlayersInRooms +
        //      ", players on master:" + PhotonNetwork.countOfPlayersOnMaster + ", rooms=" + PhotonNetwork.countOfRooms;
    }

    public void AccelerometerControlSensivitySlider_ValueChanged()
    {
        SettingData.AccelerometerControlSensivity = Mathf.Clamp01(accelerometerSensivitySlider.value);
    }

    public void JoystickControlSensivitySlider_ValueChanged()
    {
        SettingData.JoystickControlSensivity= Mathf.Clamp01(joystickSensivitySlider.value);
    }
}