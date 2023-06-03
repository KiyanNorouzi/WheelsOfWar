using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    public Slider healthSlider, healthShadowSlider;
    public float duration;


    float health;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }


    float value;
    public float Value
    {
        get { return this.value; }
        set 
        { 
            health = this.value = healthSlider.value = healthShadowSlider.value = value; 
        }
    }


    public void AddShadowHealth(float addingValue)
    {
//        Debug.Log("shadow health=" + addingValue + " at " + Time.time);

        health = healthShadowSlider.value = health + addingValue;
        targetTime = Time.time + 0.35f;
    }

    float targetTime;
    void Update()
    {
        if (targetTime != -1 && targetTime <= Time.time)
        {
            healthSlider.value = health;
            targetTime = -1;
        }
    }
}