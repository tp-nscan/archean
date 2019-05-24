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

    public class SortableVm
    {
        public SortableVm(
            Brush backgroundBrush,
            Brush foregroundBrush,
            int stageSection,
            StagePos stagePos,
            int keyLinePos,
            int label, 
            int sortableValue)
        {
            BackgroundBrush = backgroundBrush;
            ForegroundBrush = foregroundBrush;
            StageSection = stageSection;
            StagePos = stagePos;
            KeyLinePos = keyLinePos;
            Label = label;
            SortableValue = sortableValue;
        }

        public Brush BackgroundBrush { get; }
        public Brush ForegroundBrush { get; }
        public int Label { get; }
        public int StageSection { get; }
        public StagePos StagePos { get; }
        public int KeyLinePos { get; set; }
        public int SortableValue { get; }
        public static Typeface Typeface = new Typeface("Segoe UI");
    }

    public static class SortableVmExt
    {


        public static SortableVm[] StartingPositionInts(int order, int[] positions)
        {
            var csF = ColorSets.ColorSpan(order, Colors.Red, Colors.Blue)
                                    .Select(c=> new SolidColorBrush(c))
                                    .ToArray();

            var csB = ColorSets.ColorSpan(order, Colors.Blue, Colors.Red)
                                    .Select(c => new SolidColorBrush(c))
                                    .ToArray();

            return
                Enumerable.Range(0, order).Select
                (i =>
                   new SortableVm(
                        backgroundBrush: csB[i],
                        foregroundBrush: Brushes.White,
                        stageSection: -1,
                        stagePos: StagePos.Left,
                        keyLinePos: positions[i],
                        label: i,
                        sortableValue: i)
                ).ToArray();
        }


        public static SortableVm ToLeftSortableVm(this SortableVm sortableVm)
        {
            return new SortableVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    stageSection: -1,
                    stagePos: sortableVm.StagePos,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }


        public static Tuple<StagePos, int> ToStageSection(
            this SortableVm sortableVm, KeyPairVm[] keyPairVms)
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

        public static SortableVm ToPresortSortableVm(this SortableVm sortableVm, KeyPairVm[] keyPairVms)
        {
            var tup = sortableVm.ToStageSection(keyPairVms);
            return new SortableVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    stageSection: tup.Item2,
                    stagePos: tup.Item1,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }


        public static SortableVm ToPostSortSortableVm(this SortableVm sortableVm, KeyPairVm[] keyPairVms)
        {
            var tup = sortableVm.ToStageSection(keyPairVms);
            return new SortableVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    stageSection: tup.Item2,
                    stagePos: tup.Item1,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }

        public static SortableVm ToRightSortableVm(this SortableVm sortableVm)
        {
            return new SortableVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    stageSection: 0,
                    stagePos: StagePos.Right,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }

        public static SortableVm ToMissingSortableVm(this SortableVm sortableVm)
        {
            return new SortableVm(
                    backgroundBrush: sortableVm.BackgroundBrush,
                    foregroundBrush: sortableVm.ForegroundBrush,
                    stageSection: 0,
                    stagePos: StagePos.Missing,
                    keyLinePos: sortableVm.KeyLinePos,
                    label: sortableVm.Label,
                    sortableValue: sortableVm.SortableValue
                );
        }


        public static SortableVm[] ToLeftStep(this SortableVm[] sortableVms)
        {
            return sortableVms.Select(svm => svm.ToLeftSortableVm())
                              .ToArray();
        }

        public static SortableVm[] ToPreSortStep(this SortableVm[] sortableVms, KeyPairVm[] keyPairVms)
        {
            return sortableVms.Select(svm => svm.ToPresortSortableVm(keyPairVms))
                              .ToArray();
        }

        public static SortableVm[] ToPostSortStep(this SortableVm[] sortableVms, KeyPairVm[] keyPairVms)
        {
            return sortableVms.Select(svm => svm.ToPostSortSortableVm(keyPairVms))
                              .ToArray();
        }

        public static SortableVm[] ToRightStep(this SortableVm[] sortableVms)
        {
            return sortableVms.Select(svm => svm.ToRightSortableVm())
                              .ToArray();
        }

        public static SortableVm[] ToMissingStep(this SortableVm[] sortableVms)
        {
            return sortableVms.Select(svm => svm.ToMissingSortableVm())
                              .ToArray();
        }

        public static void SortWithAKeyPairVm(this SortableVm[] sortableVms, KeyPairVm keyPairVm)
        {
            var swL = sortableVms.FirstOrDefault(
                                    swm => (swm.KeyLinePos == keyPairVm.LowKey));

            var swH = sortableVms.FirstOrDefault(
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

        public static void SortTheSortableVms(this SortableVm[] sortableVms, KeyPairVm[] keyPairVms)
        {
            foreach(var kpvm in keyPairVms)
            {
                SortWithAKeyPairVm(sortableVms, kpvm);
            }
        }
    }
}
