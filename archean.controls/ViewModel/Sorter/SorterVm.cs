using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace archean.controls.ViewModel.Sorter
{
    public class SorterVm
    {
        public SorterVm(IEnumerable<StageVm> stageVms)
        {
            StageVms = new ObservableCollection<StageVm>(stageVms);
        }

        public ObservableCollection<StageVm> StageVms { get; set; }

        public int ActiveStageIndex { get; }

        #region StepCommand

        //RelayCommand _stepCommand;

        //public ICommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
        //    DoStep,
        //    CanStep
        //    ));

        private void DoStep()
        {
          //  StageVm = StageVm.ToNextStep();
        }

        bool CanStep()
        {
            return true;
        }

        #endregion // StepCommand
    }

    public static class SorterVmExt
    {
        public static SorterVm MakeEmpty()
        {
            return new SorterVm(Enumerable.Empty<StageVm>());
        }

        public static SorterVm MakeEmpty2(core.Sorting.StagedSorterDef stagedSorter)
        {
            return new SorterVm(Enumerable.Empty<StageVm>());
        }


        public static IEnumerable<Func<core.Sorting.Switch[][], StageVm>> StageVmMaps(
                                            int order, SortableVm[] sortableVms)
        {

            yield return
                switchblocks => switchblocks.SwitchBlocksToStageVm(StageVmStyle.StandardE, order, sortableVms);

            while (true)
                {
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(StageVmStyle.Standard0, order, new SortableVm[0]);
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(StageVmStyle.StandardE, order, new SortableVm[0]);
            };
        }

        public static IEnumerable<StageVm> ToStageVms(this core.Sorting.StagedSorterDef stagedSorterDef)
        {
            var keyCount = stagedSorterDef.sorterDef.order;
            var stm = core.Sorting.StageLayout.LayoutStagedSorter(stagedSorterDef);

            return stm.Zip(StageVmMaps(keyCount, StageVmProcs.ScrambledSortableVms(keyCount)), 
                                    (s, m) => m(s));

        }
    }
}
