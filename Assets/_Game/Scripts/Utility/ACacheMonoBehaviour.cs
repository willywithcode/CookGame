using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACacheMonoBehaviour : MonoBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
}
