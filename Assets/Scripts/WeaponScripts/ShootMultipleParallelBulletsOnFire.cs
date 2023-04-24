using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMultipleParallelBulletsOnFire : MonoBehaviour
{
    public GameObject bulletPrefab;

    private WeaponCore weaponCore;

    [SerializeField]
    private Transform[] additionalFirePoints;

    [SerializeField]
    private GameObject particleEffectPrefab;
    private ParticleSystem[] particleEffectObjects;

    [SerializeField] private AudioClip fireSound;

    private static readonly Quaternion defaultParticleSystemRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
    private static readonly Quaternion flippedParticleSystemRotation = Quaternion.Euler(new Vector3(0, 0, 90f));

    private float maxSpreadAngle;

    private bool ParticleEffectIsSet;

    private void Awake()
    {       
        weaponCore = GetComponent<WeaponCore>();
        weaponCore.OnFire += Fire;

        if (particleEffectPrefab != null)
        {
            particleEffectObjects = new ParticleSystem[1 + additionalFirePoints.Length];

            ParticleEffectIsSet = true;

            particleEffectObjects[0] = Instantiate(particleEffectPrefab, weaponCore.firePoint).GetComponent<ParticleSystem>();

            for (int i = 0; i < additionalFirePoints.Length; i++)
            {
                particleEffectObjects[i+1] = Instantiate(particleEffectPrefab, additionalFirePoints[i]).GetComponent<ParticleSystem>();
            }
            
            weaponCore.OnDeactivate += _ => PlayerController._instance.OnFlip -= SetParticleEffectRotation;

            PlayerController._instance.OnFlip += SetParticleEffectRotation;
            SetParticleEffectRotation(PlayerController._instance.isFacingRight);
        }
    }

    private void Start()
    {
        maxSpreadAngle = (Mathf.PI / 6) - Mathf.PI / 6 * (weaponCore.item.itemData.accuracy / 100);
    }

    private void Fire()
    {
        if (true == InstantiateBullet(weaponCore.firePoint))
        {
            foreach(Transform firePoint in additionalFirePoints)
            {
                if (false == InstantiateBullet(firePoint))
                {
                    break;
                }
            }

            SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
            controller.PlaySoundEffect(fireSound);
        }
    }

    private bool InstantiateBullet(Transform firePoint)
    {
        if (weaponCore.remainingAmmoInMag > 0) //Ammo avaliable
        {
            BulletCore bulletCore = Instantiate(bulletPrefab, firePoint.position, weaponCore.weaponTransform.rotation).GetComponent<BulletCore>(); //possible pooling
            bulletCore.transform.right = weaponCore.weaponTransform.right;
            bulletCore.weapon = weaponCore.item;

            float spreadAngle = maxSpreadAngle * PlayerController._instance.bulletspreadMultiplier;

            bulletCore.rigidbody.velocity = (weaponCore.item.itemData.bulletSpeed * weaponCore.weaponTransform.lossyScale.x * (Vector2)weaponCore.weaponTransform.right).RotateWithRad(Random.Range(-spreadAngle, spreadAngle));

            weaponCore.remainingAmmoInMag--;

            ToolbarUIController._instance.SetAmmoText(weaponCore.remainingAmmoInMag);

            if (ParticleEffectIsSet)
            {
                PlayParticleEffect();
            }

            if (weaponCore.remainingAmmoInMag == 0)
            {
                weaponCore.Reload();
            }

            return true;
        }
        else
        {
            weaponCore.Reload();
            //Possible gun jam sound effect
            return false;
        }
    }

    private void PlayParticleEffect()
    {
        foreach(var particleEffectObject in particleEffectObjects)
        {
            if (particleEffectObject != null)
            {
                particleEffectObject.Play();
            }
        }        
    }

    private void SetParticleEffectRotation(bool isFacingRight)
    {
        foreach(var particleEffectObject in particleEffectObjects)
        {
            particleEffectObject.transform.rotation = isFacingRight ? defaultParticleSystemRotation : flippedParticleSystemRotation;
        }        
    }
}
