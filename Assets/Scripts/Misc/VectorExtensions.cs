using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class VectorExtensions
{
    #region Z

    public static Vector3 AddZ(this Vector2 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static Vector3Int AddZ(this Vector2Int vector, int z)
    {
        return new Vector3Int(vector.x, vector.y, z);
    }

    public static Vector3Int SetZ(this Vector3Int vector, int z)
    {
        return new Vector3Int(vector.x, vector.y, z);
    }

    public static Vector2 RemZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector2Int RemZ(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }

    #endregion

    #region ScaleBy

    public static Vector3 ScaleBy(this Vector3 vector1, Vector3 vector2)
    {
        return Vector3.Scale(vector1, vector2);
    }

    public static Vector3 ScaleBy(this Vector3 vector1, float x, float y, float z)
    {
        return new Vector3(vector1.x * x, vector1.y * y, vector1.z * z);
    }

    public static Vector2 ScaleBy(this Vector2 vector1, Vector2 vector2)
    {
        return Vector2.Scale(vector1, vector2);
    }

    public static Vector2 ScaleBy(this Vector2 vector1, float x, float y)
    {
        return new Vector2(vector1.x * x, vector1.y * y);
    }

    public static Vector3Int ScaleBy(this Vector3Int vector1, Vector3Int vector2)
    {
        return Vector3Int.Scale(vector1, vector2);
    }

    public static Vector3Int ScaleBy(this Vector3Int vector1, int x, int y, int z)
    {
        return new Vector3Int(vector1.x * x, vector1.y * y, vector1.z * z);
    }

    public static Vector2Int ScaleBy(this Vector2Int vector1, Vector2Int vector2)
    {
        return Vector2Int.Scale(vector1, vector2);
    }

    public static Vector2Int ScaleBy(this Vector2Int vector1, int x, int y)
    {
        return new Vector2Int(vector1.x * x, vector1.y * y);
    }

    #endregion

    #region DividedBy

    public static Vector2 DividedBy(this Vector2 vector1, Vector2 vector2)
    {
        return new Vector2(vector1.x / vector2.x, vector1.y / vector2.y);
    }

    public static Vector2 DividedBy(this Vector2 vector1, float x, float y)
    {
        return new Vector2(vector1.x / x, vector1.y / y);
    }

    public static Vector2 DividedBy(this Vector3 vector1, Vector2 vector2)
    {
        return new Vector2(vector1.x / vector2.x, vector1.y / vector2.y);
    }

    #endregion

    public static Vector2 Floor(this Vector2 vector)
    {
        return new Vector2(Mathf.Floor(vector.x), Mathf.Floor(vector.y));
    }

    public static Vector3 Floor(this Vector3 vector)
    {
        return new Vector3(Mathf.Floor(vector.x), Mathf.Floor(vector.y), Mathf.Floor(vector.z));
    }

    public static Vector2Int FloorToInt(this Vector2 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }

    public static Vector3Int FloorToInt(this Vector3 vector)
    {
        return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z));
    }

    public static Vector2 Ceil(this Vector2 vector)
    {
        return new Vector2(Mathf.Ceil(vector.x), Mathf.Ceil(vector.y));
    }

    public static Vector3 Ceil(this Vector3 vector)
    {
        return new Vector3(Mathf.Ceil(vector.x), Mathf.Ceil(vector.y), Mathf.Ceil(vector.z));
    }

    public static Vector2Int CeilToInt(this Vector2 vector)
    {
        return new Vector2Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y));
    }

    public static Vector3Int CeilToInt(this Vector3 vector)
    {
        return new Vector3Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y), Mathf.CeilToInt(vector.z));
    }

    public static Vector3 Abs(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector2 Abs(this Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }
}
