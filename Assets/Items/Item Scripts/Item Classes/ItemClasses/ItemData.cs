using UnityEngine;

public enum ItemType
{
    Armor,
    Weapon,
    Trinket,
    Utility,
    Pickup
}

public class ItemData : ScriptableObject
{
    [HideInInspector]
    public ItemType itemType;

    public Sprite itemSprite;

    [SerializeField]
    public string itemName;    

    public GameObject itemObject;

    public int ItemIdentifier;
}
