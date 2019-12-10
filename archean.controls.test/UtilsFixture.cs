using System;
using System.Linq;
using archean.controls.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace archean.controls.test
{
    [TestClass]
    public class UtilsFixture
    {

        [TestMethod]
        public void TestGetValueRange()
        {
            var closedLowerBound = 3;
            var openUpperBound = 8;

            var res = Enumerable.Range(0, 10).GetValueRange(closedLowerBound: closedLowerBound, 
                openUpperBound: openUpperBound).ToList();

            CollectionAssert.AreEqual(
                Enumerable.Range(closedLowerBound, openUpperBound- closedLowerBound).ToList(),
                res);

        }

    }
}
