using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : ThrowableController
{
    [SerializeField]
    private new Rigidbody2D rigidbody;

    [SerializeField]
    private int baseDamage;
    private int damage;

    [SerializeField]
    private float baseArmorPiercing;
    private float armorPiercing;

    public override void Initiate(Vector2 velocityVector, UtilityCore core)
    {
        rigidbody.velocity = velocityVector;

        damage = Mathf.CeilToInt(baseDamage * core.item.powerMultiplier);
        armorPiercing = baseArmorPiercing * PlayerController._instance.armorPiercingMultiplier;
    }

    public void AttackEntity(EntityController entity)
    {
        entity.health.Damage(damage, armorPiercing);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D()
    {
        Destroy(gameObject);
    }
}
