using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.Utils
{
    public static class MathLib
    {

        public static int[] RandomPermutation(int order, int seed)
        {
            return
                    core.Combinatorics.FisherYatesShuffle(
                    new Random(seed),
                    Enumerable.Range(0, order).ToArray()
                ).ToArray();
        }

        public static int[] Random_0_1_Array(int order, int seed)
        {
            return
                    core.Combinatorics.Random_0_1(
                    rnd: new Random(seed),
                    len: order,
                    pctOnes: 0.5
                ).ToArray();
        }

    }
}
