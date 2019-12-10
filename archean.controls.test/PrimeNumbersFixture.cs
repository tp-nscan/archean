using System;
using System.Linq;
using archean.controls.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace archean.controls.test
{
    [TestClass]
    public class PrimeNumbersFixture
    {
        [TestMethod]
        public void TestPrimesUpTo()
        {
            var limit = 100;
            var res = PrimeNumbers.PrimesUpTo(limit).ToList();
        }


        [TestMethod]
        public void TestCompPrimeBins()
        {
            var limit = 1000;
            var bucketSize = 12;
            var expected = PrimeNumbers.OddPrimesUpTo(limit).ToList();
            var chunks = PrimeNumbers.CompPrimeBins(limit, bucketSize);
            var actual = chunks.SelectMany(c => c.Item1.Concat(c.Item2))
                               .OrderBy(i => i)
                               .ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFillGoldbachBuckets()
        {
            var primeUpperBound = 10240;
            var bucketSize = 300;

            var gb = GoldbachBucketsExt.MakeGoldbachBuckets(
                midPoint: primeUpperBound,
                bucketSize: bucketSize);

            var gb2 = gb.FillGoldbachBuckets();
            var gb3 = gb2.StepDown();
            var gb4 = gb3.FillGoldbachBuckets();
        }


        [TestMethod]
        public void TestGetGoldBackTerms()
        {
            var logLimit = 8;
            var logFraction = 3;

            var res = GoldbachBucketsExt.GetGoldBackTerms(
                logLimit: logLimit,
                logFraction: logFraction);

        }




    }
}
