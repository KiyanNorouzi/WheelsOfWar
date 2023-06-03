using UnityEngine;
using System.Collections;

public class LittleMessageWindow : MonoBehaviour 
{
    public GameObject myGameObject;


    float deactivateTime;


	
    public void Activate()
    {
        deactivateTime = Time.time + 1.25f;
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
	
	void Update()
    {
        if (Time.time >= deactivateTime)
            Deactivate();
	}
}