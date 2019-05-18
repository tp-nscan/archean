using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using Sorting.Evals;
using Sorting.Stages;

namespace SorterControls.ViewModel.SorterOld
{
    public class SorterStageVm : NotifyPropertyChanged
    {
        public SorterStageVm
            (
                ISorterStage<ISwitchEval> sorterStage,
                List<Brush> lineBrushes,
                List<Brush> switchBrushes,
                int width,
                bool showUnusedSwitches,
                int switchableGroupCount
            )
        {
            _sorterStage = sorterStage;
            _lineBrushes = lineBrushes;
            _switchBrushes = switchBrushes;
            _showUnusedSwitches = showUnusedSwitches;
            _width = width;
            _switchableGroupCount = switchableGroupCount;


            for (var i = 0; i < SorterStage.KeyPairCount; i++)
            {
                var keyPair = SorterStage.KeyPair(i);
                if ((keyPair.UseCount < 1) && !ShowUnusedSwitches)
                {
                    continue;
                }

                var switchBrushIndex = Math.Ceiling(
                        (keyPair.UseCount * (SwitchBrushes.Count -1))
                            /
                        SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchGraphicVm(keyPair, SorterStage.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }

        }

        private readonly ISorterStage<ISwitchEval> _sorterStage;
        ISorterStage<ISwitchEval> SorterStage
        {
            get { return _sorterStage; }
        }

        public ObservableCollection<SwitchGraphicVm> SwitchVms { get; set; } = new ObservableCollection<SwitchGraphicVm>();

        private readonly List<Brush> _lineBrushes;
        private List<Brush> LineBrushes
        {
            get { return _lineBrushes; }
        }

        private readonly bool _showUnusedSwitches;
        public bool ShowUnusedSwitches
        {
            get { return _showUnusedSwitches; }
        }

        private readonly List<Brush> _switchBrushes;
        private List<Brush> SwitchBrushes
        {
            get { return _switchBrushes; }
        }


        private readonly int _switchableGroupCount;
        public int SwitchableGroupCount
        {
            get { return _switchableGroupCount; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }
    }
}
