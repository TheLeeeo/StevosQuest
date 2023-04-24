using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplayController : MonoBehaviour
{
    [SerializeField] private Image StatDot;
    [SerializeField] private Text StatName;
    [SerializeField] private Text StatValue;

    public void DisplayStat(ItemStat _stat, float powerMultiplier)
    {
        StatDot.gameObject.SetActive(true);

        StatDot.color = _stat.color;

        StatName.text = _stat.statName;
        StatName.color = _stat.color;

        StatValue.color = _stat.color;

        if (_stat.value != 0)
        {
            StatValue.text = (_stat.value * (_stat.scaleWithPower ? powerMultiplier : 1)).ToString("G");            
        }
        else
        {
            StatValue.text = "";
        }
    }

    public void Clear() //Does not actually clear it but just deactivates it, duh
    {
        StatDot.gameObject.SetActive(false);
    }
}
