using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationEffectBehaviour : EffectBase
{
    [SerializeField]
    private float interval;

    private float timer;

    public override void Activate(EntityController _entityController, float duration, int _effectLevel)
    {
        base.Activate(_entityController, duration, _effectLevel);

        timer = interval;
    }

    public override void Deactivate(EntityController _entityController)
    {
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        this.enabled = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        timeLeft -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = interval + timer;

            entityController.health.Heal(Mathf.CeilToInt(entityController.health.MaxHealth * (0.05f * Item.RarityPowerMultipliers[effectLevel])));
        }
        if (timeLeft <= 0)
        {
            entityController.health.RemoveEffect(effectID);
        }
    }
}
