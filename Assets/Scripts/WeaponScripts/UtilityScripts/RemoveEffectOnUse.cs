using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEffectOnUse : MonoBehaviour
{
    [SerializeField] private EffectEnum effectID;

    private ItemCore itemCore;

    private void Awake()
    {
        itemCore = GetComponent<ItemCore>();
        itemCore.OnMainUse += Use;
    }

    private void Use(EntityController entityController, ClickType clickType)
    {
        if (clickType == ClickType.Press)
        {
            entityController.health.RemoveEffect(effectID);
        }
    }
}
