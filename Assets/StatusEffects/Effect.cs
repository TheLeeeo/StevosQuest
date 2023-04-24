using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Effect
{
    public const int NumberOfEffectTypes = 22;
    public const int MaxNumberOfActiveEffects = 15; //The highest possible number of effects active at one time //I have fucking lost track

    public EffectData effectData;
    public float Duration;

    /// <summary>
    /// The level of the effect. Determines the effect strength
    /// </summary>
    public ItemRarity EffectLevel = ItemRarity.Common;
}
