using UnityEngine;
using UnityEngine.InputSystem;

public enum ClickType
{
    Press,
    Release
}

public class PlayerEquipedItemsController : EquipedItemsController
{
    public static PlayerEquipedItemsController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }


    public override void DeactivateItem()
    {
        base.DeactivateItem();

        PlayerInventory._instance.ActiveItem = null;
    }

    public override void SetActiveItem(Item _item)
    {
        base.SetActiveItem(_item);

        PlayerInventory._instance.ActiveItem = _item;
    }

    public void MainUse(InputAction.CallbackContext context)
    {
        if (AnyActiveItem)
        {
            if (context.started)
            {
                ActiveItemCore.MainUse(ClickType.Press);
            }
            else if (context.canceled)
            {
                ActiveItemCore.MainUse(ClickType.Release);
            }
        }
    }

    public void SecondaryUse(InputAction.CallbackContext context)
    {
        if (AnyActiveItem)
        {
            if (context.started)
            {
                ActiveItemCore.SecondaryUse(ClickType.Press);
            }
            else if (context.canceled)
            {
                ActiveItemCore.SecondaryUse(ClickType.Release);
            }
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (AnyActiveItem)
        {
            if (context.started)
            {
                ActiveItemCore.Reload();
            }
        }
    }
}
