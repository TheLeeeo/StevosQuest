using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrinketDisplayController : DisplayControllerBase
{
    //[SerializeField] private Text Durability;

    [SerializeField]
    private Sprite DefaultRarityFrame;

    public override void Initiate(Item _displayItem)
    {
        base.Initiate(_displayItem);

        Trinket _displayTrinket = (Trinket)_displayItem;        

        //Durability.text = _displayTrinket.remainingDurability.ToString();

        tooltipText.text = _displayTrinket.itemData.itemTooltip;

        if (false == _displayTrinket.itemData.scalesWithPower)
        {
            ItemFrame.sprite = DefaultRarityFrame;
        }

        DisplayStats(_displayTrinket);
    }

    private void DisplayStats(Trinket _trinket)
    {
        for (int i = 0; i < _trinket.itemData.itemStats.Length; i++)
        {
            statDisplayControllers[i].DisplayStat(_trinket.itemData.itemStats[i], _trinket.powerMultiplier);
        }
    }    
}
