using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDoor : ACacheMonoBehaviour
{
    [SerializeField] private TeleDoor refDoor;
    [SerializeField] private Transform appearPoint;
    public void Teleport(Transform target)
    {
        refDoor.Appear(target);
    }

    private void Appear(Transform tf)
    {
        tf.position = appearPoint.position;
        tf.GetComponent<Player>().character.enabled = true;
    }
}
