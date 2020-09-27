using UnityEngine;


public static class ArrayExtension
{
    public static T RandomObject<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
