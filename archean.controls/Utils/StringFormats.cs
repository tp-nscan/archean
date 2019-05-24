using System;
using System.Collections.Generic;
using System.Linq;

namespace archean.controls.Utils
{
    public static class StringFormats
    {
        public static string FormatA(this TimeSpan timespan)
        {
            return $"{timespan.Hours.ToString("0")}:" +
                   $"{timespan.Minutes.ToString("00")}:" +
                   $"{timespan.Seconds.ToString("00")}:" +
                   $"{timespan.Milliseconds.ToString("000")}";
        }

        public static float FractionOf(this int numerator, int denominator)
        {
            return ((float)numerator) / denominator;
        }

        public static IEnumerable<double> ToUnitInterp(this int stepCount)
        {
            return Enumerable.Range(0, stepCount).Select(i => ((double)i) / stepCount);
        }

        public static string ToLegendFormatCode(this float val)
        {
            if (Math.Abs(val) > 1000) return val.ToString("0");
            if (Math.Abs(val) > 100) return val.ToString("0.0");
            if (Math.Abs(val) > 10) return val.ToString("0.00");
            if (Math.Abs(val) > 1) return val.ToString("0.000");
            if (Math.Abs(val) > 0.1) return val.ToString("0.0000");
            return val.ToString("0.0000");
        }

    }
}
