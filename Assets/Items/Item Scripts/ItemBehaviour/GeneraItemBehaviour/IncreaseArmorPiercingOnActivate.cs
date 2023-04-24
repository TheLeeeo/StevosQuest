using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseArmorPiercingOnActivate : MonoBehaviour
{
    [SerializeField]
    private float defaultModifier;

    private float armorPiercingModifier;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        armorPiercingModifier = defaultModifier * (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? itemCore.item.powerMultiplier : 1);

        PlayerController._instance.armorPiercingMultiplier *= armorPiercingModifier;
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerController._instance.armorPiercingMultiplier /= armorPiercingModifier;
    }
}
