using UnityEngine;
using UnityEngine.UI;

public class SlotBarController : MonoBehaviour
{
    public BarSlotHandler[] ItemSlots;
    [SerializeField]
    private Item[] Items;
    public int ActiveSlotID = 0;

    [SerializeField]
    private GameObject ActiveSlotIndicator;
    [SerializeField]
    private GameObject HighlightedSlotIndicator;


    public void Initiate(Item[] items)
    {
        Items = items;

        for (int i = 0; i < Items.Length; i++)
        {
            ItemSlots[i].SetItem(Items[i]);
        }

        ActiveSlotIndicator.transform.position = ItemSlots[0].transform.position;
    }

    public void ActivateSlot(int slotIndex)
    {
        if(slotIndex >= ItemSlots.Length || slotIndex < 0) //Can be possible if called from outside source.
        {
            
            return; 
        }
        else if (ActiveSlotID != slotIndex)
        {
            if (PlayerUIController._instance.ChangeComparedItem(Items[slotIndex], ItemSlots[slotIndex].transform.localPosition + transform.localPosition)) //Change was successful. Unsuccessful in animation is already playing.
            {
                ActiveSlotID = slotIndex;
                ActiveSlotIndicator.transform.position = ItemSlots[slotIndex].transform.position;
            }
        }
    }

    public void HighlightSlot(int slotIndex)
    {
        HighlightedSlotIndicator.SetActive(true);
        HighlightedSlotIndicator.transform.position = ItemSlots[slotIndex].transform.position;
    }

    public void UnHighlightSlot()
    {
        HighlightedSlotIndicator.SetActive(false);
    }
}
