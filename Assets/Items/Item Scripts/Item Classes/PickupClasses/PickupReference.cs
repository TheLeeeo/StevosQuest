using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupReference : ItemReference
{
    [SerializeField]
    private PickupData itemData;

    public override Item GetItem()
    {
        return new Pickup(itemData);
    }
}
