using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemHolding : Item
{
    #if UNITY_EDITOR
    public override void CreateSOPooling()
    {
        ItemFactory itemFactory = ScriptableObject.CreateInstance<ItemFactory>();
        itemFactory.SetUp(this, 10);
        UnityEditor.AssetDatabase.CreateAsset(itemFactory, "Assets/_Game/PoolFactory/" + this.itemName + "InWorld.asset");
        this.itemFactory = itemFactory;
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif
}
