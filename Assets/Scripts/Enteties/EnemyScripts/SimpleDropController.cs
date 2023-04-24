using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDropController : DropControllerBase
{
    [SerializeField]
    private PickupTableBase pickupTable;

    [SerializeField]
    private TableItem<EquipableReference>[] itemReferences;

    private RandomTable<EquipableReference> itemLootTable;

    [SerializeField]
    private float DropChance = 1f;

    private void Start()
    {
        if (null == pickupTable)
        {
            pickupTable = LevelData.GetDefaultPickupTable();
        }

        if (itemReferences.Length != 0)
        {
            itemLootTable = new RandomTable<EquipableReference>(itemReferences);
        }
    }

    public override void GenerateDrops()
    {
        Item dropItem = itemLootTable?.GetItem().GetItem(); ;

        if (dropItem != null && Random.Range(0f, 1f) <= DropChance)
        {
            DropController.DropItem(dropItem, transform.position);
        }

        dropItem = pickupTable.GetLoot();

        if (dropItem != null && Random.Range(0f, 1f) <= DropChance)
        {
            DropController.DropPickup(dropItem, transform.position);
        }
    }
}
