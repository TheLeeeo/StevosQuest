using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomWeaponAtStart : MonoBehaviour
{
    [SerializeField]
    private ItemTableBase weaponTable;

    [SerializeField]
    private PickupData[] ammoDatas;

    private void Start()
    {
        Weapon weapon = (Weapon)weaponTable.GetLoot();

        DropController.ReleaseItem(weapon, transform.position);

        DropController.DropPickup(new Pickup(ammoDatas[(int)weapon.itemData.ammoType]), transform.position);
        DropController.DropPickup(new Pickup(ammoDatas[(int)weapon.itemData.ammoType]), transform.position);
        DropController.DropPickup(new Pickup(ammoDatas[(int)weapon.itemData.ammoType]), transform.position);
    }
}
