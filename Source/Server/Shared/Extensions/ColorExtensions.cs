using System.Drawing;

namespace Shared.Extensions;

public static class ColorExtensions
{
    public static string ToRgb(this Color c)
    {
        return $"rgb({c.R}, {c.G}, {c.B})";
    }
}
