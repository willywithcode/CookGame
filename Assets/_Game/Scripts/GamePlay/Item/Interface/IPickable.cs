using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate bool CheckPickup(object obj); 
public interface IPickable
{
    public void PickUp(CheckPickup onPickUp = null);
}
