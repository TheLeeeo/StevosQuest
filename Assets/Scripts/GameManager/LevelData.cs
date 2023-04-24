using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData Current;

    private void Awake()
    {
        if (Current == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            Current = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + Current.gameObject + "\n");
            Destroy(this);
        }

        entityPowerMultiplier = powerMultiplier * 0.8f;
    }



    [SerializeField]
    private float powerMultiplier;

    private float entityPowerMultiplier;

    public static float GetPowerMultiplier()
    {
        return 1f;
        //return Current.powerMultiplier;
    }

    public static float GetEntityPowerMultiplier()
    {
        return 1f;
        //return Current.powerMultiplier;
        //return entityPowerMultiplier;
    }



    [SerializeField]
    private Transform damageTextCanvas;

    public static Transform GetDamageTextCanvas()
    {
        return Current.damageTextCanvas;
    }



    [SerializeField]
    private PickupMetaTable defaultPickupDropTable;

    public static PickupMetaTable GetDefaultPickupTable()
    {
        return Current.defaultPickupDropTable;
    }

    [SerializeField]
    private ItemTableBase defaultItemDropTable;

    public static ItemTableBase GetDefaultItemTable()
    {
        return Current.defaultItemDropTable;
    }

    [SerializeField]
    private EntityTable defaultEntityDropTable;

    public static EntityTable GetDefaultEntityTable()
    {
        return Current.defaultEntityDropTable;
    }
}
