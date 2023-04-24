using UnityEngine;

public static class Debugging
{
    public static void DrawCross(Vector2 Position, float Size, Color color)
    {
        float halfOfSize = Size / 2;

        Debug.DrawRay(new Vector2(Position.x - halfOfSize, Position.y), Vector2.right * Size, color);
        Debug.DrawRay(new Vector2(Position.x, Position.y + halfOfSize), Vector2.down * Size, color);
    }
}
