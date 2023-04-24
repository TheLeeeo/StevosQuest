using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingEffectBehaviour : EffectBase
{
    [SerializeField]
    private float SpeedModifier;

    public override void Activate(EntityController _entityController, float duration, int _effectLevel)
    {
        base.Activate(_entityController, duration, _effectLevel);

        entityController.movementSpeed *= SpeedModifier / Item.RarityPowerMultipliers[effectLevel];
    }

    public override void Deactivate(EntityController _entityController) //should not call base, particlesystem handles destruction
    {
        entityController.movementSpeed /= SpeedModifier / Item.RarityPowerMultipliers[effectLevel];

        this.enabled = false;
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    public override void UpdateEffectLevel(int newLevel)
    {
        if (newLevel > effectLevel)
        {
            entityController.movementSpeed /= SpeedModifier / Item.RarityPowerMultipliers[effectLevel];
            effectLevel = newLevel;
            entityController.movementSpeed *= SpeedModifier / Item.RarityPowerMultipliers[newLevel];
        }
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
