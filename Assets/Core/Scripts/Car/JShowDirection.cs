using UnityEngine;
using System.Collections;

public class JShowDirection : MonoBehaviour {

	public JFinish finish;
	public JCar car;
		
	// Update is called once per frame
	void Update () {
		Vector3 v = finish.Target() - car.transform.position;
		v.y = 0f;
		if (v.sqrMagnitude < 0.1) return;
//		v.Normalize();
		transform.rotation = Quaternion.LookRotation(v) * Quaternion.Inverse(car.transform.rotation);
	}
}
