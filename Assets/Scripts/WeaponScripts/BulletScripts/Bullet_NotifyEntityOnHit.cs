using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_NotifyEntityOnHit : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<BulletCore>().OnEntityHit += Notify;
    }

    private void Notify(EntityController entityController)
    {
        entityController.AI.NotifyOfAttack((int)Mathf.Sign(transform.position.x - entityController.entityRigidbody.position.x));
    }
}
