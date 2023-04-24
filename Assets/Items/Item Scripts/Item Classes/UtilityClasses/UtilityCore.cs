using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCore : ItemCore
{
    [HideInInspector]
    public new Utility item;

    private void Start()
    {
        //use animation //(dev thoghts: NO FUCK YOU I DONT WANT TO)

        item = (Utility)base.item;
    }


    public override void MainUse(ClickType clickType)
    {        
        if(null != OnMainUse)
        {
            OnMainUse(entityController, clickType);
        }
        
        if (clickType == ClickType.Press)
        {
            PlayerInventory._instance.UseUtility(item);
        }
    }
}
