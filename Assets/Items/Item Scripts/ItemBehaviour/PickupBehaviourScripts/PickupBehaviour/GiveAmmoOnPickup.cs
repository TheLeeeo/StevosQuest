using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAmmoOnPickup : MonoBehaviour
{
    [SerializeField] private AmmoType ammoType;

    private PickupCore pickupCore;

    private void Awake()
    {
        pickupCore = GetComponent<PickupCore>();
        pickupCore.OnActivate += Activate;
    }

    private void Activate(EntityController entityController)
    {
        int bulletsRemaining = BulletManager.AddBulletsOfType(ammoType, pickupCore.item.itemCount);

        if(0 != bulletsRemaining)
        {
            pickupCore.item.itemCount = bulletsRemaining;

            DropController.DropPickup(pickupCore.item, transform.position);
        }
    }
}
