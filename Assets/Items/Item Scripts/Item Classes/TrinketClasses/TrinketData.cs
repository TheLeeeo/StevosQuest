using UnityEngine;

[CreateAssetMenu(fileName = "NewTrinket", menuName = "Item/Trinket")]
public class TrinketData : EquipableItemData
{
    private void Awake()
    {
        itemType = ItemType.Trinket;
    }

    public int fullDurability;
}
