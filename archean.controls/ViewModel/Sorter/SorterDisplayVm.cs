using archean.controls.Utils;
using archean.controls.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using archean.core;

namespace archean.controls.ViewModel.Sorter
{
    public class SorterDisplayVm : BindableBase
    {
        public SorterDisplayVm(
                        int order,
                        SortableItemVm[] sortableItemVms,
                        IEnumerable<StageVm> stageVms,
                        int currentstageIndex)
        {
            Order = order;
            SortableItemVms = sortableItemVms;
            StageVms = new ObservableCollection<StageVm>(stageVms);
            CurrentStageVm = StageVms[currentstageIndex];
        }

        public List<Sorting.ISwitch[][]> SwitchBlockSets { get; private set;}

        public int Order { get; set; }

        public Sorting.StagedSorterDef StagedSorterDef { get; }

        public SortableItemVm[] SortableItemVms { get; set; }

        ObservableCollection<StageVm> _stageVms;
        public ObservableCollection<StageVm> StageVms
        {
            get => _stageVms;
            set
            {
                SetProperty(ref _stageVms, value);
            }
        }

        StageVm _currentStageVm;
        public StageVm CurrentStageVm
        {
            get
            {
                return _currentStageVm;
            }
            set
            {
                _currentStageVm = value;
                if (subscription != null)
                {
                    subscription.Dispose();
                }
                subscription = _currentStageVm.OnAnimationFinished.Subscribe(AnimationFinished);
                StageVms[value.StageIndex] = value;
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

        private bool keepGoing;
        public bool KeepGoing
        {
            get => keepGoing;
            set
            {
                keepGoing = value;
            }
        }


        public void MakeStep()
        {
            if (CurrentStageVm.StageVmStep == StageVmStep.Right)
            {
                var sortableItemVms = CurrentStageVm.SortableItemVms;
                if (CurrentStageVm.StageIndex + 1 > StageVms.Count - 1) return;

                //since this stage is finished, change it to a sortable free config
                CurrentStageVm = CurrentStageVm.ToNextStep();

                // put the sortables in the  left position on the next stage
                CurrentStageVm = StageVms[CurrentStageVm.StageIndex + 1]
                                    .ToNextStep(sortableItemVms.ToLeftStep());
                if (KeepGoing)
                {
                    MakeStep();
                }
            }
            else
            {
                CurrentStageVm = CurrentStageVm.ToNextStep();
            }
        }
    }

    public static class SorterDisplayVmExt
    {
        public static IEnumerable<Func<Sorting.ISwitch[][], StageVm>> StageVmMaps(
                                            int keyCount, 
                                            SortableItemVm[] sortableVms,
                                            AnimationSpeed animationSpeed)
        {
            var indexInSorter = 0;
            yield return
                switchblocks => switchblocks.SwitchBlocksToStageVm(
                    stageIndex: indexInSorter++, 
                    stageVmStyle: StageVmStyle.Standard(false, animationSpeed),
                    keyCount: keyCount, 
                    sortableVms: sortableVms,
                    stageVmStep: StageVmStep.Left);

            while (true)
                {
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            stageIndex: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(true, animationSpeed),
                            keyCount: keyCount,
                            sortableVms: null,
                            stageVmStep: StageVmStep.None);
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            stageIndex: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(false, animationSpeed),
                            keyCount: keyCount,
                            sortableVms: null,
                            stageVmStep: StageVmStep.None);
            };
        }

        public static IEnumerable<StageVm> ToStageVms(
            this IEnumerable<Sorting.ISwitch[][]> switchBlockSets,
            int keyCount,
            SortableItemVm[] sortableItemVms,
            AnimationSpeed animationSpeed)
        {
            return switchBlockSets.Zip
                (
                    StageVmMaps(keyCount, sortableItemVms, animationSpeed),
                    (s, m) => m(s)
                );
        }


        public static IEnumerable<Sorting.ISwitch[][]> ToSwitchBlockSets(
            this Sorting.StagedSorterDef stagedSorterDef,
            StageLayout stageLayout)
        {
            IEnumerable<Sorting.ISwitch[][]> switchBlockSets = Enumerable.Empty<Sorting.Switch[][]>();

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
                    throw new Exception($"{stageLayout} not handled");
            }

            return switchBlockSets;
        }


        public static IEnumerable<StageVm> ResetSortables(
            this IEnumerable<StageVm> stageVms, SortableItemVm[] sortableItemVms)
        {
            foreach(var stvm in stageVms)
            {
                if(stvm.StageIndex==0)
                {
                    yield return new StageVm(
                        stageVmStep: StageVmStep.Left,
                        stageIndex: stvm.StageIndex,
                        stageVmStyle: stvm.StageVmStyle,
                        keyCount: stvm.KeyCount,
                        keyPairVms: stvm.KeyPairVms,
                        sortableItemVms: sortableItemVms,
                        sortableItemVmsOld: null
                    );
                }
                else
                {
                    yield return new StageVm(
                        stageVmStep: StageVmStep.None,
                        stageIndex: stvm.StageIndex,
                        stageVmStyle: stvm.StageVmStyle,
                        keyCount: stvm.KeyCount,
                        keyPairVms: stvm.KeyPairVms,
                        sortableItemVms: null,
                        sortableItemVmsOld: null
                    );
                }
            }
        }


        public static SorterDisplayVm ChangeAnimationSpeed(this SorterDisplayVm sorterDisplayVm, AnimationSpeed animationSpeed)
        {
            return new SorterDisplayVm(
                order: sorterDisplayVm.Order,
                sortableItemVms: sorterDisplayVm.SortableItemVms,
                stageVms: sorterDisplayVm.StageVms.Select(
                    stvm => stvm.SetStageVmStyle(stvm.StageVmStyle.ChangeAnimationSpeed(animationSpeed))),
                currentstageIndex: sorterDisplayVm.CurrentStageVm.StageIndex

                );
        }
    }
}
