using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class MainGarageCarController : MonoBehaviour
{

    #region Singleton

    static MainGarageCarController _instance;

    public static MainGarageCarController Instance
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

    #region Variables

    [System.Serializable]
    public class AudioStructure : AudioStructBase
    {
        public AudioClip lockBreak;
        public AudioClip carFall;
    }

    public AudioStructure audioPlayer;
    public float musicVolume;


    [Header("Machine Gameobjects")]
    public GameObject[] machinesShow;
    public GameObject[] machinesLogo;

    [Space(20)]
    public Transform spawnPoint;

    public GameObject[] historyImage;
    public GameObject[] historyText_FA;
    public Text historyText_EN;
   


    [Header("Car Store States")]
    public Text machineName;
    public Text machinePrice;
    public Text machineLevel;

    public GameObject commingSoon;


    public Button upgradeButton;
    public Button cosmeticButton;
    public GameObject[] lockOverlay;
    public GameObject[] upgradeSlidersAndTexts;

    float maxSpeed, maxAcc, maxHandeling, maxArmor;

    CarSelectionState selectionState;


    public CarTransactionButtons[] carTransactionButtons;

    [Header("Camera Positions")]
    public GameObject[] userProfileCamera;
    public Camera[] cameras;



    #endregion







    #region Car Level Properties

    private int currentCarLevel;
    public int CurrentCarLevel
    {
        get
        {
            return currentCarLevel;
        }
        set
        {
            currentCarLevel = value;
        }
    }

    #endregion


    #region Car Index Properties

    int lastCarIndex;
    int currentCarIndex;
    public int CurrentCarIndex
    {
        get
        {
            return currentCarIndex;
        }
        set
        {
            if (currentCarIndex >= 0 && currentCarIndex < machinesShow.Length && currentCarIndex < machinesLogo.Length)
            {
                machinesShow[currentCarIndex].SetActive(false); //this code set deactivated machine prefab gameobjects
                machinesLogo[currentCarIndex].SetActive(false); //this code set deactivated machinesLogo gameobjects
                historyImage[currentCarIndex].SetActive(false); //This code set deactivated history image gameobject
                historyText_FA[currentCarIndex].SetActive(false); //This code set deactivated history text gameobject 
                userProfileCamera[currentCarIndex].SetActive(false);
            }


            lastCarIndex = currentCarIndex;
            currentCarIndex = value;

            if (currentCarIndex < 0)
                currentCarIndex = machinesShow.Length - 1;
            else if (currentCarIndex >= machinesShow.Length)
                currentCarIndex = 0;


            #region Available Cars

            if (!Information.Instance.carInfo[currentCarIndex].isAvailable)
            {
                int diff = currentCarIndex - lastCarIndex;
                currentCarIndex += diff;

                if (currentCarIndex < 0)
                    currentCarIndex = machinesShow.Length - 1;
                else if (currentCarIndex >= machinesShow.Length)
                    currentCarIndex = 0;
            }

            #endregion

            #region locked Cars



            UpgradeAndCosmeticButtonEnable();


            #endregion

            #region Comming Soon Car

            if (Information.Instance.carInfo[currentCarIndex].commingSoon)
            {

            }

            #endregion


            machinesShow[currentCarIndex].SetActive(true);       //this code set activated machines prefab game object
            machinesLogo[currentCarIndex].SetActive(true);       //this code set activated machines Logo game object
            historyImage[currentCarIndex].SetActive(true);       //This code set activated history image gameobject
            historyText_FA[currentCarIndex].SetActive(true);     //This code set activated history text gameobject 

            machineName.text = _GetCarNameEN(currentCarIndex).ToString();  //Included Car Name
            machinePrice.text = _GetCarPrice(currentCarIndex).ToString();  //Included Car Price
            machineLevel.text = "Unlock at level: " + _GetCarRequirmentLevel(currentCarIndex).ToString();
            historyText_EN.text = _GetCarHistoryEn(currentCarIndex).ToString();

            int level = Data.GetCarLevel(currentCarIndex);
            level = Mathf.Clamp(level, 0, Information.Instance.carInfo[currentCarIndex].levels.Length);
            int classIndex = (int)Information.Instance.carInfo[currentCarIndex].Class;
            CurrentCarLevel = Data.GetCarLevel(Information.Instance.carInfo[currentCarIndex].carTag);
            bool currentCarIsLocked = (CurrentCarLevel == -1 && !Information.Instance.carInfo[currentCarIndex].levels[0].price.IsFree());

            if (Information.Instance.carInfo[currentCarIndex].levels[level].price.IsFree())
                currentCarIsLocked = false;

            if (Information.Instance.carInfo[currentCarIndex].commingSoon)
            {
                selectionState = CarSelectionState.NotAvailableYet;
            }
            else
            {
                if (currentCarIsLocked)
                {
                    selectionState = CarSelectionState.Locked;
                    SetCarInformation(currentCarIndex);
                }
                else
                {
                    selectionState = CarSelectionState.Available;
                    SetCarInformation(currentCarIndex);
                }
            }


            if (selectionState == CarSelectionState.NotAvailableYet || selectionState == CarSelectionState.Locked || selectionState == CarSelectionState.Available)
            {
                machinesShow[currentCarIndex].transform.position = spawnPoint.position;
                machinesShow[currentCarIndex].transform.rotation = spawnPoint.rotation;
            }

            Debug.Log("car index=" + currentCarIndex);
            UpgradeSliderController.Instance.ShowValues();



            /*if (Accounting.Instance.currentUser.LeagueIndex >= Information.Instance.carInfo[currentCarIndex].requiredLevel)
            {
                Information.Instance.carInfo[currentCarIndex].enoughRequired = true;
            }
            else if (Accounting.Instance.currentUser.LeagueIndex < Information.Instance.carInfo[currentCarIndex].requiredLevel)
            {
                Information.Instance.carInfo[currentCarIndex].enoughRequired = false;
            }*/

        }
    }

    #endregion


    void Start()
    {
        CommonUI.Instance.menuMusicManager.SetVolume(0, musicVolume);
        //Inventory.Instance.Disable();

        for (int i = 0; i < machinesShow.Length; i++)
        {
            machinesShow[i].SetActive(false);
            historyImage[i].SetActive(false);

            maxSpeed = Mathf.Max(maxSpeed, _GetSpeed(i));
            maxAcc = Mathf.Max(maxAcc, _GetAcceleration(i));
            maxHandeling = Mathf.Max(maxHandeling, _GetHandling(i));
            maxArmor = Mathf.Max(maxArmor, _GetArmor(i));
        }


        CurrentCarIndex = Accounting.Instance.currentUser.SelectedCarIndex;


        TransactionButtonManager();
        UpgradeAndCosmeticButtonEnable();



    }



    void SetCarInformation(int _currentCarIndex)
    {
        CarGarageStats.Instance.SpeedInformationFunction(_GetSpeed(_currentCarIndex), _GetMinSpeed(), _GetMaxSpeed(), _GetSpeed(_currentCarIndex));

        CarGarageStats.Instance.RifleInformationFunction(_GetRifleMaxDamage(_currentCarIndex), _GetRifleNameEN(_currentCarIndex), _GetRifleNameFA(_currentCarIndex), _GetRifleMinDamage(_currentCarIndex), _GetRifleMaxDamage(_currentCarIndex), _GetRifleAccuracy(_currentCarIndex), _GetRifleFireDistance(_currentCarIndex), _GetRifleAccuracyMin(), _GetRifleAccuracyMax(), _GetRifleFireRate(_currentCarIndex), _GetRifleMaxDamage(_currentCarIndex));

        CarGarageStats.Instance.MineInformationFunction(_GetMineMaxDamage(_currentCarIndex), _GetMineNameEN(_currentCarIndex), _GetMineNameFA(_currentCarIndex), _GetMineMinDamage(_currentCarIndex), _GetMineMaxDamage(_currentCarIndex), _GetMineCoolDown(_currentCarIndex), _GetMineExplosionArea(_currentCarIndex), _GetMineMinExplosionArea(), _GetMineMaxExplosionArea(), _GetMineMaxDamage(_currentCarIndex));

        CarGarageStats.Instance.RocketInformationFunction(_GetRocketMaxDamage(_currentCarIndex), _GetRocketNameEN(_currentCarIndex), _GetRocketNameFA(_currentCarIndex), _GetRocketMinDamage(_currentCarIndex), _GetRocketMaxDamage(_currentCarIndex), _GetrocketFireRate(_currentCarIndex), _GetRocketDistance(_currentCarIndex), _GetRocketExplosionArea(_currentCarIndex), _GetRocketMinExplosionArea(), _GetRocketMaxExplosionArea(), _GetRocketMaxDamage(_currentCarIndex));

        CarGarageStats.Instance.ArmorInformationFunction(_GetArmor(_currentCarIndex), _GetArmorNameEN(_currentCarIndex), _GetArmorNameFA(_currentCarIndex), _GetArmorBlockingDamage(_currentCarIndex), _GetArmor(_currentCarIndex));

        CarGarageStats.Instance.HealthInformationFunction(_GetHealthValue(_currentCarIndex), _GetMinHealth(), _GetMaxHealth(), _GetClassShield(_currentCarIndex), _GetHealthValue(_currentCarIndex));
    }



    public void CarBuy_Button()
    {
        CommonUI.Instance.PlayButtonClick();
        if (!Information.Instance.carInfo[currentCarIndex].enoughRequired)
        {
            Debug.Log("not enough level");
            CommonUI.Instance.messageBox.ShowMessage(Messages.YouHaveToBeLevelXToBuyThisCar, null, true);
        }
        else
        {
            Accounting.Instance.currentUser.Buy(Information.Instance.carInfo[currentCarIndex].levels[0].price, _BuyProcessDone);
        }
    }

    private void _BuyProcessDone()
    {
        Accounting.Instance.currentUser.BuyCar(Information.Instance.carInfo[currentCarIndex].carTag);
        Accounting.Instance.currentUser.SelectedCarIndex = currentCarIndex;
        //
        //        Accounting.Instance.currentUser.SelectedCarIndex = CurrentCarIndex;

        SaveView();
        TransactionButtonManager();
        UpgradeAndCosmeticButtonEnable();
    }


    void CancelBuy()
    {
        Debug.Log("Cancel buy");
    }


    public void CarSelect_Button()
    {
        CommonUI.Instance.PlayButtonClick();
        Accounting.Instance.currentUser.SelectedCarIndex = currentCarIndex;

        if (selectionState == CarSelectionState.NotAvailableYet)
        {
            CommonUI.Instance.messageBox.ShowMessage(Messages.CarIsNotAvailable, null, true);
        }

        SaveView();
        TransactionButtonManager();
    }



    public void NextMachine()
    {
        CommonUI.Instance.PlayButtonClick();
        CurrentCarIndex++;
        TransactionButtonManager();
    }

    public void PreviousMachine()
    {
        CommonUI.Instance.PlayButtonClick();
        CurrentCarIndex--;
        TransactionButtonManager();
    }



    void TransactionButtonManager()
    {
        if (Accounting.Instance.currentUser.HasCar(Information.Instance.carInfo[currentCarIndex].carTag))
        {
            if (Accounting.Instance.currentUser.SelectedCarIndex == currentCarIndex)
            {
                EquippButtonActive();
                userProfileCamera[currentCarIndex].SetActive(true);
            }
            else
            {
                SelectButtonActive();
                userProfileCamera[currentCarIndex].SetActive(false);
            }
        }
        else
        {
            //if (Accounting.Instance.currentUser.SelectedCarIndex != currentCarIndex)
            BuyButtonActive();
        }
    }


    void Update()
    {

        if (Accounting.Instance.currentUser.Level >= Information.Instance.carInfo[currentCarIndex].requiredLevel)
        {
            carTransactionButtons[currentCarIndex].requirementLevelGameobject.SetActive(false);
        }

        else if (Accounting.Instance.currentUser.Level < Information.Instance.carInfo[currentCarIndex].requiredLevel)
        {
            carTransactionButtons[currentCarIndex].requirementLevelGameobject.SetActive(true);
            carTransactionButtons[currentCarIndex].buyImageGameobject.SetActive(false);
            carTransactionButtons[currentCarIndex].buyButtonGameObject.SetActive(false);
        }



    }


    void BuyButtonActive()
    {
        carTransactionButtons[currentCarIndex].buyButtonGameObject.SetActive(true);
        carTransactionButtons[currentCarIndex].buyImageGameobject.SetActive(true);
        carTransactionButtons[currentCarIndex].locked.SetActive(true);
        //carTransactionButtons[currentCarIndex].requirementLevelGameobject.SetActive(true);

        carTransactionButtons[currentCarIndex].selectGameObject.SetActive(false);
        carTransactionButtons[currentCarIndex].equippedGameObject.SetActive(false);
    }

    void SelectButtonActive()
    {
        carTransactionButtons[currentCarIndex].buyButtonGameObject.SetActive(false);
        carTransactionButtons[currentCarIndex].buyImageGameobject.SetActive(false);
        carTransactionButtons[currentCarIndex].locked.SetActive(false);
        carTransactionButtons[currentCarIndex].requirementLevelGameobject.SetActive(false);

        carTransactionButtons[currentCarIndex].selectGameObject.SetActive(true);
        carTransactionButtons[currentCarIndex].equippedGameObject.SetActive(false);

    }

    void EquippButtonActive()
    {
        carTransactionButtons[currentCarIndex].buyButtonGameObject.SetActive(false);
        carTransactionButtons[currentCarIndex].buyImageGameobject.SetActive(false);
        carTransactionButtons[currentCarIndex].locked.SetActive(false);
        carTransactionButtons[currentCarIndex].requirementLevelGameobject.SetActive(false);

        carTransactionButtons[currentCarIndex].selectGameObject.SetActive(false);
        carTransactionButtons[currentCarIndex].equippedGameObject.SetActive(true);

        /////////////////////////
        userProfileCamera[currentCarIndex].SetActive(true);
    }

    void UpgradeAndCosmeticButtonEnable()
    {
        if (!Accounting.Instance.currentUser.HasCar(Information.Instance.carInfo[currentCarIndex].carTag))
        {
            upgradeButton.enabled = false;
            cosmeticButton.enabled = false;

            for (int i = 0; i < lockOverlay.Length; i++)
            {
                lockOverlay[i].SetActive(true);
            }
        }
        else
        {
            upgradeButton.enabled = true;
            cosmeticButton.enabled = true;

            for (int i = 0; i < lockOverlay.Length; i++)
            {
                lockOverlay[i].SetActive(false);
            }

        }
    }


    public void SaveView()
    {
        RenderTexture rendText = RenderTexture.active;
        RenderTexture.active = cameras[CurrentCarIndex].targetTexture;

        cameras[CurrentCarIndex].Render();
    
        Texture2D cameraImage = new Texture2D(cameras[CurrentCarIndex].targetTexture.width, cameras[CurrentCarIndex].targetTexture.height, TextureFormat.RGB24, false);
        cameraImage.ReadPixels(new Rect(0, 0, cameras[CurrentCarIndex].targetTexture.width, cameras[CurrentCarIndex].targetTexture.height), 0, 0);
        cameraImage.Apply();
        RenderTexture.active = rendText;

        byte[] bytes = cameraImage.EncodeToPNG();

        string url = string.Concat(Application.persistentDataPath, "/", Accounting.Instance.currentUser.Username, "-UserProfileImage.png");
        System.IO.File.WriteAllBytes(url, bytes);
    }






    //******************************Stat Get Functions*******************************
    #region Stat Get Function


    #region Main Car Information

    float _GetHandling(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].handling;
    }
    float _GetAcceleration(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].attack;
    }
    string _GetCarNameEN(int _index)
    {
        return Information.Instance.carInfo[_index].carName;
    }
    string _GetCarNameFA(int _index)
    {
        return Information.Instance.carInfo[_index].carFaName;
    }
    string _GetCarClass(int _index)
    {
        return Information.Instance.carInfo[_index].Class.ToString();
    }
    string _GetCarPrice(int _index)
    {
        return Information.Instance.carInfo[_index].levels[0].price.Bills.ToString();
    }

    string _GetCarRequirmentLevel(int _index)
    {
        return Information.Instance.carInfo[_index].requiredLevel.ToString();
    }


    string _GetCarHistoryEn(int _Index)
    {
        return Information.Instance.carInfo[_Index].carHistory.ToString();
    }


    #endregion

    #region Speed Get Function

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


    #endregion


}
public enum CarSelectionState
{
    Available,
    Locked,
    NotAvailableYet
}

[System.Serializable]
public class CarTransactionButtons
{
    public GameObject buyButtonGameObject;
    public GameObject selectGameObject;
    public GameObject equippedGameObject;

    public GameObject buyImageGameobject;
    public GameObject locked;
    public GameObject requirementLevelGameobject;

}