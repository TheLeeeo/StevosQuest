using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamageOnContact : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {            
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }

    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == CommonLayerMasks.Player)
        {
            PlayerHealth._instance.Damage((int)(damage * LevelData.GetEntityPowerMultiplier()));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == CommonLayerMasks.Player)
        {
            PlayerHealth._instance.Damage((int)(damage * LevelData.GetEntityPowerMultiplier()));
        }

    }*/
}
