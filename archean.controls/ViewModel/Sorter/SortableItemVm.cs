using archean.core;
using System;
using System.Linq;
using System.Windows.Media;

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
        public int KeyLinePos { get; set; }
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
            this SortableItemVm sortableVm, KeyPairVm[] keyPairVms)
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


        public static SortableItemVm ToPresortSortableVm(this SortableItemVm sortableVm, KeyPairVm[] keyPairVms)
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


        public static SortableItemVm ToPostSortSortableVm(this SortableItemVm sortableVm, KeyPairVm[] keyPairVms)
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


        public static SortableItemVm[] ToLeftStep(this SortableItemVm[] sortableItemVms)
        {
            return sortableItemVms.Select(svm => svm.ToLeftSortableVm())
                              .ToArray();
        }

        public static SortableItemVm[] ToPreSortStep(this SortableItemVm[] sortableItemVms, KeyPairVm[] keyPairVms)
        {
            return sortableItemVms.Select(svm => svm.ToPresortSortableVm(keyPairVms))
                              .ToArray();
        }

        public static SortableItemVm[] ToPostSortStep(this SortableItemVm[] sortableItemVms, KeyPairVm[] keyPairVms)
        {
            return sortableItemVms.Select(svm => svm.ToPostSortSortableVm(keyPairVms))
                              .ToArray();
        }

        public static SortableItemVm[] ToRightStep(this SortableItemVm[] sortableItemVms)
        {
            return sortableItemVms.Select(svm => svm.ToRightSortableVm())
                              .ToArray();
        }

        public static SortableItemVm[] ToMissingStep(this SortableItemVm[] sortableItemVms)
        {
            return sortableItemVms.Select(svm => svm.ToMissingSortableVm())
                              .ToArray();
        }

        public static void SortWithAKeyPairVm(this SortableItemVm[] sortableItemVms, KeyPairVm keyPairVm)
        {
            var swL = sortableItemVms.FirstOrDefault(
                                    swm => (swm.KeyLinePos == keyPairVm.LowKey));

            var swH = sortableItemVms.FirstOrDefault(
                                    swm => (swm.KeyLinePos == keyPairVm.HiKey));

            if ((swL == null) || (swH == null)) return;

                if (swL.SortableValue > swH.SortableValue)
            {
                var lv = swL.KeyLinePos;
                swL.KeyLinePos = swH.KeyLinePos;
                swH.KeyLinePos = lv;
                keyPairVm.KeyPairUse = KeyPairUse.InUse;
            }
        }

        public static void SortTheSortableVms(this SortableItemVm[] sortableItemVms, KeyPairVm[] keyPairVms)
        {
            foreach(var kpvm in keyPairVms)
            {
                SortWithAKeyPairVm(sortableItemVms, kpvm);
            }
        }
    }
}
