using UnityEngine;

public class ArmorController : MonoBehaviour
{
    #region Agent body
    //Head
    [SerializeField]
    private SpriteRenderer AgentHead;

    //Chest + Shoulders
    [SerializeField]
    private SpriteRenderer AgentChest;
    [SerializeField]
    private SpriteRenderer AgentShoulder_1;
    [SerializeField]
    private SpriteRenderer AgentShoulder_2;

    //Legs
    [SerializeField]
    private SpriteRenderer AgentUpperLeg_1;
    [SerializeField]
    private SpriteRenderer AgentUpperLeg_2;
    [SerializeField]
    private SpriteRenderer AgentLowerLeg_1;
    [SerializeField]
    private SpriteRenderer AgentLowerLeg_2;

    //Feet
    [SerializeField]
    private SpriteRenderer AgentFoot_1;
    [SerializeField]
    private SpriteRenderer AgentFoot_2;
    #endregion

    #region Armor data
    private Sprite OriginalHeadwear;

    private Sprite OriginalChestwear;
    private Sprite OriginalShoulderwear;

    private Sprite OriginalUpperLegwear;
    private Sprite OriginalLowerLegwear;

    private Sprite OriginalFootwear;
    #endregion

    [SerializeField]
    private EntityController entityController;

    GameObject[] armorObjects = new GameObject[4];


    public void Start()
    {
        OriginalHeadwear = AgentHead.sprite;
        OriginalChestwear = AgentChest.sprite;
        OriginalShoulderwear = AgentShoulder_1.sprite;
        OriginalUpperLegwear = AgentUpperLeg_1.sprite;
        OriginalLowerLegwear = AgentLowerLeg_1.sprite;
        OriginalFootwear = AgentFoot_1.sprite;
    }

    public (Sprite head, Sprite chest, Sprite shoulder) GetPlayerSprites()
    {
        return (AgentHead.sprite, AgentChest.sprite, AgentShoulder_1.sprite);
    }


    public void Equip(Armor _newArmor)
    {
        switch (_newArmor.itemData.armorType)
        {
            case ArmorType.Headwear:
                Equip((HeadwearData)_newArmor.itemData);
                break;

            case ArmorType.Chestwear:
                Equip((ChestwearData)_newArmor.itemData);
                break;

            case ArmorType.Legwear:
                Equip((LegwearData)_newArmor.itemData);
                break;

            case ArmorType.Footwear:
                Equip((FootwearData)_newArmor.itemData);
                break;
        }

        armorObjects[(int)_newArmor.itemData.armorType] = Instantiate(_newArmor.itemData.itemObject, transform);
        
        ArmorCore core = armorObjects[(int)_newArmor.itemData.armorType].GetComponent<ArmorCore>();
        core.item = _newArmor;
        core.GetComponent<ItemCore>().item = _newArmor; //workaround?
        core.Activate(entityController);
    }

    private void Equip(HeadwearData _headwear)
    {
        AgentHead.sprite = _headwear.itemSprite;
    }

    private void Equip(ChestwearData _chestWear)
    {
        AgentChest.sprite = _chestWear.chestSprite;
        AgentShoulder_1.sprite = AgentShoulder_2.sprite = _chestWear.shoulderSprite;
    }

    private void Equip(LegwearData _legwear)
    {
        AgentUpperLeg_1.sprite = AgentUpperLeg_2.sprite = _legwear.upperLegSprite;
        AgentLowerLeg_1.sprite = AgentLowerLeg_2.sprite = _legwear.lowerLegSprite;
    }

    private void Equip(FootwearData _footwear)
    {
        AgentFoot_1.sprite = AgentFoot_2.sprite = _footwear.footSprite;
    }


    public void UnEquip(Armor _oldArmor)
    {
        switch (_oldArmor.itemData.armorType)
        {
            case ArmorType.Headwear:
                RemoveHeadwear();
                break;

            case ArmorType.Chestwear:
                RemoveChestwear();
                break;

            case ArmorType.Legwear:
                RemoveLegwear();
                break;

            case ArmorType.Footwear:
                RemoveFootwear();
                break;
        }

        armorObjects[(int)_oldArmor.itemData.armorType].GetComponent<ItemCore>().Deactivate();
        armorObjects[(int)_oldArmor.itemData.armorType] = null;
    }



    private void RemoveHeadwear()
    {
        AgentHead.sprite = OriginalHeadwear;
    }

    private void RemoveChestwear()
    {
        AgentChest.sprite = OriginalChestwear;
        AgentShoulder_1.sprite = AgentShoulder_2.sprite = OriginalShoulderwear;
    }

    private void RemoveLegwear()
    {
        AgentUpperLeg_1.sprite = AgentUpperLeg_2.sprite = OriginalUpperLegwear;
        AgentLowerLeg_1.sprite = AgentLowerLeg_2.sprite = OriginalLowerLegwear;
    }

    private void RemoveFootwear()
    {
        AgentFoot_1.sprite = AgentFoot_2.sprite = OriginalFootwear;
    }
}
