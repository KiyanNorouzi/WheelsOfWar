using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardWindow : MonoBehaviour 
{
    public GameObject myGameObject;
    public GameObject[] signsGameObject;
    public GameObject amountFieldGameObject, levelUpGameObject;
    public Animator animator;
    public Text amountText, levelText, vipPackageName, descText;
    public float duration;




    [ContextMenu("test")]
    void test()
    {
        if (Random.value > 0.5f)
            ActivateLevelUp(Random.Range(1, 20));
        else
            ActivateReward((DailyGiftRewardType)Random.Range(0, 4), Random.Range(0, 3), "");
    }



    public void ActivateLevelUp(int level, float animationSpeed = 1)
    {
        for (int i = 0; i < signsGameObject.Length; i++)
            signsGameObject[i].SetActive(false);

        amountFieldGameObject.SetActive(false);
        levelUpGameObject.SetActive(true);

        levelText.text = level.ToString();

        animator.speed = animationSpeed;
        _Activate();
    }

    public void ActivateReward(DailyGiftRewardType type, int amount, string desc, float animationSpeed = 1)
    {
        int typeIndex = (int)type;
        for (int i = 0; i < signsGameObject.Length; i++)
            signsGameObject[i].SetActive(i == typeIndex);

        amountFieldGameObject.SetActive(true);
        levelUpGameObject.SetActive(false);

        descText.text = desc;

        if (type == DailyGiftRewardType.VIP)
        {
            amountText.text = "-";
            vipPackageName.text = VIPPackagesSettings.Instance.packages[amount].name;
        }
        else
            amountText.text = amount.ToString();

        animator.speed = animationSpeed;
        _Activate();
    }

    void _Activate()
    {
        myGameObject.SetActive(true);
        Invoke("_AnimDone", duration / animator.speed);
    }


    void _AnimDone()
    {
        Deactivate();
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}