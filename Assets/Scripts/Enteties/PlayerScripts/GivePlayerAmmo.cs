using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerAmmo : MonoBehaviour
{
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int ammount;

    private void Start()
    {       
        BulletManager.AddBulletsOfType(ammoType, ammount);
        SaveSystem.RecordBullets();
    }
}
