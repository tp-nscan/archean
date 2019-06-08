using archean.controls.ViewModel.Sorter;


namespace archean.controls.DesignVms.Sorter
{
    public class SorterVmD : SorterVm
    {
        public SorterVmD() : base(
            stagedSorterDef: StagedSorterDefD, 
            sortableItemVms: StageVmProcs.ScrambledSortableVms(StagedSorterDefD.sorterDef.order, System.DateTime.Now.Millisecond, true),
            animationSpeed: ViewModel.AnimationSpeed.None
            )
        {
        }

        static core.Sorting.StagedSorterDef _stagedSorterDefD;

        public static core.Sorting.StagedSorterDef StagedSorterDefD
        {
            get
            {
                return _stagedSorterDefD ?? (
                    _stagedSorterDefD = core.SortersFromData.RefSorterModule.CreateRefStagedSorter(
                            core.SortersFromData.RefSorter.Order8));
            }
        }
    }

}
