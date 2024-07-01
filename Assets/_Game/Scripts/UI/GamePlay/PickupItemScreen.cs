using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupItemScreen : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private PickUpItemBtn pickUpItemBtn;
    private Dictionary<int, PickUpItemBtn> pickupItems = new Dictionary<int, PickUpItemBtn>();
    public void AddItem(DataItem<Item> dataItem, UnityAction action, Collider collider, int quantity)
    {
        var item = Instantiate(pickUpItemBtn, container);
        item.SetUp(dataItem, () =>
        {
            action?.Invoke();
            if(pickupItems.ContainsKey(collider.GetInstanceID()))
            {
                Destroy(pickupItems[collider.GetInstanceID()].gameObject);
                pickupItems.Remove(collider.GetInstanceID());
            }

            if (pickupItems.Count == 0)
            {
                gameObject.SetActive(false);
            }
        }, quantity);
        pickupItems.Add(collider.GetInstanceID(), item);
        this.gameObject.SetActive(true);
    }
    public void RemoveItem(Collider collider)
    {
        if (pickupItems.ContainsKey(collider.GetInstanceID()))
        {
            Destroy(pickupItems[collider.GetInstanceID()].gameObject);
            pickupItems.Remove(collider.GetInstanceID());
            
        }
        if(pickupItems.Count == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
