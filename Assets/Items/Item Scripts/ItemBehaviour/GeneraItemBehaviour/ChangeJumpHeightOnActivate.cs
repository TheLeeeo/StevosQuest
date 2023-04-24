using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeJumpHeightOnActivate : MonoBehaviour
{
    [Tooltip("Linnear change in jumpheight")]
    [SerializeField]
    private float defaultModifier;

    private float jumpHeighChange;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        jumpHeighChange = defaultModifier * (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? itemCore.item.powerMultiplier : 1);

        entityController.jumpHeight += jumpHeighChange;
    }

    private void Deactivate(EntityController entityController)
    {
        entityController.jumpHeight -= jumpHeighChange;
    }
}
