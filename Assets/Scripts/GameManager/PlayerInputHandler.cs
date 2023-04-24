using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler _instance;

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

    private InputActionMap[] inputActionMaps = new InputActionMap[6];

    private void Start()
    {
        InputActionAsset inputActionAsset = GetComponent<PlayerInput>().actions;

        inputActionMaps[0] = inputActionAsset.actionMaps[0]; //PlayerMovement
        inputActionMaps[1] = inputActionAsset.actionMaps[1]; //ItemComparing
        inputActionMaps[2] = inputActionAsset.actionMaps[2]; //Toolbar
        inputActionMaps[3] = inputActionAsset.actionMaps[3]; //EquipedItem
        inputActionMaps[4] = inputActionAsset.actionMaps[4]; //PlayerAction
        inputActionMaps[5] = inputActionAsset.actionMaps[5]; //GameControl

        SetDefault();
    }

    public void DisableAll()
    {
        inputActionMaps[0].Disable();
        inputActionMaps[1].Disable();
        inputActionMaps[2].Disable();
        inputActionMaps[3].Disable();
        inputActionMaps[4].Disable();
        inputActionMaps[5].Disable();
    }


    public void SetDefault()
    {
        inputActionMaps[0].Enable();
        inputActionMaps[1].Disable();
        inputActionMaps[2].Enable();
        inputActionMaps[3].Enable();
        inputActionMaps[4].Enable();
        inputActionMaps[5].Disable();
    }


    public void EnableCompareMode()
    {
        inputActionMaps[1].Enable();
        inputActionMaps[3].Disable();
        inputActionMaps[4].Disable();
    }

    public void DisableCompareMode()
    {
        inputActionMaps[1].Disable();
        inputActionMaps[3].Enable();
        inputActionMaps[4].Enable();
    }


    public void EnableInventory()
    {
        inputActionMaps[2].Disable();
        inputActionMaps[3].Disable();
    }

    public void DisableInventory()
    {
        inputActionMaps[2].Enable();
        inputActionMaps[3].Enable();
    }

    public void EnableToolbar()
    {
        inputActionMaps[2].Enable();
    }

    public void DisableToolbar()
    {
        inputActionMaps[2].Disable();
    }

    public void EnableGameControl()
    {
        inputActionMaps[5].Enable();
    }

    public void DisableGameControl()
    {
        inputActionMaps[5].Disable();
    }

    #region Toolbar
    public void OnSlot_1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(0);
        }
    }

    public void OnSlot_2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(1);
        }
    }

    public void OnSlot_3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(2);
        }
    }

    public void OnSlot_4(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(3);
        }
    }

    public void OnSlot_5(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(4);
        }
    }

    public void OnSlot_6(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(5);
        }
    }

    public void OnSlot_7(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(6);
        }
    }

    public void OnSlot_8(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(7);
        }
    }

    public void OnSlot_9(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(8);
        }
    }

    public void OnSlot_10(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarUIController._instance.ActivateSlot(9);
        }
    }
    #endregion Toolbar

    #region Inventory
    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToolbarInventoryHandler._instance.ToggleInventory();
        }
    }

    public void DeactivateInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ToolbarInventoryHandler._instance.inventoryIsActive)
            {
                ToolbarInventoryHandler._instance.DeactivateInventory();
            }
            else
            {
                PauseScreenUIController.TogglePause();
            }
        }
    }
    #endregion Inventory

    #region Comparing
    public void OnAcceptCompare(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.AcceptCompare();
        }
    }

    public void OnCancelCompare(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.CancelCompare();
        }
    }

    public void OnChangeDisplaySides(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //PlayerUIController._instance.ChangeDisplaySides();
        }
    }

    public void OnNextBarSlot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.NextBarSlot();
        }
    }

    public void OnPreviousBarSlot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.PreviousBarSlot();
        }
    }

    public void OnBarSlot_1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.BarSlot_1();
        }
    }

    public void OnBarSlot_2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.BarSlot_2();
        }
    }

    public void OnBarSlot_3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.BarSlot_3();
        }
    }

    public void OnBarSlot_4(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.BarSlot_4();
        }
    }

    public void OnBarSlot_5(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerUIController._instance.BarSlot_5();
        }
    }
    #endregion Comparing

    public void ReloadGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneLoader.ReloadScene();
        }
    }
}
