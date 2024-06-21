using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "dataItemContainer", menuName = "ScriptableObjects/dataItemContainer", order = 1)]
public class DataItemContainer : SerializedScriptableObject
{
    public Dictionary<string, DataItem<Item>> dataItems = new Dictionary<string, DataItem<Item>>();
}
[Serializable]
public class DataItem<T> where T : PoolElement
{
    public string name;
    public string description;
    public Sprite icon;
    public T prefab;
    public GameObject prefabGameObject;
    public bool isStackable;
    [ShowIf("@(isStackable)")]public int maxStack;
}
