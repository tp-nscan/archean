using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.ViewModel.Sorter2
{
    public static class RenderUtils
    {
        public static double ChildRenderHeight(double parentVmHeight, double childVmHeight, double parentRenderHeight)
        {
            return (parentRenderHeight * childVmHeight) / parentVmHeight;
        }
    }
}
