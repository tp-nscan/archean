using archean.controls.ViewModel.Sorter;

namespace archean.controls.DesignVms.Sorter
{
    public class StageVmD : StageVm
    {
        public StageVmD() :
            base(
                    stageVmStep: StageVmStep.Left,
                    indexInSorter:0,
                    stageVmStyle: StageVmStyle.Standard(false),
                    keyCount: _KeyCount,
                    keyPairVms: StageVmStyle.Standard(false).ToRandomKeyPairVms(_KeyCount),
                    sortableItemVms: StageVmProcs.ScrambledSortableVms(_KeyCount, true)
                )
        {
        }

        public static int _KeyCount = 32;

    }

}
