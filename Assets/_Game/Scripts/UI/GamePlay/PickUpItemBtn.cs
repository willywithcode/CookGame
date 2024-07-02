using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickUpItemBtn : PoolElement
{
    [SerializeField] private Image content;
    [SerializeField] private TextMeshProUGUI nameItem;
    [SerializeField] private TextMeshProUGUI quantityItem;
    [SerializeField] private ButtonCustom btn;
    [SerializeField] private ButtonPickUpPool pool;
    private DataItem data;
    private int quantity;
    public void SetUp(DataItem data, UnityAction action, int quantity)
    {
        this.data = data;
        content.sprite = data.icon;
        nameItem.text = data.title;
        quantityItem.gameObject.SetActive(data.isStackable && quantity > 1);
        quantityItem.text = quantity.ToString();
        btn.customButtonOnClick += action;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        data = null;
        quantity = 0;
        content.sprite = null;
        nameItem.text = string.Empty;
        quantityItem.text = string.Empty;
        btn.customButtonOnClick = null;
        pool.ReturnObject(this);
    }

}
