using archean.controls.ViewModel.Common;
using archean.controls.ViewModel.Sorter;

namespace archean.controls.DesignVms.Sorter
{
    public class StageVmD : StageVm
    {
        public StageVmD() :
            base(
                    stageVmStep: StageVmStep.Left,
                    indexInSorter:0,
                    stageVmStyle: StageVmStyle.Standard(false, ViewModel.AnimationSpeed.None),
                    keyCount: _KeyCount,
                    keyPairVms: StageVmStyle.Standard(false, ViewModel.AnimationSpeed.None).ToRandomKeyPairVms(_KeyCount),
                    sortableItemVms: StageVmProcs.ScrambledSortableVms(_KeyCount, System.DateTime.Now.Millisecond, true)
                )
        {
        }

        public static int _KeyCount = 32;

    }

}
