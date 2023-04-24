using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : EquipableItem
{
    public Weapon(ItemData data, ItemRarity _rarity) : base(data, _rarity)
    {
        itemData = (WeaponData)base.itemData;

        currentAmmoAmmount = itemData.magazineSize;
    }

    public new WeaponData itemData;

    public int currentAmmoAmmount;
}
