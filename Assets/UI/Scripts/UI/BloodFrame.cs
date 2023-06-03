using UnityEngine;
using System.Collections;

public class BloodFrame : MonoBehaviour 
{
    public GameObject myGameObject;
    public Animator myAnimator;
    public RectTransform myTransform;
    public UnityEngine.UI.Image image;

    public bool IsActive
    {
        get { return myGameObject.activeInHierarchy; }
    }
    
    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Activate(float wait)
    {
        myTransform.localScale = new Vector3(Random.value > 0.5f ? -1 : 1, 1, 1);
        myGameObject.SetActive(true);

        if (myGameObject.activeInHierarchy)
            StartCoroutine(FadeAfter(wait));
        else
            myGameObject.SetActive(false);
    }

    IEnumerator FadeAfter(float wait)
    {
        yield return new WaitForSeconds(wait);
        myAnimator.SetTrigger("fade");
    }
}