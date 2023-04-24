using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_DamageOnHit: MonoBehaviour
{
    private BulletCore bulletCore;

    private void Awake()
    {
        bulletCore = GetComponent<BulletCore>();
        bulletCore.OnEntityHit += GiveDamage;
    }

    private void GiveDamage(EntityController entityController)
    {
        entityController.health.Damage(Mathf.CeilToInt(bulletCore.weapon.itemData.damage * bulletCore.weapon.powerMultiplier * PlayerController._instance.damageMultiplier), bulletCore.weapon.itemData.armorPiercing * PlayerController._instance.armorPiercingMultiplier);
    }
}
