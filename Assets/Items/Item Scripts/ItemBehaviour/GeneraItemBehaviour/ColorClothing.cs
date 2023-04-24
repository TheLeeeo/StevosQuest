using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothColor
{
    Black,
    Red,
    Pink,
    White,
    Orange,
    Green,
    Blue,
    Purple,
    Yellow,
    Cyan    
}

public enum HeadType
{
    none,
    HP,
    Defence,
    Damage
}

[System.Serializable]
public class ClothingData
{
    public ArmorType armorType;
    public ClothColor clothColor;
    public HeadType headType;
}

public class ColorClothing : MonoBehaviour
{
    [SerializeField]
    ClothingData clothingData;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();

        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        PlayerController._instance.SetClothColor(clothingData);
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerController._instance.RemoveClothColor(clothingData.armorType);
    }
}
