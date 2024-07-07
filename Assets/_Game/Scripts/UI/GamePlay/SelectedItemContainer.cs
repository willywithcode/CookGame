using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItemContainer : MonoBehaviour
{
    [SerializeField] private SelectedItem[] selectedItems;
    private SelectedItem currentSelectedItem;
    public SelectedItem CurrentSelectedItem => currentSelectedItem;
    public void SetUp()
    {
        currentSelectedItem = selectedItems[0];
        for (int i = 0; i < selectedItems.Length; i++)
        {
            selectedItems[i].IndexInInventory = i;
            InventoryItem dataItem = UIManager.Instance.GetUI<UIInventory>().GetInventoryItem(i);
            selectedItems[i].SetData(dataItem.DataItem, dataItem.QuantityItem); 
        }
        selectedItems[0].ToggleBorder(true);
    }
    public void HandleOnClickedItem()
    {
        for (int i = 0; i < selectedItems.Length; i++)
        {
            int temp = i;
            InventoryItem dataItem = UIManager.Instance.GetUI<UIInventory>().GetInventoryItem(i);
            selectedItems[i].refInventoryItem = dataItem;
            dataItem.onChange += (data, quantity) => selectedItems[temp].SetData(data, quantity, false);
            selectedItems[i].onClick += OnClickItem;
        }
    }

    public void OnClickItem(SelectedItem item)
    {
        this.ToggleAllBorder(false);
        currentSelectedItem = item;
        currentSelectedItem.ToggleBorder(true);
    }
    public void ToggleAllBorder(bool isShow)
    {
        for (int i = 0; i < selectedItems.Length; i++)
        {
            selectedItems[i].ToggleBorder(isShow);
        }
    }
}
