using UnityEngine;
using System.Collections;

public class BlinkingGasBar : MonoBehaviour 
{
    public delegate void reCheckGasTime();
    public event reCheckGasTime OnRecheckGasTime;



    public GameObject myGameObject;
    public RectTransform myTransform;
    public Animator myAnimator;
    public float minAnimatorSpeed, maxAnimatorSpeed;

    float timeK = 1;


    float secondsRemaining;

    public void Activate()
    {
        Debug.Log("activate");
        myTransform.anchoredPosition = new Vector2(Accounting.Instance.currentUser.Gas * 6 + 0.5f, myTransform.anchoredPosition.y);

        secondsRemaining  = Accounting.Instance.currentUser.LastRefillTimeInSeconds - 0.5f;
        _RefreshSpeed();
        
        myGameObject.SetActive(true);

        timeK = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.GasRefillTime);
        //Invoke("Tick", 1);
    }


    void Tick()
    {
        secondsRemaining++;
        if (secondsRemaining >= (GeneralSettings.Instance.GasRegenerateTime * timeK))
        {
            Deactivate();

            if (OnRecheckGasTime != null)
                OnRecheckGasTime();
        }
        else
        {
            //Invoke("Tick", 1);
            _RefreshSpeed();
        }
    }

    private void _RefreshSpeed()
    {
        float speed = secondsRemaining / (GeneralSettings.Instance.GasRegenerateTime * timeK);
        //speed = 1 - speed;
        myAnimator.speed = Mathf.Lerp(minAnimatorSpeed, maxAnimatorSpeed, speed);
    }

	public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    float t;
    void Update()
    {
        t += Time.deltaTime;
        if (t > 1)
        {
            t %= 1;
            Tick();
        }
    }
}
