using archean.controls.ViewModel.Sorter;
using System.Windows.Media;

namespace archean.controls.DesignVms.Sorter
{
    public class StageVmD : StageVm
    {
        public StageVmD() :
            base(
                    stageVmStep: StageVmStep.Left,
                    stageIndex:0,
                    stageVmStyle: StageVmStyle.Standard(Brushes.AliceBlue, ViewModel.TicsPerStep.Stopped, max),
                    order: _KeyCount,
                    keyPairVms: StageVmStyle.Standard(Brushes.AliceBlue, ViewModel.TicsPerStep.Stopped, max).ToRandomKeyPairVms(0, _KeyCount),
                    sortableItemVms: StageVmProcs.ScrambledSortableVms(_KeyCount, System.DateTime.Now.Millisecond, true),
                    sortableItemVmsOld: null
                )
        {
        }

        public static int _KeyCount = 32;
        public static object maxUses = 0;
        public static SwitchUseWrap max = new SwitchUseWrap();
    }

}
