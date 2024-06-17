using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "dataItemContainer", menuName = "ScriptableObjects/dataItemContainer", order = 1)]
public class DataItemContainer : SerializedScriptableObject
{
    public Dictionary<string, DataItem> dataItems = new Dictionary<string, DataItem>();
}
[Serializable]
public class DataItem
{
    public string name;
    public string description;
    public Sprite icon;
    public GameUnit prefab;
    public bool isStackable;
    [ShowIf("@(isStackable)")]public int maxStack;
}
