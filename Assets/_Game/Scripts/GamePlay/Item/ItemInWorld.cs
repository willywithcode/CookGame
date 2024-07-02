using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemInWorld : Item, IPickable
{
    public void PickUp(CheckPickup onPickUp = null)
    {
        if(onPickUp.Invoke(this))
            this.OnDespawn();
    }
#if UNITY_EDITOR
    [OnInspectorGUI]
    public void OnInspectorGUI()
    {
        var dataItem = SaveGameManager.Instance.dataItemContainer.dataItems
            .FirstOrDefault(e => e.Value.prefab == this);
        if(dataItem.Value != null)
        {
            itemName = dataItem.Key;
            itemType = dataItem.Value.type;
        }
    }
#endif
}
