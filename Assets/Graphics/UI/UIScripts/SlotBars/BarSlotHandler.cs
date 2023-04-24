using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarSlotHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private SlotBarController BarController;
    [SerializeField]
    public Image SlotImage;
    public int SlotIndex;

    public void SetItem(Item _item)
    {
        SlotImage.sprite = _item.itemData.itemSprite;
        SlotImage.preserveAspect = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BarController.ActivateSlot(SlotIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BarController.HighlightSlot(SlotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BarController.UnHighlightSlot();
    }
}
