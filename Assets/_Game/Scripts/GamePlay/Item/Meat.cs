using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Meat : Item, IPickable
{
    public void PickUp(CheckPickup onPickUp = null)
    {
        if(onPickUp.Invoke(this))
            this.OnDespawn();
    }
}
