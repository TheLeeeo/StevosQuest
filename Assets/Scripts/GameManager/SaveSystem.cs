using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    private const string SaveFileName = "/swagfile.sex";

    private static Save currentSave;

    public static void SaveGame()
    {
        currentSave.Level = SceneLoader.currentLevelBuildIndex;

        Armor[] armors = PlayerInventory._instance.GetArmors();
        Weapon[] weapons = PlayerInventory._instance.GetWeapons();
        Trinket[] trinkets = PlayerInventory._instance.GetTrinkets();
        Utility[] utilities = PlayerInventory._instance.GetUtilities();

        for (int i = 0; i < 4; i++)
        {
            if(null != armors[i])
            {
                currentSave.armorIDs[i] = armors[i].itemData.ItemIdentifier;
                currentSave.itemRarities[i] = (int)armors[i].rarity;
                currentSave.powerMultipliers[i] = armors[i].powerMultiplier;
            }
            else
            {
                currentSave.armorIDs[i] = -1;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (null != weapons[i])
            {
                currentSave.weaponIDs[i] = weapons[i].itemData.ItemIdentifier;
                currentSave.itemRarities[i] = (int)weapons[i].rarity;
                currentSave.powerMultipliers[i] = weapons[i].powerMultiplier;
            }
            else
            {
                currentSave.weaponIDs[i] = -1;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (null != trinkets[i])
            {
                currentSave.trinketIDs[i] = trinkets[i].itemData.ItemIdentifier;
                currentSave.itemRarities[i] = (int)trinkets[i].rarity;
                currentSave.powerMultipliers[i] = trinkets[i].powerMultiplier;

                //currentSave.trinketDurabilities[i] = trinkets[i].remainingDurability;
            }
            else
            {
                currentSave.trinketIDs[i] = -1;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (null != utilities[i])
            {
                currentSave.utilityIDs[i] = utilities[i].itemData.ItemIdentifier;
                currentSave.itemRarities[i] = (int)utilities[i].rarity;
                currentSave.powerMultipliers[i] = utilities[i].powerMultiplier;
                currentSave.utilityCount[i] = utilities[i].itemCount;
            }
            else
            {
                currentSave.utilityIDs[i] = -1;
            }
        }

        currentSave.ammoCounts = BulletManager.remainingBullets;
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + SaveFileName);
        bf.Serialize(file, currentSave);
        file.Close();
    }

    public static void NewSaveFile()
    {
        currentSave = new Save();

        for (int i = 0; i < currentSave.armorIDs.Length; i++)
        {
            currentSave.armorIDs[i] = -1;
        }

        for (int i = 0; i < currentSave.weaponIDs.Length; i++)
        {
            currentSave.weaponIDs[i] = -1;
        }

        for (int i = 0; i < currentSave.trinketIDs.Length; i++)
        {
            currentSave.trinketIDs[i] = -1;
        }

        for (int i = 0; i < currentSave.utilityIDs.Length; i++)
        {
            currentSave.utilityIDs[i] = -1;
        }
    }

    /// <summary>
    /// Loads the save file and returns true. If no save exists, returns false;
    /// </summary>
    /// <returns></returns>
    public static bool LoadSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + SaveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + SaveFileName, FileMode.Open);
            currentSave = (Save)bf.Deserialize(file);
            file.Close();

            return true;
        }

        return false;
    }

    public static int GetLevel()
    {
        return currentSave.Level;
    }

    /// <summary>
    /// Returns the sprite for the clothes nessecary for nosemobile cutscene
    /// </summary>
    public static Sprite[] GetClothesSprites()
    {
        Sprite[] sprites = new Sprite[3];

        if(currentSave.armorIDs[0] != -1)
        {
            sprites[0] = PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.armorIDs[0]).itemSprite; //head
        }

        if (currentSave.armorIDs[1] != -1)
        {
            ChestwearData chestwearData = (ChestwearData)PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.armorIDs[1]);
            sprites[1] = chestwearData.chestSprite;
            sprites[2] = chestwearData.shoulderSprite;
        }               

        return sprites;
    }

    public static void SetLevelAsCompleted()
    {
        currentSave.Level = -1;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + SaveFileName);
        bf.Serialize(file, currentSave);
        file.Close();
    }

    public static void RecordBullets()
    {
        currentSave.ammoCounts = BulletManager.remainingBullets;
    }

    public static void LoadGame()
    {
        for (int i = 0; i < 4; i++) //Armor
        {
            if (-1 != currentSave.armorIDs[i])
            {
                ArmorData itemData = (ArmorData)PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.armorIDs[i]);

                Armor armor = new Armor(itemData, (ItemRarity)currentSave.itemRarities[i]);

                PlayerInventory._instance.AddItem(armor);
            }
        }

        for (int i = 0; i < 5; i++) //Weapons
        {
            if (-1 != currentSave.weaponIDs[i])
            {
                try
                {
                    WeaponData itemData = (WeaponData)PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.weaponIDs[i]);

                    Weapon weapon = new Weapon(itemData, (ItemRarity)currentSave.itemRarities[i]);

                    PlayerInventory._instance.AddItem(weapon);
                }
                catch //debug
                {
                    Debug.Log(currentSave.weaponIDs[i] + " : " + PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.weaponIDs[i]).itemName);
                }
            }
        }

        for (int i = 0; i < 4; i++) //Trinkets
        {
            if (-1 != currentSave.trinketIDs[i])
            {
                TrinketData itemData = (TrinketData)PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.trinketIDs[i]);

                Trinket trinket = new Trinket(itemData, (ItemRarity)currentSave.itemRarities[i]);
                //trinket.remainingDurability = currentSave.trinketDurabilities[i];

                PlayerInventory._instance.AddItem(trinket);
            }
        }

        for (int i = 0; i < 5; i++) //Utilities
        {
            if (-1 != currentSave.utilityIDs[i])
            {
                UtilityData itemData = (UtilityData)PrefabManager._instance.ItemManager.GetItemdataByID(currentSave.utilityIDs[i]);

                Utility utility = new Utility(itemData, (ItemRarity)currentSave.itemRarities[i], currentSave.utilityCount[i]);

                PlayerInventory._instance.AddItem(utility);
            }
        }

        BulletManager.SetBullets(currentSave.ammoCounts);
    }
}
