using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pickup : Item
{
    public Pickup(ItemData data) : base(data, ItemRarity.Common)
    {
        itemData = (PickupData)base.itemData;

        itemCount = itemData.baseItemCount;
    }

    public Pickup(ItemData data, int _itemCount) : base(data, ItemRarity.Common)
    {
        itemData = (PickupData)base.itemData;
        itemCount = _itemCount;
    }

    public new PickupData itemData;

    [HideInInspector]
    public int itemCount;
}
