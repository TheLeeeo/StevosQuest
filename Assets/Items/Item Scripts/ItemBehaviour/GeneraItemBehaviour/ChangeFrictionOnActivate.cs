using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFrictionOnActivate : MonoBehaviour
{
    [SerializeField]
    private float newFriction;

    private float oldFriction;

    private void Awake()
    {
        ItemCore itemCore = GetComponent<ItemCore>();

        itemCore.OnActivate += Activate;
        itemCore.OnDeactivate += Deactivate;

        oldFriction = PlayerMovement._instance.movementFriction;
    }

    private void Activate(EntityController entityController)
    {
        PlayerMovement._instance.movementFriction = newFriction;
    }

    private void Deactivate(EntityController entityController)
    {
        PlayerMovement._instance.movementFriction = oldFriction;
    }
}
