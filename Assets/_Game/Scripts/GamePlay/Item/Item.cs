using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : GameUnit
{
    [SerializeField, ValueDropdown(nameof(ValueDropdown))] public string typeItem;
    #if UNITY_EDITOR
    public IEnumerable ValueDropdown()
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems.Select(e => e.Value.name);
    }
    #endif
}
