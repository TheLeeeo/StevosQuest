using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSlot : ItemSlot
{
    public ItemSlotDragHandler dragHandler;

    public override void SetItem(Item newItem)
    {
        base.SetItem(newItem);
        dragHandler.itemType = (int)newItem.itemData.itemType;
    }

    public override void Clear()
    {
        base.Clear();

        dragHandler.itemType = -1;
    }
}
