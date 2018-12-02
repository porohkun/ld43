using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Extensions
{
    public static Color SetA(this Color color, float a)
    {
        return new Color(color.r, color.g, color.b, a);
    }

    public static bool IsZero(this float value)
    {
        return Mathf.Abs(value) < 0.00001f;
    }

    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value == null)
            return true;
        for (int index = 0; index < value.Length; ++index)
        {
            if (!char.IsWhiteSpace(value[index]))
                return false;
        }
        return true;
    }

    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        return list.ToArray().GetRandom();
    }

    public static T GetRandom<T>(this List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T GetRandom<T>(this T[] list)
    {
        if (list.Length == 0)
            return default(T);
        return list[UnityEngine.Random.Range(0, list.Length)];
    }

}
