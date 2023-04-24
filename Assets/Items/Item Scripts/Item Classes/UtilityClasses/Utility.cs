using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Utility : EquipableItem
{
    public Utility(ItemData data, ItemRarity _rarity, int _itemCount) : base(data, _rarity)
    {
        itemData = (UtilityData)base.itemData;

        itemCount = _itemCount;
    }

    public new UtilityData itemData;

    public int itemCount;
}
