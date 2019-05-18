using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterControls.View.Common;

namespace SorterControls.Test
{
    [TestClass]
    public class BrushFactoryFixture
    {
        [TestMethod]
        public void TestLogBrushOfInt()
        {
            var res = BrushFactory.LogBrushOfInt(1, 4096, BrushFactory.GrayThenBlueToRedBrushes());
        }
    }
}
