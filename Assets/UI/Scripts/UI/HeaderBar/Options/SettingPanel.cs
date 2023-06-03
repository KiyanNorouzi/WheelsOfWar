using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingPanel : OptionContentPanel
{
    public Slider joystickSlider, gyroscopeSlider;
    public Image joystickImage, gyroscopeImage;
    public Color selectedColor, unselectedColor;
    public Color buttonsSelectedColor, buttonsUnselectedColor;
    public Image[] viberationImages, soundImages, vfxImages, qualityImages;
    public Text versionText;


	void Start(){
		SettingData.TextureQuality = SettingData.TextureQuality;
		SettingData.IsViberationOn = SettingData.IsViberationOn;
		SettingData.IsMute = SettingData.IsMute;
	}

    public override void Activate()
    {
        versionText.text = string.Concat("v", CommonUI.Instance.appVersion);
        _SetInputMethodImages();
        _SetTwoStateButtonImages(viberationImages, SettingData.IsViberationOn);
        _SetTwoStateButtonImages(soundImages, !SettingData.IsMute);
        _SetTwoStateButtonImages(vfxImages, SettingData.VFX);
        _SetTwoStateButtonImages(qualityImages, SettingData.TextureQuality);


        base.Activate();
    }


    public void JoystickButton_Click()
    {
        Data.InputMethod = 1;
        _SetInputMethodImages();
    }

    public void GyroscopeButton_Click()
    {
        Data.InputMethod = 0;
        _SetInputMethodImages();
    }

    private void _SetInputMethodImages()
    {
        if (Data.InputMethod == 0)
        {
            gyroscopeImage.color = selectedColor;
            joystickImage.color = unselectedColor;
        }
        else if (Data.InputMethod == 1)
        {
            gyroscopeImage.color = unselectedColor;
            joystickImage.color = selectedColor;
        }
    }

    public void JoystickSlider_ValueChange()
    {
        SettingData.JoystickControlSensivity = joystickSlider.value;
    }

    public void GyroscopeSlider_ValueChange()
    {
        SettingData.AccelerometerControlSensivity = gyroscopeSlider.value;
    }



    public void VibrationButton_Click()
    {
        SettingData.IsViberationOn = !SettingData.IsViberationOn;
        CommonUI.Instance.PlayButtonClick();

        _SetTwoStateButtonImages(viberationImages, SettingData.IsViberationOn);
    }

    public void SoundButton_Click()
    {
        SettingData.IsMute = !SettingData.IsMute;
        CommonUI.Instance.PlayButtonClick();

        _SetTwoStateButtonImages(soundImages, !SettingData.IsMute);
    }

    public void VFXButton_Click()
    {
        SettingData.VFX = !SettingData.VFX;
        CommonUI.Instance.PlayButtonClick();

        _SetTwoStateButtonImages(vfxImages, SettingData.VFX);
    }

    public void QualityButton_Click()
    {
        SettingData.TextureQuality = !SettingData.TextureQuality;
        CommonUI.Instance.PlayButtonClick();

        _SetTwoStateButtonImages(qualityImages, SettingData.TextureQuality);
    }

    private void _SetTwoStateButtonImages(Image[] images, bool isOn)
    {
        if (isOn)
        {
            images[0].color = buttonsSelectedColor;
            images[1].color = buttonsUnselectedColor;
        }
        else
        {
            images[0].color = buttonsUnselectedColor;
            images[1].color = buttonsSelectedColor;
        }
    }
}