using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SorterControls.View.Common;
using Sorting.Evals;
using Sorting.KeyPairs;
using Sorting.Stages;

namespace SorterControls.ViewModel.Sorter
{
    public interface IStageVm
    {
        IReadOnlyList<KeyPairVm> KeyPairVms { get; }
        Brush LineBrush { get; }
        int KeyCount { get; }
        double SwitchWidth { get; }
        double LineThickness { get; }
        Brush BackgroundBrush { get; }
    }

    public static class StageVm
    {
        public static IStageVm ToStageVm
            (
                this ISorterStage<IKeyPair> sorterStage,
                double switchWidth,
                double lineThickness, 
                Brush lineBrush, 
                Brush backgroundBrush,
                Brush switchBrush
            )
        {
            return new StageVmImpl
                (
                    keyCount: sorterStage.KeyCount, 
                    keyPairVms: sorterStage.ToStageLayouts()
                                           .Select(
                                    sl=> new KeyPairVm
                                            (
                                                keyPair: sl.Item2, 
                                                switchBrush:switchBrush, 
                                                position:sl.Item1
                                            )
                                    ).ToList(), 
                    switchWidth: switchWidth,
                    lineThickness: lineThickness, 
                    lineBrush: lineBrush, 
                    backgroundBrush : backgroundBrush
                );

        }

        public static IStageVm ToStageVm
        (
            this ISorterStage<ISwitchEval> sorterStage,
            int useMax,
            double switchWidth,
            double lineThickness,
            Brush lineBrush,
            Brush backgroundBrush,
            IReadOnlyList<Brush> switchBrushes
        )
        {
            return new StageVmImpl
                (
                    keyCount: sorterStage.KeyCount,
                    keyPairVms: sorterStage.ToStageLayouts()
                                           .Select(
                                    sl => new KeyPairVm
                                            (
                                                keyPair: sl.Item2,
                                                switchBrush: BrushFactory.LogBrushOfInt
                                                (
                                                        value: (int) sl.Item2.UseCount,
                                                        max: useMax, 
                                                        brushList: switchBrushes
                                                ),
                                                position: sl.Item1
                                            )
                                    ).ToList(),
                    switchWidth: switchWidth,
                    lineThickness: lineThickness,
                    lineBrush: lineBrush,
                    backgroundBrush: backgroundBrush
                );

        }



    }

    public class StageVmImpl : IStageVm
    {
        public StageVmImpl
            (
                int keyCount, 
                IReadOnlyList<KeyPairVm> keyPairVms, 
                double switchWidth,
                double lineThickness, 
                Brush lineBrush, 
                Brush backgroundBrush
            )
        {
            _keyCount = keyCount;
            _keyPairVms = keyPairVms;
            _switchWidth = switchWidth;
            _lineThickness = lineThickness;
            _lineBrush = lineBrush;
            _backgroundBrush = backgroundBrush;
        }

        private readonly IReadOnlyList<KeyPairVm> _keyPairVms;
        public IReadOnlyList<KeyPairVm> KeyPairVms
        {
            get { return _keyPairVms; }
        }

        private readonly Brush _lineBrush;
        public Brush LineBrush
        {
            get { return _lineBrush; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly double _switchWidth;
        public double SwitchWidth
        {
            get { return _switchWidth; }
        }

        private readonly double _lineThickness;
        public double LineThickness
        {
            get { return _lineThickness; }
        }

        private readonly Brush _backgroundBrush;
        public Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
        }
    }
}
