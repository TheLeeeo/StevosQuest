using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBoulderController : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private GiveDamageOnContact giveDamageOnContact;

    [SerializeField]
    private Rigidbody2D boulderRigidbody;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private new ParticleSystem particleSystem;

    public void Initiate(Vector2 velocity, int damage)
    {
        giveDamageOnContact.damage = Mathf.CeilToInt((int)(damage * LevelData.GetEntityPowerMultiplier()));

        boulderRigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D()
    {
        SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
        controller.PlaySoundEffect(sound);

        boulderRigidbody.velocity = Vector2.zero;
        boulderRigidbody.angularVelocity = 0;
        spriteRenderer.enabled = false;
        this.enabled = false;
        particleSystem.Play(); //will destroy the boulder once finished
    }
}
