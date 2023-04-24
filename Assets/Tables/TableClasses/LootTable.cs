using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : ScriptableObject
{
    public virtual Item GetLoot()
    {
        return null;
    }
}
