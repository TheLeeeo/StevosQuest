using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZappRayOnHold : MonoBehaviour
{
    private const float RayTime = 0.2f;

    [SerializeField]
    private LineRenderer lineRenderer;

    private WeaponCore weaponCore;

    private IEnumerator fireCoroutine;

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
        float timer = 0;

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

    private IEnumerator DisableRay()
    {
        yield return new WaitForSeconds(RayTime);

        lineRenderer.enabled = false;
    }

    private void OnButtonInput(EntityController entityController, ClickType clickType)
    {
        if (ClickType.Press == clickType)
        {            
            StartCoroutine(fireCoroutine);            
        }
        else
        {
            lineRenderer.enabled = false;
            StopCoroutine(fireCoroutine);
        }
    }

    private void FireDamageRay()
    {
        if (weaponCore.remainingAmmoInMag > 0) //Ammo avaliable
        {
            lineRenderer.enabled = true;

            audioSource.Play();

            RaycastHit2D[] raycastHits = Physics2D.LinecastAll(weaponCore.firePoint.position, hitPoint, CommonLayerMasks.HasHealth);

            foreach (RaycastHit2D rayHit in raycastHits)
            {
                rayHit.collider.gameObject.GetComponent<Health>().Damage(Mathf.CeilToInt(weaponCore.item.itemData.damage * weaponCore.item.powerMultiplier * PlayerController._instance.damageMultiplier));
            }

            weaponCore.remainingAmmoInMag--;

            ToolbarUIController._instance.SetAmmoText(weaponCore.remainingAmmoInMag);

            if (weaponCore.remainingAmmoInMag == 0)
            {
                weaponCore.Reload();
            }

            StartCoroutine(DisableRay());
        }
        else
        {
            weaponCore.Reload();

            lineRenderer.enabled = false;
            //Possible gun jam sound effect
        }
    }
}
