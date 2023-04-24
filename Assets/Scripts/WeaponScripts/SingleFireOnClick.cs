using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireOnClick : MonoBehaviour
{
    private WeaponCore weaponCore;

    private float TimeOfLastFire;

    private void Awake()
    {
        weaponCore = GetComponent<WeaponCore>();
        weaponCore.OnMainUse += Fire; //Invokes OnFire on main use
    }

    private void Fire(EntityController entityController, ClickType clickType)
    {
        if (ClickType.Press == clickType && Time.time - TimeOfLastFire >= weaponCore.item.itemData.firerate * PlayerController._instance.firerateModifier)
        {
            TimeOfLastFire = Time.time;
            weaponCore.OnFire();
        }
    }
}
