using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstOnHold : MonoBehaviour
{
    private WeaponCore weaponCore;

    private IEnumerator fireCoroutine;

    [SerializeField]
    private int burstCount;

    private int currentBurstCount;

    private void Awake()
    {
        weaponCore = GetComponent<WeaponCore>();
        weaponCore.OnMainUse += OnButtonInput;

        fireCoroutine = FireWeapon();
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            if (currentBurstCount < burstCount)
            {
                weaponCore.OnFire();
                currentBurstCount++;

                yield return new WaitForSeconds(weaponCore.item.itemData.firerate);
            }
            else
            {
                StopCoroutine(fireCoroutine);
                yield return null;
            }
        }
    }

    private void OnButtonInput(EntityController entityController, ClickType clickType)
    {
        if (ClickType.Press == clickType)
        {            
            StartCoroutine(fireCoroutine);
        }
        else
        {
            StopCoroutine(fireCoroutine);
            currentBurstCount = 0;
        }
    }
}
