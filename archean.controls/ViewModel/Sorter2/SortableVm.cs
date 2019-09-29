namespace archean.controls.ViewModel.Sorter2
{
    public class SortableVm
    {
       //internal readonly double WidthToHeight;

        public SortableVm(
            SortableVmStyle sortableVmStyle,
            int order,
            SortableItemVm[] currentSortableItemVms,
            SortableItemVm[] nextSortableItemVms,
            StageVmStep stageVmStep,
            double animationPct)
        {
            SortableVmStyle = sortableVmStyle;
            Order = order;
            CurrentSortableItemVms = currentSortableItemVms;
            NextSortableItemVms = nextSortableItemVms;
            StageVmStep = stageVmStep;
            AnimationPct = animationPct;
        }
        public double AnimationPct { get; }
        public SortableItemVm[] CurrentSortableItemVms { get; }
        public SortableItemVm[] NextSortableItemVms { get; }
        public int Order { get; }
        public SortableVmStyle SortableVmStyle { get; }
        public StageVmStep StageVmStep { get; }
    }


    public static class SortableVmExt
    {
        public static SortableVm InitSortableVm(this SortableItemVm[] sortableItemVms, SortableVmStyle sortableVmStyle)
        {
            return new SortableVm
                (
                    sortableVmStyle: sortableVmStyle,
                    order: sortableItemVms.Length,
                    currentSortableItemVms: sortableItemVms,
                    nextSortableItemVms: null,
                    stageVmStep: StageVmStep.Left,
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
                    nextSortableItemVms: sortableVm.NextSortableItemVms,
                    stageVmStep: sortableVm.StageVmStep,
                    animationPct: sortableVm.AnimationPct
                );
        }
    }
}
