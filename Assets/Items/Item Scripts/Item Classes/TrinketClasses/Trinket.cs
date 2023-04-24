using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trinket : EquipableItem
{
    public Trinket(ItemData data, ItemRarity _rarity) : base(data, _rarity)
    {
        itemData = (TrinketData)base.itemData;
    }

    public new TrinketData itemData;

    //[HideInInspector] public int remainingDurability;
}
