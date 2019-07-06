using archean.controls.ViewModel.Common;
using archean.controls.ViewModel.Sorter;

namespace archean.controls.DesignVms.Sorter
{
    public class StageVmD : StageVm
    {
        public StageVmD() :
            base(
                    stageVmStep: StageVmStep.Left,
                    stageIndex:0,
                    stageVmStyle: StageVmStyle.Standard(false, ViewModel.AnimationSpeed.None),
                    keyCount: _KeyCount,
                    keyPairVms: StageVmStyle.Standard(false, ViewModel.AnimationSpeed.None).ToRandomKeyPairVms(0, _KeyCount),
                    sortableItemVms: StageVmProcs.ScrambledSortableVms(_KeyCount, System.DateTime.Now.Millisecond, true),
                    sortableItemVmsOld: null
                )
        {
        }

        public static int _KeyCount = 32;

    }

}
