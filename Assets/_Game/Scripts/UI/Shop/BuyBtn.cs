using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuyBtn : ACacheMonoBehaviour
{
    [SerializeField]
    [ValueDropdown("GetNameItem")]
    private string itemName;
    [SerializeField] private int price;

    private void Start()
    {
        this.GetComponent<ButtonCustom>().button.onClick.AddListener(Buy);
    }

    public void Buy()
    {
        if(SaveGameManager.Instance.CurrentCoin >= price)
        {
            UIManager.Instance.GetUI<UIGamePlay>().TweenIncressCoin(-price);
            GameManager.Instance.Player.AddItemToInventory(1, SaveGameManager.GetDataItem(itemName).prefab);
            UIManager.Instance.GetUI<UIShop>().PopUpText("Buy " + itemName + " success");
        }
        else
        {
            UIManager.Instance.GetUI<UIShop>().PopUpText("You don't have enough money");
        }
    }
    public IEnumerable GetNameItem()
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems.Select(x => x.Key);
    }
}
