using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    public static PlayerUIController _instance { get; private set; }

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

    [SerializeField] private GameObject ArmorDisplayObject;
    [SerializeField] private GameObject WeaponDisplayObject;
    [SerializeField] private GameObject TrinketDisplayObject;
    [SerializeField] private GameObject UtilityDisplayObject;

    [SerializeField] private GameObject Bar_2Slot;
    [SerializeField] private GameObject Bar_4Slot;
    [SerializeField] private GameObject Bar_5Slot;


    private Item item_1;
    private Item item_2;

    private GameObject ActiveDisplay_1;
    private RectTransform DisplayRectTransform_1;
    private DisplayControllerBase DisplayController_1;
    private bool Display_1_IsSet;
    
    private GameObject ActiveDisplay_2;
    private RectTransform DisplayRectTransform_2;
    private DisplayControllerBase DisplayController_2;
    private bool Display_2_IsSet;

    private GameObject ActiveSlotBar;
    private RectTransform SlotBarRectTransform;
    private SlotBarController slotBarController;
    private bool SlotBarIsSet;

    private const float DisplayX = 300f;
    private const float UnactiveDisplayX = 1200f;
    private const float UnactiveDisplayY = 846.875f;
    private const float SlotBarX = -625;
    //private const float UnactiveSlotbarY = 668.75f;

    [SerializeField]
    private float AnimationSpeed; //The time the animations takes to run

    private bool CurrentlyAnimatingChange = false;

    private bool CurrentlyComparing = false;

    #region Input

    public void AcceptCompare()
    {
        if (CurrentlyComparing)
        {
            AcceptItemSwap();
        } else //Inspecting
        {
            Animate_Away_Down(DisplayRectTransform_1);
            PlayerInputHandler._instance.DisableCompareMode();
            Display_1_IsSet = false;

            ToolbarInventoryHandler._instance.ActivateInventory();
        }
    }

    public void CancelCompare()
    {
        if (CurrentlyComparing)
        {
            DeclineItemSwap();
        } else //inspecting
        {
            Animate_Away_Down(DisplayRectTransform_1);
            PlayerInputHandler._instance.DisableCompareMode();
            Display_1_IsSet = false;

            ToolbarInventoryHandler._instance.ActivateInventory();
        }
        
    }

    public void NextBarSlot()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(slotBarController.ActiveSlotID + 1);
        }
    }

    public void PreviousBarSlot()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(slotBarController.ActiveSlotID - 1);
        }
    }

    public void BarSlot_1()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(0);
        }
    }

    public void BarSlot_2()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(1);
        }
    }

    public void BarSlot_3()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(2);
        }
    }

    public void BarSlot_4()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(3);
        }
    }

    public void BarSlot_5()
    {
        if (SlotBarIsSet)
        {
            slotBarController.ActivateSlot(4);
        }
    }

    #endregion Input

    #region Animations
    private void Animate_InspectIn()
    {
        LeanTween.move(DisplayRectTransform_1, Vector3.zero, AnimationSpeed);
    }

    private void Animate_CompareIn(RectTransform uiRectTransform, int side) //Side is 1 or -1.
    {
        LeanTween.move(uiRectTransform, new Vector3(DisplayX *  side, 0, 0), AnimationSpeed);
    }

    private void Animate_CompareIn_FromPoint(RectTransform uiRectTransform)
    {
        
        LeanTween.move(uiRectTransform, new Vector3(-DisplayX, 0, 0), AnimationSpeed);
        LeanTween.scale(uiRectTransform, new Vector3(1.25f, 1.25f, 0), AnimationSpeed);
    }

    private void Animate_CompareInMainDisplay_FromPoint() //Animates the left Display emerging from the itemslot of the SlotBar that has called. Yet again, LeedTween cringe smh...
    {
        Animate_CompareIn_FromPoint(DisplayRectTransform_1);
    }

    private LTDescr Animate_CompareAway_ToPoint(RectTransform uiRectTransform, Vector3 point) //Side is 1 or -1.
    {
        LTDescr descr = LeanTween.move(uiRectTransform, point, AnimationSpeed);
        LeanTween.scale(uiRectTransform, Vector3.zero, AnimationSpeed);
        return descr;
    }

    private void Animate_CompareAccept(RectTransform uiRectTransform, int side)
    {
        LeanTween.move(uiRectTransform, new Vector3(UnactiveDisplayX * side, 0, 0), AnimationSpeed).destroyOnComplete = true;
    }

    private void Animate_SlotBarIn()
    {
        LeanTween.move(SlotBarRectTransform, new Vector3(SlotBarX, 0, 0), AnimationSpeed);
    }


    private void Animate_Away_Up(RectTransform uiRectTransform) //Animates the RectTransform out to the top and destroys the gameObject afterwards.
    {
        LeanTween.move(uiRectTransform, new Vector3(uiRectTransform.transform.localPosition.x, UnactiveDisplayY, 0), AnimationSpeed).destroyOnComplete = true;
    }

    private void Animate_Away_Down(RectTransform uiRectTransform) //Animates the RectTransform out to the top and destroys the gameObject afterwards.
    {
        LeanTween.move(uiRectTransform, new Vector3(uiRectTransform.transform.localPosition.x, -UnactiveDisplayY, 0), AnimationSpeed).destroyOnComplete = true;
    }
    #endregion Animations

    public void DeactiveteDisplayObjects() //Destroys all of the displays
    {
        if (Display_1_IsSet)
        {
            Animate_Away_Up(DisplayRectTransform_1);
            Display_1_IsSet = false;
        }

        if (Display_2_IsSet)
        {
            Animate_Away_Up(DisplayRectTransform_2);
            Display_2_IsSet = false;
        }

        if (SlotBarIsSet)
        {
            Animate_Away_Up(SlotBarRectTransform);
            SlotBarIsSet = false;
        }

        PlayerInputHandler._instance.DisableCompareMode();

        ToolbarInventoryHandler._instance.ActivateToolbar();
    }

    private void ChangeLeftDisplayItem() //Called when the animation to change the left display item is completed.
    {
        DisplayController_1.Reset();
        DisplayController_1.Initiate(item_1);
        LeanTween.delayedCall(AnimationSpeed, ToggleAnimationFlag);
    }

    private void ToggleAnimationFlag()
    {
        CurrentlyAnimatingChange = !CurrentlyAnimatingChange;
    }

    #region Compare
    private void InstantiateDisplays(GameObject displayObjectType)
    {
        CurrentlyComparing = true;

        ///2 before one to the tooltips are rendered in front
        ActiveDisplay_2 = Instantiate(displayObjectType, transform);
        DisplayRectTransform_2 = ActiveDisplay_2.GetComponent<RectTransform>();
        DisplayRectTransform_2.localPosition = new Vector3(DisplayX, -UnactiveDisplayY, 0);
        Display_2_IsSet = true;
        ActiveDisplay_2.SetActive(true);


        ActiveDisplay_1 = Instantiate(displayObjectType, transform);
        DisplayRectTransform_1 = ActiveDisplay_1.GetComponent<RectTransform>();
        DisplayRectTransform_1.localPosition = new Vector3(-DisplayX, -UnactiveDisplayY, 0);
        Display_1_IsSet = true;
        ActiveDisplay_1.SetActive(true);
        

        Animate_CompareIn(DisplayRectTransform_1, -1);
        Animate_CompareIn(DisplayRectTransform_2, 1);

        PlayerInputHandler._instance.EnableCompareMode();
        ToolbarInventoryHandler._instance.DeactivateInventory();
        ToolbarInventoryHandler._instance.DeactivateToolbar();
    }

    private void InstantiateDisplayControllers<T>(Item _item1, Item _item2) where T : DisplayControllerBase
    {
        DisplayController_1 = ActiveDisplay_1.GetComponent<T>();
        DisplayController_1.Initiate(_item1);

        DisplayController_2 = ActiveDisplay_2.GetComponent<T>();
        DisplayController_2.Initiate(_item2);
    }

    private void InstantiateSlotBar(GameObject barObject, Item[] equipedItems)
    {
        ActiveSlotBar = Instantiate(barObject, transform);
        slotBarController = ActiveSlotBar.GetComponent<SlotBarController>();
        slotBarController.Initiate(equipedItems);
        SlotBarRectTransform = slotBarController.GetComponent<RectTransform>();
        SlotBarRectTransform.localPosition = new Vector3(-UnactiveDisplayX, 0, 0);
        SlotBarIsSet = true;
        ActiveSlotBar.SetActive(true);

        Animate_SlotBarIn();
    }


    public void Compare(Armor armor1, Armor armor2)
    {
        DeactiveteDisplayObjects();

        InstantiateDisplays(ArmorDisplayObject);
        InstantiateDisplayControllers<ArmorDisplayController>(armor1, armor2);

        item_1 = armor1;
        item_2 = armor2;
    }


    public void Compare(Weapon[] equipedWeapons, Weapon weapon2)
    {
        DeactiveteDisplayObjects();

        InstantiateDisplays(WeaponDisplayObject);
        InstantiateDisplayControllers<WeaponDisplayController>(equipedWeapons[0], weapon2);

        InstantiateSlotBar(Bar_2Slot, equipedWeapons);

        item_1 = equipedWeapons[0];
        item_2 = weapon2;
    }

    public void Compare(Weapon weapon1, Weapon weapon2)
    {
        DeactiveteDisplayObjects();

        InstantiateDisplays(WeaponDisplayObject);
        InstantiateDisplayControllers<WeaponDisplayController>(weapon1, weapon2);

        item_1 = weapon1;
        item_2 = weapon2;
    }


    public void Compare(Trinket[] equipedTrinkets, Trinket trinket2)
    {
        DeactiveteDisplayObjects();

        InstantiateDisplays(TrinketDisplayObject);
        InstantiateDisplayControllers<TrinketDisplayController>(equipedTrinkets[0], trinket2);

        InstantiateSlotBar(Bar_4Slot, equipedTrinkets);

        item_1 = equipedTrinkets[0];
        item_2 = trinket2;
    }

    public void Compare(Utility utility1, Utility utility2)
    {
        DeactiveteDisplayObjects();

        InstantiateDisplays(UtilityDisplayObject);
        InstantiateDisplayControllers<UtilityDisplayController>(utility1, utility2);

        item_1 = utility1;
        item_2 = utility2;
    }

    public void Compare(Utility[] equipedUtilities, Utility utility2)
    {
        DeactiveteDisplayObjects();

        InstantiateDisplays(UtilityDisplayObject);
        InstantiateDisplayControllers<UtilityDisplayController>(equipedUtilities[0], utility2);

        InstantiateSlotBar(Bar_5Slot, equipedUtilities);

        item_1 = equipedUtilities[0];
        item_2 = utility2;
    }

    /// <summary>
    /// Changes the item in the left display to another one from the slotbar.
    /// </summary>
    public bool ChangeComparedItem(Item _item, Vector3 animationPosition)
    {
        if(false == CurrentlyAnimatingChange)
        {
            CurrentlyAnimatingChange = true;

            item_1 = _item;
            Animate_CompareAway_ToPoint(DisplayRectTransform_1, animationPosition).setOnComplete(Animate_CompareInMainDisplay_FromPoint);
            LeanTween.delayedCall(AnimationSpeed, ChangeLeftDisplayItem);

            return true;
        }

        return false;


    }
    #endregion Compare

    #region Inspect
    private void InstantiateSingleDisplay(GameObject displayObjectType)
    {
        ActiveDisplay_1 = Instantiate(displayObjectType, transform);
        DisplayRectTransform_1 = ActiveDisplay_1.GetComponent<RectTransform>();
        DisplayRectTransform_1.localPosition = new Vector3(0, -UnactiveDisplayY, 0);
        Display_1_IsSet = true;
        ActiveDisplay_1.SetActive(true);

        PlayerInputHandler._instance.EnableCompareMode();

        ToolbarInventoryHandler._instance.DeactivateInventory();
        ToolbarInventoryHandler._instance.DeactivateToolbar();
    }

    private void InstantiateInspectDisplayController<T>(Item _item1) where T : DisplayControllerBase
    {
        DisplayController_1 = ActiveDisplay_1.GetComponent<T>();
        DisplayController_1.Initiate(_item1);
    }


    public void Inspect(Armor _armor)
    {
        DeactiveteDisplayObjects();

        InstantiateSingleDisplay(ArmorDisplayObject);
        InstantiateInspectDisplayController<ArmorDisplayController>(_armor);

        Animate_InspectIn();
    }

    public void Inspect(Weapon _weapon)
    {
        DeactiveteDisplayObjects();

        InstantiateSingleDisplay(WeaponDisplayObject);
        InstantiateInspectDisplayController<WeaponDisplayController>(_weapon);

        Animate_InspectIn();
    }

    public void Inspect(Trinket _trinket)
    {
        DeactiveteDisplayObjects();

        InstantiateSingleDisplay(TrinketDisplayObject);
        InstantiateInspectDisplayController<TrinketDisplayController>(_trinket);

        Animate_InspectIn();
    }

    public void Inspect(Utility _utility)
    {
        DeactiveteDisplayObjects();

        InstantiateSingleDisplay(UtilityDisplayObject);
        InstantiateInspectDisplayController<UtilityDisplayController>(_utility);

        Animate_InspectIn();
    }
    #endregion Inspect

    public void AcceptItemSwap()
    {        
        Animate_CompareAccept(DisplayRectTransform_1, 1);
        Animate_CompareAccept(DisplayRectTransform_2, -1);

        if (SlotBarIsSet)
        {
            Animate_CompareAccept(SlotBarRectTransform, -1);
        }

        PlayerInventory._instance.DropItem(item_1);
        PlayerInventory._instance.AddItem(item_2);

        Display_1_IsSet = false;
        Display_2_IsSet = false;
        SlotBarIsSet = false;

        PlayerInputHandler._instance.DisableCompareMode();

        ToolbarInventoryHandler._instance.ActivateToolbar();

        CurrentlyComparing = false;
    }

    public void DeclineItemSwap()
    {       
        DropController.ReleaseItem(item_2, PlayerInventory._instance.transform.position);

        DeactiveteDisplayObjects();

        CurrentlyComparing = false;
    }

}
