using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using Sorting.Evals;
using Sorting.Stages;

namespace SorterControls.ViewModel.SorterOld
{
    public static class StagedSorterVm
    {
        public static ISorterEvalVm ToStagedSorterVm
            (
                this ISorterEval sorterEval,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                int height,
                bool showUnusedSwitches
            )
        {
            return new StagedSorterVmImpl
                (
                    sorterEval: sorterEval,
                    lineBrushes: lineBrushes,
                    switchBrushes: switchBrushes,
                    width: width,
                    height: height,
                    showUnusedSwitches: showUnusedSwitches
                );
        }
    }

    public class StagedSorterVmImpl : NotifyPropertyChanged, ISorterEvalVm
    {
        public StagedSorterVmImpl
            (
                ISorterEval sorterEval,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                int height,
                bool showUnusedSwitches
            )
        {
            _sorterEval = sorterEval;
            _stagedSorter = sorterEval.ToStagedSorter(includeUnused: showUnusedSwitches);
            _lineBrushes = lineBrushes;
            _switchBrushes = switchBrushes;
            _showUnusedSwitches = showUnusedSwitches;
            _height = height;
            _width = width;

            foreach (var sorterStage in StagedSorter.SorterStages)
            {
                SorterStageVms.Add
                    (
                        new SorterStageVm
                            (
                                sorterStage: sorterStage,
                                lineBrushes: LineBrushes,
                                switchBrushes: SwitchBrushes,
                                showUnusedSwitches: ShowUnusedSwitches,
                                width: Width,
                                switchableGroupCount: SorterEval.SwitchableGroupCount
                            )
                    );
            }
        }

        private readonly List<Brush> _lineBrushes;
        private List<Brush> LineBrushes
        {
            get { return _lineBrushes; }
        }

        private readonly int _height;
        public int Height
        {
            get { return _height; }
        }

        private readonly bool _showUnusedSwitches;
        public bool ShowUnusedSwitches
        {
            get { return _showUnusedSwitches; }
        }

        private readonly IStagedSorter<ISwitchEval> _stagedSorter;
        IStagedSorter<ISwitchEval> StagedSorter
        {
            get { return _stagedSorter; }
        }

        private ObservableCollection<SorterStageVm> _sorterStageVms = new ObservableCollection<SorterStageVm>();
        public ObservableCollection<SorterStageVm> SorterStageVms
        {
            get { return _sorterStageVms; }
            set { _sorterStageVms = value; }
        }

        public SorterVmType SorterVmType
        {
            get { return SorterVmType.Staged;}
        }

        private readonly ISorterEval _sorterEval;
        ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        private readonly List<Brush> _switchBrushes;
        private List<Brush> SwitchBrushes
        {
            get { return _switchBrushes; }
        }

        public int KeyCount
        {
            get { return SorterEval.KeyCount; }
        }

        public int SwitchUseCount
        {
            get { return SorterEval.SwitchUseCount; }
        }

        public bool Success
        {
            get { return SorterEval.Success; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }

    }

}
