using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPickupMetaTable", menuName = "RandomTables/MetaTables/PickupMetaTable")]
[System.Serializable]
public class PickupMetaTable : PickupTableBase
{
    [SerializeField]
    private TableItem<PickupLootTable>[] tableReferences;

    private RandomTable<PickupLootTable> lootTable;

    private void OnEnable()
    {
        if (tableReferences != null)
        {
            lootTable = new RandomTable<PickupLootTable>(tableReferences);
        }            
    }

    public override Item GetLoot()
    {
        return lootTable.GetItem().GetLoot();
    }
}
