using UnityEngine;
using System.Collections;

public class Accelerometer : MonoBehaviour 
{
    public GameObject myGameObject, iconGameObject;




    bool isSteeringEnabled = true;
    public bool IsSteeringEnabled
    {
        get { return isSteeringEnabled; }
        set 
        { 
            isSteeringEnabled = value;
            iconGameObject.SetActive(isSteeringEnabled);
        }
    }

    Vector2 moveAmount;
    public Vector2 MoveAmount
    {
        get { return moveAmount; }
    }


    public void AccelerateButton_PointerDown()
    {
        moveAmount.y = 1;
    }

    public void AccelerateButton_PointerUp()
    {
        moveAmount.y = 0;
    }

    public void BrakeButton_PointerDown()
    {
        moveAmount.y = -1;
    }

    public void BrakeButton_PointerUp()
    {
        moveAmount.y = 0;
    }


    void OnDisable()
    {
        moveAmount = Vector2.zero;
    }


	void Update ()
    {
        if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) && (Application.isEditor || GeneralSettings.Instance.AllowKeyboardControl))
        {
            float x  = Input.GetAxis("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            moveAmount.x = x;
            moveAmount.y = y;
        }
        else
        {
#if UNITY_ANDROID
            moveAmount.x = Input.acceleration.x;
#else
            moveAmount.x = Input.GetAxis("Horizontal");
#endif
        }

        if (!isSteeringEnabled)
            moveAmount.x = 0;

        if (iconGameObject.activeSelf && Mathf.Abs(MoveAmount.x) > 0.5f)
            iconGameObject.SetActive(false);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Activate()
    {
        iconGameObject.SetActive(true);
        myGameObject.SetActive(true);
    }
}