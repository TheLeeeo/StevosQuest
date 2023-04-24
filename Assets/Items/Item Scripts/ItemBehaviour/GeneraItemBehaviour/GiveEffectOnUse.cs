using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveEffectOnUse : MonoBehaviour
{
    [SerializeField] private Effect effect;

    private ItemCore itemCore;

    private void Awake()
    {
        itemCore = GetComponent<ItemCore>();
        itemCore.OnMainUse += Use;
    }

    private void Use(EntityController entityController, ClickType clickType)
    {
        if(clickType == ClickType.Press)
        {
            entityController.health.GiveEffect(effect);
        }
    }
}
