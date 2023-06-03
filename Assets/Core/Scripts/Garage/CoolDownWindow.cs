using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoolDownWindow : MonoBehaviour 
{
    public event Data.generalDelegate OnCoolDownTimeFinished;

    public GameObject myGameObject;
    public Text timeText;

    float time;
    float remainedTime;

    public void Activate(float timeRemained)
    {
        time = 0;
        this.remainedTime = timeRemained;
        timeText.text = MathHelper.GetTimeString(timeRemained);
        myGameObject.SetActive(true);
    }
	
	void Update() 
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time %= 1;
            timeText.text = MathHelper.GetTimeString(--remainedTime);

            if (remainedTime == 0)
            {
                Deactivate();
                if (OnCoolDownTimeFinished != null)
                    OnCoolDownTimeFinished();
            }
        }
	}

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}