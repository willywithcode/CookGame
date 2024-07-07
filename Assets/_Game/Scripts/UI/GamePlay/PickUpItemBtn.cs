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
    private ItemInWorld data;
    public ItemInWorld Data => data;
    private int idCollider;
    public int IdCollider => idCollider;

    public void OnClick()
    {
        this.PostEvent(EventID.OnPickUpItem, this);
    }
    public void SetUp(ItemInWorld dataParam, int id)
    {
        this.data = dataParam;
        DataItem item = SaveGameManager.GetDataItem(dataParam.ItemName);
        content.sprite = item.icon;
        nameItem.text = item.title;
        this.idCollider = id;
        quantityItem.gameObject.SetActive(item.isStackable && data.quantity > 1);
        quantityItem.text = data.quantity.ToString();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        data = null;
        content.sprite = null;
        nameItem.text = string.Empty;
        quantityItem.text = string.Empty;
        pool.ReturnObject(this);
    }

}
