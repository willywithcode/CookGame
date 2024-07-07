using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pig : ACacheMonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private NavMeshAgent navmeshAgent;
    public void Rotate(Vector3 positionTarget)
    {
        Vector3 targetAngle = positionTarget - TF.position;
        float targetAngleY = Mathf.Atan2(targetAngle.x, targetAngle.z) * Mathf.Rad2Deg;
        TF.rotation = Quaternion.Euler(0f, targetAngleY, 0f);
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngleY, 0f);
        float rotationSpeed = 50f; 
        TF.rotation = Quaternion.Slerp(TF.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
