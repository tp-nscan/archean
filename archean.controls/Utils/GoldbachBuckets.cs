using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.Utils
{
    public class GoldbachBuckets
    {
        public GoldbachBuckets(int midPoint, int bucketSize, Dictionary<int, List<int>> hiBuckets)
        {
            MidPoint = midPoint;
            BucketSize = bucketSize;

            LowBuckets = Enumerable.Range(1, BucketSize)
                                   .ToDictionary(k => MidPoint - 2 * k, k => new List<int>());
            LowLimit = MidPoint - 2 * BucketSize;
            HiLimit = MidPoint + 2 * BucketSize;
            HiBuckets = hiBuckets;
        }

        public Dictionary<int, List<int>> LowBuckets { get; }
        public Dictionary<int, List<int>> HiBuckets { get; }
        public int MidPoint { get; }
        public int BucketSize { get; }
        public int LowLimit { get; }
        public int HiLimit { get; }

    }

    public static class GoldbachBucketsExt
    {
        public static GoldbachBuckets MakeGoldbachBuckets(int midPoint, int bucketSize)
        {
            return new GoldbachBuckets(midPoint: midPoint, 
                                       bucketSize: bucketSize,
                                       hiBuckets: Enumerable.Range(0, bucketSize)
                                                            .ToDictionary(k => midPoint + 2 * k, k => new List<int>()));
        }

        public static GoldbachBuckets StepDown(this GoldbachBuckets origBucket)
        {
            return new GoldbachBuckets(midPoint: origBucket.LowLimit,
                                       bucketSize: origBucket.BucketSize,
                                       hiBuckets: origBucket.LowBuckets);
        }

        public static void AddTerm(this GoldbachBuckets goldbachBuckets, int evenNum, int smallerPrime)
        {
            if (evenNum < goldbachBuckets.LowLimit)
            {
                throw new Exception($"evenNum: {evenNum} is less than LowLimit: {goldbachBuckets.LowLimit}");
            }
            if (evenNum < goldbachBuckets.MidPoint)
            {
                goldbachBuckets.LowBuckets[evenNum].Add(smallerPrime);
                return;
            }
            if (evenNum < goldbachBuckets.HiLimit)
            {
                goldbachBuckets.HiBuckets[evenNum].Add(smallerPrime);
                return;
            }
            throw new Exception($"evenNum: {evenNum} is >= HiLimit:{goldbachBuckets.HiLimit}");
        }

        public static GoldbachBuckets FillGoldbachBuckets(this GoldbachBuckets goldbachBuckets)
        {
            var primeLimit = goldbachBuckets.MidPoint;
            var chunks = PrimeNumbers.CompPrimeBins(primeLimit, goldbachBuckets.BucketSize * 2)
                                     .ToList();

            foreach(var compL in chunks)
            {
                foreach (var lowerItem in compL.Item1)
                {
                    foreach (var upperItem in compL.Item2)
                    {
                        goldbachBuckets.AddTerm(lowerItem + upperItem, lowerItem);
                    }
                }
            }
            return goldbachBuckets;
        }

        public static Dictionary<int, List<int>> GetGoldBackTerms(int logLimit, int logFraction)
        {


            return null;
        }
    }
}
