using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayOnFire : MonoBehaviour
{
    private WeaponCore weaponCore;

    private void Awake()
    {
        weaponCore = GetComponent<WeaponCore>();
        weaponCore.OnMainUse += OnButtonInput;
    }

    private void OnButtonInput(EntityController entityController, ClickType clickType)
    {
        if (ClickType.Press == clickType)
        {
            Vector3 rotation_euler = transform.rotation.eulerAngles;

            Debug.DrawRay(weaponCore.firePoint.position,  20 * new Vector3(Mathf.Cos(rotation_euler.z * Mathf.Deg2Rad) * transform.lossyScale.x, Mathf.Sin(rotation_euler.z * Mathf.Deg2Rad) * transform.lossyScale.x), Color.blue, 5);
        }
    }
}
