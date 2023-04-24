using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum ImmunitiesEnum
{
    Fire = 1 << 0,
    FireResistance = 1 << 1,
    Freezing = 1 << 2,
    FreezingResistance = 1 << 3,
    Poison = 1 << 4,
    PoisonResistance = 1 << 5,
    Bleeding = 1 << 6,
    Regeneration = 1 << 7,
    DamageUp = 1 << 8,
    DamageDown = 1 << 9,
    DefenceUp = 1 << 10,
    DefenceDown = 1 << 11,
    Speed = 1 << 12,
    JumpHeight = 1 << 13,
    FireRateUp = 1 << 14,
    FireRateDown = 1 << 15,
    ArmorPiercingUp = 1 << 16,
    ArmorPiercingDown = 1 << 17,
    Healthy = 1 << 18,
    Vulnerable = 1 << 19,
    AccuracyUp = 1 << 20,
    AccuracyDown = 1 << 21
}

[Serializable]
public struct Immunity
{
    [SerializeField]
    private ImmunitiesEnum immunities;

    public bool IsImmuneTo(EffectEnum type)
    {
        return (immunities & (ImmunitiesEnum)(1 << (int)type)) > 0;
    }

    public void AddImmunity(EffectEnum newImmunity)
    {
        immunities |= (ImmunitiesEnum)(1 << (int)newImmunity);
    }

    public void RemoveImmunity(EffectEnum type)
    {
        immunities &= ~(ImmunitiesEnum)(1 << (int)type);
    }
}
