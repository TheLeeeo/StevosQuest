using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDropController : DropControllerBase
{
    [SerializeField]
    private PickupTableBase pickupTable;

    [SerializeField]
    private ItemTableBase itemTable;

    [SerializeField]
    private float DropChance = 1f;

    private void Start()
    {
        if (null == pickupTable)
        {
            pickupTable = LevelData.GetDefaultPickupTable();
        }

        if (null == itemTable)
        {
            itemTable = LevelData.GetDefaultItemTable();
        }
    }

    public override void GenerateDrops()
    {        
        Item dropItem = itemTable?.GetLoot();

        if(dropItem != null && Random.Range(0f,1f) <= DropChance)
        {
            DropController.DropItem(dropItem, transform.position);
        }

        dropItem = pickupTable.GetLoot();

        if (dropItem != null/*&& Random.Range(0f, 1f) <= DropChance*/)
        {
            DropController.DropPickup(dropItem, transform.position);
        }       
    }
}
