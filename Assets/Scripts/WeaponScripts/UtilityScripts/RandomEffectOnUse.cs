using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEffectOnUse : MonoBehaviour
{
    [SerializeField]
    private TableItem<Effect>[] effects;
    private RandomTable<Effect> effectTable;

    private ItemCore itemCore;

    private void Awake()
    {
        itemCore = GetComponent<ItemCore>();
        itemCore.OnMainUse += Use;

        effectTable = new RandomTable<Effect>(effects);
    }

    private void Use(EntityController entityController, ClickType clickType)
    {
        if (clickType == ClickType.Press)
        {
            Effect effect = effectTable.GetItem();
            effect.EffectLevel = itemCore.item.rarity;

            entityController.health.GiveEffect(effect);
        }
    }
}
