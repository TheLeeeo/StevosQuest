using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePickupRangeOnActivate : MonoBehaviour
{
    [SerializeField]
    private float defaultModifier;

    private float pickupModifier;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        pickupModifier = defaultModifier * (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? itemCore.item.powerMultiplier : 1);

        PickupController.DetectionRange *= pickupModifier;
        PlayerTriggerHandler._instance.size *= pickupModifier;
    }

    private void Deactivate(EntityController entityController)
    {
        PickupController.DetectionRange /= pickupModifier;
        PlayerTriggerHandler._instance.size /= pickupModifier;
    }
}
