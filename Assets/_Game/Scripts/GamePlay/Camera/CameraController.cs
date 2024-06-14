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

    [HideInInspector] public TouchField Touchfield;

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
        if(Touchfield.TouchDist.SqrMagnitude() <= 0.1) return;
        xInput += Touchfield.TouchDist.x * Time.deltaTime * sensitivity;
        yInput -= Touchfield.TouchDist.y * Time.deltaTime * sensitivity;
        xInput = Mathf.Repeat(xInput, 360f);
        yInput = Mathf.Clamp(yInput, minX, maxX);
        cameraRoot.transform.rotation = Quaternion.Euler(yInput, xInput, 0f);
    }
    
}