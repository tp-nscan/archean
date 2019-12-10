using System;
using System.Collections.Generic;
using System.Linq;

namespace archean.controls.Utils
{
    public static class PrimeNumbers
    {

        public static IEnumerable<int> PrimesUpTo(int limit)
        {
            var primeFlags = Enumerable.Repeat(true, limit).ToList();
            primeFlags[0] = false;
            primeFlags[1] = false;

            for (var i=0; i*i<limit; i++)
            {
                if(primeFlags[i])
                {
                    for(int j = 2*i; j < limit; j+=i)
                    {
                        primeFlags[j] = false;
                    }
                }
            }

            return Enumerable.Range(2, limit-2).Where(i => primeFlags[i]);
        }

        public static IEnumerable<int> OddPrimesUpTo(int limit)
        {
            var primeFlags = Enumerable.Repeat(true, limit).ToList();
            primeFlags[0] = false;
            primeFlags[1] = false;

            for (var i = 0; i * i < limit; i++)
            {
                if (primeFlags[i])
                {
                    for (int j = 2 * i; j < limit; j += i)
                    {
                        primeFlags[j] = false;
                    }
                }
            }

            return Enumerable.Range(3, limit - 3).Where(i => primeFlags[i]);
        }

        public static IEnumerable<Tuple<List<int>, List<int>>> CompPrimeBins(int limit, int bucketSize)
        {
            var primeList = PrimesUpTo(limit).ToList();
            var wholeSpan = new Span(3, limit);
            foreach(var tup in wholeSpan.CompTrimSet(bucketSize))
            {
                yield return new Tuple<List<int>, List<int>>(
                    item1: primeList.GetValueRange(tup.Item1.Min, tup.Item1.Max).ToList(),
                    item2: primeList.GetValueRange(tup.Item2.Min, tup.Item2.Max).ToList());
            }
        }
    }
}
