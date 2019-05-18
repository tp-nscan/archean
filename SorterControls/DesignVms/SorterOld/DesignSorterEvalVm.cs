using System.Collections.ObjectModel;
using System.Linq;
using SorterControls.View.SorterOld;
using SorterControls.ViewModel.SorterOld;
using Sorting.Evals;
using Sorting.TestData;

namespace SorterControls.DesignVms.SorterOld
{
    public class DesignSorterEvalVm : SorterEvalVm
    {
        public DesignSorterEvalVm()
            : base(
                        sorterEval: DesignSorterEval(),
                        lineBrushes: LineBrushFactory.GradedBlueBrushes(keyCount),
                        switchBrushes: LineBrushFactory.GradedRedBrushes(keyCount),
                        width: 8,
                        height: 150,
                        showUnusedSwitches: false,
                        showStages: false,
                        sorterVmType: SorterVmType.Staged
                 )
        {
        }

        private const int keyCount = 16;
        private static ISorterEval DesignSorterEval()
        {
            return SorterEvals.TestSorterEval(keyCount, 123, 800);
        }
    }

    public class DesignSorterEvalVms
    {
        public DesignSorterEvalVms()
        {
            for (var i = 0; i < 200; i++)
            {
                _sorterEvalVms.Add(
                    new SorterEvalVm
                        (
                            sorterEval: SorterEvals.TestSorterEval(KeyCount, 1323 + i, 700),
                            lineBrushes: LineBrushFactory.GradedBlueBrushes(KeyCount),
                            switchBrushes: LineBrushFactory.GradedRedBrushes(KeyCount),
                            width: 8,
                            height: 150,
                            showUnusedSwitches: false,
                            showStages: false,
                            sorterVmType: SorterVmType.Staged
                        )
                    );
            }

            _sorterEvalVms = new ObservableCollection<SorterEvalVm>(_sorterEvalVms.OrderBy(e => e.SwitchUseCount));
        }

        private const int KeyCount = 8;

        private ObservableCollection<SorterEvalVm> _sorterEvalVms
                = new ObservableCollection<SorterEvalVm>();
        public ObservableCollection<SorterEvalVm> SorterEvalVms
        {
            get { return _sorterEvalVms; }
            set { _sorterEvalVms = value; }
        }
    }
}
