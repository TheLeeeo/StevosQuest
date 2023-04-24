using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeJumpCountOnActivate : MonoBehaviour
{
    [Tooltip("Linnear change in jumpcount")]
    [SerializeField]
    private float defaultModifier;

    private int jumpCountChange;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        jumpCountChange = (int)(defaultModifier + (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? ((int)itemCore.item.rarity - 1) : 0));

        PlayerMovement._instance.maxNumberOfJumps += jumpCountChange;
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerMovement._instance.maxNumberOfJumps -= jumpCountChange;
    }
}
