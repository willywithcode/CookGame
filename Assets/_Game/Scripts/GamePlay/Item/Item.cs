using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
public class Item : PoolElement
{
    public int quantity  = 1;
    [SerializeField] private ItemFactory itemFactory;
    public ItemFactory ItemFactory => itemFactory;
    [SerializeField, ValueDropdown(nameof(ValueDropdown))] public string typeItem;
    public string TypeItem => typeItem;
    public IEnumerable ValueDropdown()
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems.Select(e => e.Key);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        quantity = 1;
        itemFactory.ReturnObject(this);
    }
    #if UNITY_EDITOR
    [Button]
    public void CreateSOPooling()
    {
        ItemFactory itemFactory = ScriptableObject.CreateInstance<ItemFactory>();
        itemFactory.SetUp(this, 10);
        UnityEditor.AssetDatabase.CreateAsset(itemFactory, "Assets/_Game/PoolFactory/" + this.typeItem + ".asset");
        this.itemFactory = itemFactory;
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif
}
