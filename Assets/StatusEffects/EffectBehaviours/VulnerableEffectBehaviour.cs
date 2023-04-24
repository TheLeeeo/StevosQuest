using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableEffectBehaviour : EffectBase
{
    [SerializeField]
    private float baseHealthMultiplier;

    float healthMultiplier;

    public override void Activate(EntityController _entityController, float duration, int _effectLevel)
    {
        base.Activate(_entityController, duration, _effectLevel);

        healthMultiplier = baseHealthMultiplier / Item.RarityPowerMultipliers[effectLevel];

        entityController.health.SetNewMaxHealth(Mathf.CeilToInt(entityController.health.MaxHealth * healthMultiplier));        
    }

    public override void Deactivate(EntityController _entityController)
    {
        entityController.health.SetNewMaxHealth(Mathf.FloorToInt(entityController.health.MaxHealth / healthMultiplier));

        Destroy(gameObject);
    }

    public void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            entityController.health.RemoveEffect(effectID);
        }
    }
}
