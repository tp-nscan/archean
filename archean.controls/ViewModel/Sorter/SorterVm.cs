using archean.controls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using archean.core;

namespace archean.controls.ViewModel.Sorter
{
    public enum StageLayout
    {
        Single,
        Loose,
        Tight
    }

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
                                StagedSorterDef.ToStageVms(StageLayout,
                                SortableItemVms));
            CurrentStageVm = StageVms[0];
        }

        StageLayout _stageLayout;
        public StageLayout StageLayout
        {
            get => _stageLayout;
            private set
            {
                _stageLayout = value;
                SetupStageVms();
            }
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
            StageVms = new ObservableCollection<StageVm>(
                    StageVms.Select(stvm=>stvm.ClearSwitchUses())
                    );
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
            if(param == "Tight")
            {
                StageLayout = StageLayout.Tight;
            }
            else if (param == "Loose")
            {
                StageLayout = StageLayout.Loose;
            }
            else
            {
                StageLayout = StageLayout.Single;
            }
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
            SortableItemVms = StageVmProcs.ScrambledSortableVms(
                StagedSorterDef.sorterDef.order, DateTime.Now.Millisecond, true);
            StageVms = new ObservableCollection<StageVm>(
                     StageVms.ResetSortables(SortableItemVms));
            CurrentStageVm = StageVms[0];
        }

        bool CanReset(string param)
        {
            return (CurrentStageVm != null) && (CurrentStageVm.IndexInSorter < StageVms.Count);
        }

        #endregion // ResetCommand


    }

    public static class SorterVmExt
    {
        public static IEnumerable<Func<Sorting.Switch[][], StageVm>> StageVmMaps(
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

        public static IEnumerable<StageVm> ToStageVms(
            this Sorting.StagedSorterDef stagedSorterDef,
            StageLayout stageLayout,
            SortableItemVm[] sortableItemVms)
        {
            var keyCount = stagedSorterDef.sorterDef.order;
            IEnumerable<Sorting.Switch[][]> switchBlockSets = Enumerable.Empty<Sorting.Switch[][]>();

            switch (stageLayout)
            {
                case StageLayout.Single:
                    switchBlockSets = Sorting.StageLayout.LayoutStagedSorterSingle(stagedSorterDef);
                    break;
                case StageLayout.Loose:
                    switchBlockSets = Sorting.StageLayout.LayoutStagedSorterLoose(stagedSorterDef);
                    break;
                case StageLayout.Tight:
                    switchBlockSets = Sorting.StageLayout.LayoutStagedSorterTight(stagedSorterDef);
                    break;
                default:
                    break;
            }

            return switchBlockSets.Zip
                (
                    StageVmMaps(keyCount, sortableItemVms), 
                    (s, m) => m(s)
                 );

        }


        public static IEnumerable<StageVm> ResetSortables(this IEnumerable<StageVm> stageVms, SortableItemVm[] sortableItemVms)
        {
            foreach(var stvm in stageVms)
            {
                if(stvm.IndexInSorter==0)
                {
                    yield return new StageVm(
                        stageVmStep: StageVmStep.Left,
                        indexInSorter: stvm.IndexInSorter,
                        stageVmStyle: stvm.StageVmStyle,
                        keyCount: stvm.KeyCount,
                        keyPairVms: stvm.KeyPairVms,
                        sortableItemVms: sortableItemVms
                    );
                }
                else
                {
                    yield return new StageVm(
                        stageVmStep: StageVmStep.None,
                        indexInSorter: stvm.IndexInSorter,
                        stageVmStyle: stvm.StageVmStyle,
                        keyCount: stvm.KeyCount,
                        keyPairVms: stvm.KeyPairVms,
                        sortableItemVms: null
                    );
                }
            }
        }



    }
}
