using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickUpItemBtn : ACacheMonoBehauviour
{
    [SerializeField] private Image content;
    [SerializeField] private TextMeshProUGUI nameItem;
    [SerializeField] private TextMeshProUGUI quantityItem;
    [SerializeField] private ButtonCustom btn;
    private DataItem<Item> data;
    private int quantity;
    public void SetUp(DataItem<Item> data, UnityAction action, int quantity)
    {
        this.data = data;
        content.sprite = data.icon;
        nameItem.text = data.title;
        quantityItem.gameObject.SetActive(data.isStackable && quantity > 1);
        quantityItem.text = quantity.ToString();
        btn.Btn.onClick.AddListener(() =>
        {
            action?.Invoke();
            Destroy(gameObject);
        });
    }
}
