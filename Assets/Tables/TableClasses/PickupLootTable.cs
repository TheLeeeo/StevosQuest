using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPickupTable", menuName = "RandomTables/PickupTable")]
public class PickupLootTable : PickupTableBase
{
    [SerializeField]
    private TableItem<PickupReference>[] itemReferences;

    private RandomTable<PickupReference> itemLootTable;

    private void OnEnable()
    {
        if (itemReferences != null)
        {
            itemLootTable = new RandomTable<PickupReference>(itemReferences);
        }
    }

    public override Item GetLoot()
    {
        return itemLootTable.GetItem().GetItem();
    }
}
