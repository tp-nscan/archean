using archean.controls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using archean.core;

namespace archean.controls.ViewModel.Sorter
{
    public class SorterVm : BindableBase
    {
        public SorterVm(Sorting.StagedSorterDef stagedSorterDef, SortableItemVm[] sortableItemVms)
        {
            StagedSorterDef = stagedSorterDef;
            SortableItemVms = sortableItemVms;
            SetupStageVms();
        }

        void SetupStageVms()
        {
            StageVms = new ObservableCollection<StageVm>(
                                StagedSorterDef.ToStageVms(
                                SortableItemVms));
            CurrentStageVm = StageVms[0];
        }

        public Sorting.StagedSorterDef StagedSorterDef { get; }

        public SortableItemVm[] SortableItemVms { get; private set; }

        ObservableCollection<StageVm> _stageVms;
        public ObservableCollection<StageVm> StageVms
        {
            get => _stageVms;
            set => SetProperty(ref _stageVms, value);
        }

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
                if(subscription != null)
                {
                    subscription.Dispose();
                }
                subscription = _currentStageVm.OnAnimationFinished.Subscribe(AnimationFinished);
                StageVms[value.IndexInSorter] = value;
            }
        }

        IDisposable subscription;

        void AnimationFinished(StageVm stageVm)
        {
            if (KeepGoing)
            {
                MakeStep();
            }
        }

        bool KeepGoing { get; set; }

        #region StepCommand

        RelayCommand _stepCommand;

        public ICommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
                DoStep,
                CanStep
            ));

        private void DoStep()
        {
            KeepGoing = false;
            MakeStep();
            if (CurrentStageVm.StageVmStep == StageVmStep.Right)
            {
                MakeStep();
            }
        }

        bool CanStep()
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
        }

        #endregion // StepCommand


        #region RunCommand

        RelayCommand _runCommand;

        public ICommand RunCommand => _runCommand ?? (_runCommand = new RelayCommand(
                DoRun,
                CanRun
            ));

        private void DoRun()
        {
            KeepGoing = true;
            MakeStep();
        }

        bool CanRun()
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
        }

        #endregion // RunCommand


        void MakeStep()
        {
            if (CurrentStageVm.StageVmStep == StageVmStep.Right)
            {
                var sortableVms = CurrentStageVm.SortableItemVms;
                if (CurrentStageVm.IndexInSorter + 1 > StageVms.Count - 1) return;

                CurrentStageVm = CurrentStageVm.ToNextStep();

                CurrentStageVm = StageVms[CurrentStageVm.IndexInSorter + 1]
                                    .ToNextStep(sortableVms.ToLeftStep());
                if(KeepGoing)
                {
                    MakeStep();
                }
            }
            else
            {
                CurrentStageVm = CurrentStageVm.ToNextStep();
            }
        }


        #region ClearCommand

        RelayCommand _clearCommand;

        public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(
                DoClear,
                CanClear
            ));

        private void DoClear()
        {

        }

        bool CanClear()
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
        }

        #endregion // ClearCommand


        #region PackCommand

        RelayCommand<string> _packCommand;

        public ICommand PackCommand => _packCommand ?? (_packCommand = new RelayCommand<string>(
                DoPack,
                CanPack
            ));

        private void DoPack(string param)
        {

        }

        bool CanPack(string param)
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
        }

        #endregion // PackCommand


        #region ResetCommand

        RelayCommand<string> _resetCommand;

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new RelayCommand<string>(
                DoReset,
                CanReset
            ));

        private void DoReset(string param)
        {
            SortableItemVms = StageVmProcs.ScrambledSortableVms(StagedSorterDef.sorterDef.order, DateTime.Now.Millisecond, true);
            SetupStageVms();
        }

        bool CanReset(string param)
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
        }

        #endregion // ResetCommand


    }

    public static class SorterVmExt
    {
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
                            sortableVms: null,
                            stageVmStep: StageVmStep.None);
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            indexInSorter: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(false),
                            keyCount: keyCount,
                            sortableVms: null,
                            stageVmStep: StageVmStep.None);
            };
        }

        public static IEnumerable<StageVm> ToStageVms(this core.Sorting.StagedSorterDef stagedSorterDef, SortableItemVm[] sortableItemVms)
        {
            var keyCount = stagedSorterDef.sorterDef.order;
            var switchBlockSets = core.Sorting.StageLayout.LayoutStagedSorterTight(stagedSorterDef);
            //StageVmProcs.ScrambledSortableVms(keyCount, showLabels)
            return switchBlockSets.Zip
                (
                    StageVmMaps(keyCount, sortableItemVms), 
                    (s, m) => m(s)
                 );

        }
    }
}
