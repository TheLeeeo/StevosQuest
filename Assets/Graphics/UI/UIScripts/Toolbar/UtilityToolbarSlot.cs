using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilityToolbarSlot : ToolbarSlot
{
    [SerializeField]
    private Text utilityCountText;

    public override void SetItem(Item newItem)
    {
        base.SetItem(newItem);

        utilityCountText.enabled = true;
        utilityCountText.text = ((Utility)newItem).itemCount.ToString();
    }

    public override void Clear()
    {
        base.Clear();

        utilityCountText.enabled = false;
    }

    public override void UpdateCount(int newAmmount)
    {
        utilityCountText.text = newAmmount.ToString();
    }
}
