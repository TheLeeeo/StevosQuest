using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TableItem<T>
{
    public TableItem(T _item, uint _weight)
    {
        item = _item;
        weight = _weight;
    }

    public T item;

    public uint weight;
}

public class RandomTable<T>
{
    public RandomTable(params TableItem<T>[] tableItems)
    {
        items = tableItems;

        SetupTable();
    }

    private TableItem<T>[] items;

    private uint weightSum;

    public void SetupTable()
    {
        SortTable();
        CompileTable();
    }

    private void CompileTable()
    {
        for (uint i = 0; i < items.Length; i++)
        {
            weightSum += items[i].weight;
        }
    }

    private void SortTable()
    {
        TableItem<T> holder;

        for (int i = items.Length; i > 0; i--)
        {
            for (int j = 1; j < items.Length; j++)
            {
                if (items[j].weight > items[j - 1].weight)
                {
                    holder = items[j - 1];
                    items[j - 1] = items[j];
                    items[j] = holder;
                }
            }
        }       
    }

    public T GetItem()
    {
        uint randomNumber = (uint)Random.Range(0, (int)weightSum);
        uint currentWeightSum = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (randomNumber < currentWeightSum + items[i].weight)
            {
                return items[i].item;
            }
            else
            {
                currentWeightSum += items[i].weight;
            }
        }

        throw new System.Exception("WTFFFFFFFF"); //Weights are probably zero or there is no item
    }
}
