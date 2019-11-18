using archean.controls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using archean.core;
using System.Windows.Media;

namespace archean.controls.ViewModel.Sorter2
{
    public class SorterVm : BindableBase
    {
        public SorterVm(
                        int order,
                        IEnumerable<StageVm> stageVms)
        {
            Order = order;
           // StageVms = stageVms.ToList();
            StageVms = new ObservableCollection<StageVm>(stageVms);
        }

        public int Order { get; set; }

        public Sorting.StagedSorterDef StagedSorterDef { get; }

        ObservableCollection<StageVm> _stageVms;
        public ObservableCollection<StageVm> StageVms
        {
            get => _stageVms;
            private set
            {
                SetProperty(ref _stageVms, value);
            }
        }
    }

    public static class SorterDisplayVmExt
    {
        public static SortableVmStyle GetSortableVmStyle(this SorterVm sorterDisplayVm)
        {
            return sorterDisplayVm.StageVms
                        .Where(svm=> svm.SortableVm != null)
                        .Select(svm => svm.SortableVm.SortableVmStyle)
                        .FirstOrDefault();
        }

        public static IEnumerable<Func<Sorting.ISwitch[][], StageVm>> StageVmMaps(
                                            StageVmStyle stageVmStyle,
                                            Brush alternating,
                                            int order, 
                                            SortableVm sortableVm)
        {
            var indexInSorter = 0;
            yield return
                switchblocks => switchblocks.SwitchBlocksToStageVm(
                    stageIndex: indexInSorter++, 
                    stageVmStyle: stageVmStyle,
                    order: order, 
                    sortableVm: sortableVm);

            while (true)
            {
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            stageIndex: indexInSorter++,
                            stageVmStyle: stageVmStyle.ChangeBackground(alternating),
                            order: order,
                            sortableVm: null);
                    yield return
                        switchblocks => switchblocks.SwitchBlocksToStageVm(
                            stageIndex: indexInSorter++,
                            stageVmStyle: stageVmStyle,
                            order: order,
                            sortableVm: null);
            };
        }

        public static IEnumerable<StageVm> ToStageVms(
                            this IEnumerable<Sorting.ISwitch[][]> switchBlockSets,
                            StageVmStyle stageVmStyle,
                            Brush alternatingBrush,
                            int order,
                            SortableVm sortableVm)
        {
            return switchBlockSets.Zip
                (
                    StageVmMaps(stageVmStyle, alternatingBrush, order, sortableVm),
                    (s, m) => m(s)
                );
        }

        public static IEnumerable<StageVm> ResetSortables(
                                this IEnumerable<StageVm> stageVms, 
                                SortableVm sortableVm)
        {
            foreach(var stvm in stageVms)
            {
                if(stvm.StageIndex==0)
                {
                    yield return new StageVm(
                        stageIndex: stvm.StageIndex,
                        stageVmStyle: stvm.StageVmStyle,
                        order: stvm.Order,
                        keyPairVms: stvm.KeyPairVms,
                        sortableVm: sortableVm
                    );
                }
                else
                {
                    yield return new StageVm(
                        stageIndex: stvm.StageIndex,
                        stageVmStyle: stvm.StageVmStyle,
                        order: stvm.Order,
                        keyPairVms: stvm.KeyPairVms,
                        sortableVm: null
                    );
                }
            }
        }

        public static SorterVm Step(
                    this SorterVm sorterDisplayVm,
                    SortableVm sortableVm = null)
        {
            var newStages_sortableVm = sorterDisplayVm.StageVms.Step(sortableVm);

            return new SorterVm(
                    order: sorterDisplayVm.Order,
                    stageVms: newStages_sortableVm
                );
        }

        public static SorterVm Tic(
            this SorterVm sorterDisplayVm,
            double animationPct,
            SortableVm sortableVm = null)
        {
            var newStages_sortableVm = sorterDisplayVm.StageVms.Tic(sortableVm, animationPct);

            return new SorterVm(
                    order: sorterDisplayVm.Order,
                    stageVms: newStages_sortableVm
                );
        }

        public static SorterVm ResetSorterDisplayVm(
                this Sorting.StagedSorterDef stagedSorterDef,
                StageLayout stageLayout,
                Func<SortableItemVm[]> sortableItemVmsGen,
                SwitchUseWrap maxSwitchUseInSorter
            )
        {
            SortableVm sortableVm = null;
            if (sortableItemVmsGen != null)
            {
                sortableVm = new SortableVm(
                    order: stagedSorterDef.sorterDef.order,
                    sortableVmStyle: SortableVmStyle.Standard(stagedSorterDef.sorterDef.order, 0),
                    currentSortableItemVms: sortableItemVmsGen.Invoke(),
                    pastSortableItemVms: null,
                    stageVmStep: StageVmStep.Init,
                    animationPct: 0);
            }

            var switchBlockSets = stageLayout.ToPaddedSwitchBlockSets(stagedSorterDef, 0, 6).ToList();
            maxSwitchUseInSorter.Value = 1;

            var stageVms = switchBlockSets.ToStageVms(
                stageVmStyle: StageVmStyle.Standard(stagedSorterDef.sorterDef.order, false, 0, maxSwitchUseInSorter),
                alternatingBrush: StageVmStyleExt.AlternatingBrush,
                order: stagedSorterDef.sorterDef.order,
                sortableVm: sortableVm);

            return new SorterVm(
                    order: stagedSorterDef.sorterDef.order,
                    stageVms: stageVms
                );
        }

    }
}
