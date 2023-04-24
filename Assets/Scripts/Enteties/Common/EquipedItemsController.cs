using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItemsController : MonoBehaviour
{
    private void OnEnable()
    {
        activeTrinketObjects = new GameObject[maxNumberOfTrinkets];
        activeTrinkets = new Trinket[maxNumberOfTrinkets];
    }

    private void Start()
    {
        isFacingRight = entityController.isFacingRight;
    }

    [SerializeField] private int maxNumberOfTrinkets;
    protected GameObject[] activeTrinketObjects;
    protected Trinket[] activeTrinkets;

    public Transform MainArmPivot;
    [SerializeField]
    private int MainSpriteLayer;

    public Transform SecondItemHolder;
    public Transform SecondArmPivot;
    [SerializeField]
    private int SecondSpriteLayer;

    [SerializeField]
    private bool isFacingRight = true;

    [SerializeField]
    protected EntityController entityController;

    protected GameObject ActiveItem;
    protected bool AnyActiveItem;
    private SpriteRenderer ActiveItemSpriteRenderer;
    protected ItemCore ActiveItemCore;

    private Transform GetItemHolder() => isFacingRight ? this.transform : SecondItemHolder;

    public Transform GetArmPivot() => isFacingRight ? MainArmPivot : SecondArmPivot;

    private int GetSpriteLayer() => isFacingRight ? MainSpriteLayer : SecondSpriteLayer;

    public void RestoreFacing()
    {
        isFacingRight = entityController.isFacingRight;
    }

    public void SwapArm()
    {
       isFacingRight = !isFacingRight;

        if (true == AnyActiveItem)
        {
            ActiveItem.transform.SetParent(GetItemHolder(), false);
            ActiveItemSpriteRenderer.sortingOrder = GetSpriteLayer();
        }

    }

    public void EquipTrinket(Trinket _newTrinket)
    {
        GameObject trinketObject = Instantiate(_newTrinket.itemData.itemObject, transform);
        ItemCore core =  trinketObject.GetComponent<ItemCore>();
        core.item = _newTrinket;
        core.Activate(entityController);

        for (int i = 0; i < activeTrinketObjects.Length; i++)
        {
            if (null == activeTrinkets[i])
            {
                activeTrinketObjects[i] = trinketObject;
                activeTrinkets[i] = _newTrinket;

                return;
            }
        }
    }

    public void RemoveTrinket(Trinket _trinket)
    {
        for (int i = 0; i < activeTrinketObjects.Length; i++)
        {
            if (activeTrinkets[i] == _trinket)
            {
                activeTrinketObjects[i].GetComponent<ItemCore>().Deactivate();
                activeTrinkets[i] = null;

                Destroy(activeTrinketObjects[i]);

                activeTrinketObjects[i] = null;

                return;
            }
        }
    }


    public virtual void DeactivateItem()
    {
        ActiveItemCore.Deactivate();
        Destroy(ActiveItem);
        ActiveItem = null;
        AnyActiveItem = false;
    }

    public virtual void SetActiveItem(Item _item)
    {
        if (AnyActiveItem)
        {
            DeactivateItem();
        }

        ActiveItem = Instantiate(_item.itemData.itemObject, GetItemHolder());
        AnyActiveItem = true;

        ActiveItemSpriteRenderer = ActiveItem.GetComponent<SpriteRenderer>();
        ActiveItemSpriteRenderer.sortingOrder = GetSpriteLayer();

        ActiveItemCore = ActiveItem.GetComponent<ItemCore>();
        ActiveItemCore.item = _item;
        ActiveItemCore.Activate(entityController);
    }
}
