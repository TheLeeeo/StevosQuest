using UnityEngine;

[CreateAssetMenu(fileName = "NewUtility", menuName = "Item/Utility")]
public class UtilityData : EquipableItemData
{
    private void Awake()
    {
        itemType = ItemType.Utility;
    }

    public int stackSize;
}
