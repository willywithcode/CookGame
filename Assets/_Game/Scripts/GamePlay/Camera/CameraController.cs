using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [SerializeField] private GameObject cameraRoot;

    public TouchField Touchfield;

    private float xInput;
    private float yInput;

    private void Awake()
    {
        InputManager.Instance.OnAssignTouchField += (tough) =>
        {
            Touchfield = tough;
        };
    }

    void Update()
    {
        if (Touchfield == null) return;
        xInput += Touchfield.TouchDist.x * sensitivity;
        yInput += Touchfield.TouchDist.y * -sensitivity;

        

        yInput = Mathf.Clamp(yInput, minX, maxX);

        cameraRoot.transform.rotation = Quaternion.Euler(yInput * sensitivity , xInput * sensitivity, 0f);
    }
    
}