using System;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterControls.View.Common;
using Sorting.KeyPairs;
using Sorting.Stages;

namespace SorterControls.Test.Common
{
    [TestClass]
    public class StageLayoutFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int keyCount = 16;

            var sorterStage = Rando.Fast(123).ToPermutations(keyCount)
                                .First()
                                .Values
                                .ToTuples()
                                .ToKeyPairs(false)
                                .ToSorterStage(keyCount);

            var layouts = sorterStage.ToStageLayouts().ToList();
        }
    }
}
