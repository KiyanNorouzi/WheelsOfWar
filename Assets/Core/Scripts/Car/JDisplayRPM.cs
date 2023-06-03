using UnityEngine;
using System.Collections;

public class JDisplayRPM : MonoBehaviour {

	public JCar car;
	public float factor = 0.0372f;
	float reading = 0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		reading = 0.8f * reading + 0.2f * (car.MotorRunning?(car.MotorRPM * factor):0);
		transform.localRotation = Quaternion.Euler(0f, reading, 0f);
	}
}
