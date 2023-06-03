using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CarGarageStats : MonoBehaviour
{
    #region Singleton

    static CarGarageStats _instance;

    public static CarGarageStats Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;

    }

    #endregion

    #region Speed Region
    //This region depented to Speed information and set them in to the garage UI.

    [Header("Speed")]
    public Text speedTypeEN;
    public Text speedTypeFA;
    public Text speed;
    public string[] speedTypeStringEN;
    public string[] speedTypeStringFA;
    public float minSpeedRange; //this variables hold min speed range
    public float maxSpeedRange; //this variables hold max speed range


    public void SpeedInformationFunction(float _speed, float _minSpeedRange, float _maxSpeedRange, float _speedSlider)
    {

        speed.text = _speed.ToString();

        if (_speed < _minSpeedRange)
        {
            speedTypeEN.text = speedTypeStringEN[0].ToString();
            speedTypeFA.text = speedTypeStringFA[0].ToString();
        }

        if (_speed >= _minSpeedRange && _speed <= _maxSpeedRange)
        {
            speedTypeEN.text = speedTypeStringEN[1].ToString();
            speedTypeFA.text = speedTypeStringFA[1].ToString();
        }

        if (_speed > _maxSpeedRange)
        {
            speedTypeEN.text = speedTypeStringEN[2].ToString();
            speedTypeFA.text = speedTypeStringFA[2].ToString();
        }

    }

    #endregion

    #region Rifle Region

    //This section related to rifle Information

    [Header("Rifle")]
    public Text rifleDPS;
    public Text rifleDamage;
    public Text rifleFireDistance;
    public Text rifleFireRange;
    public Text rifleFireRate;

    public float rifleAccuracyMin;
    public float rifleAccuracyMax;

    public void RifleInformationFunction(float _rifleDPS, string _rifleNameEN, string _rifleNameFA, float _rifleMinDamage, float _rifleMaxDamage, float _rifleAccuracy, float _rifleFireDistance, float _rifleAccuracyMin, float _rifleAccuracyMax, float _rifleFireRate, float _rifleSlider)
    {
        rifleDPS.text = _rifleDPS.ToString();
        rifleDamage.text =_rifleMaxDamage.ToString();
        rifleFireDistance.text = _rifleFireDistance.ToString() + " m";
        rifleFireRate.text = _rifleFireRate.ToString() + "b/s";

    }

    #endregion

    #region Mine Region

    //This section related to mine information

    [Header("Mine")]
    public Text mineDPS;
    public Text mineDamage;
    public Text mineCoolDown;
    public Text mineExplosionAreaEN;
    public Text mineExplosionAreaFA;

    [Header("Mine Varables")]
    public string[] mineExplosionAreaStringEN;
    public string[] mineExplosionAreaStringFA;
    public float mineMinExplosionArea;
    public float mineMaxExplosionArea;

    public void MineInformationFunction(float _mineDPS, string _mineNameEN, string _mineNameFA, float _mineMinDamage, float _mineMaxDamage, float _mineCoolDown, float _mineExplosionArea, float _mineMinExplosionArea, float _mineMaxExplosionArea, float _mineSlider)
    {
        mineDPS.text = _mineDPS.ToString();
        mineDamage.text = _mineMaxDamage.ToString();
        mineCoolDown.text = _mineCoolDown.ToString() + " s";

        if (_mineExplosionArea < _mineMinExplosionArea)
        {
            mineExplosionAreaEN.text = mineExplosionAreaStringEN[0].ToString();
            mineExplosionAreaFA.text = mineExplosionAreaStringFA[0].ToString();
        }

        if (_mineExplosionArea >= _mineMinExplosionArea && _mineExplosionArea <= _mineMaxExplosionArea)
        {
            mineExplosionAreaEN.text = mineExplosionAreaStringEN[1].ToString();
            mineExplosionAreaFA.text = mineExplosionAreaStringFA[1].ToString();
        }

        if (_mineExplosionArea > _mineMaxExplosionArea)
        {
            mineExplosionAreaEN.text = mineExplosionAreaStringEN[2].ToString();
            mineExplosionAreaFA.text = mineExplosionAreaStringFA[2].ToString();
        }


    }

    #endregion

    #region Rocket Region

    //This section related to rocket information
    [Header("Rocket")]
    public Text rocketDPS;
    public Text rocketDamage;
    public Text rocketCoolDown;
    public Text rocketExplosionAreaEN;
    public Text rocketExplosionAreaFA;
    public Text rocketDistance;


    public string[] rocketExplosionAreaStringEN;
    public string[] rocketExplosionAreaStringFA;
    public float rocketMinExplosionArea;
    public float rocketMaxExplosionArea;

    public void RocketInformationFunction(float _rocketDPS, string _rocketNameEN, string _rocketNameFA, float _rocketDamageMin, float _rocketDamageMax, float _rocketCoolDown, float _rocketDistance, float _rocketExplosionArea, float _rocketMinExplosionArea, float _rocketMaxExplosionArea, float _rocketSlider)
    {
        rocketDPS.text = _rocketDPS.ToString();
        rocketDamage.text =  _rocketDamageMax.ToString();
        rocketCoolDown.text = _rocketCoolDown.ToString() + " s";
        rocketDistance.text = _rocketDistance.ToString() + " m";


        if (_rocketExplosionArea < _rocketMinExplosionArea)
        {
            rocketExplosionAreaEN.text = rocketExplosionAreaStringEN[0].ToString();
            rocketExplosionAreaEN.text = rocketExplosionAreaStringFA[0].ToString();
        }

        if (_rocketExplosionArea >= _rocketMinExplosionArea && _rocketExplosionArea <= _rocketMinExplosionArea)
        {
            rocketExplosionAreaEN.text = rocketExplosionAreaStringEN[1].ToString();
            rocketExplosionAreaEN.text = rocketExplosionAreaStringFA[1].ToString();
        }

        if (_rocketExplosionArea > _rocketMaxExplosionArea)
        {
            rocketExplosionAreaEN.text = rocketExplosionAreaStringEN[2].ToString();
            rocketExplosionAreaEN.text = rocketExplosionAreaStringFA[2].ToString();
        }

    }

    #endregion

    #region Armor Region

    //This section related to armor informayion

    [Header("Armor")]
    public Text armorValue;
    public Text armorTypeEN;
    public Text armorTypeFA;
    public Text blockingDamage;

    public void ArmorInformationFunction(float _armorValue, string _armorTypeEN, string _armorTypeFA, float _blockingDamage, float _armorSlider)
    {
        armorValue.text = _armorValue.ToString();
        armorTypeEN.text = _armorTypeEN.ToString();
        armorTypeFA.text = _armorTypeFA.ToString();
        _blockingDamage *= 100f;

        float blockingDamageNew = Mathf.Floor(_blockingDamage);
        float fineNumber = _blockingDamage - blockingDamageNew;

        float ceiledFineNumebr = fineNumber * 10;
        float floorFineNumber = Mathf.Floor(ceiledFineNumebr);

        floorFineNumber /= 10f;

        _blockingDamage -= fineNumber;
        _blockingDamage += floorFineNumber;

        blockingDamage.text = _blockingDamage.ToString() + " %";
    }

    #endregion

    #region Health region

    //This section related to health information

    [Header("Health")]
    public Text healthValue;
    public Text healthTypeEN;
    public Text healthTypeFA;
    public Text classShield;


    public string[] healthTypeStringEN;
    public string[] healthTypeStringFA;
    public float minHealth;
    public float maxHealth;

    public void HealthInformationFunction(float _healthValue, float _minHealth, float _maxHealth, string _classShield, float _healthSlider)
    {
        healthValue.text = _healthValue.ToString();
        classShield.text = _classShield.ToString();

        if (_healthValue < _minHealth)
        {
            healthTypeEN.text = healthTypeStringEN[0].ToString();
            healthTypeFA.text = healthTypeStringFA[0].ToString();
        }

        if (_healthValue >= _minHealth && _healthValue <= _maxHealth)
        {
            healthTypeEN.text = healthTypeStringEN[1].ToString();
            healthTypeFA.text = healthTypeStringFA[1].ToString();
        }

        if (_healthValue < _maxHealth)
        {
            healthTypeEN.text = healthTypeStringEN[2].ToString();
            healthTypeFA.text = healthTypeStringFA[2].ToString();
        }
    }

    #endregion

}