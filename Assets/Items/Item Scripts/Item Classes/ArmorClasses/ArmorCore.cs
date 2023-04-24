using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCore : ItemCore
{
    [HideInInspector]
    public new Armor item;

    public override void Activate(EntityController _entityController)
    {
        _entityController.health.ChangeMaxArmor((int)(item.itemData.armor * item.powerMultiplier));

        base.Activate(_entityController);
    }

    public override void Deactivate()
    {
        entityController.health.ChangeMaxArmor(-(int)(item.itemData.armor * item.powerMultiplier));

        base.Deactivate();
        Destroy(gameObject);
    }
}
