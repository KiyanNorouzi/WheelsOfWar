using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class MainGarageUIController : SceneControllerBase
{

    #region Variables

    [Header("GeneraL Section")]
    public GameObject[] generalSections;           // History,Upgrade,Paint and part and Next Buttons

    [Header("Main UI Sections")]
    public GameObject mainSection;
    public GameObject upgradeSection;
    public GameObject paintAndPart;
    public GameObject historyPanel;

    [Header("Info Panels")]
    public GameObject carInfo;          // This variable included PopUp Objects
    public GameObject cosmeticInfoPanel;
    public GameObject upgradeInfoGameObject;
    public GameObject carControllerInfo;

    [Header("Other Sections")]
    public GameObject cosmeticContents;
    public GameObject onScrren;

    [Header("Camera Controller")]
    public GarageCameraMover cameraMover;
    public Animator cameraAnimator;  //This variable included camera animator 

    GeneralGarageStates generalGarageStates;   // Main States in Scene
    CarSelectionState selectionState;          // car states in inventory
    SideStates sideStates;

    int currentCarIndex
    {
        get
        {
            return MainGarageCarController.Instance.CurrentCarIndex;
        }
    }


    public float soundVolume;            //SFX for Select



    
    #endregion

    public static int sectionIndex = -1, partIndex = -1;
    /* section
     * 0: cosmetic
     * 1: upgrades
     */

    void Start()
    {
        if (sectionIndex != -1)
            Debug.Log("Load car " + sectionIndex + " and show part " + ((UpgradeParts)partIndex));
        else
            Debug.Log("Load default garage");


        if (sectionIndex == 0)
        {
            cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
            cameraMover.SetCameraAngle(250, GeneralGarageStates.UPGRADE);

            UpgradeOfferState(GeneralGarageStates.UPGRADE);
            cameraMover.autoRotateEnabled = false;
            cameraMover.touchAgent.enabled = false;
        }
        else
        {
            GeneralSectionDeactivator(false);
            mainSection.SetActive(true);
            carInfo.SetActive(true);
            carControllerInfo.SetActive(true);
        }

        CommonUI.Instance.menuMusicManager.SetVolume(0, soundVolume);
        MainGarageCosmeticController.Instance.DeactivateInfoTablet();
        

        Behaviour flarelayer = (Behaviour)Camera.main.GetComponent("FlareLayer");

        if (flarelayer != null)
            flarelayer.enabled = false;
    }


    public void MainSection()
    {
        MainActivator(true);
    }
    public void HistorySection()
    {
        generalGarageStates = GeneralGarageStates.HISTORY;
        SetState();
    }
    public void UpgradeSection()
    {
        generalGarageStates = GeneralGarageStates.UPGRADE;
        SetState();
    }
    public void PaintAndPart()
    {
        generalGarageStates = GeneralGarageStates.PAINT_PART;
        SetState();
    }

    public override void BackButton_Click()
    {
        CommonUI.Instance.menuMusicManager.SetVolume(0, 0);
        ButtonSelectedColorManager.Instance.DisableImageColor();

        if (generalGarageStates == GeneralGarageStates.MAIN)
        {
            SceneManager.LoadScene(Scenes.MainMenu);

        }
        else if (generalGarageStates == GeneralGarageStates.COSMETIC)
        {
            MainGarageCosmeticController.Instance.CurrentCameraIndex = 7;
            generalGarageStates = GeneralGarageStates.PAINT_PART;
            MainGarageCosmeticController.Instance.BackToMainCosmetics();

            UpgradeSliderController.Instance.ShowValues();
        }
        else
        {
            generalGarageStates = GeneralGarageStates.MAIN;
            UpgradeSliderController.Instance.ShowValues();
        }

        SetState();
    }

    public void CosmeticSelection(int _index)
    {
        switch (_index)
        {
            case 0:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;

            case 1:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;

            case 2:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;

            case 3:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;

            case 4:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;

            case 5:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;

            case 6:
                paintAndPart.SetActive(false);
                cosmeticContents.SetActive(true);
                cameraMover.SetCameraAngle(250, GeneralGarageStates.COSMETIC);
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                generalGarageStates = GeneralGarageStates.COSMETIC;
                break;
        }
    }


    UpgradeItem upgradeItemState;

    void SetState()
    {
        if (generalGarageStates == GeneralGarageStates.MAIN)
        {
            cameraMover.autoRotateEnabled = true;
            cameraMover.touchAgent.enabled = true;
            //cameraMover.SetCameraAngle(250, GeneralGarageStates.MAIN);
            //cameraMover.RefreshCameraPosition();

            cameraAnimator.SetInteger("CamMove", 7);
            GeneralSectionDeactivator(false);
            MainActivator(true);
            OnScreenActivator(true);
            CarInfoAvtivator(true);
            CarControllerInfoActivator(true);
        }
        else
        {
            OnScreenActivator(false);
            CosmeticInfoActivator(false);
            UpgradeInfoActivator(false);
            CarControllerInfoActivator(false);
        }


        switch (generalGarageStates)
        {
            case GeneralGarageStates.COSMETIC:
                cameraMover.autoRotateEnabled = false;
                cameraMover.touchAgent.enabled = false;
                GeneralSectionDeactivator(false);
                CosmeticContentsActivator(true);
                break;

            case GeneralGarageStates.PAINT_PART:
                cameraMover.autoRotateEnabled = true;
                cameraMover.touchAgent.enabled = true;
                GeneralSectionDeactivator(false);
                PaintAndPartActivator(true);

                //cameraMover.RefreshCameraPosition();
                break;

            case GeneralGarageStates.UPGRADE:
                cameraMover.SetPosition(new Vector3(-0.48f, -25.25f, -6.78f));
                //cameraMover.SetCameraAngle(250, GeneralGarageStates.UPGRADE);
                //cameraMover.RefreshCameraPosition();

                UpgradeController.Instance.Element_Button(0);
                ButtonSelectedColorManager.Instance.ChangeUpgradeImageColor_Button(0);


                cameraMover.autoRotateEnabled = false;
                cameraMover.touchAgent.enabled = false;
                GeneralSectionDeactivator(true);
                UpgradeActivator(true);
                break;

            case GeneralGarageStates.HISTORY:
                cameraMover.autoRotateEnabled = true;
                cameraMover.touchAgent.enabled = true;
                GeneralSectionDeactivator(false);
                HistoryActivator(true);
                CarInfoAvtivator(false);
                break;

            case GeneralGarageStates.CAR_INFO:
                GeneralSectionDeactivator(false);
                CarInfoAvtivator(true);
                break;
        }
    }

    public void UpgradeOfferState(GeneralGarageStates _generalGarageStates)
    {
        if (_generalGarageStates == GeneralGarageStates.UPGRADE && sectionIndex == 0)
        {
            generalGarageStates = GeneralGarageStates.UPGRADE;
            GeneralSectionDeactivator(false);
            carInfo.SetActive(true);
            UpgradeActivator(true);
            onScrren.SetActive(false);
        }
    }

    public void SelectButton_Clicked()
    {
        SceneManager.LoadScene(Scenes.MapSelect);
    }


    #region Activatior function and variabels

    void GeneralSectionDeactivator(bool _infoPanel)
    {
        for (int i = 0; i < generalSections.Length; i++)
        {
            generalSections[i].SetActive(false);
        }

        if (_infoPanel)
        {
            upgradeInfoGameObject.SetActive(true);
        }
        else
        {
            upgradeInfoGameObject.SetActive(false);
        }

    }

    void HistoryActivator(bool _active)
    {
        if (!_active)
        {
            historyPanel.SetActive(false);
        }
        else
        {
            historyPanel.SetActive(true);
        }
    }

    void PaintAndPartActivator(bool _active)
    {
        if (!_active)
        {
            paintAndPart.SetActive(false);
        }
        else
        {
            paintAndPart.SetActive(true);
        }
    }

    void CosmeticContentsActivator(bool _active)
    {
        if (!_active)
        {
            cosmeticContents.SetActive(false);
        }
        else
        {
            cosmeticContents.SetActive(true);
        }
    }

    void MainActivator(bool _active)
    {
        if (!_active)
        {
            mainSection.SetActive(false);
        }
        else
        {
            mainSection.SetActive(true);
        }
    }

    void UpgradeActivator(bool _active)
    {
        if (!_active)
        {
            upgradeSection.SetActive(false);
        }
        else
        {
            upgradeSection.SetActive(true);
        }
    }

    void CarInfoAvtivator(bool _active)
    {
        if (!_active)
        {
            carInfo.SetActive(false);
        }
        else
        {
            carInfo.SetActive(true);
        }
    }

    void CarControllerInfoActivator(bool _active)
    {
        if (!_active)
        {
            carControllerInfo.SetActive(false);
        }
        else
        {
            carControllerInfo.SetActive(true);
        }
    }

    void CosmeticInfoActivator(bool _active)
    {
        if (!_active)
        {
            cosmeticInfoPanel.SetActive(false);
        }
        else
        {
            cosmeticInfoPanel.SetActive(true);
        }
    }

    void UpgradeInfoActivator(bool _active)
    {
        if (!_active)
        {
            upgradeInfoGameObject.SetActive(false);
        }
        else
        {
            upgradeInfoGameObject.SetActive(true);
        }
    }

    void OnScreenActivator(bool _active)
    {
        if (!_active)
        {
            onScrren.SetActive(false);
        }
        else
        {
            onScrren.SetActive(true);
        }
    }

    #endregion

}

public enum GeneralGarageStates
{
    HISTORY,
    MAIN,
    UPGRADE,
    PAINT_PART,
    COSMETIC,
    CAR_INFO,
}

[System.Serializable]
public class ContentIndex
{
    public string contentName;
    public GameObject _gameObject;
    public int _index;
}

[System.Serializable]
public class AudioStr : AudioStructBase
{
    public AudioClip lockBreak;
    public AudioClip carFall;
}