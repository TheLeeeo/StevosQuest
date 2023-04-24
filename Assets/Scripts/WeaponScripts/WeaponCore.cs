using System.Collections;
using UnityEngine;
using System;

public class WeaponCore : ItemCore
{
    [HideInInspector]
    public new Weapon item;

    public Transform weaponTransform;

    public Action OnFire;

    [HideInInspector]
    public int remainingAmmoInMag;

    public AmmoType ammoType => item.itemData.ammoType;

    /// <summary>
    /// The point of which to fire the projectiles;
    /// </summary>
    public Transform firePoint;

    private bool currentlyReloading = false;

    private int magazineSize;

    public override void Deactivate()
    {
        base.Deactivate();

        GameManager._instance.CurrentWeapon = null;

        ToolbarUIController._instance.WeaponDeactivated();

        item.currentAmmoAmmount = remainingAmmoInMag;

        StopAllCoroutines();
    }

    public override void Activate(EntityController entityController)
    {
        item = (Weapon)base.item;

        GameManager._instance.CurrentWeapon = this;

        CalculateMagazineSize();

        //remainingAmmoInMag = BulletManager.NumberOfBulletsRemovable(ammoType, item.currentAmmoAmmount);
        remainingAmmoInMag = item.currentAmmoAmmount;

        ToolbarUIController._instance.WeaponActivated(ammoType, remainingAmmoInMag, magazineSize);

        base.Activate(entityController);
    }

    public override void Reload()
    {
        if (currentlyReloading == false && remainingAmmoInMag < magazineSize)
        {
            StartCoroutine(ReloadWaitTimer());
        }    
    }

    private IEnumerator ReloadWaitTimer()
    {
        currentlyReloading = true;

        int bulletsBeforeReloading = remainingAmmoInMag;

        remainingAmmoInMag = 0;

        ToolbarUIController._instance.SetAmmoText(0);

        yield return new WaitForSeconds(item.itemData.timeToReload * PlayerController._instance.reloadTimeMultiplier);

        int bulletsRecieved = BulletManager.NumberOfBulletsRemovable(ammoType, magazineSize - bulletsBeforeReloading);

        remainingAmmoInMag += bulletsRecieved + bulletsBeforeReloading;

        BulletManager.RemoveBullets(ammoType, bulletsRecieved);

        ToolbarUIController._instance.WeaponReload(remainingAmmoInMag);

        currentlyReloading = false;
    }

    public void CalculateMagazineSize()
    {
        magazineSize = Mathf.CeilToInt(item.itemData.magazineSize * PlayerController._instance.magazineSizeMultiplier);

        ToolbarUIController._instance.UpdateMagSize(magazineSize);

        if (remainingAmmoInMag > magazineSize)
        {
            BulletManager.AddBulletsOfType(ammoType, remainingAmmoInMag - magazineSize);
            remainingAmmoInMag = magazineSize;

            ToolbarUIController._instance.WeaponReload(remainingAmmoInMag);
        }
    }
}
