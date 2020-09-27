using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 SetX(this Vector2 vector, float x)
    {
        Vector2 newVector = vector;
        newVector.x = x;
        return newVector;
    }

    public static Vector2 SetY(this Vector2 vector, float y)
    {
        Vector2 newVector = vector;
        newVector.y = y;
        return newVector;
    }

    public static Vector3 ToVector3(this Vector2 vector) => new Vector3(vector.x, vector.y);
}
