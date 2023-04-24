using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidWhenCanGetAmmo : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;

    private void Awake()
    {
        GetComponent<PickupCore>().OnValidatePickup += Validate;
    }

    public bool Validate(EntityController _entityController)
    {
        return BulletManager.CanAddBullet(ammoType);
    }
}