using UnityEngine;
using System.Collections;

public class JCollateralDamage : MonoBehaviour {

	public float forceFactor = 20f;
	public float yOffset = .5f;
	public float impactFactor = 5f;
	
	void OnCollisionEnter(Collision col) {
		if (col.relativeVelocity.magnitude > impactFactor) {
			Vector3 pos = transform.position;
			pos.y += yOffset;
			Vector3 contact = col.contacts[0].point;
			contact.y += yOffset;
			transform.position = pos;
			rigidbody.isKinematic = false;
			rigidbody.AddForceAtPosition(col.relativeVelocity * forceFactor, contact);
			rigidbody.AddForce(Vector3.up * col.relativeVelocity.magnitude);
			Destroy(this);
		}
	}
}
