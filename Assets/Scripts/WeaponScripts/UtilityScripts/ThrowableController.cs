using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowableController : MonoBehaviour
{
    public abstract void Initiate(Vector2 velocityVector, UtilityCore core);
}
