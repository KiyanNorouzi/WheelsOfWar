using UnityEngine;
using System.Collections;

public class JDisplayDrive : MonoBehaviour {

	public JCar car;
	public float factor = 51f;
	float reading = 0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int ind = 0;
		switch (car.wheelDrive) {
				case JWheelType.Front : ind = 0; break;
				case JWheelType.Back : ind = 2; break;
				case JWheelType.All : ind = 1; break;
		}
		reading = 0.8f * reading + 0.2f * ind * factor;
		transform.localRotation = Quaternion.Euler(reading, 0f, 0f);
	}
}
