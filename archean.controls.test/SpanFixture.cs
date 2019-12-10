using System;
using System.Linq;
using archean.controls.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace archean.controls.test
{
    [TestClass]
    public class SpanFixture
    {
        [TestMethod]
        public void TestCompTrim()
        {
            var min = 3;
            var max = 17;
            var sp = new Span(min: min, max: max);
            var trimmed = sp.CompTrim(2);
            Assert.AreEqual(trimmed.Item1, new Span(5, 15));
        }

        [TestMethod]
        public void TestCompTrimSet()
        {
            var min = 3;
            var max = 17;
            var sp = new Span(min: min, max: max);
            var trimmedSet = sp.CompTrimSet(2).ToList();
        }
    }
}
