using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponButton : MonoBehaviour 
{
    public event Data.generalDelegate OnClick;


    public Image buttonImage;
    public Button button;
    public Text countText;
    public Animator textAnimator;
    public Image coolDownImage;
    public float coolDownDuration;
    public bool AutoCoolDown;
    public KeyCode[] shortcutKeys;



    bool isCoolingDown;
    float time;



    int bulletsCount;
    public int BulletsCount
    {
        get { return bulletsCount; }
        set
        {
            bulletsCount = value;
            countText.text = bulletsCount.ToString();

            if (!isCoolingDown)
                _SetButtonImageEnabled(bulletsCount != 0);
        }
    }

    

    public void CoolDown()
    {
        coolDownImage.fillAmount = 1;
        coolDownImage.enabled = true;


        _SetButtonImageEnabled(false);
        time = 0;
        isCoolingDown = true;
    }

    void _SetButtonImageEnabled(bool enabled)
    {
        Color c = buttonImage.color;
        c.a = (enabled) ? 1 : 0.4f;
        buttonImage.color = c;
    }

    public void Button_Click()
    {
        if (isCoolingDown)
        {
            GameplayUI.Instance.audioPlayer.Play(GameplayUI.Instance.audioPlayer.emptyGun);
            return;
        }

        if (BulletsCount == 0)
        {
            textAnimator.SetTrigger("alarm");
            GameplayUI.Instance.audioPlayer.Play(GameplayUI.Instance.audioPlayer.emptyGun);
        }
        else
        {
            if (AutoCoolDown)
                CoolDown();

            if (OnClick != null)
                OnClick();
        }
    }
    
    void Update()
    {
        if (isCoolingDown)
        {
            time += Time.deltaTime;
            float flow = time / coolDownDuration;
            

            if (flow >= 1)
            {
                coolDownImage.fillAmount = 0;
                coolDownImage.enabled = false;

                _SetButtonImageEnabled(bulletsCount != 0);
                isCoolingDown = false;
            }
            else
                coolDownImage.fillAmount = flow;
        }


        if (Application.isEditor || GeneralSettings.Instance.AllowKeyboardControl)
        {
            for (int i = 0; i < shortcutKeys.Length; i++)
            {
                if (Input.GetKeyDown(shortcutKeys[i]))
                    Button_Click();
            }
        }
    }

    public void SkipCoolDown()
    {
        time = coolDownDuration;
    }
}