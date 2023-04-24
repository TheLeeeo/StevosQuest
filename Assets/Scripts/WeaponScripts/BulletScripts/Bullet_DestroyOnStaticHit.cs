using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_DestroyOnStaticHit : MonoBehaviour
{
    private BulletCore bulletCore;

    private void Awake()
    {
        bulletCore = GetComponent<BulletCore>();
        bulletCore.OnStaticHit += DestroyBullet;
    }

    private void DestroyBullet()
    {
        if (bulletCore.particleObject != null)
        {
            Instantiate(bulletCore.particleObject, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
