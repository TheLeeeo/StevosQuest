using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectEnum
{
    Fire,
    FireResistance,
    Freezing,
    FreezingResistance,
    Poison,
    PoisonResistance,
    Bleeding,
    Regeneration,
    DamageUp,
    DamageDown,
    DefenceUp,
    DefenceDown,
    Speed,
    JumpHeight,
    FireRateUp,
    FireRateDown,
    ArmorPiercingUp,
    ArmorPiercingDown,
    Healthy,
    Vulnerable,
    AccuracyUp,
    AccuracyDown
}

[CreateAssetMenu(fileName = "NewEffectData", menuName = "EffectData")]
public class EffectData : ScriptableObject
{
    public string EffectName;
    public string ToolTip;
    public Sprite EffectSprite;
    public bool IsNegative;
    public EffectEnum EffectType;

    public GameObject EffectObject;
}
