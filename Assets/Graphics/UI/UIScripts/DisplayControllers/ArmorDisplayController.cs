using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArmorDisplayController : DisplayControllerBase
{
    [SerializeField]
    private Text ArmorText;

    [SerializeField]
    private Sprite DefaultRarityFrame;

    public override void Initiate(Item _displayItem)
    {
        base.Initiate(_displayItem);

        Armor _displayArmor = (Armor)_displayItem;

        ArmorText.text = Mathf.Ceil(_displayArmor.itemData.armor * _displayArmor.powerMultiplier).ToString();

        tooltipText.text = _displayArmor.itemData.itemTooltip;

        if (false == _displayArmor.itemData.scalesWithPower)
        {
            ItemFrame.sprite = DefaultRarityFrame;
        }

        DisplayStats(_displayArmor);
    }

    private void DisplayStats(Armor _armor)
    {
        for (int i = 0; i < _armor.itemData.itemStats.Length; i++)
        {
            statDisplayControllers[i].DisplayStat(_armor.itemData.itemStats[i], _armor.powerMultiplier);
        }
    }
}
