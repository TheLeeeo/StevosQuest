using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidWhenHurt : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<PickupCore>().OnValidatePickup += Validate;
    }

    private bool Validate(EntityController _entityController)
    {
        return !_entityController.health.HasMaxHealth();
    }
}
