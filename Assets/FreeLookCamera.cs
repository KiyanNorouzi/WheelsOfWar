using UnityEngine;
using System.Collections;

public class FreeLookCamera : MonoBehaviour 
{
    public Animator myAnimator, blackscreenAnimator;
    public float waitTime;
    public int animationsCount;
    public float blackScreenTime;
    public new Camera camera;
    /*public FastBloom bloom;
    public ColorCorrectionCurves colorCorrection;*/



    int animations, currentAnimationIndex;
    bool actuallyChangeCameraPos;
    float time, currentWaitTime;


    void Awake()
    {
        if (!SettingData.Lights)
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Shadow"));

        if (!SettingData.TextureQuality)
            QualitySettings.SetQualityLevel(0, true);

        if (!SettingData.VFX)
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Particle"));

        /*if (!SettingData.Colors)
        {
            bloom.enabled = false;
            colorCorrection.enabled = false;
        }*/
    }

    void Start()
    {
        //animations = AnimationUtility.GetAnimationClips(gameObject).Length;
        animations = animationsCount;
        currentWaitTime = waitTime;
        currentAnimationIndex = 1;
    }

	void Update () 
    {
       
        time += Time.deltaTime;
        if (time >= currentWaitTime)
        {
            time = 0;
            if (!actuallyChangeCameraPos)
            {
                blackscreenAnimator.SetTrigger("activate");
                actuallyChangeCameraPos = true;

                currentWaitTime = blackScreenTime;
            }
            else
            {
                currentAnimationIndex++;
                if (currentAnimationIndex > animations)
                    currentAnimationIndex = 1;

                myAnimator.SetInteger("pos", currentAnimationIndex);

                actuallyChangeCameraPos = false;
                currentWaitTime = waitTime;
            }
        }
	}
}
