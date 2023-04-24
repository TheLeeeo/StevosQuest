using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveEffectOnContact : MonoBehaviour
{
    public Effect effectToGive;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {
            collision.gameObject.GetComponent<Health>().GiveEffect(effectToGive);
        }    
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {
            collision.gameObject.GetComponent<Health>().GiveEffect(effectToGive);
        }                
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == CommonLayerMasks.Player)
        {
            PlayerHealth._instance.GiveEffect(effectToGive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == CommonLayerMasks.Player)
        {
            PlayerHealth._instance.GiveEffect(effectToGive);
        }

    }*/
}
