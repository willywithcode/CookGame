using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeOrder : ACacheMonoBehaviour
{
    [SerializeField] private List<DataItem> dataItems;
    [SerializeField] private int price;
    [SerializeField] private List<int> quantity;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private ButtonCustom btn;
    [SerializeField] private OrderItem orderItemPrefab;
    [SerializeField] private Transform container;

    private void Start()
    {
        btn.button.onClick.AddListener(Trade);
    }
    public void Setup(List<DataItem> dataItem, List<int> quantity, int price)
    {
        this.dataItems = dataItem;
        this.quantity = quantity;
        this.price = price;
        priceTxt.text = price.ToString();
        for (int i = 0; i < dataItem.Count; i++)
        {
            var item = Instantiate(orderItemPrefab, container);
            item.Setup(dataItem[i], quantity[i]);
        }
    }
    public void Trade()
    {
        if (HaveEnoughItem())
        {
            UIInventory inventory = UIManager.Instance.GetUI<UIInventory>();
            UIManager.Instance.GetUI<UIGamePlay>().TweenIncressCoin(price);
            for(int i =0 ; i < dataItems.Count; i++)
            {
                DataItem item = dataItems[i];
                if (inventory.CheckHaveSameItemInventory(item, out int index))
                {
                    inventory.GetInventoryItem(index).RemoveSomeItem(quantity[i]);
                }
            }
        }
        else UIManager.Instance.GetUI<UIOrder>().PopUpText("You don't have enough items");
    }

    private bool HaveEnoughItem()
    {
        for(int i = 0 ; i < dataItems.Count; i++)
        {
            DataItem item = dataItems[i];
            UIInventory inventory = UIManager.Instance.GetUI<UIInventory>();
            if (!inventory.CheckHaveSameItemInventory(item, out int index))
            {
                return false;
            }
            else
            {
                if(inventory.GetInventoryItem(index).QuantityItem < quantity[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
 