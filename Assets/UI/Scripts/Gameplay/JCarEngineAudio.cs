using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JCarEngineAudio : MonoBehaviour 
{
    public JControlledCar jCar;
    public AudioSource[] sounds;
    public float minRPM, maxRPM;
    public float minPitch, maxPitch;

    public float[] startPitches;
    public float[] endPitches;
    public float[] increasingPitchAmount;
    public float shiftingGearDuration;


    float pitchState;
    bool isShiftingGear;
    float shiftGearTime;
    float pitch;


	void Start () 
    {
        sounds[0].loop = true;
        sounds[0].pitch = minPitch;

        if (!stopped)
            sounds[0].Play();
	}



    int currentGear;
    int lastGear;
    float startGearRPM;

    float lastValidPitch;

	void Update() 
    {
        float ftemp = jCar.MotorRPM / (maxRPM - minRPM);
        float ffTemp = minPitch + (maxPitch - minPitch) * ftemp;

        sounds[0].pitch = ffTemp;


        /*
        Vector2 velocity = new Vector2(jCar.rigidbody.velocity.x, jCar.rigidbody.velocity.z);
        float velocityMagnitude = velocity.magnitude;
        */
        




        //Debug.Log("Gear=" + jCar.CurrentGear + ", RPM=" + jCar.MotorRPM);
        return;

        
        if (isShiftingGear)
        {
            shiftGearTime += Time.deltaTime;
            float flow = shiftGearTime / shiftingGearDuration;

            if (flow >= 1)
            {
                isShiftingGear = false;
                currentGear++;

                sounds[0].pitch = pitch = startPitches[currentGear] - 0.2f;
            }
            else
            {
                int maxIndex = Mathf.Min(currentGear + 1, startPitches.Length - 1);
                sounds[0].pitch = Mathf.Lerp(pitch, startPitches[maxIndex] - 0.2f, flow);
            }
        }
        else
        {
            if (jCar.CurrentGear != currentGear)
            {
                lastGear = currentGear;
                currentGear = jCar.CurrentGear;
                startGearRPM = jCar.MotorRPM;

                //isShiftingGear = true;
                shiftGearTime = 0;
            }
            else
            {
                

                float f = jCar.MotorRPM - startGearRPM;
                float f2 = Mathf.Clamp01(f / jCar.shiftUpRPM);

                //Debug.Log("rpm=" + f + ", f2=" + f2 + ".GEAR=" + jCar.CurrentGear + ". RPM=" + jCar.MotorRPM + ", pitch=" + (startPitches[currentGear] + 0.5f * f2));
                float a = endPitches[currentGear] - startPitches[currentGear];
                sounds[0].pitch = pitch = startPitches[currentGear] + a * f2;
            }
            
        }
        
	}

    bool stopped;

    public void Stop()
    {
        stopped = true;
        sounds[0].Stop();
    }

    public void StopFor(float seconds)
    {
        stopped = true;
        sounds[0].Stop();
        Invoke("PlayAgain", seconds);
    }

    void PlayAgain()
    {
        stopped = false;
        sounds[0].Play();
    }
}