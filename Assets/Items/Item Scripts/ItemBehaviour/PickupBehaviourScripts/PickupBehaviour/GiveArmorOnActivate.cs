using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveArmorOnActivate : MonoBehaviour
{
    [SerializeField]
    private int Ammount;

    private void OnEnable()
    {
        GetComponent<ItemCore>().OnActivate = Activate;
    }

    private void Activate(EntityController _entityController)
    {
        _entityController.health.AddArmor(Ammount);
    }
}
