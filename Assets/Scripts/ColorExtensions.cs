using UnityEngine;

public static class ColorExtensions
{
    public static Color SetAlpha(this Color color, float a)
    {
        return new(color.r, color.g, color.b, a);
    }
}