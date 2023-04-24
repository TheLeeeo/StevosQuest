using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRayOnHold : MonoBehaviour //Will perhaps need the ability to run a particle simulation. Just copy the code from the other scripts in that case
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private WeaponCore weaponCore;

    private IEnumerator fireCoroutine;

    [Tooltip("The ammount of sub ammo every ammo gives. 1 is used every shot")]
    [SerializeField]
    private int subAmmoPerAmmo;

    [Tooltip("Smaller fractions of the ammo, 1 is used every shot")]
    private int subAmmo = 0;

    private Vector2 hitPoint;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        weaponCore = GetComponent<WeaponCore>();
        weaponCore.OnMainUse += OnButtonInput;
        weaponCore.OnFire += FireDamageRay;

        fireCoroutine = FireWeapon();
    }

    private IEnumerator FireWeapon()
    {
        float timer = weaponCore.item.itemData.firerate;

        while (true)
        {
            timer -= Time.deltaTime;

            lineRenderer.SetPosition(0, weaponCore.firePoint.position);

            Vector2 direction = weaponCore.firePoint.right * weaponCore.firePoint.lossyScale.x;

            hitPoint = Physics2D.Raycast(weaponCore.firePoint.position, direction, 20, CommonLayerMasks.GroundCheckLayers).point;

            if (Vector2.zero == hitPoint)
            {
                hitPoint = weaponCore.firePoint.position + (Vector3)direction * 20;
            }

            lineRenderer.SetPosition(1, hitPoint);                       

            if (timer <= 0)
            {
                timer = weaponCore.item.itemData.firerate * PlayerController._instance.firerateModifier;

                weaponCore.OnFire(); //Nessecary for if another script relies on the OnFire event
            }

            yield return null;
        }
    }

    private void OnButtonInput(EntityController entityController, ClickType clickType)
    {
        if (ClickType.Press == clickType)
        {
            lineRenderer.enabled = true;
            StartCoroutine(fireCoroutine);

            audioSource.Play();
        }
        else
        {
            lineRenderer.enabled = false;
            StopCoroutine(fireCoroutine);

            audioSource.Stop();
        }
    }

    private void FireDamageRay()
    {
        if (subAmmo <= 0)
        {
            if (weaponCore.remainingAmmoInMag > 0) //Ammo avaliable
            {                
                weaponCore.remainingAmmoInMag--;

                subAmmo = subAmmoPerAmmo;

                ToolbarUIController._instance.SetAmmoText(weaponCore.remainingAmmoInMag);
                BulletManager.RemoveBullets(weaponCore.ammoType, 1);

                if (weaponCore.remainingAmmoInMag == 0)
                {
                    weaponCore.Reload();
                }
            }
            else
            {
                weaponCore.Reload();

                lineRenderer.enabled = false;
                //Possible gun jam sound effect
            }
        }
        else
        {
            lineRenderer.enabled = true;

            subAmmo -= 1;

            RaycastHit2D[] raycastHits = Physics2D.LinecastAll(weaponCore.firePoint.position, hitPoint, CommonLayerMasks.HasHealth);

            foreach (RaycastHit2D rayHit in raycastHits)
            {
                rayHit.collider.gameObject.GetComponent<Health>().Damage(Mathf.CeilToInt(weaponCore.item.itemData.damage * weaponCore.item.powerMultiplier * PlayerController._instance.damageMultiplier));
            }
        }
    }
}
