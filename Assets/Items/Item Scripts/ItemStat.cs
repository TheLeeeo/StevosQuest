using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public static class StatColors
{
    public static Color Default = Color.grey;
}*/

[System.Serializable]
public class ItemStat
{
    public ItemStat(Color _color, string _statName, float _value, bool _scaleWithPower)
    {
        color = _color;
        statName = _statName;
        value = _value;

        scaleWithPower = _scaleWithPower;
    }

    public Color color = Color.white;
    public string statName;
    public float value;
    public bool scaleWithPower = true;
}
