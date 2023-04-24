using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMineController : ThrowableController
{
    [SerializeField] private new Rigidbody2D rigidbody;

    [SerializeField] private GameObject proximityCheck;

    [SerializeField]
    private GameObject particleEffectPrefab;

    [SerializeField]
    private int baseDamage;
    private int damage;
    [SerializeField]
    private float baseArmorPiercing;
    private float armorPiercing;

    [SerializeField]
    private float range;

    public override void Initiate(Vector2 velocityVector, UtilityCore core)
    {
        rigidbody.velocity = velocityVector;

        damage = Mathf.CeilToInt(baseDamage * core.item.powerMultiplier);
        armorPiercing = baseArmorPiercing * PlayerController._instance.armorPiercingMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(collision.transform);
        transform.up = collision.GetContact(0).normal;

        proximityCheck.SetActive(true);

        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0;
    }

    public void Explode()
    {
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity); //NOTE: If the particle effect was on this object and other grenades were attached neither would get deleted

        //soundeffect

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(rigidbody.position, range, CommonLayerMasks.HasHealth);

        foreach (Collider2D enemy in enemiesHit)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.Damage(damage, armorPiercing);
        }

        Destroy(gameObject);
    }
}
