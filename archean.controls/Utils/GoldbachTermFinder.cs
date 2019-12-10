using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.Utils
{
    public class GoldbachTermFinder
    {
        public GoldbachTermFinder(GoldbachBuckets goldbachBuckets,
                                  Dictionary<int, List<int>> results)
        {
            Results = results;
            CurrentGoldbachBuckets = goldbachBuckets;
        }
        public int Span => CurrentGoldbachBuckets.BucketSize * 2;
        public GoldbachBuckets CurrentGoldbachBuckets { get; }
        public Dictionary<int, List<int>> Results { get; }
    }

    public static class GoldbachTermFinderExt
    {
        public static GoldbachTermFinder InitGoldbachTermFinder(int logLimit, int logSpan)
        {
            var gbbs = GoldbachBucketsExt.MakeGoldbachBuckets(
                midPoint: 1 << logLimit,
                bucketSize: 1 << (logSpan + 1));

            return new GoldbachTermFinder(
                goldbachBuckets: gbbs,
                results: new Dictionary<int, List<int>>());
        }

        public static GoldbachTermFinder Update(
            this GoldbachTermFinder goldbachTermFinder,
            bool mergeResults)
        {
            if(mergeResults)
            {
                var curResults = goldbachTermFinder.Results;
                goldbachTermFinder.CurrentGoldbachBuckets
                                        .HiBuckets.ToList()
                                        .ForEach(x => curResults.Add(x.Key, x.Value));
            }


            return new GoldbachTermFinder(
                goldbachBuckets: goldbachTermFinder.CurrentGoldbachBuckets.StepDown(),
                results: goldbachTermFinder.Results);
        }

    }
}
