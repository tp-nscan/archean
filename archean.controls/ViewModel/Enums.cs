using archean.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace archean.controls.ViewModel
{
    public enum TicsPerStep
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
        public static double ToUpdateFrequency(this TicsPerStep animationSpeed)
        {
            switch (animationSpeed)
            {
                case TicsPerStep.Stopped:
                    return -1.0;
                case TicsPerStep.Slow:
                    return 50.0;
                case TicsPerStep.Medium:
                    return 50.0;
                case TicsPerStep.Fast:
                    return 50.0;
                default:
                    return -1.0;
            }
        }

        public static int ToTicsPerStep(this TicsPerStep animationSpeed)
        {
            switch (animationSpeed)
            {
                case TicsPerStep.Stopped:
                    return -1;
                case TicsPerStep.Slow:
                    return 40;
                case TicsPerStep.Medium:
                    return 10;
                case TicsPerStep.Fast:
                    return 3;
                default:
                    return -1;
            }
        }

        public static double ToUpdateSteps(this TicsPerStep animationSpeed)
        {
            switch (animationSpeed)
            {
                case TicsPerStep.Stopped:
                    return -1.0;
                case TicsPerStep.Slow:
                    return 125.0;
                case TicsPerStep.Medium:
                    return 25.0;
                case TicsPerStep.Fast:
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
                case StageLayout.Undefined:
                    switchBlockSets = Enumerable.Empty<Sorting.ISwitch[][]>();
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
