using archean.controls.Utils;
using System.Collections.Generic;
using System.Linq;

namespace archean.controls.ViewModel.Sorter2
{
    public class SortableVm
    {
        public SortableVm(
            SortableVmStyle sortableVmStyle,
            int order,
            SortableItemVm[] currentSortableItemVms,
            SortableItemVm[] pastSortableItemVms,
            StageVmStep stageVmStep,
            double animationPct)
        {
            SortableVmStyle = sortableVmStyle;
            Order = order;
            CurrentSortableItemVms = currentSortableItemVms;
            PastSortableItemVms = pastSortableItemVms;
            StageVmStep = stageVmStep;
            AnimationPct = animationPct;
        }
        public double AnimationPct { get; }
        public SortableItemVm[] CurrentSortableItemVms { get; }
        public SortableItemVm[] PastSortableItemVms { get; }
        public int Order { get; }
        public SortableVmStyle SortableVmStyle { get; }
        public StageVmStep StageVmStep { get; }
    }


    public static class SortableVmExt
    {
        public static SortableVm InitSortableVm(this SortableItemVm[] sortableItemVms, SortableVmStyle sortableVmStyle)
        {
            if(sortableVmStyle == null)
            {
                var s = 5;
            }
            return new SortableVm
                (
                    sortableVmStyle: sortableVmStyle,
                    order: sortableItemVms.Length,
                    currentSortableItemVms: sortableItemVms,
                    pastSortableItemVms: null,
                    stageVmStep: StageVmStep.Init,
                    animationPct: 0
                );
        }

        public static SortableVm ChangeSectionCount(this SortableVm sortableVm, int sectionCount)
        {
            if (sortableVm == null) return null;

            return new SortableVm
                (
                    sortableVmStyle: sortableVm.SortableVmStyle.ChangeSectionCount(sectionCount),
                    order: sortableVm.Order,
                    currentSortableItemVms: sortableVm.CurrentSortableItemVms,
                    pastSortableItemVms: sortableVm.PastSortableItemVms,
                    stageVmStep: sortableVm.StageVmStep,
                    animationPct: sortableVm.AnimationPct
                );
        }

        public static SortableVm ChangeAnimationPct(this SortableVm sortableVm, double animationPct)
        {
            if (sortableVm == null) return null;

            return new SortableVm
                (
                    sortableVmStyle: sortableVm.SortableVmStyle,
                    order: sortableVm.Order,
                    currentSortableItemVms: sortableVm.CurrentSortableItemVms,
                    pastSortableItemVms: sortableVm.PastSortableItemVms,
                    stageVmStep: sortableVm.StageVmStep,
                    animationPct: animationPct
                );
        }

        public static SortableVm ToLeftStep(this SortableVm sortableVm, IEnumerable<KeyPairVm> keyPairVms)
        {
            return new SortableVm(
                order: sortableVm.Order,
                sortableVmStyle: sortableVm.SortableVmStyle,
                currentSortableItemVms: sortableVm.CurrentSortableItemVms
                                                   .Select(svm => svm.ToPresortSortableItemVm(keyPairVms))
                                                   .ToArray(),
                pastSortableItemVms: sortableVm.CurrentSortableItemVms
                                                   .Select(svm => svm.ToLeftSortableItemVm())
                                                   .ToArray(),
                stageVmStep: StageVmStep.Left,
                animationPct: 0);
        }

        public static SortableVm ToPreSortStep(this SortableVm sortableVm, IEnumerable<KeyPairVm> keyPairVms)
        {
            var newSortableVm =
                new SortableVm(
                        order: sortableVm.Order,
                        sortableVmStyle: sortableVm.SortableVmStyle,
                        currentSortableItemVms: sortableVm.CurrentSortableItemVms.UpdateSortableVms(keyPairVms).ToArray(),
                        pastSortableItemVms: sortableVm.CurrentSortableItemVms,
                        stageVmStep: sortableVm.StageVmStep,
                        animationPct: 0);

            return new SortableVm(
                order: sortableVm.Order,
                sortableVmStyle: sortableVm.SortableVmStyle,
                currentSortableItemVms: newSortableVm.CurrentSortableItemVms
                                                  .Select(svm => svm.ToPostSortSortableItemVm(keyPairVms))
                                                  .ToArray(),
                pastSortableItemVms: sortableVm.CurrentSortableItemVms,
                stageVmStep: StageVmStep.Presort,
                animationPct: 0);
        }

        public static SortableVm ToPostSortStep(this SortableVm sortableVm)
        {
            return new SortableVm(
                order: sortableVm.Order,
                sortableVmStyle: sortableVm.SortableVmStyle,
                currentSortableItemVms: sortableVm.CurrentSortableItemVms
                                                  .Select(svm => svm.ToRightSortableItemVm())
                                                  .ToArray(),
                pastSortableItemVms: sortableVm.CurrentSortableItemVms,
                stageVmStep: StageVmStep.PostSort,
                animationPct: 0);
        }

        public static SortableVm ToInitStep(this SortableVm sortableVm)
        {
            return new SortableVm(
                order: sortableVm.Order,
                sortableVmStyle: sortableVm.SortableVmStyle,
                currentSortableItemVms: sortableVm.CurrentSortableItemVms,
                pastSortableItemVms: null,
                stageVmStep: StageVmStep.Init,
                animationPct: 0);
        }

        public static IEnumerable<SortableItemVm> UpdateSortableVms(this IEnumerable<SortableItemVm> sortableItemVms, IEnumerable<KeyPairVm> keyPairVms)
        {
            foreach (var sivm in sortableItemVms)
            {
                var kpvmLow = keyPairVms.FirstOrDefault(k => k.LowKey == sivm.KeyLinePos);
                if (kpvmLow != null)
                {
                    var sHi = sortableItemVms.First(s => (s.KeyLinePos == kpvmLow.HiKey));
                    if (sivm.SortableValue > sHi.SortableValue)
                    {
                        yield return new SortableItemVm(
                            backgroundBrush: sivm.BackgroundBrush,
                            foregroundBrush: sivm.ForegroundBrush,
                            stageSection: sivm.StageSection,
                            stagePos: sivm.StagePos,
                            keyLinePos: sHi.KeyLinePos,
                            showLabel: sivm.ShowLabel,
                            label: sivm.Label,
                            sortableValue: sivm.SortableValue);
                    }
                    else yield return sivm;
                }
                else
                {
                    var kpvmHi = keyPairVms.FirstOrDefault(k => k.HiKey == sivm.KeyLinePos);
                    if (kpvmHi == null) yield return sivm;

                    else
                    {
                        var sLow = sortableItemVms.First(s => (s.KeyLinePos == kpvmHi.LowKey));
                        if (sivm.SortableValue < sLow.SortableValue)
                        {
                            yield return new SortableItemVm(
                                backgroundBrush: sivm.BackgroundBrush,
                                foregroundBrush: sivm.ForegroundBrush,
                                stageSection: sivm.StageSection,
                                stagePos: sivm.StagePos,
                                keyLinePos: sLow.KeyLinePos,
                                showLabel: sivm.ShowLabel,
                                label: sivm.Label,
                                sortableValue: sivm.SortableValue);
                        }
                        else yield return sivm;
                    }
                }
            }
        }




    }
}
