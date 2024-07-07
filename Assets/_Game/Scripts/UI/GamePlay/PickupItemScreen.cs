using System;
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

    private void Start()
    {
        this.RegisterListener(EventID.OnPickUpItem, (param) =>
        {
            this.RemoveItem(((PickUpItemBtn)param).IdCollider);
        });
    }

    public void AddItem(ItemInWorld itemInWorld, Collider colliderItem)
    {
        PickUpItemBtn item = (PickUpItemBtn) pool.GetObject(parent:container, scale: Vector3.one);
        item.SetUp(itemInWorld, colliderItem.GetInstanceID());
        pickupItems.Add(colliderItem.GetInstanceID(), item);
        this.gameObject.SetActive(true);
    }
    public void RemoveItem(int instanceID)
    {
        if (pickupItems.ContainsKey(instanceID))
        {
            pickupItems[instanceID].OnDespawn();
            pickupItems.Remove(instanceID);
        }
        if(pickupItems.Count == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
