using UnityEngine;
using System.Collections;

public class JFlipBack : MonoBehaviour {

    void Start()
    {

    }

    public void Respawn()
    {
        Respawn(transform.localPosition + Vector3.up);
    }

    public void Respawn(Vector3 position)
    {
        Vector3 a = transform.localRotation.eulerAngles;
        a.x = 0;
        a.y = 0; // Mathf.Repeat(a.y + Input.GetAxis("Horizontal") * 5f, 360f);
        a.z = 0;
        Quaternion rotation = Quaternion.Euler(a);

        Respawn(position, rotation);
    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        rigidbody.isKinematic = true;

        transform.localRotation = rotation;
        transform.localPosition = position;

        Invoke("SetRigidBody", 0.1f);
    }

    void SetRigidBody()
    {
        rigidbody.isKinematic = false;
    }
}
