using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeedOnActivate : MonoBehaviour
{
    [Tooltip("Linnear change in speed")]
    [SerializeField]
    private float defaultModifier;

    private float speedChange;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();

        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();

        speedChange = defaultModifier * (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? itemCore.item.powerMultiplier : 1);

        entityController.movementSpeed += speedChange;
    }

    private void Deactivate(EntityController entityController)
    {
        entityController.movementSpeed -= speedChange;
    }
}
