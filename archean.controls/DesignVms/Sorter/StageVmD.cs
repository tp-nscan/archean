using archean.controls.ViewModel.Sorter;
using System.Linq;

namespace archean.controls.DesignVms.Sorter
{
    public class StageVmD : StageVm
    {
        public StageVmD() :
            base(
                    stageVmStep: StageVmStep.Left,
                    stageVmStyle: StageVmStyle.StandardE,
                    keyCount: _KeyCount,
                    keyPairVms: StageVmStyle.StandardE.ToRandomKeyPairVms(_KeyCount),
                    sortableVms: StageVmProcs.ScrambledSortableVms(_KeyCount)
                )
        {
        }

        public static int _KeyCount = 32;

    }

    public class StageVmD2 : StageVm
    {
        public StageVmD2() :
            base(
                    stageVmStep: StageVmStep.Left,
                    stageVmStyle: StageVmStyle.Standard0,
                    keyCount: _KeyCount,
                    keyPairVms: StageVmStyle.Standard0.ToRandomKeyPairVms(_KeyCount),
                    sortableVms: new SortableVm[0]
                )
        {
        }

        public static int _KeyCount = 32;

    }
}
