using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TouchAgent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void touchDelegate(Vector2 position, TouchEvent touchEvent);
    public event touchDelegate OnTouch;



    public void OnBeginDrag(PointerEventData data)
    {
        if (OnTouch != null)
            OnTouch(data.position, TouchEvent.Touched);
    }

    public void OnDrag(PointerEventData data)
    {
        if (OnTouch != null)
            OnTouch(data.position, TouchEvent.Moved);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (OnTouch != null)
            OnTouch(data.position, TouchEvent.Detouched);
    }
}

public enum TouchEvent
{
    Touched,
    Moved,
    Detouched
}