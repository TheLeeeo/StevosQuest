using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveImmunityOnActivate : MonoBehaviour
{
    [SerializeField] private EffectEnum immunity;

    private void Awake()
    {
        ItemCore itemcore = GetComponent<ItemCore>();
        itemcore.OnActivate += Activate;
        itemcore.OnDeactivate += Deactivate;
    }

    private void Activate(EntityController entityController)
    {
        entityController.health.AddImmunity(immunity);
    }

    private void Deactivate(EntityController entityController)
    {
        entityController.health.RemoveImmunity(immunity);
    }
}
