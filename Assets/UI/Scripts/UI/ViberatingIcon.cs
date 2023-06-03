using UnityEngine;
using System.Collections;

public class ViberatingIcon : MonoBehaviour 
{
    public GameObject myGameObject;
    public float duration;

    float endTime;

	public void Activate()
    {
        myGameObject.SetActive(true);
        endTime = Time.time + duration;
    }
	
	void Update () 
    {
        if (Time.time >= endTime)
            myGameObject.SetActive(false);
	}
}
