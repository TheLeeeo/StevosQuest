using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : Health
{
    public static PlayerHealth _instance;

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

    public override int GiveEffect(Effect _effect)
    {
        int returnValue = base.GiveEffect(_effect);

        if (1 == returnValue)
        {
            PlayerHUDController._instance.DisplayEffect(_effect);
        }
        else if (2 == returnValue)
        {
            PlayerHUDController._instance.UpdateEffectTime(_effect);
        }

        return 0;
    }

    public override void RemoveEffect(EffectEnum EffectID)
    {
        base.RemoveEffect(EffectID);

        PlayerHUDController._instance.RemoveEffect(EffectID);
    }

    public override void Die()
    {
        Instantiate(PrefabManager._instance.PlayerDeathParticleEffect, transform);
        PlayerInputHandler._instance.DisableAll();
        PlayerController._instance.deathEvent.Invoke();
        SceneLoader.Death();        
    }
}
