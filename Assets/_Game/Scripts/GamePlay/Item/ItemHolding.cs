using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemHolding : Item
{
    #if UNITY_EDITOR
    [OnInspectorGUI]
    public void OnInspectorGUI()
    {
        var dataItem = SaveGameManager.Instance.dataItemContainer.dataItems
            .FirstOrDefault(e => e.Value.prefabGameObject == this);
        if(dataItem.Value != null)
        {
            itemName = dataItem.Key;
            itemType = dataItem.Value.type;
        }
    }
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
