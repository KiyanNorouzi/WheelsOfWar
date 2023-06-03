using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RocketLockSign : MonoBehaviour 
{
    public GameObject myGameObject;
    public Image[] renderers;
    public float onMinTime, onMaxTime, offTime;
    public float minDistance, maxDistance;
    public AudioSource beep;

    float delay;
    float time;


    void Start()
    {
        distanceValue = 1;
        delay = onMinTime + (onMaxTime - onMinTime) * distanceValue;
    }


    bool visible;

	void Update()
    {
        if (rocket == null || rocket.IsDead)
        {
            Deactivate();
            return;
        }

        float distance = (rocket.transform.position - ScrCarController.Instance.myTransform.position).sqrMagnitude;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        distance -= minDistance;
        DistanceValue = distance / (maxDistance - minDistance);

        time += Time.deltaTime;
        if (visible)
        {
            if (time >= delay)
            {
                visible = false;
                _SetVisible(visible);

                time = 0;
                beep.Play();
            }
        }
        else
        {
            if (time >= offTime)
            {
                visible = true;
                _SetVisible(visible);

                time = 0;
            }
        }
	}
    
    void _SetVisible(bool visible)
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = visible;
    }




    float distanceValue;
    public float DistanceValue
    {
        get { return distanceValue; }
        set 
        { 
            distanceValue = Mathf.Clamp01(value);
            delay = onMinTime + (onMaxTime - onMinTime) * distanceValue;
        }
    }

    /*


    public void PlusButton()
    {
        distanceValue = Mathf.Clamp01(distanceValue + 0.1f);
        delay = onMinTime + (onMaxTime - onMinTime) * distanceValue;
    }

    public void MinusButton()
    {
        distanceValue = Mathf.Clamp01(distanceValue - 0.1f);
        delay = onMinTime + (onMaxTime - onMinTime) * distanceValue;
    }*/

    ScrBulletController rocket;
    public void Activate(ScrBulletController rocket)
    {
        this.rocket = rocket;



        float distance = Mathf.Clamp((rocket.transform.position - ScrCarController.Instance.myTransform.position).sqrMagnitude, minDistance, maxDistance);
        distance -= minDistance;
        DistanceValue = distance / (maxDistance - minDistance);

        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}