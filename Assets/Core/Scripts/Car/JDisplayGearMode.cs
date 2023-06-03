using UnityEngine;
using System.Collections;

public class JDisplayGearMode : MonoBehaviour {

	public JControlledCar car;
	public float factor = 35f;
	float reading = 0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		reading = 0.8f * reading + 0.2f * 0 * factor;
		transform.localRotation = Quaternion.Euler(reading, 0f, 0f);
	}
}
