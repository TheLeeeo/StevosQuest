using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyUpEffectBehaviour : EffectBase
{
    [SerializeField]
    private float bulletSpreadModifier;

    public override void Activate(EntityController _entityController, float duration, int _effectLevel)
    {
        base.Activate(_entityController, duration, _effectLevel);

        PlayerController._instance.bulletspreadMultiplier *= bulletSpreadModifier / Item.RarityPowerMultipliers[effectLevel];
    }

    public override void Deactivate(EntityController _entityController)
    {
        PlayerController._instance.bulletspreadMultiplier /= bulletSpreadModifier / Item.RarityPowerMultipliers[effectLevel];

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
