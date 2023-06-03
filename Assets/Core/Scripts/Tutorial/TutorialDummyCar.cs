using UnityEngine;
using System.Collections;

public class TutorialDummyCar : MonoBehaviour 
{
    public Transform myTransform, targetTransform;
    public Animator myAnimator;
    public TutorialHitTaker hitTaker;
    public Color c1, c2;
    public float fullHealth;
    public Collider myCollider;


    float health;
    float halfWidth, screenWidth;

	void Start()
    {
        hitTaker.OnGetDamage += hitTaker_OnGetDamage;

        halfWidth = TutorialSceneManager.Instance.arrowImage.rectTransform.rect.width / 2f;
        screenWidth = Screen.width - halfWidth;

        health = fullHealth;
	}

    void hitTaker_OnGetDamage(int killmethod, float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            myAnimator.SetTrigger("down");
            myCollider.enabled = false;

            TutorialSceneManager.Instance.Level++;
            TutorialSceneManager.Instance.arrowImage.color = c2;
            enabled = false;
        }
    }

    void Update()
    {
        Vector3 indicatorPos = EnvironmentController.Instance.followCam.camera.WorldToScreenPoint(targetTransform.position);

        bool outOfScreen = false;
        if (indicatorPos.x < halfWidth)
        {
            outOfScreen = true;
            indicatorPos.x = halfWidth;
            TutorialSceneManager.Instance.arrowTransform.localRotation = Quaternion.AngleAxis(-90, Vector3.forward);
        }

        if (indicatorPos.x > screenWidth)
        {
            outOfScreen = true;
            indicatorPos.x = screenWidth;
            TutorialSceneManager.Instance.arrowTransform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
        }

        if (indicatorPos.y < 0)
        {
            outOfScreen = true;
            indicatorPos.y = 0;
            TutorialSceneManager.Instance.arrowTransform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
        }

        if (indicatorPos.z < 0)
        {
            outOfScreen = true;
            indicatorPos.y = 0;
            indicatorPos.x = Screen.width - indicatorPos.x;

            TutorialSceneManager.Instance.arrowTransform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
        }


        //RefreshColor();

        if (!outOfScreen)
            TutorialSceneManager.Instance.arrowTransform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);

        //usernameText.enabled = !outOfScreen;
        TutorialSceneManager.Instance.arrowTransform.position = indicatorPos;
        TutorialSceneManager.Instance.arrowImage.color = Color.Lerp(c1, c2, health / fullHealth);
    }
}