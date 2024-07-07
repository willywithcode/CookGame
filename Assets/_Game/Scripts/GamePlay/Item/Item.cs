using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : PoolElement
{
    public int quantity  = 1;
    [SerializeField] protected ItemFactory itemFactory;
    public ItemFactory ItemFactory => itemFactory;
    [SerializeField, ReadOnly] protected ItemType itemType;
    [SerializeField, ReadOnly] protected string itemName;
    public string ItemName => itemName;
    public ItemType ItemType => itemType;
    public override void OnDespawn()
    {
        base.OnDespawn();
        quantity = 1;
        TF.eulerAngles = Vector3.zero;
        itemFactory.ReturnObject(this);
    }

    public void ReturnRoot()
    {
        this.TF.SetParent(itemFactory.Root);
    }
    #if UNITY_EDITOR
    [Button]
    public virtual void CreateSOPooling()
    {
        ItemFactory itemFactory = ScriptableObject.CreateInstance<ItemFactory>();
        itemFactory.SetUp(this, 10);
        UnityEditor.AssetDatabase.CreateAsset(itemFactory, "Assets/_Game/PoolFactory/Item/" + this.itemName + ".asset");
        this.itemFactory = itemFactory;
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif
}
