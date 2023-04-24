using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffectBehaviour : EffectBase
{
    [SerializeField]
    private float SpeedModifier;    

    private static readonly Quaternion flippedParticleSystemRotation = Quaternion.Euler(new Vector3(0, 0, 180f));

    public override void Activate(EntityController _entityController, float duration, int _effectLevel)
    {
        base.Activate(_entityController, duration, _effectLevel);

        entityController.movementSpeed += SpeedModifier * Item.RarityPowerMultipliers[effectLevel];

        PlayerController._instance.OnFlip += SetParticleEffectRotation;

        SetParticleEffectRotation(PlayerController._instance.isFacingRight);
    }

    public override void Deactivate(EntityController _entityController) //should not call base, particlesystem handles destruction
    {
        entityController.movementSpeed -= SpeedModifier * Item.RarityPowerMultipliers[effectLevel];
        PlayerController._instance.OnFlip -= SetParticleEffectRotation;

        this.enabled = false;
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    public override void UpdateEffectLevel(int newLevel)
    {
        if (newLevel > effectLevel)
        {
            entityController.movementSpeed -= SpeedModifier * Item.RarityPowerMultipliers[effectLevel];
            effectLevel = newLevel;
            entityController.movementSpeed += SpeedModifier * Item.RarityPowerMultipliers[newLevel];
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

    private void SetParticleEffectRotation(bool isFacingRight)
    {
        transform.rotation = isFacingRight ? Quaternion.identity : flippedParticleSystemRotation;
    }
}
