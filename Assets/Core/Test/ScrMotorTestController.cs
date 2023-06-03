using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrMotorTestController : MonoBehaviour {

[System.Serializable]
public class AxleInfo 
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public List<AxleInfo> axleInfos;
public float maxMotorTorque;
public float maxSteeringAngle;
public float breakForce;
public float centerOfMass;
// finds the corresponding visual wheel
// correctly applied transform


void Awake()
{
    rigidbody.centerOfMass = new Vector3(0, -centerOfMass, 0);
}


public void FixedUpdate()
{

    float motor = maxMotorTorque * Input.GetAxis("Vertical");
    float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

    foreach (AxleInfo axleInfo in axleInfos) 
    {
        if (axleInfo.steering) 
        {
            axleInfo.leftWheel.steerAngle = steering;
            axleInfo.rightWheel.steerAngle = steering;
        }

        if (axleInfo.motor) 
        {
            axleInfo.leftWheel.motorTorque = motor;
            axleInfo.rightWheel.motorTorque = motor;
        }

        WheelSetPosition(axleInfo);

    }

    if (Input.GetKey(KeyCode.Space))
    {
        axleInfos[1].leftWheel.brakeTorque = breakForce;
        axleInfos[1].rightWheel.brakeTorque = breakForce;
    }

    if (Input.GetKeyUp(KeyCode.Space))
    {
        axleInfos[1].leftWheel.brakeTorque = 0;
        axleInfos[1].rightWheel.brakeTorque = 0;
    }
}

void WheelSetPosition(AxleInfo axleInfo)
{
    RaycastHit hit;

    // Left Wheel
    if (Physics.Raycast(axleInfo.leftWheel.transform.position, -axleInfo.leftWheel.transform.up, out hit, axleInfo.leftWheel.radius * axleInfo.leftWheel.suspensionDistance))
        axleInfo.leftWheel.transform.position = hit.point + axleInfo.leftWheel.transform.up * axleInfo.leftWheel.radius;
    else
        axleInfo.leftWheel.transform.position = axleInfo.leftWheel.transform.position - axleInfo.leftWheel.transform.up * axleInfo.leftWheel.radius;

    // Right Wheel
    if (Physics.Raycast(axleInfo.rightWheel.transform.position, -axleInfo.rightWheel.transform.up, out hit, axleInfo.rightWheel.radius * axleInfo.rightWheel.suspensionDistance))
        axleInfo.rightWheel.transform.position = hit.point + axleInfo.rightWheel.transform.up * axleInfo.rightWheel.radius;
    else
        axleInfo.rightWheel.transform.position = axleInfo.rightWheel.transform.position - axleInfo.rightWheel.transform.up * axleInfo.rightWheel.radius;

}

}
