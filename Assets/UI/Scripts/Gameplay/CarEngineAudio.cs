using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarEngineAudio : MonoBehaviour 
{
    public AudioSource[] sounds;

    public float[] startPitches;
    public float[] endPitches;
    public float[] increasingPitchAmount;
    public Text pitchText;
    public float shiftingGearDuration;

    public float minPitch, maxPitch;


    float pitchState;
    bool isShiftingGear;


	void Start () 
    {
        pitch = 1;

        sounds[0].loop = true;

        sounds[0].pitch = pitch = startPitches[0];
        sounds[0].Play();
	}


    int index;
    float pitch;
    float shiftGearTime;
	void Update() 
    {
        if (isShiftingGear)
        {
            shiftGearTime += Time.deltaTime;
            float flow = shiftGearTime / shiftingGearDuration;

            /*if (index == 1)
                Debug.Break();*/

            if (flow >= 1)
            {
                isShiftingGear = false;
                index++;

                sounds[0].pitch = pitch = startPitches[index] - 0.2f;
            }
            else
            {
                int maxIndex = Mathf.Min(index + 1, startPitches.Length - 1);
                sounds[0].pitch = Mathf.Lerp(pitch, startPitches[maxIndex] - 0.2f, flow);
            }
        }
        else
        {
            pitch += increasingPitchAmount[index] * Time.deltaTime * pitchState;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            if (pitch == maxPitch)
                pitch += Random.Range(-0.05f, 0.05f);

            if (index > 0 && pitchState < 0 && pitch < startPitches[index - 1])
                index--;

            sounds[0].pitch = pitch;

            if (pitch >= endPitches[index] && index < 3)
            {
                shiftGearTime = 0;
                isShiftingGear = true;
            }
        }


        //pitchText.text = "Gear=" + (index + 1).ToString() + ", rpm=" + (sounds[0].pitch * 3).ToString();
        pitchText.text = string.Format("Gear={0}{1}RPM={2:0.00}", index + 1, System.Environment.NewLine,  sounds[0].pitch);
	}





    public void PlusButton_Down()
    {
        pitchState = 1;
    }

    public void PlusButton_Up()
    {
        pitchState = -1;
    }

    public void MinusButton_Down()
    {
        pitchState = -1;
    }

    public void MinusButton_Up()
    {
        pitchState = 0;
    }
}
