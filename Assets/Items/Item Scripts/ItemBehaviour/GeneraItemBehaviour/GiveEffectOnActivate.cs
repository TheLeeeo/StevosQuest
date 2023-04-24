using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveEffectOnActivate : MonoBehaviour
{
    [SerializeField] private Effect effect;

    private void Awake()
    {
        GetComponent<ItemCore>().OnActivate += Activate;
    }

    private void Activate(EntityController entityController)
    {
        entityController.health.GiveEffect(effect);
    }
}
