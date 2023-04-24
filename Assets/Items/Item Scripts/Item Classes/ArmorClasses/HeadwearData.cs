using UnityEngine;

[CreateAssetMenu(fileName = "NewHeadwear", menuName = "Item/Armor/Headwear")]
public class HeadwearData : ArmorData
{
    private void Awake()
    {
        armorType = ArmorType.Headwear;
    }
}
