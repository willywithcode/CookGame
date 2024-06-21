using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    public void PickUp(Action<object> onPickUp = null);
}
