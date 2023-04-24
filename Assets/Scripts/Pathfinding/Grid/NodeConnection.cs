using UnityEngine;

/// <summary>
/// A connection between two worldnodes
/// </summary>
public class NodeConnection
{
    public float Distance;

    public IntVector2 RelativePosition;

    public bool IsSet { get; private set; }

    public NodeConnection(float _Distance, IntVector2 _RelativePosition)
    {
        Distance = _Distance;
        RelativePosition = _RelativePosition;

        IsSet = true;
    }
}
