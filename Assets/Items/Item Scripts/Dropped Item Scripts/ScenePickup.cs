using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePickup : MonoBehaviour
{
    [SerializeField]
    private PickupData itemData;

    [SerializeField]
    private PickupController pickupBase;

    private void Start()
    {
        Pickup item = new Pickup(itemData);

        pickupBase.InstantiateDrop(item);
    }
}
