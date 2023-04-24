using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity
{
    Broken,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Item
{
    public static readonly float[] RarityPowerMultipliers = { 0.75f, 1f, 1.25f, 1.5f, 1.75f, 2f};

    public Item(Item item)
    {
        itemData = item.itemData;
        powerMultiplier = item.powerMultiplier;
        rarity = item.rarity;
    }

    public Item(ItemData data, ItemRarity _rarity)
    {
        itemData = data;
        powerMultiplier = LevelData.GetPowerMultiplier() * RarityPowerMultipliers[(int)_rarity];
        rarity = _rarity;
    }

    public ItemData itemData { get; private set; }

    public float powerMultiplier { get; protected set; } = 1;

    public ItemRarity rarity { get; private set; }
}
