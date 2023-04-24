using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// Serialized class used to represent an item in the inspector
/// </summary>
public class EquipableReference : ItemReference
{
    [SerializeField]
    private EquipableItemData itemData;    

    [SerializeField]
    private bool randomRarity = true;
    [SerializeField]
    private ItemRarity _definedRarity;
    private ItemRarity GetItemRarity
    {
        get
        {
            return randomRarity ? LootManager.GetRandomRarity() : _definedRarity;
        }
    }

    [SerializeField]
    private int itemCount;

    public override Item GetItem()
    {
        Item returnItem = null;

        switch (itemData?.itemType) //if no itemdata is selected, null will be returned upon and no item will be dropped
        {
            case ItemType.Armor:
                returnItem = new Armor(itemData, GetItemRarity);
                break;
            case ItemType.Weapon:
                returnItem = new Weapon(itemData, GetItemRarity);
                break;
            case ItemType.Trinket:
                returnItem = new Trinket(itemData, GetItemRarity);
                break;
            case ItemType.Utility:
                returnItem = new Utility(itemData, GetItemRarity, itemCount);
                break;
        }

        return returnItem;
    }
}
