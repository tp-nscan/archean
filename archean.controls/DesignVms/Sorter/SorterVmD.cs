using archean.controls.ViewModel.Sorter;


namespace archean.controls.DesignVms.Sorter
{
    public class SorterVmD : SorterVm
    {
        public SorterVmD() : base(StagedSorterDef.ToStageVms(true))
        {
        }

        public static core.Sorting.StagedSorterDef StagedSorterDef
        {
            get
            {
                return core.SortersFromData.RefSorterModule.CreateRefStagedSorter(
                            core.SortersFromData.RefSorter.Order32);
            }
        }
    }

}
