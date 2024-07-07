using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : ACacheMonoBehaviour
{
    [SerializeField] private Outline outline;
    public void ToggleOutLine(bool state) => outline.enabled = state;
}
