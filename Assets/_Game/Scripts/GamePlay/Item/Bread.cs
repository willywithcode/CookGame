using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bread : Item, IPickable
{
    public void PickUp(Action<object> onPickUp = null)
    {
        this.OnDespawn();
        onPickUp?.Invoke(this);
    }
}
