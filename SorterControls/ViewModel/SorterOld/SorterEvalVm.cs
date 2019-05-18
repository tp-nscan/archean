using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Sorting.Evals;
using Sorting.Stages;

namespace SorterControls.ViewModel.SorterOld
{
    public interface ISorterEvalVm : ISorterVm
    {
        bool Success { get; }
        int SwitchUseCount { get; }
    }

    public class SorterEvalVm : ISorterEvalVm
    {
        public SorterEvalVm(
            ISorterEval sorterEval, 
            List<Brush> lineBrushes,
            List<Brush> switchBrushes,
            int width,
            int height,
            bool showUnusedSwitches,
            bool showStages, SorterVmType sorterVmType)
        {
            _sorterEval = sorterEval;
            LineBrushes = lineBrushes;
            SwitchBrushes = switchBrushes;
            _height = height;
            _width = width;
            _showUnusedSwitches = showUnusedSwitches;
            _showStages = showStages;
            _sorterVmType = sorterVmType;
            SetSwitchVms();
        }

        void SetSwitchVms()
        {
            _switchVms.Clear();

            if (ShowStages)
            {
                SetStagedSwitchVms();
                return;
            }

            SetUnstagedSwitchVms();
        }

        void SetUnstagedSwitchVms()
        {
            for (var i = 0; i < SorterEval.KeyPairCount; i++)
            {
                if ((SorterEval.SwitchEvals[i].UseCount == 0) && !ShowUnusedSwitches)
                {
                    continue;
                }

                var keyPair = SorterEval.KeyPair(i);
                var switchBrushIndex = Math.Ceiling(
                        (SorterEval.SwitchEvals[i].UseCount * SwitchBrushes.Count)
                            /
                        SorterEval.SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchGraphicVm(keyPair, SorterEval.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }
        }

        void SetStagedSwitchVms()
        {
            var stagedKeyPairs = new List<ISwitchEval>();

            if (ShowUnusedSwitches)
            {

                stagedKeyPairs = SorterEval.SwitchEvals
                    .ToList()
                    .ToSorterStages(SorterEval.KeyPairCount)
                    .SelectMany(st => st.KeyPairs)
                    .ToList();
            }
            else
            {
                stagedKeyPairs = SorterEval.SwitchEvals
                    .Where(ev => ev.UseCount > 0)
                    .ToList()
                    .ToSorterStages(SorterEval.KeyPairCount)
                    .SelectMany(st => st.KeyPairs)
                    .ToList();
            }


            foreach (var stagedKeyPair in stagedKeyPairs)
            {
                var switchBrushIndex = Math.Ceiling(
                        (stagedKeyPair.UseCount * SwitchBrushes.Count)
                            /
                        SorterEval.SwitchableGroupCount
                    );

                SwitchVms.Add(new SwitchGraphicVm(stagedKeyPair, SorterEval.KeyCount, LineBrushes, Width)
                {
                    SwitchBrush = SwitchBrushes[(int)switchBrushIndex]
                });
            }
        }

        private readonly bool _showUnusedSwitches;
        private bool ShowUnusedSwitches
        {
            get { return _showUnusedSwitches; }
        }

        private readonly bool _showStages;
        private bool ShowStages
        {
            get { return _showStages; }
        }

        private List<Brush> LineBrushes { get; set; }

        private List<Brush> SwitchBrushes { get; set; }

        private readonly SorterVmType _sorterVmType;
        public SorterVmType SorterVmType
        {
            get { return _sorterVmType; }
        }

        public int SwitchUseCount
        {
            get { return SorterEval.SwitchUseCount; }
        }

        public bool Success
        {
            get { return SorterEval.Success; }
        }

        private readonly int _height;
        public int Height
        {
            get { return _height; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }

        public int KeyCount
        {
            get { return SorterEval.KeyCount; }
        }

        private readonly ISorterEval _sorterEval;
        ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        private ObservableCollection<SwitchGraphicVm> _switchVms = new ObservableCollection<SwitchGraphicVm>();
        public ObservableCollection<SwitchGraphicVm> SwitchVms
        {
            get { return _switchVms; }
            set { _switchVms = value; }
        }

        public string StringValue
        {
            get { return String.Empty; }
        }
    }
}
