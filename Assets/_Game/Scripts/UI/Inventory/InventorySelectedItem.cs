using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySelectedItem : InventoryItem, IDropHandler
{
    [SerializeField] private RenUIPlayer player;
    public override void OnDrop(PointerEventData eventData)
    {
        this.onItemDropped?.Invoke(this);
    }
}
