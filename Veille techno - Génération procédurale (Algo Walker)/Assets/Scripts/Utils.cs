using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static readonly Dictionary<string, Vector2> DIRECTIONS_4 = new Dictionary<string, Vector2>
    {
        { "Up", Vector2.up },
        { "Down", Vector2.down },
        { "Left", Vector2.left },
        { "Right", Vector2.right }
    };

    public static List<Vector2> GetAdjacentsCells(Vector2 cell)
    {
        List<Vector2> adjacents = new List<Vector2>();

        foreach (Vector2 dir in DIRECTIONS_4.Values)
        {
            adjacents.Add(cell + dir);
        }

        return adjacents;
    }

    public static List<int> CreateListFromInt(int value)
    {
        List<int> ints = new List<int>();

        for (int i = 0; i < value; i++)
        {
            ints.Add(i);
        }

        return ints;
    }

    public static void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
