using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    public static PlayerController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    public System.Action<bool> OnFlip;

    public Transform playerTransform;

    public CompositeCollider2D playerCollider;

    public EquipedItemsController equipedItemsController;

    public float bulletspreadMultiplier; //lower is better
    public float damageMultiplier; //higher is better
    public float armorPiercingMultiplier; //higher is better, is a modifier so 1 is no change
    public int extraBullets; //Extra bullets in a shotgun shell
    public float firerateModifier; //lower is better;
    public float reloadTimeMultiplier; //lower is better
    public float magazineSizeMultiplier; //higher is better

    public override void Flip()
    {
        equipedItemsController.SwapArm();

        base.Flip();

        if(OnFlip != null)
        {
            OnFlip(isFacingRight);
        }
    }

    private ClothingData[] clothingDatas = new ClothingData[4];
    private int clothCount;
    private bool buffActive = false;

    private const int HP_BOOST = 100;
    private const int ARMOR_BOOST = 100;
    private const float DAMAGE_BOOST = 2f;

    public void SetClothColor(ClothingData clothingData)
    {
        clothingDatas[(int)clothingData.armorType] = clothingData;
        clothCount++;

        if(clothCount == 4 && clothingDatas[0].clothColor == clothingDatas[1].clothColor && clothingDatas[1].clothColor == clothingDatas[2].clothColor && clothingDatas[2].clothColor == clothingDatas[3].clothColor)
        {
            buffActive = true;

            switch (clothingDatas[0].headType)
            {
                case HeadType.HP:
                    PlayerHealth._instance.ChangeMaxHealth((int)(HP_BOOST * LevelData.GetPowerMultiplier()));
                    break;
                case HeadType.Defence:
                    PlayerHealth._instance.ChangeMaxArmor((int)(ARMOR_BOOST * LevelData.GetPowerMultiplier()));
                    break;
                case HeadType.Damage:
                    damageMultiplier *= DAMAGE_BOOST;
                    break;
            }
        }
    }

    public void RemoveClothColor(ArmorType armorType)
    {        
        if(true == buffActive)
        {
            buffActive = false;

            switch (clothingDatas[0].headType)
            {
                case HeadType.HP:
                    PlayerHealth._instance.ChangeMaxHealth(-(int)(HP_BOOST * LevelData.GetPowerMultiplier()));
                    break;
                case HeadType.Defence:
                    PlayerHealth._instance.ChangeMaxArmor(-(int)(ARMOR_BOOST * LevelData.GetPowerMultiplier()));
                    break;
                case HeadType.Damage:
                    damageMultiplier /= DAMAGE_BOOST;
                    break;
            }
        }

        clothingDatas[(int)armorType] = null;
        clothCount--;
    }
}
