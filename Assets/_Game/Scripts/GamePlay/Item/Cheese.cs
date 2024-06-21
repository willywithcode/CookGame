using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cheese : Item, IPickable
{
    public void PickUp(Action<object> onPickUp = null)
    {
        
    }
}
