using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SorterControls.ViewModel.Sorter;

namespace SorterControls.DesignVms.Sorter
{
    public class DesignSorterVm : SorterVmImpl
    {
        public DesignSorterVm()
            : base(DesignStageVm._keyCount, _StageVms.ToList())
        {
        }

        static IEnumerable<IStageVm> _StageVms
        {
            get
            {
                yield return new DesignStageVm();

                yield return new StageVmImpl(
                        keyCount: _keyCount,
                        keyPairVms: DesignStageVm.KeyPairVms.ToList(),
                        switchWidth: _switchWidth,
                        lineThickness: _lineThickness,
                        lineBrush: _lineBrush,
                        backgroundBrush: _backgroundBrush1
                    );

                yield return new DesignStageVm();

                yield return new StageVmImpl(
                        keyCount: _keyCount,
                        keyPairVms: DesignStageVm.KeyPairVms.ToList(),
                        switchWidth: _switchWidth,
                        lineThickness: _lineThickness,
                        lineBrush: _lineBrush,
                        backgroundBrush: _backgroundBrush1
                    );
            }
        }

        public const int _keyCount = 8;
        private const double _switchWidth = 0.3;
        private const double _lineThickness = 0.07;
        private static readonly Brush _lineBrush = new SolidColorBrush(Colors.Black);
        private static readonly Brush _backgroundBrush = new SolidColorBrush(Colors.Gray);
        private static readonly Brush _backgroundBrush1 = new SolidColorBrush(Colors.WhiteSmoke);
    }
}
