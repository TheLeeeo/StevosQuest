using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMath
{
    public static void FlipTransform(Transform transform)
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
    }
}
