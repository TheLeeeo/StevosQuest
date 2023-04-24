using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMagazineSizeOnActivate : MonoBehaviour
{
    [SerializeField]
    private float defaultModifier;

    private float sizeChange;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        sizeChange = defaultModifier * (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? itemCore.item.powerMultiplier : 1);

        PlayerController._instance.magazineSizeMultiplier *= sizeChange;

        GameManager._instance.CurrentWeapon?.CalculateMagazineSize();
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerController._instance.magazineSizeMultiplier /= sizeChange;

        GameManager._instance.CurrentWeapon?.CalculateMagazineSize();
    }
}
