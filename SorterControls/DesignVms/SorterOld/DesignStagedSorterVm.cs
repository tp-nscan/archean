using SorterControls.View.SorterOld;
using SorterControls.ViewModel.SorterOld;
using Sorting.TestData;

namespace SorterControls.DesignVms.SorterOld
{
    public class DesignStagedSorterVm : StagedSorterVmImpl
    {
        public DesignStagedSorterVm() 
            : base
            (
                sorterEval: SorterEvals.TestSorterEval(__keyCount, 123, 800),
                lineBrushes: LineBrushFactory.GradedBlueBrushes(__keyCount),
                switchBrushes: LineBrushFactory.GradedRedBrushes(__keyCount),
                width: 8,
                height: 150,
                showUnusedSwitches:true
            )
        {
        }

        private const int __keyCount = 16;
    }
}
