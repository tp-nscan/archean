namespace archean.controls.ViewModel.Sorter2
{
    public class SortableVm
    {
        public SortableVm(SortableItemVm[] currentSortableItemVms,
                          SortableItemVm[] nextSortableItemVms,
                          StageVmStep stageVmStep,
                          double animationPct)
        {
            CurrentSortableItemVms = currentSortableItemVms;
            NextSortableItemVms = nextSortableItemVms;
            StageVmStep = stageVmStep;
            AnimationPct = animationPct;
        }
        public double AnimationPct { get; }
        public SortableItemVm[] CurrentSortableItemVms { get; }
        public SortableItemVm[] NextSortableItemVms { get; }
        public StageVmStep StageVmStep { get; }


    }

    public static class SortableVmExt
    {
        public static SortableVm MakeSortableVm(this SortableVm sortableVm)
        {
            return sortableVm;
        }
    }
}
