using UnityEngine;

public class ShootSingleBulletOnFire : MonoBehaviour
{
    public GameObject bulletPrefab;

    private WeaponCore weaponCore;

    [SerializeField]
    private GameObject particleEffectPrefab;
    private ParticleSystem particleEffectObject;

    [SerializeField] private AudioClip fireSound;

    private static readonly Quaternion defaultParticleSystemRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
    private static readonly Quaternion flippedParticleSystemRotation = Quaternion.Euler(new Vector3(0, 0, 90f));

    private float maxSpreadAngle;

    private bool ParticleEffectIsSet;

    private void Awake ()
    {
        weaponCore = GetComponent<WeaponCore>();
        weaponCore.OnFire += InstantiateBullet;

        if(particleEffectPrefab != null)
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
        maxSpreadAngle = (Mathf.PI / 8) - Mathf.PI / 8 * (weaponCore.item.itemData.accuracy / 100);
    }

    private void InstantiateBullet ()
    {
        if (weaponCore.remainingAmmoInMag > 0) //Ammo avaliable
        {
            BulletCore bulletCore = Instantiate(bulletPrefab, weaponCore.firePoint.position, weaponCore.weaponTransform.rotation).GetComponent<BulletCore>(); //possible pooling
            bulletCore.transform.right = weaponCore.weaponTransform.right * Mathf.Sign(weaponCore.weaponTransform.lossyScale.x);
            bulletCore.weapon = weaponCore.item;

            float spreadAngle = maxSpreadAngle * PlayerController._instance.bulletspreadMultiplier;

            bulletCore.rigidbody.velocity = (weaponCore.item.itemData.bulletSpeed * weaponCore.weaponTransform.lossyScale.x * (Vector2)weaponCore.weaponTransform.right).RotateWithRad(Random.Range(-spreadAngle, spreadAngle));            

            weaponCore.remainingAmmoInMag--;

            ToolbarUIController._instance.SetAmmoText(weaponCore.remainingAmmoInMag);

            SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
            controller.PlaySoundEffect(fireSound);

            if (ParticleEffectIsSet)
            {
                PlayParticleEffect();
            }

            if(weaponCore.remainingAmmoInMag == 0)
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

    private void PlayParticleEffect()
    {
        if(particleEffectObject != null)
        {
            particleEffectObject.Play();
        }        
    }

    private void SetParticleEffectRotation(bool isFacingRight)
    {        
        particleEffectObject.transform.rotation = isFacingRight ? defaultParticleSystemRotation : flippedParticleSystemRotation;
    }
}