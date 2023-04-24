using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    private Vector3 OriginalPosition;

    [SerializeField]
    private InventoryItemSlot itemSlot;

    [SerializeField]
    private CanvasGroup canvasGroup;

    /// <summary>
    /// Itemslot RectTransforms the item can be dropped in.
    /// </summary>
    [SerializeField]
    private GameObject[] SwappableSlots;
    private bool SwappableSlotsAreSet;

    /// <summary>
    /// The item is dropped from inventory if dropped outside of this RectTransform.
    /// </summary>
    [SerializeField]
    private RectTransform BoundingRectTransform;
    private bool BoundingRectTransformIsSet;

    [HideInInspector]
    public int itemType = -1;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemType != -1)
        {
            transform.SetAsLastSibling();
            OriginalPosition = transform.position;

            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.7f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemType != -1)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemType != -1)
        {
            transform.position = OriginalPosition;

            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;

            if (true == BoundingRectTransformIsSet && false == RectTransformUtility.RectangleContainsScreenPoint(BoundingRectTransform, Input.mousePosition)) //Item is dropped outside of bounding area.
            {
                InventoryUIController._instance.DropFunctions[itemType](itemSlot.SlotId);
                itemSlot.Clear();

                return;
            }
        }
    }

    public void EndSwapDrag()
    {
        transform.position = OriginalPosition;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void OnDrop(PointerEventData eventData) //For swapping items
    {
        if (true == SwappableSlotsAreSet)
        {
            for (int i = 0; i < SwappableSlots.Length; i++)
            {
                if(SwappableSlots[i] == eventData.pointerDrag)
                {
                    InventoryItemSlot otherSlot = eventData.pointerDrag.GetComponent<InventoryItemSlot>();
                    PlayerInventory._instance.Swap(otherSlot.dragHandler.itemType, otherSlot.SlotId, itemSlot.SlotId);

                    otherSlot.dragHandler.EndSwapDrag();
                }
            }
        }
    }

    void Start()
    {
        BoundingRectTransformIsSet = BoundingRectTransform != null;
        SwappableSlotsAreSet = SwappableSlots.Length > 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemType != -1 && eventData.button == PointerEventData.InputButton.Right) //Item exists in current slot and slot is clicked with the right mouse button.
        {
            InventoryUIController._instance.InspectItem(itemType, itemSlot.SlotId);
        }
    }
}
