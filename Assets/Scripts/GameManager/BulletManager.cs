using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static int[] remainingBullets { get; private set; } = new int[8];
    private static int[] maxBulletsOfType = { 128, 128, 128, 128, 128, 128, 128, 128};

    public static void SetBullets(int [] bulletCounts)
    {
        for (int i = 0; i < 8; i++)
        {
            remainingBullets[i] = bulletCounts[i];

            AmmoDisplayController._instance.UpdateAmmoOfType((AmmoType) i, remainingBullets[i]);
        }
    }

    public static bool CanAddBullet(AmmoType ammoType)
    {
        return remainingBullets[(int)ammoType] < maxBulletsOfType[(int)ammoType];
    }

    public static bool CanRemoveBullet(AmmoType ammoType)
    {
        return remainingBullets[(int)ammoType] > 0;
    }

    public static void RemoveBullets(AmmoType ammoType, int ammount) //No checking
    {
        remainingBullets[(int)ammoType] -= ammount;

        UpdateBulletUI(ammoType);
    }

    /// <summary>
    /// Returns the ammount of bullets that could not be added
    /// </summary>
    public static int AddBulletsOfType(AmmoType ammoType, int ammount)
    {
        remainingBullets[(int)ammoType] += ammount;

        int bulletsRemaining = Mathf.Max(remainingBullets[(int)ammoType] - maxBulletsOfType[(int)ammoType], 0);

        remainingBullets[(int)ammoType] = Mathf.Min(remainingBullets[(int)ammoType], maxBulletsOfType[(int)ammoType]);

        UpdateBulletUI(ammoType);

        return bulletsRemaining;
    }

    /// <summary>
    /// returnes the number of bullets that can be removed out of the ones specified
    /// </summary>
    public static int NumberOfBulletsRemovable(AmmoType ammoType, int ammount)
    {
        return ammount + Mathf.Min(remainingBullets[(int)ammoType] - ammount, 0);
    }

    /// <summary>
    /// Removes the number of bullets that can be removed out of the ones specified and then returnes that number
    /// </summary>
    public static int RemoveMaxBullets(AmmoType ammoType, int ammount)
    {
        int returnAmmount = NumberOfBulletsRemovable(ammoType, ammount);
        remainingBullets[(int)ammoType] -= returnAmmount;

        UpdateBulletUI(ammoType);

        return returnAmmount;
    }

    public static int NumberOfRemainingBullets(AmmoType ammoType)
    {
        return remainingBullets[(int)ammoType];
    }

    private static void UpdateBulletUI(AmmoType ammoType)
    {
        AmmoDisplayController._instance.UpdateAmmoOfType(ammoType, remainingBullets[(int)ammoType]);
        ToolbarUIController._instance.UpateTotalAmmoText(ammoType, remainingBullets[(int)ammoType]);
    }
}
