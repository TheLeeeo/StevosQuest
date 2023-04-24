using UnityEngine;

/// <summary>
/// What class of weapon it is.
/// </summary>
public enum WeaponType
{
    Type_1,
    Type_2,
    Type_3,
    Special
}

/// <summary>
/// What type of ammo the weapon uses
/// </summary>
public enum AmmoType
{
    Pistol,
    SMG,
    Shotgun,
    Rifle,
    HighCalibre,
    Rocket,
    Grenade,
    EnergyCell
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class WeaponData : EquipableItemData
{
    private void Awake()
    {
        itemType = ItemType.Weapon;
    }

    public WeaponType weaponType;
    public AmmoType ammoType;

    public float damage;

    [HideInInspector]
    public int currentAmmo;
    public int magazineSize;

    /// <summary>
    /// Time in seconds to reload.
    /// </summary>
    [Tooltip("Time in seconds to reload")]
    public float timeToReload;

    /// <summary>
    /// How accurate the weapon is. 100 being the best and 0 being the worst.
    /// A value of 100 means the weapon is point accurate and 0 means that the maximum spread of the weapon is 30 degrees (π/6 rad).
    /// </summary>
    [Tooltip("Higher is more accurate")]
    [Range(0, 100)]
    public float accuracy;


    /// <summary>
    /// The time in seconds between shots
    /// </summary>
    [Tooltip("Time between shots in seconds")]
    public float firerate;

    public float bulletSpeed;

    [Tooltip("The multiplier added to the damage towards armor")]
    public float armorPiercing = 1f;
}
