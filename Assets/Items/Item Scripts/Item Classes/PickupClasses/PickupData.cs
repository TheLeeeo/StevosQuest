using UnityEngine;

[CreateAssetMenu(fileName = "NewPickup", menuName = "Item/Pickup")]
public class PickupData : ItemData
{
    private void Awake()
    {
        itemType = ItemType.Pickup;
    }

    [Tooltip("How many \"Sub-items\" the item consists of")]
    public int baseItemCount;
}
