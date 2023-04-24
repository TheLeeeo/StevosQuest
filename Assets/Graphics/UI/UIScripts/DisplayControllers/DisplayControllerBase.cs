using UnityEngine;
using UnityEngine.UI;

public abstract class DisplayControllerBase : MonoBehaviour
{
    [SerializeField]
    protected Image ItemSprite;
    [SerializeField]
    protected Image ItemFrame;
    [SerializeField]
    protected Text ItemName;

    [SerializeField]
    protected Text tooltipText;

    [SerializeField]
    protected StatDisplayController[] statDisplayControllers;

    [SerializeField]
    protected Sprite[] RarityFrames;

    public virtual void Initiate(Item _displayItem)
    {
        ItemSprite.sprite = _displayItem.itemData.itemSprite;
        ItemSprite.preserveAspect = true;

        ItemName.text = _displayItem.itemData.itemName;

        ItemFrame.sprite = RarityFrames[(int)_displayItem.rarity];
    }

    public void Reset()
    {        
        for (int i = 0; i < statDisplayControllers.Length; i++)
        {
            statDisplayControllers[i].Clear();
        }
    }
}
