using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    private static GameMusicController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource_2;

    public static void Play(AudioClip audioClip)
    {
        _instance.audioSource.clip = audioClip;
        _instance.audioSource.Play();
    }

    public static void Play()
    {
        _instance.audioSource.Play();
    }

    public static void Stop()
    {
        _instance.audioSource.Stop();
    }

    public static void Pause()
    {
        _instance.audioSource.Pause();
    }

    public static void PlayOverlayTrack(AudioClip audioClip)
    {
        _instance.audioSource_2.clip = audioClip;
        _instance.audioSource_2.Play();
    }

    public static void StopOverlayTrack()
    {
        _instance.audioSource_2.Stop();
    }
}
