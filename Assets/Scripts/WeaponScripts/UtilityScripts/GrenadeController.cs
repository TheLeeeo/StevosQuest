using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : ThrowableController
{
    public float explosionTime;
    private float timer;

    [SerializeField]
    private new Rigidbody2D rigidbody;

    //[SerializeField]
    //private SpriteRenderer spriteRenderer;

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

    [SerializeField]
    private AudioClip sound;

    [SerializeField] private Effect effectToGive;

    public override void Initiate(Vector2 velocityVector, UtilityCore core)
    {
        rigidbody.velocity = velocityVector;
        timer = explosionTime;

        damage = Mathf.CeilToInt(baseDamage * core.item.powerMultiplier * LevelData.GetPowerMultiplier());
        armorPiercing = baseArmorPiercing * PlayerController._instance.armorPiercingMultiplier;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Explode();
            this.enabled = false;
        }
    }

    public void Explode()
    {
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity); //NOTE: If the particle effect was on this object and other grenades were attached neither would get deleted

        SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
        controller.PlaySoundEffect(sound);

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(rigidbody.position, range, CommonLayerMasks.HasHealth);

        foreach(Collider2D enemy in enemiesHit)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.Damage(damage, armorPiercing);

            if(null != effectToGive.effectData)
            {
                enemyHealth.GiveEffect(effectToGive);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
