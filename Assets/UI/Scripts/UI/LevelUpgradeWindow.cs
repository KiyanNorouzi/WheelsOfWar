using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpgradeWindow : MonoBehaviour 
{
    public GameObject myGameObject, levelUpSectionGameObject, rewardSectionGameObject;
    public Text levelText, moneyText;
    public Text rewardMoneyText, rewardXPText;
    public Text titleFAText, titleENText;
    public GameObject rewardXPGameObject;
    public AudioStruct audios;
    public Image medalImage;
    public Sprite[] medalSprites;
    public Animator myAnimator;

    public string[] titlesFA, titlesEN;





    public bool IsActive
    { get { return myGameObject.activeSelf; } }



    Data.generalDelegate doneMethod;
    int money;


    [ContextMenu("activate level up")]
    void activatel()
    {
        ActivateLevelUp(2, 10000, null);
    }

    public void ActivateLevelUp(int level, int money, Data.generalDelegate doneMethod)
    {
        this.money = money;
        this.doneMethod = doneMethod;

        levelText.text = level.ToString();
        moneyText.text = money.ToString();


        rewardSectionGameObject.SetActive(false);
        levelUpSectionGameObject.SetActive(true);
        myGameObject.SetActive(true);

        myAnimator.SetTrigger("levelup");
    }

    [ContextMenu("activate reward")]
    void activater()
    {
        ActivateReward(5, 22000, 150000, null);
    }

    public void ActivateReward(int id, int money, int xp, Data.generalDelegate doneMethod)
    {
        this.money = money;
        this.doneMethod = doneMethod;

        rewardMoneyText.text = MathHelper.GetStringWithComma(money);
        /*if (xp <= 0)
            //rewardXPGameObject.SetActive(false);
        else*/
        {
            rewardXPText.text = MathHelper.GetStringWithComma(xp);
            //rewardXPGameObject.SetActive(true);
        }

        titleFAText.text = titlesFA[id - 1];
        titleENText.text = titlesEN[id - 1];

        medalImage.sprite = medalSprites[id - 1];

        rewardSectionGameObject.SetActive(true);
        levelUpSectionGameObject.SetActive(false);
        myGameObject.SetActive(true);

        myAnimator.SetTrigger("medal");
    }


    public void ContinueButton_Click()
    {
        myGameObject.SetActive(false);

        //MoneyTablet.Instance.Add(money, true);
        //Accounting.Instance.SyncScoreTillSuccess(Data.UserName, Data.TotalScore, Data.Money, "");

        if (doneMethod != null)
            doneMethod();
    }


    void _PlaySound1()
    {
        audios.Play(audios.sound1);
    }

    void _PlaySound2()
    {
        audios.Play(audios.sound2);
    }

    void _PlaySound3()
    {
        audios.Play(audios.sound3);
    }





    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip sound1, sound2, sound3;
    }
}