using archean.controls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using archean.core;

namespace archean.controls.ViewModel.Sorter
{
    public class SorterDisplayVm : BindableBase
    {
        public SorterDisplayVm(Sorting.StagedSorterDef stagedSorterDef,
                        SortableItemVm[] sortableItemVms,
                        AnimationSpeed animationSpeed,
                        StageLayout stageLayout)
        {
            StagedSorterDef = stagedSorterDef;
            SortableItemVms = sortableItemVms;
            _animationSpeed = animationSpeed;
            _stageLayout = stageLayout;
            SetupStageVms();
        }

        void SetupStageVms()
        {
            StageVms = new ObservableCollection<StageVm>(
                                StagedSorterDef.ToStageVms(StageLayout,
                                SortableItemVms, AnimationSpeed));
        }

        private AnimationSpeed _animationSpeed;
        public AnimationSpeed AnimationSpeed
        {
            get => _animationSpeed;
            set  {

                _animationSpeed = value;
                StageVms = new ObservableCollection<StageVm>(
                    StageVms.Select(stvm => stvm.SetStageVmStyle(
                        stvm.StageVmStyle.ChangeAnimationSpeed(value))));
            }
        }

        StageLayout _stageLayout;
        public StageLayout StageLayout
        {
            get => _stageLayout;
            set
            {
                _stageLayout = value;
                SetupStageVms();
            }
        }

        public Sorting.StagedSorterDef StagedSorterDef { get; }

        public SortableItemVm[] SortableItemVms { get; set; }

        ObservableCollection<StageVm> _stageVms;
        public ObservableCollection<StageVm> StageVms
        {
            get => _stageVms;
            set
            {
                SetProperty(ref _stageVms, value);
                CurrentStageVm = value[0];
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
                if (CurrentStageVm.IndexInSorter + 1 > StageVms.Count - 1) return;

                //since this stage is finished, change it to a sortable free config
                CurrentStageVm = CurrentStageVm.ToNextStep();

                // put the sortables in the  left position on the next stage
                CurrentStageVm = StageVms[CurrentStageVm.IndexInSorter + 1]
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

    public static class SorterVmExt
    {
        public static IEnumerable<Func<Sorting.Switch[][], StageVm>> StageVmMaps(
                                            int keyCount, 
                                            SortableItemVm[] sortableVms,
                                            AnimationSpeed animationSpeed)
        {
            var indexInSorter = 0;
            yield return
                switchblocks => switchblocks.SwitchBlocksToStageVm(
                    indexInSorter: indexInSorter++, 
                    stageVmStyle: StageVmStyle.Standard(false, animationSpeed),
                    keyCount: keyCount, 
                    sortableVms: sortableVms,
                    stageVmStep: StageVmStep.Left);

            while (true)
                {
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            indexInSorter: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(true, animationSpeed),
                            keyCount: keyCount,
                            sortableVms: null,
                            stageVmStep: StageVmStep.None);
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            indexInSorter: indexInSorter++,
                            stageVmStyle: StageVmStyle.Standard(false, animationSpeed),
                            keyCount: keyCount,
                            sortableVms: null,
                            stageVmStep: StageVmStep.None);
            };
        }

        public static IEnumerable<StageVm> ToStageVms(
            this Sorting.StagedSorterDef stagedSorterDef,
            StageLayout stageLayout,
            SortableItemVm[] sortableItemVms,
            AnimationSpeed animationSpeed)
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
                    StageVmMaps(keyCount, sortableItemVms, animationSpeed), 
                    (s, m) => m(s)
                 );

        }


        public static IEnumerable<StageVm> ResetSortables(
            this IEnumerable<StageVm> stageVms, SortableItemVm[] sortableItemVms)
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
