using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCloudController : MonoBehaviour
{
    [SerializeField]
    private Effect effectToGive;

    [SerializeField]
    private new Rigidbody2D rigidbody;

    [SerializeField]
    private new ParticleSystem particleSystem;

    [SerializeField]
    private float cloudLifetime;

    private float time;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {
            collision.gameObject.GetComponent<Health>().GiveEffect(effectToGive);            
        }
        else
        {
            rigidbody.drag = 1;            
        }
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= cloudLifetime)
        {
            particleSystem.Stop();
        }
    }

    public void Initiate(Vector2 velocity)
    {
        rigidbody.velocity = velocity;
    }
}
