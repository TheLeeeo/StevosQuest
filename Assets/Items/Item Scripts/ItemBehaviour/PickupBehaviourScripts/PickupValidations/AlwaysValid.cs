using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysValid : MonoBehaviour
{
    private void Start()
    {
        GetComponent<PickupCore>().OnValidatePickup += (_entityController) => true;
    }
}
