using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayController : DisplayControllerBase
{
    [SerializeField]
    private Sprite[] ammoImages;
    [SerializeField]
    private Sprite[] weaponTypeImages;

    [SerializeField]
    private Image weaponAmmo;
    [SerializeField]
    private Image weaponType;
    [SerializeField]
    private Text weaponMagSize;

    public override void Initiate(Item _displayItem)
    {
        base.Initiate(_displayItem);

        Weapon _displayWeapon = (Weapon)_displayItem;

        weaponAmmo.sprite = ammoImages[(int)_displayWeapon.itemData.ammoType];
        weaponAmmo.preserveAspect = true;

        weaponType.sprite = weaponTypeImages[(int)_displayWeapon.itemData.weaponType];
        weaponType.preserveAspect = true;

        weaponMagSize.text = _displayWeapon.itemData.magazineSize.ToString();

        tooltipText.text = _displayWeapon.itemData.itemTooltip;

        DisplayStats(_displayWeapon);
    }

    private void DisplayStats(Weapon _displayWeapon)
    {
        /*for (int i = 0; i < _displayWeapon.itemData.itemStats.Length; i++)
        {
            statDisplayControllers[i].DisplayStat(_displayWeapon.itemData.itemStats[i], _displayWeapon.powerMultiplier);
        }*/

        statDisplayControllers[0].DisplayStat(new ItemStat(Color.white, "Damage", _displayWeapon.itemData.damage, true), _displayWeapon.powerMultiplier); //Damage stat
        statDisplayControllers[1].DisplayStat(new ItemStat(Color.white, "Reload speed", _displayWeapon.itemData.timeToReload, false), _displayWeapon.powerMultiplier); //Reload speed stat
        statDisplayControllers[2].DisplayStat(new ItemStat(Color.white, "Accuracy", _displayWeapon.itemData.accuracy, false), _displayWeapon.powerMultiplier); //Accuracy stat

        int armorPiercingShown = _displayWeapon.itemData.armorPiercing != 1 ? 1 : 0;

        if (1 == armorPiercingShown)
        {
            statDisplayControllers[3].DisplayStat(new ItemStat(Color.white, "Armor Piercing", _displayWeapon.itemData.armorPiercing, false), _displayWeapon.powerMultiplier);
        }

        if(_displayWeapon.itemData.itemStats.Length > 0)
        {
            statDisplayControllers[3 + armorPiercingShown].DisplayStat(_displayWeapon.itemData.itemStats[0], _displayWeapon.powerMultiplier);

            if (_displayWeapon.itemData.itemStats.Length > 1)
            {
                statDisplayControllers[4 + armorPiercingShown].DisplayStat(_displayWeapon.itemData.itemStats[1], _displayWeapon.powerMultiplier);
            }
        }    
    }
}
