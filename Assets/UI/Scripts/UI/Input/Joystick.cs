using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(Image))]
public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject myGameObject;
    public RectTransform myTransform;
    public RectTransform handleTransform;
    public float radius;
    public float maxDistance;

    public bool fixedPosition;
    public bool backToDefaultPosition;

    public float maxDistanceAway;
    public float distanceKoefficent = 1;

    public float[] snapDegrees;
    public float snapArea;



    float power;
    public float Power
    {
        get { return power; }
    }

    float angle;
    public float Angle
    {
        get { return angle; }
    }

    Vector2 moveAmount;
    public Vector2 MoveAmount
    {
        get { return moveAmount; }
        set { moveAmount = value; }
    }

    bool isDragging;
    public bool IsDragging
    {
        get
        {
            return isDragging;
        }
    }


    Vector2 defaultPosition;
    bool initialized;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if (!initialized)
        {
            defaultPosition = myTransform.position;
            initialized = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPosition = myTransform.position;
        isDragging = true;
    }

    void OnDisable()
    {
        moveAmount = Vector2.zero;

        isDragging = false;
        angle = 0;
        power = 0;

        handleTransform.anchoredPosition = Vector2.zero;

        if (initialized)
            myTransform.position = defaultPosition;
    }


    public void OnDrag(PointerEventData eventData)
    {
        angle = MathHelper.AngleBetween(eventData.position, defaultPosition, true);

        for (int i = 0; i < snapDegrees.Length; i++)
        {
            float diffA =  Mathf.Abs(snapDegrees[i] - angle);
            if (diffA < snapArea)
            {
                angle = snapDegrees[i];
                break;
            }
        }


        Vector2 diff = eventData.position - defaultPosition;
        /*moveAmount = diff.normalized;*/

        moveAmount.x = Mathf.Cos(MathHelper.ToRadians(angle)) * 1;
        moveAmount.y = Mathf.Sin(MathHelper.ToRadians(angle)) * 1;

        float distance = diff.magnitude;

        float tRadius = 0;
        if (distance < radius)
        {
            power = distance / radius;
            tRadius = distance;
        }
            
        else if (fixedPosition || distance < maxDistance)
        {
            power = 1;
            tRadius = radius;
        }
        else
        {
            power = 1;

            tRadius = radius;
            float d = maxDistance - distance;



            d *= distanceKoefficent;

            if (maxDistanceAway > 0)
            {
                float k = (d > 0) ? 1 : -1;
                d = Mathf.Min(maxDistanceAway, Mathf.Abs(d));
                d *= k;
            }

            Vector2 pp = new Vector2();
            pp.x = Mathf.Cos(MathHelper.ToRadians(angle + 180)) * d + defaultPosition.x;
            pp.y = Mathf.Sin(MathHelper.ToRadians(angle + 180)) * d + defaultPosition.y;

            myTransform.position = pp;
        }

        Vector2 p = new Vector2();
        p.x = Mathf.Cos(MathHelper.ToRadians(angle)) * tRadius;
        p.y = Mathf.Sin(MathHelper.ToRadians(angle)) * tRadius;

        handleTransform.anchoredPosition = p;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handleTransform.anchoredPosition = new Vector2(0, 0);
        power = 0;
        moveAmount = Vector2.zero;

        if (backToDefaultPosition)
            myTransform.position = defaultPosition;

        isDragging = false;
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Activate()
    {
        myGameObject.SetActive(true);
    }

    public void CancelMovement()
    {
        moveAmount = Vector2.zero;
    }
}
