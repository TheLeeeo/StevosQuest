using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemController : DroppedItemBase, IIsInteractable
{
    public void PlayerInRange()
    {
        InteractTextController._instance.SetupText(transform.position, "Pickup");
    }

    public void PlayerInteraction()
    {
        PlayerInventory._instance.AddItem(droppedItem);
        Destroy(gameObject);
    }
}
