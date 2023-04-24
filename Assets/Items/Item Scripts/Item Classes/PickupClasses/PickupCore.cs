using UnityEngine;

public class PickupCore : ItemCore
{
    [HideInInspector]
    public new Pickup item;

    public delegate bool InformPickupClasses(EntityController _entityController);
    public InformPickupClasses OnValidatePickup;

    public void Start()
    {
        item = (Pickup)base.item;
    }

    public override void Activate(EntityController _entityController)
    {
        base.Activate(_entityController);

        Destroy(gameObject);
    }

    public bool ValidatePickup(EntityController _entityController)
    {
        return OnValidatePickup(_entityController);
    }
}
