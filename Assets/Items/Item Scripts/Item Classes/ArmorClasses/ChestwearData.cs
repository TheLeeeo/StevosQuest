using UnityEngine;

[CreateAssetMenu(fileName = "NewChestwear", menuName = "Item/Armor/Chestwear")]
public class ChestwearData : ArmorData
{
    private void Awake()
    {
        armorType = ArmorType.Chestwear;
    }

    public Sprite chestSprite;
    public Sprite shoulderSprite;
}
