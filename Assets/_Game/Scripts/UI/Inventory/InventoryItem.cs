using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private bool isTweening = false;

    public void SetupItem(DataItem dataItemSetup , int quantity = 0)
    {
        if(quantity <= 0) return;
        this.haveItem = true;
        this.contentItem.gameObject.SetActive(true);
        this.contentItem.sprite = dataItemSetup.icon;
        this.dataItem = dataItemSetup;
        this.quantityItem = quantity;
        if (!dataItemSetup.isStackable || quantity < 2)
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
        if (state && !isTweening)
        {
            borderImg.transform.DOScale(0.95f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            isTweening = true;
        }
        borderImg.enabled = state;
    }

    public void ThrowItem()
    {
        Item item = dataItem.prefab.ItemFactory.GetObject(1);
        item.TF.position = new Vector3(GameManager.Instance.Player.TF.position.x + Random.Range(3f,4f)
            , GameManager.Instance.Player.TF.position.y + 1
            , GameManager.Instance.Player.TF.position.z + Random.Range(3f,4f));
        if (this.quantityItem < 2)
        {
            this.RemoveItem();
            return;
        }

        quantityItem -= 1;
        quantityText.text = quantityItem.ToString();
        if(quantityItem == 1) quantityText.gameObject.SetActive(false);
    }

    public void ThrowAllItem()
    {
        Item item = dataItem.prefab.ItemFactory.GetObject(this.quantityItem);
        item.TF.position = new Vector3(GameManager.Instance.Player.TF.position.x + Random.Range(3f,4f)
            , GameManager.Instance.Player.TF.position.y + 1
            , GameManager.Instance.Player.TF.position.z + Random.Range(3f,4f));
        this.RemoveItem();
    }

    public int SplitItem()
    {
        int temp = quantityItem / 2;
        quantityItem -= temp;
        quantityText.text = quantityItem.ToString();
        if(quantityItem == 1) quantityText.gameObject.SetActive(false);
        return temp;
    }

    public void Hold()
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
}
