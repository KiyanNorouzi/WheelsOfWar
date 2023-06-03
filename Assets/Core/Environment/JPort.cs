using UnityEngine;
using System.Collections;


public class JPort : MonoBehaviour {
	public JFinish finish;
	public int nr;
	public bool passed = false;

	void OnTriggerEnter(Collider colInfo) {
		if (colInfo.attachedRigidbody != finish.who) return;
		passed = finish.PassPort(nr);
	}
}
