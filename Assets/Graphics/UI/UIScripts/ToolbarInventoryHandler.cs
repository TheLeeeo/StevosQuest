using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolbarInventoryHandler : MonoBehaviour
{
    public static ToolbarInventoryHandler _instance { get; private set; }

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


    [SerializeField] private RectTransform inventoryTransform;
    [SerializeField] private RectTransform toolbarTransform;
    [SerializeField] private RectTransform ammobarTransform;

    private readonly Vector3 UnactiveAmmobarLocation = new Vector3(0, -100, 0);
    private readonly Vector3 ActiveAmmobarLocation = new Vector3(0, 100, 0);
    private readonly Vector3 UnactiveInventoryLocation = new Vector3(0, 350f, 0);
    private readonly Vector3 ActiveInventoryLocation = new Vector3(0, -540f, 0);
    private readonly Vector3 UnactiveToolbarLocation = new Vector3(0, -50, 0);
    private readonly Vector3 ActiveToolbarLocation = new Vector3(0, 50, 0);
    private const float ANIMATION_SPEED = 0.3f;

    public bool inventoryIsActive = false;

    public void ToggleInventory()
    {
        if (inventoryIsActive)
        {
            DeactivateInventory();
        }
        else
        {
            ActivateInventory();
        }
    }

    public void ActivateInventory()
    {
        if (false == inventoryIsActive)
        {
            inventoryIsActive = true;

            EnableInventory();
            EnableAmmobar();
            LeanTween.move(inventoryTransform, ActiveInventoryLocation, ANIMATION_SPEED);
            LeanTween.move(ammobarTransform, ActiveAmmobarLocation, ANIMATION_SPEED);
            PlayerInputHandler._instance.EnableInventory();
            DeactivateToolbar();
        }
    }

    public void DeactivateInventory()
    {
        if (inventoryIsActive)
        {
            inventoryIsActive = false;

            LeanTween.move(inventoryTransform, UnactiveInventoryLocation, ANIMATION_SPEED).setOnComplete(DisableInventory);
            LeanTween.move(ammobarTransform, UnactiveAmmobarLocation, ANIMATION_SPEED).setOnComplete(DisableAmmobar);
            PlayerInputHandler._instance.DisableInventory();
            ActivateToolbar();
        }
    }

    public void ActivateToolbar()
    {
        EnableToolbar();
        LeanTween.cancel(toolbarTransform);
        LeanTween.move(toolbarTransform, ActiveToolbarLocation, ANIMATION_SPEED);
        PlayerInputHandler._instance.EnableToolbar();
    }

    public void DeactivateToolbar()
    {
        LeanTween.move(toolbarTransform, UnactiveToolbarLocation, ANIMATION_SPEED).setOnComplete(DisableToolbar);
        PlayerInputHandler._instance.DisableToolbar();
    }

    private void EnableToolbar()
    {
        toolbarTransform.gameObject.SetActive(true);
    }

    private void DisableToolbar()
    {
        toolbarTransform.gameObject.SetActive(false);
    }

    private void EnableInventory()
    {
        inventoryTransform.gameObject.SetActive(true);
    }

    private void DisableInventory()
    {
        inventoryTransform.gameObject.SetActive(false);
    }

    private void EnableAmmobar()
    {
        ammobarTransform.gameObject.SetActive(true);
    }

    private void DisableAmmobar()
    {
        ammobarTransform.gameObject.SetActive(false);
    }
}
