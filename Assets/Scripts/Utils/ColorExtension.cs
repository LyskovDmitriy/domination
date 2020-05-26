using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtension
{
    public static Color SetR(this Color color, float r)
    {
        Color newColor = color;
        newColor.r = r;
        return newColor;
    }


    public static Color SetG(this Color color, float g)
    {
        Color newColor = color;
        newColor.g = g;
        return newColor;
    }


    public static Color SetB(this Color color, float b)
    {
        Color newColor = color;
        newColor.b = b;
        return newColor;
    }


    public static Color SetA(this Color color, float a)
    {
        Color newColor = color;
        newColor.a = a;
        return newColor;
    }
}
