using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int pointerId = -1;
    [HideInInspector]
    public bool Pressed;
    private List<int> listPointerId = new List<int>();


    void Update()
    {
        if (Pressed)
        {
            if (pointerId >= 0 && pointerId < Input.touchCount)
            {
                TouchDist = Input.GetTouch(pointerId).position - PointerOld;
                PointerOld = Input.GetTouch(pointerId).position;
            }
#if UNITY_EDITOR
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
#endif
        }
        else
        {
            TouchDist = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        foreach (var touch in Input.touches)
        {
            if(touch.position == eventData.position)
            {
                listPointerId.Add(touch.fingerId);
                if (pointerId == -1)
                {
                    if(this.GetToughID(listPointerId[0]) != -1)
                    {
                        Pressed = true;
                        pointerId = this.GetToughID(listPointerId[0]);
                        PointerOld = Input.GetTouch(pointerId).position;
                    }
                }
                return;
            }
        }
#endif
#if UNITY_EDITOR
        if (pointerId == -1)
        {
            Pressed = true;
            pointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }
#endif
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_ANDROID 
        for (int i = 0; i < Input.touches.Length;i ++)
        {
            if (Input.GetTouch(i).position == eventData.position)
            {
                if (pointerId == i)
                {
                    listPointerId.Remove(Input.GetTouch(i).fingerId);
                    if(listPointerId.Count > 0)
                    {
                        pointerId = this.GetToughID(listPointerId[0]);
                        PointerOld = Input.GetTouch(pointerId).position;
                    }
                    else
                    {
                        pointerId = -1;
                        Pressed = false;
                    }
                }
                else
                {
                    listPointerId.Remove(Input.GetTouch(i).fingerId);
                }
                return;
            }
        }
#endif
#if UNITY_EDITOR
        if(listPointerId.Count == 0)
        {
            pointerId = -1;
            Pressed = false;
            return;
        }
        if(eventData.pointerId == pointerId)
        {
            pointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }
#endif
    }

    public int GetToughID(int fingerId)
    {
        int index = -1;
        for(int i = 0; i < Input.touches.Length; i++)
        {
            if(Input.GetTouch(i).fingerId == fingerId)
            {
                index = i;
                break;
            }
        }

        return index;
    }

}