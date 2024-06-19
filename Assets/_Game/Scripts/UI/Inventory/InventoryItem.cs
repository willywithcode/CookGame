using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler,
    IEndDragHandler, IDropHandler, IDragHandler
{
    public UnityAction<InventoryItem> onItemBeginDrag,onItemDropped, onItemClicked, onItemEndDrag, onItemDrag;
    [SerializeField] protected Image contentItem;
    [SerializeField] protected TextMeshProUGUI quantityText;
    [SerializeField] protected Image borderImg;
    protected DataItem dataItem;
    protected bool haveItem = false;
    protected int quantityItem;
    public bool HaveItem => haveItem;
    public DataItem DataItem => dataItem;
    public int QuantityItem => quantityItem;

    public void SetupItem(DataItem dataItem , int quantity = 0)
    {
        if(quantity <= 0) return;
        this.haveItem = true;
        this.contentItem.gameObject.SetActive(true);
        this.contentItem.sprite = dataItem.icon;
        this.dataItem = dataItem;
        this.quantityItem = quantity;
        if (!dataItem.isStackable || quantity < 2)
        {
            this.quantityText.gameObject.SetActive(false);
            return;
        }
        this.quantityText.gameObject.SetActive(true);
        this.quantityText.text = quantity.ToString();
    }

    public void Setup()
    {
        this.contentItem.gameObject.SetActive(false);
        this.quantityText.gameObject.SetActive(false);
    }

    public void RemoveItem()
    {
        this.haveItem = false;
        this.dataItem = null;
        this.quantityItem = 0;
        this.ToggleBorder(false);
        this.contentItem.gameObject.SetActive(false);
        this.quantityText.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        this.onItemClicked?.Invoke(this);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        this.onItemBeginDrag?.Invoke(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        this.onItemEndDrag?.Invoke(this);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        this.onItemDropped?.Invoke(this);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        onItemDrag?.Invoke(this);
    }

    public void ToggleBorder(bool state)
    {
        borderImg.enabled = state;
    }

    public void ThrowItem()
    {
        if (this.quantityItem < 2)
        {
            this.RemoveItem();
            return;
        }

        quantityItem -= 1;
        quantityText.text = quantityItem.ToString();
        if(quantityItem == 1) quantityText.gameObject.SetActive(false);
    }

    public int SplitItem()
    {
        int temp = quantityItem / 2;
        quantityItem -= temp;
        quantityText.text = quantityItem.ToString();
        if(quantityItem == 1) quantityText.gameObject.SetActive(false);
        return temp;
    }
}
