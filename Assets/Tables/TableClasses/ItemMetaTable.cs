using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemMetaTable", menuName = "RandomTables/MetaTables/ItemMetaTable")]
[System.Serializable]
public class ItemMetaTable : ItemTableBase
{
    [SerializeField]
    private TableItem<ItemTableBase>[] tableReferences;

    private RandomTable<ItemTableBase> lootTable;

    private void OnEnable()
    {
        if(tableReferences != null)
        {
            lootTable = new RandomTable<ItemTableBase>(tableReferences);
        }        
    }

    public override Item GetLoot()
    {
        return lootTable.GetItem()?.GetLoot();
    }
}
