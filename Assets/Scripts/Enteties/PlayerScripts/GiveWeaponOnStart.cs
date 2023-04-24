using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveWeaponOnStart : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private ItemRarity itemRarity;

    private void Start()
    {
        PlayerInventory._instance.AddItem(new Weapon(weaponData, itemRarity));
    }
}
