using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int SlotId;

    [SerializeField]
    private Image ItemDisplay;

    [SerializeField]
    private Sprite DefaultSprite;
    [HideInInspector]
    public Sprite EquipedItemSprite;

    public virtual void SetItem(Item newItem)
    {
        ItemDisplay.sprite = newItem.itemData.itemSprite;
    }

    public virtual void Clear()
    {
        ItemDisplay.sprite = DefaultSprite;
    }
}
