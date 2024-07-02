using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PickupItemScreen : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private PickUpItemBtn pickUpItemBtn;
    [SerializeField] private ButtonPickUpPool pool;
    private Dictionary<int, PickUpItemBtn> pickupItems = new Dictionary<int, PickUpItemBtn>();
    public void AddItem(DataItem dataItem, UnityAction action, Collider collider, int quantity)
    {
        PickUpItemBtn item = (PickUpItemBtn) pool.GetObject(parent:container, scale: Vector3.one);
        Debug.Log(item.GetInstanceID());
        item.SetUp(dataItem, () =>
        {
            action?.Invoke();
            if(pickupItems.ContainsKey(collider.GetInstanceID()))
            {
                pickupItems[collider.GetInstanceID()].OnDespawn();
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
            pickupItems[collider.GetInstanceID()].OnDespawn();
            pickupItems.Remove(collider.GetInstanceID());
            
        }
        if(pickupItems.Count == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Check()
    {
        PickUpItemBtn item = (PickUpItemBtn) pool.GetObject(parent:container, scale: Vector3.one);
        Debug.Log(item.GetInstanceID());
        DOVirtual.DelayedCall(2, () =>
        {
            item.OnDespawn();
        });
    }
}
