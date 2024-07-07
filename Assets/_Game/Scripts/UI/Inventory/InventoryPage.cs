using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventorySpecificItem inventoryItemPrefab;
    [SerializeField] private Transform[] content;
    private Queue<InventorySpecificItem> pool = new Queue<InventorySpecificItem>();
    private List<InventorySpecificItem> inventoryItems = new List<InventorySpecificItem>();
    private int currentIndexCount = 0;
    private Transform parentPool;
    private bool isOpening = false;
    public bool IsOpening => isOpening;

    public int CurrentIndexCount
    {
        set => currentIndexCount = value;
    }
    public void AddItem(InventoryItem refInventoryItem, UnityAction<InventoryItem> action)
    {
        InventorySpecificItem item = GetInventoryItem(GetAcceptableContent());
        item.Setup();
        item.ToggleBorder(false);
        item.SetupItem(refInventoryItem.DataItem, refInventoryItem.QuantityItem);
        item.ReferenceInventoryItemItem = refInventoryItem;
        this.SetUpItem(item, action);
        currentIndexCount++;
        inventoryItems.Add(item);
        
    }

    public void SetUpItem(InventorySpecificItem item , UnityAction<InventoryItem> action)
    {
        item.AssignActionOnDespawn(ReturnPool);
        item.AssignActionOnItemClicked((inventoryItem) =>
        {
            action?.Invoke(inventoryItem);
            this.TurnOffAllBorder();
            inventoryItem.ToggleBorder(true);
        });
    }
    public void FinishAddItem()
    {
        currentIndexCount = 0;
    }
    private InventorySpecificItem GetInventoryItem()
    {
        if(pool.Count == 0)
        {
            var obj = Instantiate(inventoryItemPrefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
        InventorySpecificItem inventoryItem = pool.Dequeue();
        inventoryItem.gameObject.SetActive(true);
        return inventoryItem;
    }

    private InventorySpecificItem GetInventoryItem(Transform parent)
    {
        var inventoryItem = GetInventoryItem();
        inventoryItem.transform.SetParent(parent);
        return inventoryItem;
    }

    private Transform GetAcceptableContent()
    {
        for(int i = 0; i < content.Length; i++)
        {
            if(content[i].childCount < 12)
            {
                return content[i];
            }
        }

        return this.transform;
    }
    public void ToggleContent(bool isShow)
    {
        if (!isShow)
        {
            for(int i = 0 ; i < inventoryItems.Count; i++)
            {
                this.ReturnPool(inventoryItems[i]);
            }
            inventoryItems.Clear();
        }
        for(int i = 0; i < content.Length; i++)
        {
            content[i].gameObject.SetActive(isShow);
        }
        this.isOpening = isShow;
    }

    public void ReturnPool(InventorySpecificItem inventoryItem)
    {
        if(parentPool == null)
        {
            parentPool = new GameObject("Pool").transform;
            parentPool.SetParent(GameManager.Instance.transform);
        }
        inventoryItem.transform.SetParent(parentPool);
        inventoryItem.gameObject.SetActive(false);
        pool.Enqueue(inventoryItem);
    }
    public void TurnOffAllBorder()
    {
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].ToggleBorder(false);
        }
    }
}