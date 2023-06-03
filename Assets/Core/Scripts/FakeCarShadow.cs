using UnityEngine;
using System.Collections;

public class FakeCarShadow : MonoBehaviour 
{
    public Transform carTransform;
    public Rigidbody carRigidBody;
    public SpriteRenderer mysprite;


    float defaultAlpha;

    [ContextMenu("Collect info")]
    void collectinfo()
    {
        carTransform = gameObject.GetComponentInParent<ScrCarController>().transform;
        carRigidBody = gameObject.GetComponentInParent<ScrCarController>().rigidbody;
        mysprite = GetComponent<SpriteRenderer>();
        
    }

    Color color;

    void Start()
    {
        color = mysprite.color;
        defaultAlpha = color.a;
        endAlpha = 1;
    }
	
	void Update()
    {
        float x = Mathf.Min(carTransform.rotation.eulerAngles.x, 360 - carTransform.rotation.eulerAngles.x);
        float z = Mathf.Min(carTransform.rotation.eulerAngles.z, 360 - carTransform.rotation.eulerAngles.z);
        float forceY = Mathf.Abs(carRigidBody.velocity.y);
        z = Mathf.Abs(z);


        if (z <= 1 && forceY <= 1)
            SetAlphaTo(1, 0.15f);
        else
            SetAlphaTo(0, 0.15f);

        
        /*mysprite.enabled = (forceY <= 0.1f);

        if (z <= 1)
        {
            color.a = defaultAlpha;
            mysprite.color = color;

            Debug.Log("x="  + x + ", z=" + z + ", alpha=" + 1);
        }
        else
        {
            color.a = Mathf.Clamp01(1 - (z / 15f)) * defaultAlpha;
            mysprite.color = color;

            Debug.Log("x=" + x + ", z=" + z + ", alpha=" + Mathf.Clamp01(1 - z / 15f));
        }*/

        if (changingAlpha)
        {
            time += Time.deltaTime;
            flow = time / duration;
            if (flow >= 1)
            {
                color.a = endAlpha;
                changingAlpha = false;
            }
            else
                color.a = Mathf.Lerp(startAlpha, endAlpha, flow);

            color.a *= defaultAlpha;
            mysprite.color = color;
        }
	}

    float time, duration, startAlpha, endAlpha, flow;
    bool changingAlpha;

    void SetAlphaTo(float alpha, float duration)
    {
        if (endAlpha == alpha)
            return;

        if (duration == 0)
        {
            color.a = alpha;
            mysprite.color = color;
        }
        else
        {
            time = flow = 0;
            this.duration = duration;
            startAlpha = mysprite.color.a;
            endAlpha = alpha;
            changingAlpha = true;
        }
            
    }
}