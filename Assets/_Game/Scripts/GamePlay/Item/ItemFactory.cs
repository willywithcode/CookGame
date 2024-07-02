using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemFactory : AFactory<Item>
{
    public Item GetObject(int quantity, Vector3 pos = default, Vector3 rot = default, Vector3 scale = default)
    {
        Item obj = this.GetObject(pos : pos, scale : scale, rot : rot);
        obj.quantity = quantity;
        return obj;
    }
}
