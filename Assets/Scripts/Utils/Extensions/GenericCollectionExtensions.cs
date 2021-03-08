using System.Collections.Generic;
using UnityEngine;


public static class GenericCollectionExtensions
{
    public static T RandomObject<T>(this IList<T> list) => list[Random.Range(0, list.Count)];
}

