using UnityEngine;
using System.Collections;

public class GarageCameraMover : MonoBehaviour
{
    public Transform cameraMoverTransform;
    public float CameraRotationSpeed;
    public Animator cameraAnimator;
    public TouchAgent touchAgent;
    public float rotationKoEfficent, yMoveKoEfficent;
    public float rotationDrag, yMoveDrag, xRot;
    public float yMoveMin, yMoveMax;
    public float autoRotateMenuSpeed;


    Transform cameraTransform;
    float currentAutoRotationSpeed;
    public bool autoRotateEnabled;
    
    float distance;
    float defaultCameraY;

    float cameraDegrees;



    float CameraDegrees
    {
        get { return cameraDegrees; }
        set
        {
            cameraDegrees = value % 360;
            // Debug.Log("camerqa angle=" + cameraDegrees);

            if (cameraDegrees < 0)
                cameraDegrees += 360;

            float x = Mathf.Cos(MathHelper.ToRadians(cameraDegrees)) * distance + MainGarageCarController.Instance.spawnPoint.position.x;
            float z = Mathf.Sin(MathHelper.ToRadians(cameraDegrees)) * distance + MainGarageCarController.Instance.spawnPoint.position.z;


            yDiff = Mathf.Clamp(yDiff, yMoveMin, yMoveMax);
            cameraMoverTransform.position = new Vector3(x, defaultCameraY + yDiff, z);

            Vector3 euler = new Vector3();
            euler.x = yDiff * xRot;
            euler.y = 270 - cameraDegrees;

            cameraMoverTransform.localRotation = Quaternion.Euler(euler);

        }
    }

    public void SetPosition(Vector3 position)
    {
        cameraMoverTransform.position = position;
        cameraMoverTransform.LookAt(MainGarageCarController.Instance.spawnPoint);
    }

    public void SetCameraAngle(float angle, GeneralGarageStates _generalGarageStates)
    {
        CameraDegrees = angle;
        /*
        switch (_generalGarageStates)
        {

            case GeneralGarageStates.COSMETIC:
               
                cameraMoverTransform.localRotation = Quaternion.Euler(0f, 15f, 0f);

                break;

            case GeneralGarageStates.PAINT_PART:
                cameraMoverTransform.localRotation = Quaternion.Euler(0f, 15f, 0f);
                break;

            case GeneralGarageStates.UPGRADE:
                cameraMoverTransform.localRotation = Quaternion.Euler(0f, 15f, 0f);
                break;

            default:
                CameraDegrees = angle;
                break;
        }*/

    }

    void Start()
    {
        Vector2 p1 = new Vector2(cameraMoverTransform.position.x, cameraMoverTransform.position.z);
        Vector2 p2 = new Vector2(MainGarageCarController.Instance.spawnPoint.position.x, MainGarageCarController.Instance.spawnPoint.position.z);

        distance = (p1 - p2).magnitude;
        cameraDegrees = MathHelper.AngleBetween(p1, p2, true);

        defaultCameraY = cameraMoverTransform.position.y;
        touchAgent.OnTouch += touchAgent_OnTouch;

        currentAutoRotationSpeed = autoRotateMenuSpeed;
        cameraTransform = cameraAnimator.transform;
    }

    


    float rotationAmount, lastRotationAmount, lastNotZeroRotationAmountTime;
    Vector2 touchPos;
    bool dragging;
    float yDiff;
    void touchAgent_OnTouch(Vector2 position, TouchEvent touchEvent)
    {
        switch (touchEvent)
        {
            case TouchEvent.Touched:
                dragging = true;
                touchPos = position;
                break;
            case TouchEvent.Detouched:
                dragging = false;



                if (rotationAmount == 0 && Time.time - lastNotZeroRotationAmountTime < 0.1f)
                    rotationAmount = lastRotationAmount;


                if (rotationAmount > 0)
                    currentAutoRotationSpeed = autoRotateMenuSpeed;
                else if (rotationAmount < 0)
                    currentAutoRotationSpeed = -autoRotateMenuSpeed;

                break;

            case TouchEvent.Moved:
                Vector2 diff = position - touchPos;

                if (rotationAmount != 0)
                {
                    lastNotZeroRotationAmountTime = Time.time;
                    lastRotationAmount = rotationAmount;
                }


                rotationAmount = -diff.x * rotationKoEfficent;

                CameraDegrees += rotationAmount;
                yDiff -= diff.y * yMoveKoEfficent;

                touchPos = position;
                break;
        }
    }

    public void Shake()
    {
        cameraAnimator.SetTrigger("shake");
    }

    void Update()
    {
        if (!autoRotateEnabled)
            return;

        if (!dragging)
        {
            if (rotationAmount != 0)
            {
                CameraDegrees += rotationAmount;
                rotationAmount *= rotationDrag;

                if (Mathf.Abs(rotationAmount) < Mathf.Abs(currentAutoRotationSpeed)) // 0.01f)
                    rotationAmount = 0;
            }
            else
            {
                CameraDegrees += currentAutoRotationSpeed;
            }

            if (yDiff != 0)
            {
                yDiff *= yMoveDrag;

                if (Mathf.Abs(yDiff) < autoRotateMenuSpeed)
                    yDiff = 0;

                CameraDegrees = cameraDegrees; // refresh camera position
            }
        }
    }

    public void RefreshCameraPosition()
    {
        Vector2 p1 = new Vector2(cameraTransform.position.x, cameraTransform.position.z);
        Vector2 p2 = new Vector2(MainGarageCarController.Instance.spawnPoint.position.x, MainGarageCarController.Instance.spawnPoint.position.z);

        float degrees = MathHelper.AngleBetween(p1, p2, true);
        SetCameraAngle(degrees, GeneralGarageStates.CAR_INFO);
        Debug.Log("degrees=" + degrees + ", p1=" + p1 +", p2=" + p2);
    }
}