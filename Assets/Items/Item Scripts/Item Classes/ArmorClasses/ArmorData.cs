using UnityEngine;

public enum ArmorType
{
    Headwear,
    Chestwear,
    Legwear,
    Footwear
}

public class ArmorData : EquipableItemData
{
    private void Awake()
    {
        itemType = ItemType.Armor;
    }

    [HideInInspector]
    public ArmorType armorType;

    public int armor;

    //public int swagFactor;
}
