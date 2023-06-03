using UnityEngine;
using System.Collections;

public class ScrSmoothFlowController : MonoBehaviour
{
    public GameObject myGameObject;
    public Transform myTransform, targetPointTransform;
    public MainCameraMover mainCameraMover;
    public float autoRotationSpeed;
    public Camera camera;
    

    public float targetHeight = 0.0f;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    public float carSpeed;
    public float multiplyer = 1.25f;




    Transform targetTransform;
    Rigidbody targetRigidBody;
    float farDistance, farTargetHeight, farHeight;

    bool rotating;


    //**************************COMMENT****************************
    //float CamFirstFOV ; //Field of View
    //public float FarFieldOfView = 0;
    //public float FarFieldOfViewReverse = 0;
    //JControlledCar CurGear ;
    //**************************COMMENT****************************

    void Start()
    {
        //**************************COMMENT****************************
        //CamFirstFOV = camera.fieldOfView;
      //  CurGear = GameObject.FindGameObjectWithTag("Player").GetComponent<JControlledCar>();
        //**************************COMMENT****************************
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Activate(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        this.targetRigidBody = targetTransform.GetComponent<Rigidbody>() as Rigidbody;

        if (this.targetRigidBody == null)
            rotating = true;
        else
            rotating = false;

        myGameObject.SetActive(true);
    }


    public float normalFieldOfView, farFieldOfView;
    Vector3 camPos, carPos;
    float currentHeight2, currentDistance, currentTargetHeight;

    public bool isReverse;
    bool lastIsReverse;

    void LateUpdate()
    {
        if (rotating)
        {
            float wantedHeight = targetTransform.position.y + currentHeight2;
            float currentHeight = myTransform.position.y;

            float currentRotationAngle = myTransform.eulerAngles.y;
            float wantedRotationAngle = currentRotationAngle + Time.deltaTime * autoRotationSpeed;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            carPos = targetTransform.position;
            carPos.y += currentTargetHeight;

            camPos = carPos;
            camPos -= currentRotation * Vector3.forward * currentDistance;
            camPos.y = currentHeight;

            myTransform.position = camPos;
        }
        else
        {
            if (!targetTransform)
                return;

            if (targetTransform.tag != "Player")
            {
                GameObject g = GameObject.FindGameObjectWithTag("Player");
                if (g != null)
                    targetTransform =  g.transform;
            }
                

            if (!targetTransform)
                return;

            float wantedRotationAngle;

            float velocityMagnitude = /*v.magnitude;*/ targetTransform.rigidbody.velocity.magnitude;

            /*if (isReverse != lastIsReverse)
            {
                wantedRotationAngle = Quaternion.LookRotation(targetTransform.forward).eulerAngles.y;
            }
            else
            {
                if (velocityMagnitude < 0.01 && !isReverse)
                    wantedRotationAngle = myTransform.eulerAngles.y;
                else
                    wantedRotationAngle = Quaternion.LookRotation(targetTransform.forward).eulerAngles.y;
            //}*/

            wantedRotationAngle = Quaternion.LookRotation(targetTransform.forward).eulerAngles.y;


            if (isReverse)
                wantedRotationAngle += 180;

            //Debug.Log("r=" + isReverse + ", lr=" + lastIsReverse + ", wanted=" + wantedRotationAngle);

            lastIsReverse = isReverse;

            float t = velocityMagnitude / carSpeed;
            camera.fieldOfView = Mathf.Lerp(normalFieldOfView, farFieldOfView, t);

            t = 1;

            currentHeight2 = Mathf.Lerp(farHeight, height, t);

            //**************************COMMENT****************************
            currentDistance = Mathf.Lerp(farDistance, distance, t);
            //if (CurGear .CurrentGear== 0)
            //    camera.fieldOfView = Mathf.Lerp(CamFirstFOV, FarFieldOfViewReverse, t);
            //else
            //    camera.fieldOfView = Mathf.Lerp(CamFirstFOV, FarFieldOfView, t);
                
            //Debug.Log(CurGear);
            //**************************COMMENT****************************

            currentTargetHeight = Mathf.Lerp(farTargetHeight, targetHeight, t);


            float wantedHeight = targetTransform.position.y + currentHeight2;

            float currentRotationAngle = myTransform.eulerAngles.y;
            float currentHeight = myTransform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            carPos = targetTransform.position;
            carPos.y += currentTargetHeight;

            camPos = carPos;


            //**************************COMMENT****************************
            camPos -= currentRotation * Vector3.forward * currentDistance;
          //  camPos -= currentRotation * Vector3.forward * farDistance;
            //**************************COMMENT****************************
            camPos.y = currentHeight;

            myTransform.position = camPos;
        }


        myTransform.LookAt(carPos);
    }

    public void SetInfo(float cameraHeight, float distance, float targetHeight)
    {
        this.height = cameraHeight;
        this.distance = distance;
        this.targetHeight = targetHeight;

        farDistance = distance * multiplyer;
        farTargetHeight = targetHeight * multiplyer;
        farHeight = height * multiplyer;

        targetPointTransform.localPosition = new Vector3(0, -cameraHeight, distance);
    }
}
