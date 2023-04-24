using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Instance of singleton class \"" + this + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    public ItemManagerSO ItemManager;

    public GameObject AggrevatedParticleEffect;

    public GameObject PlayerDeathParticleEffect;

    public GameObject FloatingDamageTextPrefab;

    public GameObject SoundEffectPrefab;

    [SerializeField]
    private GameObject[] EntitySpawnedEffects;
    public static GameObject GetRandomSpawnedEntityEffect()
    {
        return RandomNumbers.RandomFromArgs(_instance.EntitySpawnedEffects);
    }
}
