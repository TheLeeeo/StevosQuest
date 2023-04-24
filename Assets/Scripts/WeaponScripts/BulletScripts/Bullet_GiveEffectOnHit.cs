using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_GiveEffectOnHit : MonoBehaviour
{
    private BulletCore bulletCore;

    [SerializeField]
    private Effect effect;

    private void Awake()
    {
        bulletCore = GetComponent<BulletCore>();
        bulletCore.OnEntityHit += GiveEffect;
    }

    private void GiveEffect(EntityController entityController)
    {
        effect.EffectLevel = bulletCore.weapon.rarity;
        entityController.health.GiveEffect(effect);
    }
}