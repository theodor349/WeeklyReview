using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyReview.Shared.Extensions
{
    public static class ColorExtensions
    {
        public static string ToRgb(this Color c)
        {
            return $"rgb({c.R}, {c.G}, {c.B})";
        }
    }
}
