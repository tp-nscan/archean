using archean.core;
using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter2;
using System.Linq;
using System;
using System.Collections.Generic;

namespace archean.ViewModel.Pages
{
    public class SorterPageVm2 : BindableBase
    {
        SwitchUseWrap MaxSwitchUseInSorter;

        public SorterPageVm2()
        {
            MaxSwitchUseInSorter = new SwitchUseWrap();
        }


        SorterDisplayVm _sorterDisplayVm;
        public SorterDisplayVm SorterDisplayVm
        {
            get => _sorterDisplayVm;
            set
            {
                if(_sorterDisplayVm == null)
                {
                    SetProperty(ref _sorterDisplayVm, value);
                    return;
                }
                if(_sorterDisplayVm.StageVms.Count != value.StageVms.Count)
                {
                    SetProperty(ref _sorterDisplayVm, value);
                    return;
                }
                for(var stageDex = 0; stageDex < value.StageVms.Count; stageDex++)
                {
                    _sorterDisplayVm.StageVms[stageDex] = value.StageVms[stageDex];
                }
            }
        }


        private SortersFromData.RefSorter _refSorter;
        public SortersFromData.RefSorter RefSorter
        {
            get => _refSorter;
            set
            {
                SetProperty(ref _refSorter, value);
                StagedSorterDef = SortersFromData.RefSorterModule.CreateRefStagedSorter(value);
            }
        }


        int _order;
        public int Order
        {
            get => _order;
            private set
            {
                SetProperty(ref _order, value);
            }
        }

        Func<SortableItemVm[]> _sortableItemVmsGen;
        public Func<SortableItemVm[]> SortableItemVmsGen
        {
            get => _sortableItemVmsGen;
            set
            {
                SetProperty(ref _sortableItemVmsGen, value);
                ResetSorterDisplayVm();
            }
        }


        private Sorting.StagedSorterDef _stagedSorterDef;
        public Sorting.StagedSorterDef StagedSorterDef
        {
            get => _stagedSorterDef;
            set
            {
                SetProperty(ref _stagedSorterDef, value);
                Order = value.sorterDef.order;
                ResetSorterDisplayVm();
            }
        }


        void ResetSorterDisplayVm()
        {
            if (_stagedSorterDef == null)
            {
                return;
            }
            if (StageLayout == StageLayout.Undefined)
            {
                return;
            }

            SortableVm sortableVm = null;
            if (SortableItemVmsGen != null)
            {
                sortableVm = new SortableVm(
                    currentSortableItemVms: SortableItemVmsGen.Invoke(),
                    nextSortableItemVms:null,
                    stageVmStep:StageVmStep.Left,
                    animationPct: 0);
            }

            var switchBlockSets = _stagedSorterDef.ToSwitchBlockSets(StageLayout).ToList();

            MaxSwitchUseInSorter.Value = 1;

            var stageVms = switchBlockSets.ToStageVms(
                stageVmStyle: StageVmStyle.Standard(false, MaxSwitchUseInSorter),
                alternatingBrush: StageVmStyleExt.AlternatingBrush,
                order: _stagedSorterDef.sorterDef.order,
                sortableVm: sortableVm);

            SorterDisplayVm = new SorterDisplayVm(
                    order: _stagedSorterDef.sorterDef.order,
                    stageVms: stageVms
                );
        }

        public IEnumerable<StageLayout> StageLayouts
        {
            get
            {
                yield return StageLayout.Single;
                yield return StageLayout.Loose;
                yield return StageLayout.Tight;
                yield return StageLayout.Undefined;
            }
        }

        StageLayout _stageLayout = StageLayout.Undefined;
        public StageLayout StageLayout
        {
            get => _stageLayout;
            set
            {
                SetProperty(ref _stageLayout, value);
                _sorterDisplayVm = null;
                ResetSorterDisplayVm();
            }
        }

        object _animationState;
        public AnimationState AnimationState
        {
            set
            {
                SetProperty(ref _animationState, value);

                Console.WriteLine(value.UpdateMode.ToString());

                if (SorterDisplayVm == null) return;
                switch (value.UpdateMode)
                {
                    case UpdateMode.Stop:
                        break;
                    case UpdateMode.Tic:
                        SorterDisplayVm = SorterDisplayVm.Tic();

                        break;
                    case UpdateMode.Step:
                        SortableItemVm[] sortableItemVms = null;
                        if (SortableItemVmsGen != null)
                        {
                            sortableItemVms = SortableItemVmsGen.Invoke();
                        }
                        SorterDisplayVm = SorterDisplayVm.Step(sortableItemVms);
                        break;
                    case UpdateMode.Reset:
                        SorterDisplayVm = SorterDisplayVm.ResetSorterDisplayVm(
                                stagedSorterDef: StagedSorterDef,
                                stageLayout: StageLayout,
                                sortableItemVmsGen: SortableItemVmsGen,
                                maxSwitchUseInSorter: MaxSwitchUseInSorter
                            );
                        break;
                    default:
                        throw new Exception($"{value.UpdateMode.ToString()} unknown");
                }

            }
            get => (AnimationState)_animationState;
        }

    }
}
