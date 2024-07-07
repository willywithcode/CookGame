using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour, IPointerClickHandler
{
    public UnityAction<SelectedItem> onClick;
    [SerializeField] private GameObject border;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityTxt;
    public InventoryItem refInventoryItem;
    private DataItem dataItem;
    public DataItem DataItem => dataItem;
    private int quantityItem;
    public int QuantityItem => quantityItem;
    private int indexInInventory;
    private bool isTweening;
    private bool haveItem = false;
    public bool HaveItem => haveItem;

    public int IndexInInventory
    {
        set => indexInInventory = value;
    }
    public void SetData(DataItem dataItem, int quantity, bool isToggleBorder = true)
    {
        if(isToggleBorder) ToggleBorder(false);
        quantityTxt.text = "";
        if (dataItem == null)
        {
            RemoveAllItem();
            haveItem = false;
            return;
        }
        haveItem = true;
        this.dataItem = dataItem;
        icon.sprite = dataItem.icon;
        quantityItem = quantity;
        icon.gameObject.SetActive(true);
        quantityTxt.text = quantity.ToString();
        if(quantityItem <= 1)
        {
            quantityTxt.text = "";
        }
        
    }
    public void ToggleBorder(bool isShow)
    {
        border.SetActive(isShow);
        if(isShow && !isTweening)
        {
            border.transform.DOScale(0.95f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            isTweening = true;
        }
    }

    public void RemoveItem()
    {
        this.quantityItem--;
        quantityTxt.text = quantityItem.ToString();
        if(quantityItem <= 1 || !dataItem.isStackable)
        {
            quantityTxt.text = "";
        }
        if(quantityItem <= 0)
        {
            icon.gameObject.SetActive(false);
            haveItem = false;
        }
        refInventoryItem?.RemoveOneItem();
    }
    public void RemoveAllItem()
    {
        this.quantityItem = 0;
        quantityTxt.text = "";
        haveItem = false;
        icon.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(this);
    }
}
