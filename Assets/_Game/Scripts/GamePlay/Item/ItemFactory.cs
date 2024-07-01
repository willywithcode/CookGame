using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemFactory : AFactory<Item>
{
    public Item GetObject(int quantity)
    {
        Item obj = this.GetObject();
        obj.quantity = quantity;
        return obj;
    }
}
