using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour 
{
    public GameObject myGameObject;
    public Toggle vibration, mute;
    public Slider sfxSlider, musicSlider;
    public Toggle accelerometer, joystick;
    public Slider accelerometerSensitivitySlider, joystickSensitivitySlider;

    public Slider mtuSlider;
    public Text mtuText;

    void Start()
    {
    }

    public void SFXSlider_ValueChanged()
    {
        if (initializing)
            return;

        SettingData.SFXVolume = sfxSlider.value;
    }

    public void MusicSlider_ValueChanged()
    {
        if (initializing)
            return;

        SettingData.MusicVolume = musicSlider.value;
        AudioListener.volume = SettingData.MusicVolume;

        SettingData.IsMute = mute.isOn = false;
    }

	public void VibrationButton_Clicked()
    {
        if (initializing)
            return;

        
        SettingData.IsViberationOn = vibration.isOn;

        CommonUI.Instance.PlayButtonClick();
    }

    public void MuteButton_Clicked()
    {
        if (initializing)
            return;

        SettingData.IsMute = mute.isOn;
        CommonUI.Instance.PlayButtonClick();
    }

    bool initializing;
    public void Activate()
    {
        initializing = true;

        //sfxSlider.value = SettingData.SFXVolume;
        musicSlider.value = SettingData.MusicVolume;
        vibration.isOn = SettingData.IsViberationOn;
        mute.isOn = SettingData.IsMute;

        


        mtuSlider.value = PhotonNetwork.networkingPeer.MaximumTransferUnit;
        mtuText.text = "N/A"; // PhotonNetwork.networkingPeer.MaximumTransferUnit.ToString();
        initializing = false;


        Debug.Log("input method=" + Data.InputMethod);

        accelerometerSensitivitySlider.value = SettingData.AccelerometerControlSensivity;
        joystickSensitivitySlider.value = SettingData.JoystickControlSensivity;


        myGameObject.SetActive(true);


        if (Data.InputMethod == 0)
        {
            Debug.Log("here 0");

            accelerometer.isOn = true;
            joystick.isOn = false;
        }
        else
        {
            Debug.Log("here 1");

            accelerometer.isOn = false;
            joystick.isOn = true;
        }
    }

    void Update()
    {
        mtuText.text = "N/A";// mtuSlider.value.ToString();
    }

    public void CloseButton_Clicked()
    {
      //  PhotonNetwork.networkingPeer.MaximumTransferUnit = (int)mtuSlider.value;
      //  Debug.Log("MTU=" + PhotonNetwork.networkingPeer.MaximumTransferUnit);

        CommonUI.Instance.PlayButtonClick();
        myGameObject.SetActive(false);
    }

    public void Accelerometer_Click()
    {
        //accelerometer.isOn = true;
        joystick.isOn = false;
        Data.InputMethod = 0;
    }

    public void Joystick_Click()
    {
        accelerometer.isOn = false;
        //joystick.isOn = true;
        Data.InputMethod = 1;
    }

    public void AccelerometerSesitivity_ValueChanged()
    {
        SettingData.AccelerometerControlSensivity = accelerometerSensitivitySlider.value;
        joystickSensitivitySlider.value = SettingData.JoystickControlSensivity;
    }

    public void JoystickSesitivity_ValueChanged()
    {
        SettingData.JoystickControlSensivity = joystickSensitivitySlider.value;
    }
}
