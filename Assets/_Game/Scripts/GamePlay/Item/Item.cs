using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : PoolElement
{
    [SerializeField] private ItemFactory itemFactory;
    public ItemFactory ItemFactory => itemFactory;
    [SerializeField, ValueDropdown(nameof(ValueDropdown))] public string typeItem;
    public string TypeItem => typeItem;
    public IEnumerable ValueDropdown()
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems.Select(e => e.Value.name);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        itemFactory.ReturnObject(this);
    }
}
