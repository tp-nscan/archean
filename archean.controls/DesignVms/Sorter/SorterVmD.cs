using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace archean.controls.DesignVms.Sorter
{
    public class SorterVmD : SorterDisplayVm
    {
        public SorterVmD() : base(
            order: StagedSorterDefD.sorterDef.order,
            sortableItemVms: StageVmProcs.ScrambledSortableVms(StagedSorterDefD.sorterDef.order, System.DateTime.Now.Millisecond, true),
            stageVms: StageVmsD,
            currentstageIndex: 0
            )
            {
            }

        static core.Sorting.StagedSorterDef _stagedSorterDefD;
        public static core.Sorting.StagedSorterDef StagedSorterDefD
        {
            get
            {
                return _stagedSorterDefD ?? (
                    _stagedSorterDefD = core.SortersFromData.RefSorterModule.CreateRefStagedSorter(
                            core.SortersFromData.RefSorter.Order8));
            }
        }

        public static IEnumerable<StageVm> StageVmsD
        {
            get
            {
                var order = StagedSorterDefD.sorterDef.order;
                var switchBlockSets = StagedSorterDefD.ToSwitchBlockSets(StageLayout.Loose).ToList();
                var sortableItemVms = StageVmProcs.ScrambledSortableVms(order,
                                            System.DateTime.Now.Millisecond, true);

                SwitchUseWrap max = new SwitchUseWrap();

                return switchBlockSets.ToStageVms(
                    stageVmStyle: StageVmStyle.Standard(Brushes.AliceBlue, AnimationSpeed.Stopped, max),
                    alternatingBrush: new SolidColorBrush(Color.FromScRgb(0.3f, 0, 0, 0)),
                    order: order,
                    sortableItemVms: sortableItemVms);
            }
        }
    }

}
