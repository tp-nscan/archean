using archean.core;
using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter;
using System.Linq;
using archean.controls.ViewModel.Common;
using System;

namespace archean.ViewModel.Pages
{
    public class SorterPageVm : BindableBase
    {
        SwitchUseWrap SwitchUseWrap;

        public SorterPageVm()
        {
            SwitchUseWrap = new SwitchUseWrap();
        }


        SorterDisplayVm _sorterDisplayVm;
        public SorterDisplayVm SorterDisplayVm
        {
            get => _sorterDisplayVm;
            set => SetProperty(ref _sorterDisplayVm, value);
        }

        private SortersFromData.RefSorter _refSorter;
        public SortersFromData.RefSorter RefSorter
        {
            get => _refSorter;
            set
            {
                SetProperty(ref _refSorter, value);
                TotalUses = 0;
                StagedSorterDef = SortersFromData.RefSorterModule.CreateRefStagedSorter(value);
            }
        }


        private Sorting.StagedSorterDef _stagedSorterDef;
        public Sorting.StagedSorterDef StagedSorterDef
        {
            get => _stagedSorterDef;
            set
            {
                SetProperty(ref _stagedSorterDef, value);
                ResetSorterDisplayVm();
            }
        }

        void ResetSorterDisplayVm()
        {
            var switchBlockSets = _stagedSorterDef.ToSwitchBlockSets(StageLayout).ToList();
            var sortableItemVms = StageVmProcs.ScrambledSortableVms(
                                        _stagedSorterDef.sorterDef.order,
                                        DateTime.Now.Millisecond, true);
            SwitchUseWrap.Value = 1;

            var stageVms = switchBlockSets.ToStageVms(
                stageVmStyle: StageVmStyle.Standard(false, AnimationSpeed, SwitchUseWrap),
                order: _stagedSorterDef.sorterDef.order, 
                sortableItemVms: sortableItemVms);

            SorterDisplayVm = new SorterDisplayVm(
                    order: _stagedSorterDef.sorterDef.order,
                    sortableItemVms: sortableItemVms,
                    stageVms: stageVms,
                    currentstageIndex: 0
                );
        }

        AnimationSpeed _animationSpeed = AnimationSpeed.None;
        public AnimationSpeed AnimationSpeed
        {
            get => _animationSpeed;
            set
            {
                SetProperty(ref _animationSpeed, value);
                SorterDisplayVm = SorterDisplayVm.ChangeAnimationSpeed(value);
            }
        }

        StageLayout _stageLayout = StageLayout.Loose;
        public StageLayout StageLayout
        {
            get => _stageLayout;
            set
            {
                SetProperty(ref _stageLayout, value);
                ResetSorterDisplayVm();
            }
        }

        object _totalUses = 0;
        public int TotalUses
        {
            set
            {
                SetProperty(ref _totalUses, value);
                SwitchUseWrap.Value = value + 1;
            }
        }

    }
}
