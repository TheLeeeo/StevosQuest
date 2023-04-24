using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBoulderController : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private GiveDamageOnContact giveDamageOnContact;

    [SerializeField]
    private Rigidbody2D boulderRigidbody;

    [SerializeField]
    private new ParticleSystem particleSystem;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float rotationalSpeed;

    [SerializeField]
    private float speed;

    public void Initiate(float direction)
    {
        giveDamageOnContact.damage = Mathf.CeilToInt((int)(giveDamageOnContact.damage * LevelData.GetEntityPowerMultiplier()));

        boulderRigidbody.velocity = new Vector2(speed * direction, 0);
        boulderRigidbody.angularVelocity = rotationalSpeed * -direction;
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(boulderRigidbody.velocity.x) < 0.1f)
        {
            SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
            controller.PlaySoundEffect(sound);

            boulderRigidbody.angularVelocity = 0;
            spriteRenderer.enabled = false;
            this.enabled = false;
            particleSystem.Play(); //will destroy the boulder once finished
        }
    }
}
