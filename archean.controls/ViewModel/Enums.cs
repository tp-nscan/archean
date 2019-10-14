using archean.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace archean.controls.ViewModel
{
    public enum AnimationSpeed
    {
        Stopped,
        Slow,
        Medium,
        Fast
    }

    public enum UpdateMode
    {
        Stop,
        Tic,
        Step,
        Reset
    }

    public enum StageLayout
    {
        Single,
        Loose,
        Tight,
        Undefined
    }

    public enum SortableType
    {
        Integer,
        Bool,
        Undefined
    }


    public static class EnumExt
    {
        public static double ToUpdateFrequency(this AnimationSpeed animationSpeed)
        {
            switch (animationSpeed)
            {
                case AnimationSpeed.Stopped:
                    return -1.0;
                case AnimationSpeed.Slow:
                    return 1000.0;
                case AnimationSpeed.Medium:
                    return 50.0;
                case AnimationSpeed.Fast:
                    return 2.0;
                default:
                    return -1.0;
            }
        }

        public static double ToUpdateSteps(this AnimationSpeed animationSpeed)
        {
            switch (animationSpeed)
            {
                case AnimationSpeed.Stopped:
                    return -1.0;
                case AnimationSpeed.Slow:
                    return 125.0;
                case AnimationSpeed.Medium:
                    return 25.0;
                case AnimationSpeed.Fast:
                    return 5.0;
                default:
                    return -1.0;
            }
        }


        public static IEnumerable<Sorting.ISwitch[][]> ToSwitchBlockSets(
            this StageLayout stageLayout, 
            Sorting.StagedSorterDef stagedSorterDef)
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

        public static IEnumerable<Sorting.ISwitch[][]> ToPaddedSwitchBlockSets(
                this StageLayout stageLayout,
                Sorting.StagedSorterDef stagedSorterDef,
                int frontPad, int backPad)
        {
            foreach (var swb in Sorting.StageLayout.LayoutEmptyStages.Take(frontPad))
            {
                yield return swb;
            }
            foreach (var swb in stageLayout.ToSwitchBlockSets(stagedSorterDef))
            {
                yield return swb;
            }
            foreach (var swb in Sorting.StageLayout.LayoutEmptyStages.Take(backPad))
            {
                yield return swb;
            }
        }


    }
}
