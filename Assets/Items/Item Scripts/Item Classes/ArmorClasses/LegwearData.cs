using UnityEngine;

[CreateAssetMenu(fileName = "NewLegwear", menuName = "Item/Armor/Legwear")]
public class LegwearData : ArmorData
{
    private void Awake()
    {
        armorType = ArmorType.Legwear;
    }

    public Sprite upperLegSprite;
    public Sprite lowerLegSprite;
}
