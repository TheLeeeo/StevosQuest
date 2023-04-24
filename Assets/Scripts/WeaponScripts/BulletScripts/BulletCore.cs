using UnityEngine;
using System;

public class BulletCore : MonoBehaviour
{
    [HideInInspector]
    public Weapon weapon;
    
    public GameObject particleObject;

    public new Rigidbody2D rigidbody;

    public Action<EntityController> OnEntityHit;
    public Action OnStaticHit;

    /// <summary>
    /// Destroys the projectile if it leaves the camera view or it collides with something
    /// </summary>
    [SerializeField]
    private bool DestroyOnComplete = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (15 == collision.gameObject.layer)
        {
            if (OnEntityHit != null)
            {
                OnEntityHit(collision.gameObject.GetComponent<EntityController>());
            }
        }
        else
        {
            if (OnStaticHit != null)
            {
                OnStaticHit();
            }
        }

        if (particleObject != null)
        {
            Instantiate(particleObject, transform.position, Quaternion.identity);
        }

        if (DestroyOnComplete)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() //possibly change
    {
        if (DestroyOnComplete)
        {
            Destroy(gameObject);
        }
    }
}
