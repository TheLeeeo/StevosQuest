using UnityEngine;

public class DroppedItemBase : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public Item droppedItem;

    public virtual void InstantiateDrop(Item _item)
    {
        droppedItem = _item;

        spriteRenderer.sprite = _item.itemData.itemSprite;
    }
}
