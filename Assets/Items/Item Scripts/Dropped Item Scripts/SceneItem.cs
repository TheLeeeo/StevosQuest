using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour
{
    public EquipableItemData itemData;

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
    private int itemCount = 1; //if utility

    private void Start()
    {
        Item item = null;

        switch (itemData.itemType)
        {
            case ItemType.Weapon:
                item = new Weapon(itemData, GetItemRarity);
                break;
            case ItemType.Armor:
                item = new Armor(itemData, GetItemRarity);
                break;
            case ItemType.Trinket:
                {
                    ItemRarity itemRarity = GetItemRarity;

                    if (ItemRarity.Broken == itemRarity)
                    {
                        itemRarity = ItemRarity.Common;
                    }
                    item = new Trinket(itemData, itemRarity);
                    break;
                }
            case ItemType.Utility:
                {
                    ItemRarity itemRarity = GetItemRarity;

                    if (ItemRarity.Broken == itemRarity)
                    {
                        itemRarity = ItemRarity.Common;
                    }

                    item = new Utility(itemData, itemRarity, itemCount);
                    break;
                }
        }

        GetComponent<DroppedItemBase>().InstantiateDrop(item);
    }
}
