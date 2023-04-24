using UnityEngine;

public static class RandomNumbers
{
    //private static System.Random random = new System.Random();

    public static int RandomInt(int MinNumber, int MaxNumber)
    {
        return Random.Range(MinNumber, MaxNumber + 1);
    }

    public static T RandomFromArgs<T>(params T[] choices)
    {
        return choices[Random.Range(0, choices.Length)];
    }

    /*public static float RandomNumberFromArgs(params float[] numbers)
    {
        return numbers[Random.Range(0, numbers.Length)];
    }*/

    //-1 or 1
    public static int RandomDirection()
    {
        return (Random.Range(0, 2) << 1) - 1;
    }

    public static Vector2 RandomVector(Vector2 v1, Vector2 v2)
    {
        return new Vector2(Random.Range(v1.x, v2.x), Random.Range(v1.y, v2.y));
    }

    public static bool RandomChance(float chance)
    {
        return Random.Range(0, 1f) < chance;
    }
}
