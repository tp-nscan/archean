using archean.core;
using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter;

namespace archean.ViewModel.Pages
{
    public class SorterPageVm : BindableBase
    {
        public SorterPageVm()
        {
            StagedSorterDef = SortersFromData.RefSorterModule.CreateRefStagedSorter(
                                SortersFromData.RefSorter.Order8);
        }


        SorterDisplayVm _sorterVm;
        public SorterDisplayVm SorterDisplayVm
        {
            get => _sorterVm;
            set => SetProperty(ref _sorterVm, value);
        }


        private Sorting.StagedSorterDef _stagedSorterDef;
        public Sorting.StagedSorterDef StagedSorterDef
        {
            get => _stagedSorterDef;
            set
            {
                SetProperty(ref _stagedSorterDef, value);
                UpdateSorterDisplayVm();
            }
        }


        void UpdateSorterDisplayVm()
        {
            SorterDisplayVm = new SorterDisplayVm(
                stagedSorterDef: _stagedSorterDef,
                sortableItemVms: StageVmProcs.ScrambledSortableVms(
                                    _stagedSorterDef.sorterDef.order, 
                                    System.DateTime.Now.Millisecond, true),
                animationSpeed: AnimationSpeed,
                stageLayout: StageLayout
            );
        }


        AnimationSpeed _animationSpeed = AnimationSpeed.None;
        public AnimationSpeed AnimationSpeed
        {
            get => _animationSpeed;
            set
            {
                SetProperty(ref _animationSpeed, value);
                UpdateSorterDisplayVm();
            }
        }


        StageLayout _stageLayout = StageLayout.Single;
        public StageLayout StageLayout
        {
            get => _stageLayout;
            set
            {
                SetProperty(ref _stageLayout, value);
                UpdateSorterDisplayVm();
            }
        }

    }
}
