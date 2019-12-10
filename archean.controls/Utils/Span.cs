using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.Utils
{
    public struct Span
    {
        public Span(int min, int max)
        {
            Min = min;
            Max = max;
        }
        public int Min { get; }
        public int Max { get; }
    }

    public static class SpanExt
    {
        public static int Length(this Span span)
        {
            return span.Max - span.Min;
        }

        public static Span EmptySpan
        {
            get
            {
                return new Span(min: 0, max: 0);
            }
        }
        public static Tuple<Span, Tuple<Span, Span>> CompTrim(this Span span, int maxTrim)
        {
            Span compLow = EmptySpan;
            Span compHi = EmptySpan;
            Span remainingSpan = EmptySpan;

            if (span.Length() / 2 < maxTrim)
            {
                compLow = new Span(span.Min, span.Min + span.Length() / 2);
                compHi = new Span(compLow.Max, span.Max);
                remainingSpan = new Span(compLow.Max, compHi.Min);
                return new Tuple<Span, Tuple<Span, Span>>(remainingSpan, new Tuple<Span, Span>(compLow, compHi));
            }

            compLow = new Span(span.Min, span.Min + maxTrim);
            compHi = new Span(span.Max - maxTrim, span.Max);
            remainingSpan = new Span(compLow.Max, compHi.Min);
            return new Tuple<Span, Tuple<Span, Span>>(remainingSpan, new Tuple<Span, Span>(compLow, compHi));
        }

        public static IEnumerable<Tuple<Span, Span>> CompTrimSet(this Span span, int maxTrim)
        {
            var curSpan = span;
            while (curSpan.Length() > 0)
            {
                var nextChunk = curSpan.CompTrim(maxTrim);
                yield return nextChunk.Item2;
                curSpan = nextChunk.Item1;
            }
        }
    }
}
