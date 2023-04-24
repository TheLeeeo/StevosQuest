using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlaySoundEffect(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();

        Destroy(gameObject, 1f);
    }
}
