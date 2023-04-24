using UnityEngine;

[CreateAssetMenu(fileName = "NewFootwear", menuName = "Item/Armor/Footwear")]
public class FootwearData : ArmorData
{
    private void Awake()
    {
        armorType = ArmorType.Footwear;
    }

    public Sprite footSprite;
}
