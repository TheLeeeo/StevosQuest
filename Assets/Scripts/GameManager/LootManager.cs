using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Instance of singleton class \"" + this + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }

        rarityTable = new RandomTable<ItemRarity>(
            new TableItem<ItemRarity>(ItemRarity.Broken, ItemRarityDistrobution[(int)ItemRarity.Broken]),
            new TableItem<ItemRarity>(ItemRarity.Common, ItemRarityDistrobution[(int)ItemRarity.Common]),
            new TableItem<ItemRarity>(ItemRarity.Uncommon, ItemRarityDistrobution[(int)ItemRarity.Uncommon]),
            new TableItem<ItemRarity>(ItemRarity.Rare, ItemRarityDistrobution[(int)ItemRarity.Rare]),
            new TableItem<ItemRarity>(ItemRarity.Epic, ItemRarityDistrobution[(int)ItemRarity.Epic]),
            new TableItem<ItemRarity>(ItemRarity.Legendary, ItemRarityDistrobution[(int)ItemRarity.Legendary])
            );
    }

    private readonly uint[] ItemRarityDistrobution = { 10, 20, 15, 10, 4, 1 };

    private static RandomTable<ItemRarity> rarityTable;

    public static ItemRarity GetRandomRarity()
    {
        return rarityTable.GetItem();
    }
}
