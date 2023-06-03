using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrGarageController : MonoBehaviour
{
    #region Singleton

    static ScrGarageController _instance;

    public static ScrGarageController Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (Information.Instance == null)
            SceneManager.LoadGame(Scenes.Garage);
        else
            _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion


    public static bool BackToMainMenuAfterSelect;

    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip lockBreak;
        public AudioClip carFall;
    }

    public AudioStruct audioPlayer;
    public float musicVolume;

    public GameObject[] machinesShow;
    public GameObject[] machinesLogo; //Logo variables

    //public Text carNameText, carNameTextFA;
    //public Text[] levelTexts;
    //public string[] levelTextFormat;
    //public Text lockPriceText;
    //public CoolDownWindow coolDownWindow;

    public Transform spawnPoint, unknownCarSpawnPoint;
    public GarageCameraMover cameraMover;

    public GameObject carHistoryPanelGameObject;
    public Text carHistoryText;
    public GameObject[] carHistoryFaTexts;
    public Image[] driverImages;

    public Image[] selectCarImage, buyText;

    //public UpgradeSection upgradeSection;
    public GameObject garageTutorial;
    //public GameObject editOverlay;

    public GameObject[] additionalUI;
    public GameObject previewButton, endPreviewButton;

    float maxSpeed, maxAcc, maxHandling, maxArmor;
    int currentCarLevel;
    int CurrentCarLevel
    {
        get { return currentCarLevel; }
        set
        {
            currentCarLevel = value;
        }
    }
    int lastCarIndex;
    int currentCarIndex;
    CarSelectionState selectionState;

    public int CurrentCarIndex
    {
        get { return currentCarIndex; }
        set
        {
            if (currentCarIndex >= 0 && currentCarIndex < machinesShow.Length && currentCarIndex < machinesLogo.Length)
            {
                machinesShow[currentCarIndex].SetActive(false);
                machinesLogo[currentCarIndex].SetActive(false); //this code set deactivated machinesLogo game object
            }

            lastCarIndex = currentCarIndex;
            currentCarIndex = value;
            if (currentCarIndex < 0)
                currentCarIndex = machinesShow.Length - 1;
            else if (currentCarIndex >= machinesShow.Length)
                currentCarIndex = 0;

            if (!Information.Instance.carInfo[currentCarIndex].isAvailable)
            {
                int diff = currentCarIndex - lastCarIndex;
                currentCarIndex += diff;

                if (currentCarIndex < 0)
                    currentCarIndex = machinesShow.Length - 1;
                else if (currentCarIndex >= machinesShow.Length)
                    currentCarIndex = 0;
            }



            machinesShow[currentCarIndex].SetActive(true);
            machinesLogo[currentCarIndex].SetActive(true); //this code set activated machinesLogo game object

            //carNameText.text = Information.Instance.carInfo[currentCarIndex].carName;
            //carNameTextFA.text = Information.Instance.carInfo[currentCarIndex].carFaName;
            //levelTexts[0].text = string.Format(levelTextFormat[0], level + 1);
            //levelTexts[1].text = string.Format(levelTextFormat[1], level + 1);

            int level = Data.GetCarLevel(currentCarIndex);
            level = Mathf.Clamp(level, 0, Information.Instance.carInfo[currentCarIndex].levels.Length);


            int classInex = (int)Information.Instance.carInfo[currentCarIndex].Class;

            float speed = _GetSpeed(currentCarIndex);
            float acc = _GetAcceleration(currentCarIndex);
            float handling = _GetHandling(currentCarIndex);
            float armor = _GetArmor(currentCarIndex);


            CurrentCarLevel = Data.GetCarLevel(Information.Instance.carInfo[currentCarIndex].carTag);
            bool currentCarIsLocked = (CurrentCarLevel == -1); // Data.IsCarLocked(currentCarIndex);

            if (Information.Instance.carInfo[currentCarIndex].levels[level].price.IsFree())
                currentCarIsLocked = false;


            if (Information.Instance.carInfo[currentCarIndex].commingSoon)
            {
                selectionState = CarSelectionState.NotAvailableYet;
                //SetInformation(currentCarIndex);

            }
            else
            {
                if (currentCarIsLocked)
                {
                    selectionState = CarSelectionState.Locked;

                    SetInformation(currentCarIndex); // this method return the information inculuded in currentCarIndex but not worked for per cars! 

                }


                else
                {
                    //int coolDownEndTime = Data.GetCarEndCoolDownTime(currentCarIndex);
                    //if (coolDownEndTime > 0)
                    //{
                    //    selectionState = CarSelectionState.InCoolDown;

                    //}
                    //else
                    //{
                    selectionState = CarSelectionState.Available;
                    SetInformation(currentCarIndex); // this method return the information inculuded in currentCarIndex but not worked for per cars! 

                    //}
                }
            }


            if (selectionState == CarSelectionState.NotAvailableYet)
            {
                machinesShow[currentCarIndex].transform.position = unknownCarSpawnPoint.position;
                machinesShow[currentCarIndex].transform.rotation = unknownCarSpawnPoint.rotation;
            }
            else
            {
                machinesShow[currentCarIndex].transform.position = spawnPoint.position;
                machinesShow[currentCarIndex].transform.rotation = spawnPoint.rotation;
            }

            //coolDownWindow.Deactivate();

            cameraMover.autoRotateEnabled = false;


            if (currentCarIsLocked)
            {
                //lockPriceText.text = MathHelper.GetStringWithComma(Information.Instance.carInfo[currentCarIndex].levels[level].price);
                for (int i = 0; i < selectCarImage.Length; i++)
                {
                    selectCarImage[i].enabled = false;
                    buyText[i].enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < selectCarImage.Length; i++)
                {
                    selectCarImage[i].enabled = true;
                    buyText[i].enabled = false;
                }
            }

            //upgradeSection.CarChanged(currentCarIndex);
        }
    }

    public void ShakeCamera()
    {
        cameraMover.Shake();
    }

    IEnumerator Start()
    {

        CommonUI.Instance.menuMusicManager.SetVolume(0, musicVolume);


        Inventory.Instance.Disable();

        //machinesShow[currentCarIndex].SetActive(true);
        //machinesShow[currentCarIndex].transform.position = spawnPoint.position;
        //machinesShow[currentCarIndex].transform.rotation = spawnPoint.rotation;

        for (int i = 0; i < machinesShow.Length; i++)
        {
            machinesShow[i].SetActive(false);

            maxSpeed = Mathf.Max(maxSpeed, _GetSpeed(i));
            maxAcc = Mathf.Max(maxAcc, _GetAcceleration(i));
            maxHandling = Mathf.Max(maxHandling, _GetHandling(i));
            maxArmor = Mathf.Max(maxArmor, _GetArmor(i));
        }

        previewButton.SetActive(true);
        endPreviewButton.SetActive(false);

        carHistoryPanelGameObject.SetActive(false);
        //coolDownWindow.OnCoolDownTimeFinished += coolDownWindow_OnCoolDownTimeFinished;

        //upgradeSection.OnUpgrade += upgradeSection_OnUpgrade;

        yield return new WaitForSeconds(0.1f);
        CurrentCarIndex = Accounting.Instance.currentUser.SelectedCarIndex;

        garageTutorial.SetActive(CommonUI.Instance.IsTutorial);
    }

    void upgradeSection_OnUpgrade()
    {
        CurrentCarIndex = currentCarIndex;
    }

    public void SetInformation(int _currentCarIndex)
    {
        //CarGarageStats.Instance.CarClassTypeFunction(_GetCarNameEN(_currentCarIndex), _GetCarNameFA(_currentCarIndex), _GetCarClas(_currentCarIndex));
        //CarGarageStats.Instance.SpeedInformationFunction(_GetSpeed(_currentCarIndex), _GetMinSpeed(), _GetMaxSpeed(),_GetSpeed(_currentCarIndex));
        //CarGarageStats.Instance.RifleInformationFunction(_GetRifleDPS(_currentCarIndex), _GetRifleNameEN(_currentCarIndex), _GetRifleNameFA(_currentCarIndex), _GetRifleMinDamage(_currentCarIndex), _GetRifleMaxDamage(_currentCarIndex), _GetRifleAccuracy(_currentCarIndex), _GetRifleFireDistance(_currentCarIndex), _GetRifleAccuracyMin(), _GetRifleAccuracyMax(), _GetRifleFireRate(_currentCarIndex));
        //CarGarageStats.Instance.MineInformationFunction(_GetMineDPS(_currentCarIndex), _GetMineNameEN(_currentCarIndex), _GetMineNameFA(_currentCarIndex), _GetMineMinDamage(_currentCarIndex), _GetMineMaxDamage(_currentCarIndex), _GetMineCoolDown(_currentCarIndex), _GetMineExplosionArea(_currentCarIndex), _GetMineMinExplosionArea(), _GetMineMaxExplosionArea());
        //CarGarageStats.Instance.RocketInformationFunction(_GetRocketDPS(_currentCarIndex), _GetRocketNameEN(_currentCarIndex), _GetRocketNameFA(_currentCarIndex), _GetRocketMinDamage(_currentCarIndex), _GetRocketMaxDamage(_currentCarIndex), _GetrocketFireRate(_currentCarIndex), _GetRocketDistance(_currentCarIndex), _GetRocketExplosionArea(_currentCarIndex), _GetRocketMinExplosionArea(), _GetRocketMaxExplosionArea());
        //CarGarageStats.Instance.ArmorInformationFunction(_GetArmor(_currentCarIndex), _GetArmorNameEN(_currentCarIndex), _GetArmorNameFA(_currentCarIndex), _GetArmorBlockingDamage(_currentCarIndex));
        //CarGarageStats.Instance.HealthInformationFunction(_GetHealthValue(_currentCarIndex), _GetMinHealth(), _GetMaxHealth(), _GetClassShield(_currentCarIndex));
    }

    #region Stat Get Function

    //these functions get stats from instance of Information class

    #region Main Car Information

    string _GetCarNameEN(int _index)
    {
        return Information.Instance.carInfo[_index].carName;
    }

    string _GetCarNameFA(int _index)
    {
        return Information.Instance.carInfo[_index].carFaName;
    }

    string _GetCarClas(int _index)
    {
        return Information.Instance.carInfo[_index].Class.ToString();
    }

    #endregion

    #region Speed Get Function

    ////////////////////////////////Speed //////////////////////////////////////////////

    float _GetSpeed(int index)
    {
        return Information.Instance.carInfo[index].levels[0].speed;
    }


    float _GetMinSpeed()
    {
        return CarGarageStats.Instance.minSpeedRange;
    }


    float _GetMaxSpeed()
    {
        return CarGarageStats.Instance.maxSpeedRange;
    }


    #endregion


    #region Rifle Get Functions

    ////////////////////////////////Rifle //////////////////////////////////////////////
    float _GetRifleDPS(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.DPS;
    }

    string _GetRifleNameEN(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.nameEN;
    }

    string _GetRifleNameFA(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.nameFA;
    }

    float _GetRifleMinDamage(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.damageMin;
    }

    float _GetRifleMaxDamage(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.damageMax;
    }

    float _GetRifleFireRate(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.fireRate;
    }

    float _GetRifleAccuracy(int _index)
    {

        return Information.Instance.carInfo[_index].machineGunInfo.tolerance;
    }

    float _GetRifleFireDistance(int _index)
    {
        return Information.Instance.carInfo[_index].machineGunInfo.range;
    }

    float _GetRifleAccuracyMin()
    {
        return CarGarageStats.Instance.rifleAccuracyMin;
    }

    float _GetRifleAccuracyMax()
    {
        return CarGarageStats.Instance.rifleAccuracyMax;
    }



    #endregion


    #region Mine Get Functions

    ///////////////////////////////Mine/////////////////////////////////////////
    float _GetMineDPS(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.DPS;
    }

    string _GetMineNameEN(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.nameEN;
    }

    string _GetMineNameFA(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.nameFA;
    }

    float _GetMineMinDamage(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.damageMin;
    }

    float _GetMineMaxDamage(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.damageMax;
    }

    float _GetMineCoolDown(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.coolDownTime;
    }

    float _GetMineExplosionArea(int _index)
    {
        return Information.Instance.carInfo[_index].minerInfo.explosionArea;
    }

    float _GetMineMinExplosionArea()
    {
        return CarGarageStats.Instance.mineMinExplosionArea;
    }

    float _GetMineMaxExplosionArea()
    {
        return CarGarageStats.Instance.mineMaxExplosionArea;
    }


    #endregion


    #region Rocket Get Functions

    ///////////////////////////////Rocket/////////////////////////////////////////
    float _GetRocketDPS(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.DPS;
    }

    string _GetRocketNameEN(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.nameEN;
    }

    string _GetRocketNameFA(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.nameFA;
    }

    float _GetRocketMinDamage(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.damageMin;
    }

    float _GetRocketMaxDamage(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.damageMax;
    }

    float _GetRocketCoolDown(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.coolDownTime;
    }

    float _GetRocketDistance(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.Range;
    }

    float _GetrocketFireRate(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.coolDownTime;
    }

    float _GetRocketExplosionArea(int _index)
    {
        return Information.Instance.carInfo[_index].rocketLauncherInfo.explosionArea;
    }

    float _GetRocketMinExplosionArea()
    {
        return CarGarageStats.Instance.rocketMinExplosionArea;
    }

    float _GetRocketMaxExplosionArea()
    {
        return CarGarageStats.Instance.rocketMaxExplosionArea;
    }


    #endregion


    #region Armor Get Function

    ////////////////////////////////Armor//////////////////////////////////////
    float _GetArmor(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].armor;
    }

    string _GetArmorNameEN(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].armorNameEN;
    }


    string _GetArmorNameFA(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].armorNameFA;
    }


    float _GetArmorBlockingDamage(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].blockingDamage;
    }


    #endregion


    #region Health Get Function

    ////////////////////////////////Health//////////////////////////////////////
    float _GetHealthValue(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].health;
    }

    float _GetMinHealth()
    {
        return CarGarageStats.Instance.minHealth;
    }

    float _GetMaxHealth()
    {
        return CarGarageStats.Instance.maxHealth;
    }

    string _GetClassShield(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].classShield;
    }

    #endregion


    ////////////////////////////////////////////////////////////

    #endregion

    float _GetHandling(int index)
    {
        return Information.Instance.carInfo[index].levels[0].handling;
    }

    float _GetAcceleration(int index)
    {
        return Information.Instance.carInfo[index].levels[0].attack;
    }

    public void NextMachine()
    {
        CommonUI.Instance.PlayButtonClick();
        CurrentCarIndex++;
    }

    public void PreviousMachine()
    {
        CommonUI.Instance.PlayButtonClick();
        CurrentCarIndex--;
    }

    int moneyToCoolDownFaster;
    public void SelectButton_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();

        switch (selectionState)
        {
            case CarSelectionState.Available:
                Accounting.Instance.currentUser.SelectedCarIndex = currentCarIndex;

                if (BackToMainMenuAfterSelect)
                    SceneManager.LoadScene(Scenes.MainMenu);
                else
                    SceneManager.LoadScene(Scenes.MapSelect);
                break;

            case CarSelectionState.Locked:
                //Accounting.Instance.currentUser.Buy(Information.Instance.carInfo[currentCarIndex].levels[0].price, BuyConfirmed);
                Debug.Log("nuy bull");
                break;
            //case CarSelectionState.InCoolDown:
            //    moneyToCoolDownFaster = (int)(Data.GetCarEndCoolDownTime(currentCarIndex) * ScoreSettings.Instance.CoolDowntimeToMoneyConvertRate);
            //    CommonUI.Instance.question.Ask(Messages.CarIsCoolingDown, _PayToCoolDownRightAway, null, false, moneyToCoolDownFaster.ToString());
            //    break;
            case CarSelectionState.NotAvailableYet:
                CommonUI.Instance.messageBox.ShowMessage(Messages.CarIsNotAvailable, null, true);
                break;
        }
    }

    public void EditButton_clicked()
    {
        switch (selectionState)
        {
            case CarSelectionState.Available:
                //editOverlay.SetActive(true);
                SceneManager.LoadScene(Scenes.Store);
                break;


            case CarSelectionState.Locked:
                //editOverlay.SetActive(false);
                break;


            case CarSelectionState.NotAvailableYet:
                //editOverlay.SetActive(false);
                break;

        }

        Debug.Log("Edit");
    }

    /*private void _PayToCoolDownRightAway()
    {
        if (MoneyTablet.Instance.Coins >= moneyToCoolDownFaster)
        {
            MoneyTablet.Instance.Add(-moneyToCoolDownFaster);
            Data.CancelCoolDownForCar(currentCarIndex);
            //coolDownWindow.Deactivate();

            for (int i = 0; i < selectCarImage.Length; i++)
            {
                selectCarImage[i].enabled = true;
                buyText[i].enabled = false;
            }

            selectionState = CarSelectionState.Available;
            //audioPlayer.Play(audioPlayer.lockBreak);
        }
        else
            CommonUI.Instance.buyCoinsMenu.Activate();
    }

    /*
    void BuyConfirmed()
    {
        CurrentCarLevel++;

        //Data.Money -= Information.Instance.carInfo[currentCarIndex].levels[0].price;
        //MoneyTablet.Instance.Add(-Information.Instance.carInfo[currentCarIndex].levels[0].price);
        Data.SetCarLevel(Information.Instance.carInfo[currentCarIndex].carTag, currentCarLevel);


        for (int i = 0; i < selectCarImage.Length; i++)
        {
            selectCarImage[i].enabled = true;
            buyText[i].enabled = false;
        }

        selectionState = CarSelectionState.Available;

        audioPlayer.Play(audioPlayer.lockBreak);
        Accounting.Instance.currentUser.BuyCar(Information.Instance.carInfo[currentCarIndex].carTag);
    }*/

    void coolDownWindow_OnCoolDownTimeFinished()
    {
        selectionState = CarSelectionState.Available;
    }

    void BuyCash()
    {
        CommonUI.Instance.buyCoinsMenu.Activate();
    }

    public void BackButton_Clicked()
    {
        CommonUI.Instance.menuMusicManager.SetVolume(0, 0);

        CommonUI.Instance.PlayButtonClick();
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void CarModelLoaded(ScrInGarageCar car)
    {
        ShakeCamera();
        cameraMover.autoRotateEnabled = true;

        switch (selectionState)
        {
            case CarSelectionState.Available:
                break;
            case CarSelectionState.Locked:
                break;
            case CarSelectionState.NotAvailableYet:

                break;
        }
    }

    public void PreviewButton()
    {
        CommonUI.Instance.PlayButtonClick();

        bool previewMode = additionalUI[0].activeInHierarchy;
        previewMode = !previewMode;

        for (int i = 0; i < additionalUI.Length; i++)
            additionalUI[i].SetActive(previewMode);


        previewButton.SetActive(previewMode);
        endPreviewButton.SetActive(!previewMode);

        if (!previewMode)
        {
            carHistoryText.text = Information.Instance.carInfo[currentCarIndex].carHistory;
            for (int i = 0; i < carHistoryFaTexts.Length; i++)
                carHistoryFaTexts[i].SetActive(i == currentCarIndex);

            for (int i = 0; i < driverImages.Length; i++)
                driverImages[i].enabled = i == currentCarIndex;

            //carHistoryFAText.text = Information.Instance.carInfo[currentCarIndex].carHistoryFA;

            carHistoryPanelGameObject.SetActive(true);
        }
        else
            carHistoryPanelGameObject.SetActive(false);
    }
}