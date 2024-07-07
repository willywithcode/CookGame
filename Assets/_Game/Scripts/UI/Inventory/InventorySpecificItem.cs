using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySpecificItem : InventoryItem
{
    protected InventoryItem referenceInventoryItemItem;
    public UnityAction<InventorySpecificItem> onDespawn;
    private bool isAssignActionOnDespawn = false;
    private bool isAssignActionOnItemClicked = false;
    public InventoryItem ReferenceInventoryItemItem
    {
        set => referenceInventoryItemItem = value;
    }
    public override void Hold()
    {
        base.Hold();
        this.referenceInventoryItemItem.Hold();
    }

    public override void ThrowItem()
    {
        if (this.quantityItem < 2)
        {
            this.RemoveItem();
            return;
        }

        quantityItem -= 1;
        quantityText.text = quantityItem.ToString();
        if(quantityItem == 1) quantityText.gameObject.SetActive(false);
        this.referenceInventoryItemItem.ThrowItem();
    }

    public override void ThrowAllItem()
    {
        this.RemoveItem();
        this.referenceInventoryItemItem.ThrowAllItem();
    }

    public override void RemoveItem()
    {
        base.RemoveItem();
        this.referenceInventoryItemItem.RemoveItem();
        onDespawn?.Invoke(this);
    }
    public void AssignActionOnDespawn(UnityAction<InventorySpecificItem> action)
    {
        if (isAssignActionOnDespawn) return;
        onDespawn += action;
        isAssignActionOnDespawn = true;
    }
    public void AssignActionOnItemClicked(UnityAction<InventoryItem> action)
    {
        if (isAssignActionOnItemClicked) return;
        onItemClicked += action;
        isAssignActionOnItemClicked = true;
    }
}
