using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    [SerializeField]
    protected EffectEnum effectID;

    [HideInInspector]
    protected int effectLevel;

    protected EntityController entityController;
    [HideInInspector]
    protected float timeLeft;

    [SerializeField]
    protected new ParticleSystem particleSystem;

    public void UpdateTimeLeft(float newTime)
    {
        if(newTime > timeLeft)
        {
            timeLeft = newTime;
        }        
    }

    public virtual void UpdateEffectLevel(int newLevel)
    {
        if(newLevel > effectLevel)
        {
            effectLevel = newLevel;
        }
    }
    
    public virtual void Activate(EntityController _entityController, float duration, int _effectLevel)
    {
        entityController = _entityController;
        timeLeft = duration;
        effectLevel = _effectLevel;
    }

    public virtual void Deactivate(EntityController _entityController) { }
}
