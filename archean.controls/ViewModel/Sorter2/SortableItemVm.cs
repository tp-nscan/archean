using archean.core;
using System;
using System.Linq;
using System.Windows.Media;
using archean.controls.Utils;
using System.Collections.Generic;

namespace archean.controls.ViewModel.Sorter2
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

        public static SortableItemVm[] ToRedBlueSortableItemVms(this int[] positions, int order, bool showLabel)
        {
            var csB = ColorSets.ColorSpan(order, Colors.Blue, Colors.Red)
                                    .Select(c => new SolidColorBrush(c))
                                    .ToArray();

            return
                Enumerable.Range(0, order).Select
                (i =>
                   new SortableItemVm(
                        backgroundBrush: csB[positions[i]],
                        foregroundBrush: Brushes.White,
                        showLabel: showLabel,
                        stageSection: -1,
                        stagePos: StagePos.Left,
                        keyLinePos: i,
                        label: positions[i],
                        sortableValue: positions[i])
                ).ToArray();
        }

        public static SortableItemVm[] ToBlackOrWhiteSortableItemVms(this int[] positions, int order, bool showLabel)
        {
            Func<int, SolidColorBrush> bkB = i => (i == 0) ? Brushes.White : Brushes.Black;
            Func<int, SolidColorBrush> fgB = i => (i == 0) ? Brushes.Black : Brushes.White;

            return
                Enumerable.Range(0, order).Select
                (i =>
                   new SortableItemVm(
                        backgroundBrush: bkB(positions[i]),
                        foregroundBrush: fgB(positions[i]),
                        showLabel: showLabel,
                        stageSection: -1,
                        stagePos: StagePos.Left,
                        keyLinePos: i,
                        label: positions[i],
                        sortableValue: positions[i])
                ).ToArray();
        }

        public static SortableItemVm ToLeftSortableItemVm(this SortableItemVm sortableItemVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableItemVm.BackgroundBrush,
                    foregroundBrush: sortableItemVm.ForegroundBrush,
                    showLabel: sortableItemVm.ShowLabel,
                    stageSection: -1,
                    stagePos: StagePos.Left,
                    keyLinePos: sortableItemVm.KeyLinePos,
                    label: sortableItemVm.Label,
                    sortableValue: sortableItemVm.SortableValue
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


        public static SortableItemVm ToPresortSortableItemVm(this SortableItemVm sortableItemVm, 
            IEnumerable<KeyPairVm> keyPairVms)
        {
            var tup = sortableItemVm.ToStageSection(keyPairVms);
            return new SortableItemVm(
                    backgroundBrush: sortableItemVm.BackgroundBrush,
                    foregroundBrush: sortableItemVm.ForegroundBrush,
                    showLabel: sortableItemVm.ShowLabel,
                    stageSection: tup.Item2,
                    stagePos: tup.Item1,
                    keyLinePos: sortableItemVm.KeyLinePos,
                    label: sortableItemVm.Label,
                    sortableValue: sortableItemVm.SortableValue
                );
        }


        public static SortableItemVm ToPostSortSortableItemVm(this SortableItemVm sortableItemVm, 
            IEnumerable<KeyPairVm> keyPairVms)
        {
            var tup = sortableItemVm.ToStageSection(keyPairVms);
            return new SortableItemVm(
                    backgroundBrush: sortableItemVm.BackgroundBrush,
                    foregroundBrush: sortableItemVm.ForegroundBrush,
                    showLabel: sortableItemVm.ShowLabel,
                    stageSection: tup.Item2,
                    stagePos: tup.Item1,
                    keyLinePos: sortableItemVm.KeyLinePos,
                    label: sortableItemVm.Label,
                    sortableValue: sortableItemVm.SortableValue
                );
        }

        public static SortableItemVm ToRightSortableItemVm(this SortableItemVm sortableItemVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableItemVm.BackgroundBrush,
                    foregroundBrush: sortableItemVm.ForegroundBrush,
                    showLabel: sortableItemVm.ShowLabel,
                    stageSection: 0,
                    stagePos: StagePos.Right,
                    keyLinePos: sortableItemVm.KeyLinePos,
                    label: sortableItemVm.Label,
                    sortableValue: sortableItemVm.SortableValue
                );
        }

        public static SortableItemVm ToMissingSortableItemVm(this SortableItemVm sortableItemVm)
        {
            return new SortableItemVm(
                    backgroundBrush: sortableItemVm.BackgroundBrush,
                    foregroundBrush: sortableItemVm.ForegroundBrush,
                    showLabel: sortableItemVm.ShowLabel,
                    stageSection: 0,
                    stagePos: StagePos.Missing,
                    keyLinePos: sortableItemVm.KeyLinePos,
                    label: sortableItemVm.Label,
                    sortableValue: sortableItemVm.SortableValue
                );
        }


        public static SortableItemVm[] RandomPermutationSortableItemVms(int keyCount, int seed, bool showLabels)
        {
            return
                MathLib.RandomPermutation(keyCount, seed).ToRedBlueSortableItemVms(keyCount, showLabels);
        }

        public static SortableItemVm[] Random_0_1_SortableItemVms(int keyCount, int seed, bool showLabels)
        {
            return
                MathLib.Random_0_1_Array(keyCount, seed).ToBlackOrWhiteSortableItemVms(keyCount, showLabels);
        }

        public static IEnumerable<KeyPairVm> UpdateKeyPairVms(this IEnumerable<KeyPairVm> keyPairVms, IEnumerable<SortableItemVm> sortableItemVms)
        {
            foreach (var kpvm in keyPairVms)
            {
                var sortableItemsLow = sortableItemVms.FirstOrDefault(
                        swm => (swm.KeyLinePos == kpvm.LowKey));
                var sortableItemHi = sortableItemVms.FirstOrDefault(
                                        swm => (swm.KeyLinePos == kpvm.HiKey));

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
