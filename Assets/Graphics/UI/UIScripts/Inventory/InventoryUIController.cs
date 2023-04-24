using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }

        DropFunctions[0] = DropArmor;
        DropFunctions[1] = DropWeapon;
        DropFunctions[2] = DropTrinket;
        DropFunctions[3] = DropUtility;
    }

    [SerializeField] private InventoryItemSlot[] armorSlots = new InventoryItemSlot[4];
    [SerializeField] private InventoryItemSlot[] weaponSlots = new InventoryItemSlot[5];
    [SerializeField] private InventoryItemSlot[] trinketSlots = new InventoryItemSlot[4];
    [SerializeField] private InventoryItemSlot[] utilitySlots = new InventoryItemSlot[5];

    private Armor[] armors = new Armor[4];
    private Weapon[] weapons = new Weapon[5];
    private Trinket[] trinkets = new Trinket[4];
    private Utility[] utilities = new Utility[5];

    [SerializeField] private RectTransform rectTransform;

    public Action<int>[] DropFunctions = new Action<int>[4];


    public void InspectItem(int itemType, int index)
    {
        if (0 == itemType)
        {
            PlayerUIController._instance.Inspect(armors[index]);
        }
        else if (1 == itemType)
        {
            PlayerUIController._instance.Inspect(weapons[index]);
        }
        else if (2 == itemType)
        {
            PlayerUIController._instance.Inspect(trinkets[index]);
        }
        else if (3 == itemType)
        {
            PlayerUIController._instance.Inspect(utilities[index]);
        }

    }

    #region SetItem
    public void SetItemInSlot(Armor _armor, int slotIndex)
    {
        armors[slotIndex] = _armor;

        armorSlots[slotIndex].SetItem(_armor);
    }

    public void SetItemInSlot(Weapon _weapon, int slotIndex)
    {
        weapons[slotIndex] = _weapon;

        weaponSlots[slotIndex].SetItem(_weapon);
    }

    public void SetItemInSlot(Trinket _trinket, int slotIndex)
    {
        trinkets[slotIndex] = _trinket;

        trinketSlots[slotIndex].SetItem(_trinket);
    }

    public void SetItemInSlot(Utility _utility, int slotIndex)
    {
        utilities[slotIndex] = _utility;

        utilitySlots[slotIndex].SetItem(_utility);
    }
    #endregion SetItem


    #region Drop
    public void DropArmor(int slotIndex)
    {
        PlayerInventory._instance.DropItem(armors[slotIndex]);
        armors[slotIndex] = null;
    }

    public void DropWeapon(int slotIndex)
    {
        PlayerInventory._instance.DropItem(weapons[slotIndex]);
        weapons[slotIndex] = null;
    }

    public void DropTrinket(int slotIndex)
    {
        PlayerInventory._instance.DropItem(trinkets[slotIndex]);
        trinkets[slotIndex] = null;
    }

    public void DropUtility(int slotIndex)
    {
        PlayerInventory._instance.DropItem(utilities[slotIndex]);
        utilities[slotIndex] = null;
    }
    #endregion Drop


    #region Clear
    public void ClearArmor(int slotIndex)
    {
        armors[slotIndex] = null;

        armorSlots[slotIndex].Clear();
    }

    public void ClearWeapon(int slotIndex)
    {
        weapons[slotIndex] = null;

        weaponSlots[slotIndex].Clear();
    }

    public void ClearTrinket(int slotIndex)
    {
        trinkets[slotIndex] = null;

        trinketSlots[slotIndex].Clear();
    }

    public void ClearUtility(int slotIndex)
    {
        utilities[slotIndex] = null;

        utilitySlots[slotIndex].Clear();
    }

    public void ClearAll()
    {
        for (int i = 0; i < 4; i++)
        {
            ClearArmor(i);
            ClearWeapon(i);
            ClearTrinket(i);
            ClearUtility(i);
        }

        ClearWeapon(4);
        ClearUtility(4);
    }
    #endregion Clear
}
