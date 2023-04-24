using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EntitySoundController : MonoBehaviour
{
    public AudioClip deathSound;
    public AudioClip secondarySound;
    public AudioClip attackSound;

    [SerializeField] private AudioSource audioSource;

    private void Play(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
        controller.PlaySoundEffect(deathSound);
    }

    public void PlaySecondarySound()
    {
        Play(secondarySound);
    }

    public void PlayAttackSound()
    {
        Play(attackSound);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
