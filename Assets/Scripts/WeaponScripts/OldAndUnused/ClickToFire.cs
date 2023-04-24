using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToFire : MonoBehaviour
{
    [SerializeField]
    float timeBetweenShots;
    int _ShotsPerSecond_;
    public int ShotsPerSecond
    {
        set
        {
            timeBetweenShots = 1f / (float)value;
            _ShotsPerSecond_ = value;
        }

        get { return _ShotsPerSecond_; }
    }

    private float timeSinceLastShot;

    private WeaponCore _weaponCore;

    void Start()
    {
        _weaponCore = GetComponent<WeaponCore>();

    }
    
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (timeSinceLastShot >= timeBetweenShots)
            {
                timeSinceLastShot = 0;
                _weaponCore.OnFire();
            }
        }
    }

}
