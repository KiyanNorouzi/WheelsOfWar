using UnityEngine;
using System.Collections;

public class TutorialWayPoint : MonoBehaviour 
{
    public delegate void triggerPointReached(int index);
    public event triggerPointReached OnTriggerReached;

    public GameObject myGameObject;
    public Transform targetPointTransform;
    //public UnityEngine.UI.Image arrowImage;
    public SpriteRenderer[] sprites;
    public Collider myCollider;
    public bool doCarMove;
    public float carMoveDuration;
    //public RectTransform indicatorTransform;


    [HideInInspector]
    public int index;


    bool isDraggingCar;
    float halfWidth, screenWidth, halfHeight, screenHeight, duration;

    void Start()
    {
        halfWidth = TutorialSceneManager.Instance.arrowImage.rectTransform.rect.width / 2f;
        halfHeight = TutorialSceneManager.Instance.arrowImage.rectTransform.rect.height / 2f;

        screenWidth = Screen.width - halfWidth;
        screenHeight = Screen.height - halfHeight;
    }


    public void Activate()
    {
        myCollider.enabled = true;
        myGameObject.SetActive(true);

        duration = (carMoveDuration == 0) ? TutorialSceneManager.Instance.DragDuration : carMoveDuration;
    }

    public void Activate(Color color)
    {
        SetColor(color);
        myCollider.enabled = true;
        myGameObject.SetActive(true);

        duration = (carMoveDuration == 0) ? TutorialSceneManager.Instance.DragDuration : carMoveDuration;
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void SetColor(Color c)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Color c2 = new Color(c.r, c.g, c.b, sprites[i].color.a);
            sprites[i].color = c2;
        }
    }



    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && OnTriggerReached != null)
        {
            TutorialSceneManager.Instance.waypointReachedAudio.Play();
            OnTriggerReached(index);
        }
    }


    void Update()
    {
        Vector3 indicatorPos = EnvironmentController.Instance.followCam.camera.WorldToScreenPoint(targetPointTransform.position);

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

        if (indicatorPos.y > screenHeight)
        {
            outOfScreen = true;
            indicatorPos.y = screenHeight;
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


        if (isDraggingCar)
        {
            dragTime += Time.deltaTime;
            float flow = dragTime / duration;

            if (flow >= 1)
            {
                ScrCarController.Instance.myTransform.position = transform.position;
                isDraggingCar = false;
            }
            else
            {
                ScrCarController.Instance.myTransform.position = Vector3.Lerp(startPosition, transform.position, flow);
                ScrCarController.Instance.myTransform.rotation = Quaternion.Lerp(startRotation, transform.rotation, flow);
            }
            /*Vector3 diff = ScrCarController.Instance.myTransform.position - transform.position;
            diff.y = 0;
            float distance = diff.magnitude;

            Vector3 direction = diff.normalized;
            float distanceTime = distance * Time.deltaTime;
            if (distanceTime > TutorialSceneManager.Instance.DragSpeed)
            {
                
            }**/
                
        }
    }


    float dragTime;
    Vector3 startPosition;
    Quaternion startRotation;


    public void DragCarIntoCenter()
    {
        if (doCarMove)
        {
            startPosition = ScrCarController.Instance.myTransform.position;
            startRotation = ScrCarController.Instance.myTransform.rotation;
            isDraggingCar = true;
        }
    }
}