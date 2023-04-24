using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSpikeBallController : ThrowableController
{
    [SerializeField]
    private new Rigidbody2D rigidbody;

    [SerializeField]
    private float rotationalSpeed;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int baseDamage;
    private int damage;
    [SerializeField]
    private float baseArmorPiercing;
    private float armorPiercing;

    public override void Initiate(Vector2 velocityVector, UtilityCore core)
    {
        damage = Mathf.CeilToInt(baseDamage * core.item.powerMultiplier);
        armorPiercing = baseArmorPiercing * PlayerController._instance.armorPiercingMultiplier;

        rigidbody.velocity = velocityVector * speed;
        rigidbody.angularVelocity = rotationalSpeed * -Mathf.Sign(velocityVector.x);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<EntityController>().health.Damage(damage, armorPiercing);
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody.velocity.x) < 0.1f)
        {
            rigidbody.angularVelocity = 0;
            Destroy(gameObject);
        }
    }
}
