using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armor : EquipableItem
{
    public Armor(ItemData data,ItemRarity _rarity) : base(data, _rarity)
    {
        itemData = (ArmorData)base.itemData;
    }

    public new ArmorData itemData;
}
