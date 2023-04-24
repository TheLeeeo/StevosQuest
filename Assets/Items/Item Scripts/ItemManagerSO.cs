using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(fileName = "ItemManager", menuName = "ItemManager", order = 4)]
public class ItemManagerSO : ScriptableObject
{
    public ItemData[] ItemDataArray;

    public ItemData GetItemdataByID(int id)
    {
        return ItemDataArray[id];     
    }

#if UNITY_EDITOR
    public void GenerateDatabase()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(ItemData).Name);  //FindAssets uses tags check documentation for more info

        ItemData[] itemDatas = new ItemData[guids.Length];

        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            itemDatas[i] = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            itemDatas[i].ItemIdentifier = i;

            EditorUtility.SetDirty(itemDatas[i]);
        }

        ItemDataArray = itemDatas;

        EditorUtility.SetDirty(this);
    }
#endif
}
