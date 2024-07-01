using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRenPlayerUI : MonoBehaviour
{
    [SerializeField] private TouchField touchField;
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector3 offset;
    private float xAxis;
    private float yAxis = 235.557f;
    [SerializeField] private float initDistance;

    private void Awake()
    {
        InputManager.Instance.OnAssignTouchFieldPlayerUI += (touch) =>
        {
            touchField = touch;
        };
        this.initDistance = Vector3.Distance(target.position, transform.position);
    }

    private void Update()
    {
        if(touchField == null) return;
        yAxis += touchField.TouchDist.x * Time.deltaTime * sensitivity;
        Vector3 targetRotation = new Vector3(0, yAxis, 0f);
        transform.rotation = Quaternion.Euler(targetRotation);
        transform.position = target.position - transform.forward * initDistance + offset;
    }
}
