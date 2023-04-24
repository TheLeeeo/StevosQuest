using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityTable", menuName = "RandomTables/EntityTable")]
public class EntityTable : ScriptableObject
{
    [SerializeField]
    private TableItem<GameObject>[] PrefabReferences;

    private RandomTable<GameObject> entityTable;

    private void OnEnable()
    {
        if (PrefabReferences != null)
        {
            entityTable = new RandomTable<GameObject>(PrefabReferences);
        }
    }

    public GameObject GetEntity()
    {
        return entityTable.GetItem();
    }
}
