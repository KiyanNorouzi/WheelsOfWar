using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrCarInfoTablet : MonoBehaviour
{
    public Slider[] bars; // 0: speed, 1: acceleration, 2: handling, 3: Armor 
    public float changeDuration;
    public bool animated;


    float[] startSizes, targetSizes;
    float time;
    bool animating;


    void Start()
    {
        startSizes = new float[bars.Length];
        targetSizes = new float[bars.Length];

    }



    [ContextMenu("show test values")]
    public void testvalues()
    {
        ShowValues(0.1f, 0.1f, 0.1f, 0.1f);
    }

    public void ShowValues(float speed, float attack, float handling, float armor)
    {
        ShowValues(speed, attack, handling, armor, animated);
    }

    public void ShowValues(float speed, float attack, float handling, float armor, bool animating)
    {

        if (animating)
        {
            for (int i = 0; i < bars.Length; i++)
                startSizes[i] = bars[i].value;

            targetSizes[0] = speed;
            targetSizes[1] = attack;
            targetSizes[2] = handling;
            targetSizes[3] = armor;

            time = 0;
            this.animating = true;
        }
        else
        {
            bars[0].value = speed;
            bars[1].value = attack;
            bars[2].value = handling;
            bars[3].value = armor;
        }
    }

    void Update()
    {
        if (animating)
        {
            time += Time.deltaTime;
            float flow = time / changeDuration;

            if (flow >= 1)
            {
                for (int i = 0; i < bars.Length; i++)
                    bars[i].value = targetSizes[i];

                animating = false;
            }
            else
            {
                for (int i = 0; i < bars.Length; i++)
                    bars[i].value = startSizes[i] + (targetSizes[i] - startSizes[i]) * flow;
            }
        }
    }






}