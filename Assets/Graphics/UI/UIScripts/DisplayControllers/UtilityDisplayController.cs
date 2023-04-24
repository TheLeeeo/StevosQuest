using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilityDisplayController : DisplayControllerBase
{
    [SerializeField]
    private Text ItemCount;

    [SerializeField]
    private Sprite DefaultRarityFrame;

    public override void Initiate(Item _displayItem)
    {
        base.Initiate(_displayItem);

        Utility _displayUtility = (Utility)_displayItem;      

        ItemCount.text = _displayUtility.itemCount.ToString();

        tooltipText.text = _displayUtility.itemData.itemTooltip;

        if (false == _displayUtility.itemData.scalesWithPower)
        {
            ItemFrame.sprite = DefaultRarityFrame;
        }

        DisplayStats(_displayUtility);        
    }

    private void DisplayStats(Utility _utility)
    {
        for (int i = 0; i < _utility.itemData.itemStats.Length; i++)
        {
            statDisplayControllers[i].DisplayStat(_utility.itemData.itemStats[i], _utility.powerMultiplier);
        }
    }
}
