using archean.controls.Utils;
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
            CurrentStageVm = StageVms[0];
        }

        public ObservableCollection<StageVm> StageVms { get; set; }

        //public int ActiveStageIndex { get; private set; }

        //StageVm CurrentStageVm
        //{
        //    get
        //    {
        //        return StageVms[ActiveStageIndex];
        //    }
        //    set
        //    {
        //        StageVms[ActiveStageIndex] = value;
        //    }
        //}

        StageVm _currentStageVm;
        StageVm CurrentStageVm
        {
            get
            {
                return _currentStageVm;
            }
            set
            {
                _currentStageVm = value;
                StageVms[value.IndexInSorter] = value;
            }
        }

        #region StepCommand

        RelayCommand _stepCommand;

        public ICommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
                DoStep,
                CanStep
            ));

        private void DoStep()
        {

            if(CurrentStageVm.StageVmStep == StageVmStep.Right)
            {
                var sortableVms = CurrentStageVm.SortableVms;
                CurrentStageVm = CurrentStageVm.ToNextStep();
                if (CurrentStageVm.IndexInSorter > StageVms.Count - 1) return;
                CurrentStageVm = StageVms[CurrentStageVm.IndexInSorter + 1]
                                    .ToNextStep(sortableVms.ToLeftStep());
            }
            else
            {
                CurrentStageVm = CurrentStageVm.ToNextStep();
            }
        }

        bool CanStep()
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
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
                                            int keyCount, SortableItemVm[] sortableVms)
        {
            var indexInSorter = 0;
            yield return
                switchblocks => switchblocks.SwitchBlocksToStageVm(
                    indexInSorter: indexInSorter++, 
                    stageVmStyle: StageVmStyle.Standard(false),
                    keyCount: keyCount, 
                    sortableVms: sortableVms, 
                    stageVmStep: StageVmStep.Left);

            while (true)
                {
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            indexInSorter: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(true),
                            keyCount: keyCount,
                            sortableVms: new SortableItemVm[0],
                            stageVmStep: StageVmStep.None);
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            indexInSorter: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(false),
                            keyCount: keyCount,
                            sortableVms: new SortableItemVm[0],
                            stageVmStep: StageVmStep.None);
            };
        }

        public static IEnumerable<StageVm> ToStageVms(this core.Sorting.StagedSorterDef stagedSorterDef, bool showLabels)
        {
            var keyCount = stagedSorterDef.sorterDef.order;
            var stm = core.Sorting.StageLayout.LayoutStagedSorter(stagedSorterDef);

            return stm.Zip(StageVmMaps(keyCount, StageVmProcs.ScrambledSortableVms(keyCount, showLabels)), 
                                    (s, m) => m(s));

        }
    }
}
