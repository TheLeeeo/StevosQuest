using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ExplodeOnHit : MonoBehaviour
{
    private BulletCore bulletCore;

    [SerializeField]
    private ParticleSystem explosionParticleSystem;

    [SerializeField]
    private new Rigidbody2D rigidbody;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float range;

    [SerializeField]
    private float knockbackForce;

    [SerializeField]
    private AudioClip sound;

    private const float PlayerDamageReductionMultiplier = 0.25f;

    private void Awake()
    {
        bulletCore = GetComponent<BulletCore>();
    }

    private void OnCollisionEnter2D()
    {
        Explode();
    }
    
    private void OnTriggerEnter2D()
    {
        Explode();
    }

    private void Explode()
    {
        if(explosionParticleSystem != null)
        {
            explosionParticleSystem.Play();
        }
        
        spriteRenderer.enabled = false;
        rigidbody.angularVelocity = 0;
        rigidbody.velocity = Vector2.zero;

        SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
        controller.PlaySoundEffect(sound);

        //soundeffect

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(rigidbody.position, range, CommonLayerMasks.HasHealth);

        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.gameObject.layer == CommonLayerMasks.PlayerLayer) //Player
            {
                PlayerController._instance.health.Damage(Mathf.CeilToInt(bulletCore.weapon.itemData.damage * bulletCore.weapon.powerMultiplier * PlayerController._instance.damageMultiplier * PlayerDamageReductionMultiplier));

                Vector2 direction = PlayerController._instance.playerTransform.position - transform.position;

                PlayerMovement._instance.isAffectedByKnockback = true;
                PlayerController._instance.entityRigidbody.velocity = GetMagnitude(direction.magnitude) * direction.normalized;
            }
            else
            {
                EntityController entityController = enemy.GetComponent<EntityController>();

                entityController.health.Damage(Mathf.CeilToInt(bulletCore.weapon.itemData.damage * bulletCore.weapon.powerMultiplier * PlayerController._instance.damageMultiplier), bulletCore.weapon.itemData.armorPiercing * PlayerController._instance.armorPiercingMultiplier);
            }

        }
    }

    private float GetMagnitude(float distance)
    {
        return knockbackForce / -(range * range) * (distance * distance - range * range);
    }
}
