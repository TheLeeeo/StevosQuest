using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemTable", menuName = "RandomTables/ItemTable")]
public class ItemLootTable : ItemTableBase
{
    [SerializeField]
    private TableItem<EquipableReference>[] itemReferences;

    private RandomTable<EquipableReference> itemLootTable;

    private void OnEnable()
    {
        if (itemReferences != null)
        {
            itemLootTable = new RandomTable<EquipableReference>(itemReferences);
        }
    }

    public override Item GetLoot()
    {
        return itemLootTable.GetItem().GetItem();
    }
}
