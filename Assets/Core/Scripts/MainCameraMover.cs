using UnityEngine;
using System.Collections;

public class MainCameraMover : MonoBehaviour 
{

    public Animator myAnimator;
    public float motionMin, motionMax;

    bool isShaking;

    private static MainCameraMover _instance;
    public static MainCameraMover Instance
    {
        get
        {
            return _instance;
        }
    }


    void Awake()
    {
        _instance = this;

        if (!SettingData.Lights)
        {
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Shadow"));
        }

        if (!SettingData.TextureQuality)
        {
            QualitySettings.SetQualityLevel(0, true);
            //colorCorrection.enabled = false;
        }


        if (!SettingData.VFX)
        {
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Particle"));
            //motionBlur.enabled = false;
        }


        //bloom.enabled = SettingData.Colors;
    }

    public void Shake(int power)
    {
        if (isShaking)
            return;

        power = Mathf.Clamp(power, 1, 5);
        myAnimator.SetInteger("shakepower", power);
        isShaking = true;
    }

    public void ShakeDone()
    {
        myAnimator.SetInteger("shakepower", 0);
        isShaking = false;
    }
}
