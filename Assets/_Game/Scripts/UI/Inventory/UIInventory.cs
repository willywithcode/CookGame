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
    private InventoryItem currentSelectedItem;
    private bool isFirstTime = true;
    private bool isHaveItemSelected = false;
    public override void Setup()
    {
        base.Setup();
        if (isFirstTime)
        {
            isFirstTime = false;
            this.detailItemUI.AddEventOnclickSplitBtn(SplitItem);
            this.detailItemUI.AddEventOnclickThrowBtn(ThrowItem);
            this.detailItemUI.AddEventOnclickThrowAllBtn(ThrowAllItem);
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
            inventorySelectedItem.Setup();
            this.TurnOffAllBorder();
            this.SetupItemAction(inventorySelectedItem);
            this.LoadData();
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
            this.TurnOffAllBorder();
            this.currentSelectedItem = null;
            return;
        }
        this.TurnOnNewBorder(inventoryItem);
        this.currentSelectedItem = inventoryItem;
        this.detailItemUI.SetDetail(inventoryItem.DataItem);
    }
    public void HandleItemDropped(InventoryItem inventoryItem)
    {
        if(mouseFollower.CurrentInventoryItem == null || !mouseFollower.CurrentInventoryItem.HaveItem) return;
        this.SwapItem(mouseFollower.CurrentInventoryItem, inventoryItem);
        this.Save();
    }
    private void HandleItemBeginDrag(InventoryItem inventoryItem)
    {
        if(!inventoryItem.HaveItem) return;
        if(isHaveItemSelected) return;
        isHaveItemSelected = true;
        this.TurnOnNewBorder(inventoryItem);
        this.currentSelectedItem = inventoryItem;
        this.detailItemUI.SetDetail(inventoryItem.DataItem);
        mouseFollower.gameObject.SetActive(true);
        mouseFollower.SetFollower(inventoryItem.DataItem, inventoryItem.QuantityItem, inventoryItem);
    }
    private void HandleItemEndDrag(InventoryItem inventoryItem)
    {
        mouseFollower.gameObject.SetActive(false);
        if (isHaveItemSelected && mouseFollower.CurrentInventoryItem == inventoryItem)
        {
            isHaveItemSelected = false;
        }
    }
    private void HandleItemDrag(InventoryItem inventoryItem)
    {
        if (!inventoryItem.HaveItem) return;
        mouseFollower.FollowMouse();
    }
    public void SwapItem(InventoryItem currentItem, InventoryItem targetItem)
    {
        if(currentItem == targetItem) return;  
        this.currentSelectedItem = null;
        this.detailItemUI.ResetDetail();
        this.TurnOffAllBorder();
        if (!targetItem.HaveItem)
        {
            targetItem.SetupItem(currentItem.DataItem, currentItem.QuantityItem);
            currentItem.RemoveItem();
            if(currentItem == inventorySelectedItem)
            {
                GameManager.Instance.RenUIPlayer.ResetItem();
            }
            return;
        }
        if (currentItem.DataItem != targetItem.DataItem 
            || !currentItem.DataItem.isStackable 
            || !targetItem.DataItem.isStackable)
        {
            DataItem<Item> dataItem = currentItem.DataItem;
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
        inventoryItems[index].SetupItem(GetTypeItem(dataname), quantity);
        this.Save();
    }
    public void AddItemToInventory(string dataname, int quantity)
    {
        if(CheckHaveEmptyInventoryOrSameItem( GetTypeItem(dataname), out int indexItem))
        {
            if(inventoryItems[indexItem].QuantityItem + quantity > inventoryItems[indexItem].DataItem.maxStack)
            {
                int reminder = inventoryItems[indexItem].QuantityItem + quantity - inventoryItems[indexItem].DataItem.maxStack;
                this.AddItemToInventory(indexItem, dataname, inventoryItems[indexItem].DataItem.maxStack);
                if (CheckHaveEmptyInventory(out int index))
                {
                    AddItemToInventory(index, name,  quantity);
                }
            }
            else
            {
                this.AddItemToInventory(indexItem,dataname, inventoryItems[indexItem].QuantityItem + quantity);
            }
        }
        else if (CheckHaveEmptyInventory(out int index))
        {
            this.AddItemToInventory(index,dataname, inventoryItems[index].QuantityItem + quantity);
        }
        
    }
    public void ThrowItem() 
    {
        if (this.currentSelectedItem == null) return;
        currentSelectedItem.ThrowItem();
        this.Save();
    }

    public void ThrowAllItem()
    {
        if (currentSelectedItem == null) return;
        currentSelectedItem.ThrowAllItem();
        this.Save();
    }

    public void SplitItem()
    {
        if(currentSelectedItem == null) return;
        if (CheckHaveEmptyInventory(out int index))
        {
            int splitQuantity = currentSelectedItem.SplitItem();
            inventoryItems[index].SetupItem(currentSelectedItem.DataItem, splitQuantity);
        }
        this.Save();
    }

    public bool CheckHaveEmptyInventory(out int index)
    {
        index = -1;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (!inventoryItems[i].HaveItem)
            {
                index = i;
                return true;
            }
        }

        return false;
    }
    public bool CheckHaveEmptyInventoryOrSameItem(DataItem<Item> type,out int index)
    {
        index = -1;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].HaveItem && inventoryItems[i].DataItem == type 
                                           && inventoryItems[i].DataItem.isStackable && inventoryItems[i].QuantityItem < type.maxStack)
            { 
                index = i;
                return true;
            }
        }
        return false;
    }

    public void TurnOffAllBorder()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].ToggleBorder(false);
        }
        inventorySelectedItem.ToggleBorder(false);
    }

    public void TurnOnNewBorder(InventoryItem inventoryItem)
    {
        this.TurnOffAllBorder();
        inventoryItem.ToggleBorder(true);
    }
    public DataItem<Item> GetTypeItem(string typeItem)
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems[typeItem];
    }

    public void Save()
    {
        SaveGameManager.Instance.InventoryItems = new List<InventoryData>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].HaveItem)
            {
                SaveGameManager.Instance.InventoryItems.Add(new InventoryData(inventoryItems[i].DataItem.name,
                    inventoryItems[i].DataItem.isStackable ? inventoryItems[i].QuantityItem: 1) );
            }
            else
            {
                SaveGameManager.Instance.InventoryItems.Add(new InventoryData("", 0));
            }
        }
        SaveGameManager.Instance.SaveData();
    }

    public void LoadData()
    {
        if(SaveGameManager.Instance.InventoryItems.Count <= 0) return;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (SaveGameManager.Instance.InventoryItems[i].quantity > 0)
            {
                inventoryItems[i].SetupItem(SaveGameManager.Instance.dataItemContainer.dataItems[SaveGameManager.Instance.InventoryItems[i].name],
                    SaveGameManager.Instance.InventoryItems[i].quantity);
            }
        }
    }
 }
