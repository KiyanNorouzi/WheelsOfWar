using UnityEngine;
using System.Collections;

public class JTeleport : MonoBehaviour {

	public Transform dest;
	
	void OnTriggerEnter(Collider colInfo) {
		Vector3 p = dest.position;
		p.y = colInfo.attachedRigidbody.transform.position.y + 0.1f;
		colInfo.attachedRigidbody.transform.position = p;
	}
}
