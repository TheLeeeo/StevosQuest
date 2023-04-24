using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseReloadspeedOnActivate : MonoBehaviour
{
    [SerializeField]
    private float defaultModifier;

    private float reloadTimeMultiplier;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        reloadTimeMultiplier = defaultModifier / (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? (int)itemCore.item.rarity + 1 : 1);

        PlayerController._instance.reloadTimeMultiplier *= reloadTimeMultiplier;
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerController._instance.reloadTimeMultiplier /= reloadTimeMultiplier;
    }
}
