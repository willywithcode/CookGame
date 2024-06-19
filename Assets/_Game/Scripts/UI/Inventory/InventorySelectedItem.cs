using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySelectedItem : InventoryItem, IDropHandler
{
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        GameManager.Instance.RenUIPlayer.SetCurrentItem(this.dataItem);
    }
}
