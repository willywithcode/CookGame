using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectStagePlant : PoolElement
{
    [SerializeField] private PoolObjectPlant poolObjectPlant;
    public PoolObjectPlant PoolObjectPlant => poolObjectPlant;
    public override void OnDespawn()
    {
        base.OnDespawn();
        poolObjectPlant.ReturnObject(this);
    }
#if UNITY_EDITOR
    [SerializeField]string itemName;
    [Button]
    public virtual void CreateSOPooling()
    {
        if(string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Item name is null or empty");
            return;
        }
        PoolObjectPlant pool = ScriptableObject.CreateInstance<PoolObjectPlant>();
        pool.SetUp(this, 10);
        UnityEditor.AssetDatabase.CreateAsset(pool, "Assets/_Game/PoolFactory/Seed/" + this.itemName + ".asset");
        this.poolObjectPlant = pool;
        UnityEditor.AssetDatabase.SaveAssets();;
    }
#endif
}
