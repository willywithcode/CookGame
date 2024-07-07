using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInWorld : Item, IPickable
{
    [SerializeField] private Collider colliderObject;
    public override void OnInit()
    {
        base.OnInit();
        this.SetUp(true);
    }

    public void SetUp(bool isInWorld)
    {
        if (isInWorld)
        {
            if (!this.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                this.AddComponent<Rigidbody>();
            }
            colliderObject.enabled = true;
        }
        else
        {
            if (this.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Destroy(rb);
            }
            colliderObject.enabled = false;
        }
    }

    public void PickUp()
    {
        this.OnDespawn();
    }
#if UNITY_EDITOR
    [OnInspectorGUI]
    public void OnInspectorGUI()
    {
        var dataItem = SaveGameManager.Instance.dataItemContainer.dataItems
            .FirstOrDefault(e => e.Value.prefab == this);
        if(dataItem.Value != null)
        {
            itemName = dataItem.Key;
            itemType = dataItem.Value.type;
        }
    }

    [Button]
    public void GetCollider()
    {
        colliderObject = this.GetComponent<Collider>();
    }

#endif
}
