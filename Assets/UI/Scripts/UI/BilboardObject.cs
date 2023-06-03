using UnityEngine;
using System.Collections;

public class BilboardObject : MonoBehaviour 
{
    public float SizeMultiplyer = 1;
    public Transform myTransform;
    public Transform cameraTransform;
    public bool correctRotation, correctSize;

	void Start()
    {
        if (myTransform == null)
            myTransform = transform;

        if (cameraTransform == null && Camera.main!= null)
            cameraTransform = EnvironmentController.Instance.followCam.transform; // Camera.main.transform;
	}
	
	void Update()
    {
        if (cameraTransform == null || myTransform == null)
        {
            if (myTransform == null)
                myTransform = transform;

            if (cameraTransform == null)
                cameraTransform = EnvironmentController.Instance.followCam.transform; // Camera.main.transform;

            return;
        }

        if (correctRotation)
        {
            Vector3 rot = cameraTransform.rotation.eulerAngles;
            rot.x = 0;
            rot.z = 0;
            myTransform.rotation = Quaternion.Euler(rot);
        }

        if (correctSize)
        {
            float distance = (cameraTransform.position - myTransform.position).magnitude * SizeMultiplyer;
            myTransform.localScale = new Vector3(distance, distance, 1);
        }
	}
}