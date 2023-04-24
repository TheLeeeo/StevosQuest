using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseExtraBulletsOnActivate : MonoBehaviour
{
    [SerializeField]
    private int defaultModifier;

    private int extraBullets;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        ItemCore itemCore = GetComponent<ItemCore>();
        extraBullets = defaultModifier + (((EquipableItemData)itemCore.item.itemData).scalesWithPower ? (int)itemCore.item.rarity : 0);

        PlayerController._instance.extraBullets += extraBullets;
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerController._instance.extraBullets -= extraBullets;
    }
}
