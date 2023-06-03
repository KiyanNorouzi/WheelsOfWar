using UnityEngine;
using System.Collections;

public class JDisplayGear : MonoBehaviour {

	public JCar car;
	public float factor = -36f;
	float reading = 0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		reading = 0.8f * reading + 0.2f * car.CurrentGear * factor;
		transform.localRotation = Quaternion.Euler(reading, 0f, 0f);
	}
}
