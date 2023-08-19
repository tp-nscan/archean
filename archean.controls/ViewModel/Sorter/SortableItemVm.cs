using archean.core;
using System;
using System.Linq;
using System.Windows.Media;
using archean.controls.Utils;
using System.Collections.Generic;

namespace archean.controls.ViewModel.Sorter
{
    public enum StagePos
    {
        Left,
        Center,
        Right,
        Missing
    }

    public class SortableItemVm
    {
        public SortableItemVm(
            Brush backgroundBrush,
            Brush foregroundBrush,
            int stageSection,
            StagePos stagePos,
            int keyLinePos,
            bool showLabel,
            int label, 
            int sortableValue)
        {
            BackgroundBrush = backgroundBrush;
            ForegroundBrush = foregroundBrush;
            ShowLabel = showLabel;
            StageSection = stageSection;
            StagePos = stagePos;
            KeyLinePos = keyLinePos;
            Label = label;
            SortableValue = sortableValue;
        }

        public Brush BackgroundBrush { get; }
        public Brush ForegroundBrush { get; }
        public bool ShowLabel { get; }
        public int Label { get; }
        public int StageSection { get; }
        public StagePos StagePos { get; }
        public int KeyLinePos { get; }
        public int SortableValue { get; }
        public static Typeface Typeface = new Typeface("Segoe UI");
    }

    public static class SortableItemVmExt
    {
        public static SortableItemVm Copy(this SortableItemVm sortableItemVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableItemVm.BackgroundBrush,
                    foregroundBrush: sortableItemVm.ForegroundBrush,
                    stageSection: sortableItemVm.StageSection,
                    stagePos: sortableItemVm.StagePos,
                    keyLinePos: sortableItemVm.KeyLinePos,
                    showLabel: sortableItemVm.ShowLabel,
                    label: sortableItemVm.Label,
                    sortableValue: sortableItemVm.SortableValue
                );
        }

        public static SortableItemVm[] ToRedBlueSortableVms(this int[] positions, int order, bool showLabel)
        {
            var csB = ColorSets.ColorSpan(order, Colors.Blue, Colors.Red)
                                    .Select(c => new SolidColorBrush(c))
                                    .ToArray();

            return
                Enumerable.Range(0, order).Select
                (i =>
                   new SortableItemVm(
                        backgroundBrush: csB[i],
                        foregroundBrush: Brushes.White,
                        showLabel: showLabel,
                        stageSection: -1,
                        stagePos: StagePos.Left,
                        keyLinePos: positions[i],
                        label: i,
                        sortableValue: i)
                ).ToArray();
        }


        public static SortableItemVm ToLeftSortableVm(this SortableItemVm sortableVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    showLabel: sortableVm.ShowLabel,
                    stageSection: -1,
                    stagePos: StagePos.Left,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }


        public static Tuple<StagePos, int> ToStageSection(
            this SortableItemVm sortableVm, IEnumerable<KeyPairVm> keyPairVms)
        {
            var swH = keyPairVms.FirstOrDefault(
                kpv => (kpv.HiKey == sortableVm.KeyLinePos)
                        || (kpv.LowKey == sortableVm.KeyLinePos)
                );
            if (swH != null)
            {
                return new Tuple<StagePos, int>(StagePos.Center, swH.StageSection);
            }
            return new Tuple<StagePos, int>(StagePos.Right, 0);
        }


        public static SortableItemVm ToPresortSortableVm(this SortableItemVm sortableVm, IEnumerable<KeyPairVm> keyPairVms)
        {
            var tup = sortableVm.ToStageSection(keyPairVms);
            return new SortableItemVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    showLabel: sortableVm.ShowLabel,
                    stageSection: tup.Item2,
                    stagePos: tup.Item1,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }


        public static SortableItemVm ToPostSortSortableVm(this SortableItemVm sortableVm, IEnumerable<KeyPairVm> keyPairVms)
        {
            var tup = sortableVm.ToStageSection(keyPairVms);
            return new SortableItemVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    showLabel: sortableVm.ShowLabel,
                    stageSection: tup.Item2,
                    stagePos: tup.Item1,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }

        public static SortableItemVm ToRightSortableVm(this SortableItemVm sortableVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    showLabel: sortableVm.ShowLabel,
                    stageSection: 0,
                    stagePos: StagePos.Right,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }

        public static SortableItemVm ToMissingSortableVm(this SortableItemVm sortableVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    showLabel: sortableVm.ShowLabel,
                    stageSection: 0,
                    stagePos: StagePos.Missing,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }


        public static SortableItemVm[] ToLeftStep(this IEnumerable<SortableItemVm> sortableItemVms)
        {
            return sortableItemVms.Select(svm => svm.ToLeftSortableVm())
                              .ToArray();
        }

        public static SortableItemVm[] ToPreSortStep(this IEnumerable<SortableItemVm> sortableItemVms, IEnumerable<KeyPairVm> keyPairVms)
        {
            return sortableItemVms.NullToEnumerable().Select(svm => svm.ToPresortSortableVm(keyPairVms))
                              .ToArray();
        }

        public static SortableItemVm[] ToPostSortStep(this IEnumerable<SortableItemVm> sortableItemVms, IEnumerable<KeyPairVm> keyPairVms)
        {
            return sortableItemVms.Select(svm => svm.ToPostSortSortableVm(keyPairVms))
                              .ToArray();
        }

        public static SortableItemVm[] ToRightStep(this IEnumerable<SortableItemVm> sortableItemVms)
        {
            return sortableItemVms.Select(svm => svm.ToRightSortableVm())
                              .ToArray();
        }

        public static SortableItemVm[] ToMissingStep(this IEnumerable<SortableItemVm> sortableItemVms)
        {
            return sortableItemVms.Select(svm => svm.ToMissingSortableVm())
                              .ToArray();
        }

        public static IEnumerable<SortableItemVm> UpdateSortableVms(this IEnumerable<SortableItemVm> sortableItemVms, IEnumerable<KeyPairVm> keyPairVms)
        {
            foreach (var sivm in sortableItemVms)
            {
                var kpvmLow = keyPairVms.FirstOrDefault(k => k.LowKey == sivm.KeyLinePos);
                if(kpvmLow != null)
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


        public static IEnumerable<KeyPairVm> UpdateKeyPairVms(this IEnumerable<KeyPairVm> keyPairVms, IEnumerable<SortableItemVm> sortableItemVms)
        {
            foreach (var kpvm in keyPairVms)
            {
                var sortableItemsLow = sortableItemVms.FirstOrDefault(
                        swm => (swm.KeyLinePos == kpvm.LowKey));
                var sortableItemHi = sortableItemVms.FirstOrDefault(
                                        swm => (swm.KeyLinePos == kpvm.HiKey));

                if (sortableItemsLow == null) continue;
                if (sortableItemHi == null) continue;

                if (sortableItemsLow.SortableValue > sortableItemHi.SortableValue)
                {
                    var kpv = new KeyPairVm(
                            disabledBrush: kpvm.DisabledBrush,
                            inUseBrush: kpvm.InUseBrush,
                            wasUsedBrush: kpvm.WasUsedBrush,
                            keyPairUse: KeyPairUse.InUse,
                            stageSection: kpvm.StageSection,
                            stageIndex: kpvm.StageIndex,
                            hiKey: kpvm.HiKey,
                            lowKey: kpvm.LowKey,
                            useCount: kpvm.UseCount + 1
                        );
                    yield return kpv;
                }
                else
                {
                    yield return kpvm;
                }
            }
        }

    }
}
