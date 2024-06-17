using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIInventory : UICanvas
{
    [SerializeField] private int numRow;
    [SerializeField] private Transform content;
    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private DetailItemUI detailItemUI;
    [SerializeField] private InventorySelectedItem inventorySelectedItem;
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    private bool isFirstTime = true;
    public override void Setup()
    {
        base.Setup();
        if (isFirstTime)
        {
            isFirstTime = false;
            for(int i = 0; i < numRow; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, content);
                    inventoryItem.Setup();
                    inventoryItems.Add(inventoryItem);
                    this.SetupItemAction(inventoryItem);
                }
            }

            this.SetupItemAction(inventorySelectedItem);
            this.AddItemToInventory(0, Constant.BREAD_STRING, 1);
            this.AddItemToInventory(3, Constant.CABBAGE_STRING, 10);
            this.AddItemToInventory(4, Constant.TOMATO_STRING, 2);
            this.AddItemToInventory(5, Constant.CABBAGE_STRING, 99);
            this.AddItemToInventory(10, Constant.TOMATO_STRING, 2);

        }
    }
    private void SetupItemAction(InventoryItem item)
    {
        item.onItemClicked += HandleItemClicked;
        item.onItemDropped += HandleItemDropped;
        item.onItemBeginDrag += HandleItemBeginDrag;
        item.onItemEndDrag += HandleItemEndDrag;
        item.onItemDrag += HandleItemDrag;
    }
    private void HandleItemClicked(InventoryItem inventoryItem)
    {
        if (!inventoryItem.HaveItem)
        {
            this.detailItemUI.ResetDetail();
            return;
        }
        this.detailItemUI.SetDetail(inventoryItem.DataItem);
    }
    public void HandleItemDropped(InventoryItem inventoryItem)
    {
        if(mouseFollower.CurrentInventoryItem == null || !mouseFollower.CurrentInventoryItem.HaveItem) return;
        this.SwapItem(mouseFollower.CurrentInventoryItem, inventoryItem);
    }
    private void HandleItemBeginDrag(InventoryItem inventoryItem)
    {
        if(!inventoryItem.HaveItem) return;
        this.detailItemUI.SetDetail(inventoryItem.DataItem);
        mouseFollower.gameObject.SetActive(true);
        mouseFollower.SetFollower(inventoryItem.DataItem, inventoryItem.QuantityItem, inventoryItem);
    }
    private void HandleItemEndDrag(InventoryItem inventoryItem)
    {
        mouseFollower.gameObject.SetActive(false);
    }
    private void HandleItemDrag(InventoryItem inventoryItem)
    {
        if (!inventoryItem.HaveItem) return;
        mouseFollower.FollowMouse();
    }
    public void SwapItem(InventoryItem currentItem, InventoryItem targetItem)
    {
        if(currentItem == targetItem) return;  
        if (!targetItem.HaveItem)
        {
            targetItem.SetupItem(currentItem.DataItem, currentItem.QuantityItem);
            currentItem.RemoveItem();
            return;
        }
        if (currentItem.DataItem != targetItem.DataItem 
            || !currentItem.DataItem.isStackable 
            || !targetItem.DataItem.isStackable)
        {
            DataItem dataItem = currentItem.DataItem;
            int quantity = currentItem.QuantityItem;
            currentItem.SetupItem(targetItem.DataItem, targetItem.QuantityItem);
            targetItem.SetupItem(dataItem, quantity);
            return;
        }
        int totalQuantity = currentItem.QuantityItem + targetItem.QuantityItem;
        if (totalQuantity > currentItem.DataItem.maxStack)
        {
            int reminder = totalQuantity - currentItem.DataItem.maxStack;
            targetItem.SetupItem(currentItem.DataItem, currentItem.DataItem.maxStack);
            currentItem.SetupItem(targetItem.DataItem, reminder);
            return;
        }
        targetItem.SetupItem(currentItem.DataItem, totalQuantity);
        currentItem.RemoveItem();
    }

    public void AddItemToInventory(int index, string dataname, int quantity)
    {
        if (index < 0 || index >= inventoryItems.Count) return;
        inventoryItems[index].SetupItem(SaveGameManager.Instance.dataItemContainer.dataItems[dataname], quantity);
    }
}
