using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMultipleBulletsOnFire : MonoBehaviour
{
    public GameObject bulletPrefab;

    private WeaponCore weaponCore;

    [SerializeField]
    private int BaseBulletCount = 1;

    [SerializeField]
    private GameObject particleEffectPrefab;
    private ParticleSystem particleEffectObject;

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
            ParticleEffectIsSet = true;

            particleEffectObject = Instantiate(particleEffectPrefab, weaponCore.firePoint).GetComponent<ParticleSystem>();            

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
        if (weaponCore.remainingAmmoInMag > 0) //Ammo avaliable
        {
            for (int i = 0; i < BaseBulletCount + PlayerController._instance.extraBullets; i++)
            {
                InstantiateBullet();
            }

            weaponCore.remainingAmmoInMag--;

            ToolbarUIController._instance.SetAmmoText(weaponCore.remainingAmmoInMag);

            SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
            controller.PlaySoundEffect(fireSound);

            if (ParticleEffectIsSet)
            {
                PlayParticleEffect();
            }

            if (weaponCore.remainingAmmoInMag == 0)
            {
                weaponCore.Reload();
            }
        }
        else
        {
            weaponCore.Reload();
            //Possible gun jam sound effect
        }

    }

    private void InstantiateBullet()
    {
        BulletCore bulletCore = Instantiate(bulletPrefab, weaponCore.firePoint.position, weaponCore.weaponTransform.rotation).GetComponent<BulletCore>(); //possible pooling
        bulletCore.transform.right = weaponCore.weaponTransform.right;
        bulletCore.weapon = weaponCore.item;

        float spreadAngle = maxSpreadAngle * PlayerController._instance.bulletspreadMultiplier;

        bulletCore.rigidbody.velocity = (weaponCore.item.itemData.bulletSpeed * weaponCore.weaponTransform.lossyScale.x * (Vector2)weaponCore.weaponTransform.right).RotateWithRad(Random.Range(-spreadAngle, spreadAngle));                
    }

    private void PlayParticleEffect()
    {
        if (particleEffectObject != null)
        {
            particleEffectObject.Play();
        }
    }

    private void SetParticleEffectRotation(bool isFacingRight)
    {
        particleEffectObject.transform.rotation = isFacingRight ? defaultParticleSystemRotation : flippedParticleSystemRotation;
    }
}
