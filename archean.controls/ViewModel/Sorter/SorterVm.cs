using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archean.controls.ViewModel.Sorter
{
    public class SorterVm
    {
        public SorterVm(IEnumerable<StageVm> stageVms)
        {
            StageVms = new ObservableCollection<StageVm>(stageVms);
        }

        public ObservableCollection<StageVm> StageVms { get; set; }
    }

    public static class SorterVmExt
    {
        public static SorterVm MakeEmpty()
        {
            return new SorterVm(Enumerable.Empty<StageVm>());
        }

        public static SorterVm MakeEmpty2(core.Sorting.StagedSorterDef stagedSorter)
        {
            return new SorterVm(Enumerable.Empty<StageVm>());
        }

    }
}
