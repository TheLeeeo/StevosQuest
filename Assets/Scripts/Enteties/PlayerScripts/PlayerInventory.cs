using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory _instance;

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
    }

    public ArmorController armorController;

    private Armor[] armors = new Armor[4];
    private Weapon[] weapons = new Weapon[5];
    private Trinket[] trinkets = new Trinket[4];
    private Utility[] utilities = new Utility[5];

    public Armor[] GetArmors()
    {
        return armors;
    }

    public Weapon[] GetWeapons()
    {
        return weapons;
    }

    public Trinket[] GetTrinkets()
    {
        return trinkets;
    }

    public Utility[] GetUtilities()
    {
        return utilities;
    }

    [HideInInspector]
    public Item ActiveItem;

    #region SwapItems
    public void Swap(int itemType, int index_1, int index_2)
    {
        if(1 == itemType) //Weapon
        {
            SwapSpecialWeapons();
        }
        else if (2 == itemType) //Trinket
        {
            SwapTrinkets(index_1, index_2);
        }
        else //Utility
        {
            SwapUtilities(index_1, index_2);
        }
    }

    public void SwapSpecialWeapons()
    {
        Weapon temp = weapons[3];
        weapons[3] = weapons[4];
        weapons[4] = temp;

        if(weapons[3] != null)
        {
            InventoryUIController._instance.SetItemInSlot(weapons[3], 3);
            ToolbarUIController._instance.SetItemInSlot(weapons[3], 3);
        } else
        {
            InventoryUIController._instance.ClearWeapon(3);
            ToolbarUIController._instance.ClearSlot(3);
        }
        if (weapons[3] != null)
        {
            InventoryUIController._instance.SetItemInSlot(weapons[4], 4);
            ToolbarUIController._instance.SetItemInSlot(weapons[4], 4);
        } else
        {
            InventoryUIController._instance.ClearWeapon(4);
            ToolbarUIController._instance.ClearSlot(4);
        }
    }

    public void SwapTrinkets(int index_1, int index_2)
    {
        Trinket temp = trinkets[index_1];
        trinkets[index_1] = trinkets[index_2];
        trinkets[index_2] = temp;

        if (trinkets[index_1] != null)
        {
            InventoryUIController._instance.SetItemInSlot(trinkets[index_1], index_1);
        } else
        {
            InventoryUIController._instance.ClearTrinket(index_1);
        }
        InventoryUIController._instance.SetItemInSlot(trinkets[index_2], index_2);
    }

    public void SwapUtilities(int index_1, int index_2)
    {
        Utility temp = utilities[index_1];
        utilities[index_1] = utilities[index_2];
        utilities[index_2] = temp;

        if(utilities[index_1] != null)
        {
            InventoryUIController._instance.SetItemInSlot(utilities[index_1], index_1);
            
            ToolbarUIController._instance.SetItemInSlot(utilities[index_1], 5 + index_1);
        }
        else
        {
            InventoryUIController._instance.ClearUtility(index_1);
            ToolbarUIController._instance.ClearSlot(5 + index_1);
        }
        InventoryUIController._instance.SetItemInSlot(utilities[index_2], index_2);
        ToolbarUIController._instance.SetItemInSlot(utilities[index_2], 5 + index_2);
    }
    #endregion SpawItems


    #region AddItem
    /// <summary>
    /// Tries to add an item to the players inventory.
    /// </summary>
    public void AddItem(Item _newItem)
    {
        switch (_newItem.itemData.itemType)
        {
            case ItemType.Armor:
                AddItem((Armor)_newItem);
                break;

            case ItemType.Weapon:
                AddItem((Weapon)_newItem);
                break;

            case ItemType.Trinket:
                AddItem((Trinket)_newItem);
                break;

            case ItemType.Utility:
                AddItem((Utility)_newItem);
                break;
        }
    }

    public void AddItem(Armor _newArmor)
    {
        if(armors[(int)_newArmor.itemData.armorType] == null)
        {
            armors[(int)_newArmor.itemData.armorType] = _newArmor;

            InventoryUIController._instance.SetItemInSlot(_newArmor, (int)_newArmor.itemData.armorType);
            armorController.Equip(_newArmor);
        } 
        else
        {
            PlayerUIController._instance.Compare(armors[(int)_newArmor.itemData.armorType], _newArmor);
        }
    }

    public void AddItem(Weapon _newWeapon)
    {
        if(WeaponType.Special == _newWeapon.itemData.weaponType)
        {
            if(null == weapons[3])
            {
                weapons[3] = _newWeapon;

                ToolbarUIController._instance.SetItemInSlot(_newWeapon, 3);
                InventoryUIController._instance.SetItemInSlot(_newWeapon, 3);
            }
            else if (null == weapons[4])
            {
                weapons[4] = _newWeapon;

                ToolbarUIController._instance.SetItemInSlot(_newWeapon, 4);
                InventoryUIController._instance.SetItemInSlot(_newWeapon, 4);
            }
            else
            {
                PlayerUIController._instance.Compare(new Weapon[] { weapons[3], weapons[4] }, _newWeapon);
            }
        }
        else if (null == weapons[(int)_newWeapon.itemData.weaponType])
        {
            weapons[(int)_newWeapon.itemData.weaponType] = _newWeapon;

            ToolbarUIController._instance.SetItemInSlot(_newWeapon, (int)_newWeapon.itemData.weaponType);
            InventoryUIController._instance.SetItemInSlot(_newWeapon, (int)_newWeapon.itemData.weaponType);
        }
        else
        {
            PlayerUIController._instance.Compare(weapons[(int)_newWeapon.itemData.weaponType], _newWeapon);
        }
    }

    public void AddItem(Trinket _newTrinket)
    {
        for (int i = 0; i < 4; i++)
        {
            if (null == trinkets[i])
            {
                trinkets[i] = _newTrinket;
                PlayerEquipedItemsController._instance.EquipTrinket(_newTrinket);
                InventoryUIController._instance.SetItemInSlot(_newTrinket, i);

                return;
            }
        }

        //No slot avaliable
        PlayerUIController._instance.Compare(trinkets, _newTrinket);
    }

    public void AddItem(Utility _newUtility)
    {
        for (int i = 0; i < 5; i++)
        {
            if (null == utilities[i]) //avaliable slot
            {
                utilities[i] = _newUtility;

                ToolbarUIController._instance.SetItemInSlot(_newUtility, 5 + i);
                InventoryUIController._instance.SetItemInSlot(_newUtility, i);

                return;
            }
            else if (utilities[i].itemData == _newUtility.itemData) //utility of same type exists
            {
                if (utilities[i].rarity == _newUtility.rarity) //the utility has the same  rarity
                {
                    utilities[i].itemCount += _newUtility.itemCount;

                    if (utilities[i].itemCount > utilities[i].itemData.stackSize)
                    {                        
                        int leftoverCount = utilities[i].itemCount - utilities[i].itemData.stackSize;
                        utilities[i].itemCount -= leftoverCount;

                        DropController.DropItem(new Utility(_newUtility.itemData, _newUtility.rarity, _newUtility.itemCount), transform.position);
                    }

                    ToolbarUIController._instance.UpdateUtilityCount(5 + i, utilities[i].itemCount);
                }
                else //the utility does not have the same rarity
                {
                    PlayerUIController._instance.Compare(utilities[i], _newUtility);
                }

                return;
            }
        }

        //No slot avaliable
        PlayerUIController._instance.Compare(utilities, _newUtility);
    }

    public void AddPickup(Pickup _newPickup)
    {
        PickupCore pickupCore = Instantiate(_newPickup.itemData.itemObject, transform).GetComponent<PickupCore>();
        pickupCore.item = _newPickup;
        pickupCore.Activate(PlayerController._instance);
    }
    #endregion AddItem

    public void DropItem(Item _itemToDrop)
    {
        DropController.DropItem(_itemToDrop, transform.position);

        switch (_itemToDrop.itemData.itemType)
        {
            case ItemType.Armor:
                Armor _armor = (Armor)_itemToDrop;

                armors[(int)(_armor.itemData).armorType] = null;

                armorController.UnEquip(_armor);

                InventoryUIController._instance.ClearArmor((int)((ArmorData)_armor.itemData).armorType);
                break;

            case ItemType.Weapon:

                if(_itemToDrop == ActiveItem)
                {
                    PlayerEquipedItemsController._instance.DeactivateItem();
                    ActiveItem = null;
                }

                for (int i = 0; i < 5; i++)
                {
                    if (weapons[i] == _itemToDrop)
                    {
                        InventoryUIController._instance.ClearWeapon(i);
                        ToolbarUIController._instance.ClearSlot(i);

                        weapons[i] = null;
                    }
                }

                break;

            case ItemType.Trinket:

                for (int i = 0; i < 4; i++)
                {
                    if (trinkets[i] == _itemToDrop)
                    {
                        PlayerEquipedItemsController._instance.RemoveTrinket((Trinket)_itemToDrop);
                        InventoryUIController._instance.ClearTrinket(i);
                        trinkets[i] = null;
                    }
                }

                break;

            case ItemType.Utility:

                if (_itemToDrop == ActiveItem)
                {
                    PlayerEquipedItemsController._instance.DeactivateItem();
                    ActiveItem = null;
                }

                for (int i = 0; i < 5; i++)
                {
                    if (utilities[i] == _itemToDrop)
                    {
                        InventoryUIController._instance.ClearUtility(i);
                        ToolbarUIController._instance.ClearSlot(5 + i);
                        utilities[i] = null;
                    }
                }

                break;
        }
    }

    public void UseUtility(Utility utility)
    {
        for (int i = 0; i < utilities.Length; i++)
        {
            if (utilities[i] == utility)
            {
                utility.itemCount -= 1;

                ToolbarUIController._instance.UpdateUtilityCount(5 + i, utility.itemCount);

                if (0 == utility.itemCount)
                {
                    DestroyUtility(i);
                }
            }
        }
    }

    /// <summary>
    /// Called by UtilityCore when the itemcount reaches 0 and the equiped utility should be destroyed
    /// </summary>
    public void DestroyUtility(int index)
    {
        ToolbarUIController._instance.ClearSlot(5 + index);
        ToolbarUIController._instance.DeactivateCurrentSlot();
        PlayerEquipedItemsController._instance.DeactivateItem();
        InventoryUIController._instance.ClearUtility(index);

        utilities[index] = null;
    }
}