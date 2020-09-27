using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 SetX(this Vector3 vector, float x)
    {
        Vector3 newVector = vector;
        newVector.x = x;
        return newVector;
    }

    public static Vector3 SetY(this Vector3 vector, float y)
    {
        Vector3 newVector = vector;
        newVector.y = y;
        return newVector;
    }

    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        Vector3 newVector = vector;
        newVector.z = z;
        return newVector;
    }

    public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
}
