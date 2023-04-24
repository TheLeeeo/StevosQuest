using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAccuracyOnActivate : MonoBehaviour
{
    [SerializeField]
    private float defaultModifier;

    private float bulletspreadModifier;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        bulletspreadModifier = defaultModifier * (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? itemCore.item.powerMultiplier : 1);

        PlayerController._instance.bulletspreadMultiplier *= bulletspreadModifier;
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerController._instance.bulletspreadMultiplier /= bulletspreadModifier;
    }
}
