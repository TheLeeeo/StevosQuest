using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOnHold : MonoBehaviour
{
    private WeaponCore weaponCore;

    private IEnumerator fireCoroutine;

    private float timeOfLastShot; //so that one can not spam fire

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
            if((Time.time - timeOfLastShot) >= (weaponCore.item.itemData.firerate * PlayerController._instance.firerateModifier))
            {
                timeOfLastShot = Time.time;
                weaponCore.OnFire();
            }

            yield return null; //could spam fire if it was wait for time
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
        }
    }
}
