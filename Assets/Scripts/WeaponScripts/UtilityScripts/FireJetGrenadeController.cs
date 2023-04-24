using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJetGrenadeController : ThrowableController
{
    [SerializeField] private GameObject fireJet;
    [SerializeField] private GiveEffectOnContact effectGiver;
    [SerializeField] private new Rigidbody2D rigidbody;

    private UtilityCore utilityCore;

    [SerializeField]
    private float jetDuration;

    [SerializeField] private AudioSource audioSource;

    public override void Initiate(Vector2 velocityVector, UtilityCore core)
    {
        utilityCore = core;
        rigidbody.velocity = velocityVector;
        rigidbody.angularVelocity = 180f;

        effectGiver.effectToGive.EffectLevel = utilityCore.item.rarity;        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(collision.transform);
        transform.up = collision.GetContact(0).normal;

        fireJet.SetActive(true);        

        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0;
        //rigidbody.simulated = false;

        audioSource.Play();

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(jetDuration);

        Destroy(gameObject);
    }
}
