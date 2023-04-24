using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarSlot : ItemSlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolbarUIController._instance.HighlightSlot(SlotId);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToolbarUIController._instance.ActivateSlot(SlotId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolbarUIController._instance.UnHighlightSlot();
    }

    public virtual void UpdateCount(int newAmmount) { }
}
