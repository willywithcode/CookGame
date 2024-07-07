using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderContainer : ACacheMonoBehaviour
{
    [SerializeField] private Outline outline;
    public void ToggleOutLine(bool state) => outline.enabled = state;
}
