using UnityEngine;

public struct IntVector2
{
    public int x;
    public int y;

    public static IntVector2 zero { get; private set; }
    public static IntVector2 max { get; private set; }
    public static IntVector2 min { get; private set; }

    static IntVector2()
    {
        zero = new IntVector2(0, 0);
        max = new IntVector2(int.MaxValue, int.MaxValue);
        min = new IntVector2(int.MinValue, int.MinValue);
    }

    public IntVector2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public IntVector2(Vector2 RegularVector2)
    {
        x = (int)RegularVector2.x;
        y = (int)RegularVector2.y;
    }

    public IntVector2(Vector3 RegularVector3)
    {
        x = (int)RegularVector3.x;
        y = (int)RegularVector3.y;
    }

    public float Distance()
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    public float SquaredDistance()
    {
        return x * x + y * y;
    }

    public float DistanceTo(IntVector2 OtherPoint)
    {
        return Mathf.Sqrt(Mathf.Pow(x - OtherPoint.x, 2) + Mathf.Pow(y - OtherPoint.y, 2));
    }

    public float SquaredDistanceTo(IntVector2 OtherPoint)
    {
        return Mathf.Pow(x - OtherPoint.x, 2) + Mathf.Pow(y - OtherPoint.y, 2);
    }

    public IntVector2 RelativePositionTo(IntVector2 OtherPoint)
    {
        return new IntVector2(OtherPoint.x - x, OtherPoint.y - y);
    }

    public float Simpledistance()
    {
        /// <summary>
        /// The manhattan distance of the vector with diagonals;
        /// </summary>

        return 1.4f * Mathf.Min(x, y) + Mathf.Abs(y - x);
    }

    public float SimpleDistanceTo(IntVector2 OtherPoint)
    {
        IntVector2 AbsoluteVectorToPoint = new IntVector2(Mathf.Abs(OtherPoint.x - x), Mathf.Abs(OtherPoint.y - y));
        return 1.4f * Mathf.Min(x, y) + Mathf.Abs(y - x);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return !(a.x == b.x && a.y == b.y);
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x + b.x, a.y + b.y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x - b.x, a.y - b.y);
    }

    public static IntVector2 operator -(IntVector2 a)
    {
        return new IntVector2(-a.x, -a.y);
    }

    public static implicit operator Vector2(IntVector2 a)
    {
        return new Vector2(a.x, a.y);
    }

    public static explicit operator IntVector2(Vector2 a)
    {
        return new IntVector2((int)a.x, (int)a.y);
    }

    public static implicit operator Vector3(IntVector2 a)
    {
        return new Vector3(a.x, a.y, 0);
    }

    public static explicit operator IntVector2(Vector3 a)
    {
        return new IntVector2((int)a.x, (int)a.y);
    }

    public override string ToString() => $"{x}, {y}";

    public override int GetHashCode() => throw new System.NotImplementedException();

    public override bool Equals(object obj) => throw new System.NotImplementedException();
}
