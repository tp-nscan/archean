using SorterControls.View.SorterOld;
using SorterControls.ViewModel.SorterOld;
using Sorting.TestData;

namespace SorterControls.DesignVms.SorterOld
{
    public class DesignSorterVm : SorterVmImpl
    {
        public DesignSorterVm()
            : base
            (
                sorter: SorterEvals.TestSorterEval(__keyCount, 123, 800),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(__keyCount),
                width: 8
            )
        { }

        private const int __keyCount = 16;
    }
}
