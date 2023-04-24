using UnityEngine;
using System;

public class ItemCore : MonoBehaviour
{
    public Item item;

    [HideInInspector]
    public EntityController entityController;

    public Action<EntityController> OnActivate;
    public Action<EntityController> OnDeactivate;
    public Action<EntityController, ClickType> OnMainUse;
    public Action<EntityController, ClickType> OnSecondaryUse;

    public virtual void Activate(EntityController _entityController)
    {
        entityController = _entityController;

        if (null != OnActivate)
        {            
            OnActivate(entityController);
        }       
    }

    public virtual void Deactivate()
    {
        if (null != OnDeactivate)
        {
            OnDeactivate(entityController);
        }
    }

    public virtual void MainUse(ClickType clickType)
    {
        if (null != OnMainUse)
        {
            OnMainUse(entityController, clickType);
        }
    }

    public virtual void SecondaryUse(ClickType clickType)
    {
        if (null != OnSecondaryUse)
        {
            OnSecondaryUse(entityController, clickType);
        }
    }

    public virtual void Reload() { }
}
