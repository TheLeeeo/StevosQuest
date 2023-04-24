using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int Level; //-1 if save is a completed game

    public int[] weaponIDs = new int[5];
    public int[] armorIDs = new int[4];
    public int[] trinketIDs = new int[4];
    public int[] utilityIDs = new int[5];

    public int[] itemRarities = new int[18];
    public float[] powerMultipliers = new float[18];
    public int[] trinketDurabilities = new int[4];
    public int[] utilityCount = new int[5];

    public int[] ammoCounts = new int[8];
}
