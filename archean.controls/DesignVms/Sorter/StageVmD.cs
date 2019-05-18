using archean.controls.ViewModel.Sorter;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace archean.controls.DesignVms.Sorter
{
    public class StageVmD : StageVm
    {
        public StageVmD() :
            base(
                    stageVmStep: StageVmStep.Left,
                    keyLineBrush: Brushes.DarkBlue,
                    keyCount: _KeyCount,
                    switchLineWidth: _SwitchWidth,
                    switchSpacing: _SwitchSpacing,
                    lineThickness: _LineThickness,
                    keyLineSpacing: _LineSpacing,
                    hPadding: _HPadding,
                    vPadding: _VPadding,
                    backgroundBrush: _BackgroundBrush,
                    keyPairVms: _KeyPairVms,
                    sortableVms: SortableVmExt.StartingPositionInts(
                                                        _KeyCount, 
                                                        ScramblePos(_KeyCount))
                )
        {
        }

        public static int[] ScramblePos(int order)
        {
            return
                core.Combinatorics.FisherYatesShuffle(
                new System.Random(),
                Enumerable.Range(0, order).ToArray()
                ).ToArray();
        }

        public static Brush _LineBrush = Brushes.DarkBlue;
        public static int _KeyCount = 16;
        public static double _SwitchWidth = 1.0;
        public static double _SwitchSpacing = 2.0;
        public static double _LineThickness = 1.0;
        public static double _LineSpacing = 3.0;
        public static double _HPadding = 3.0;
        public static double _VPadding = 1.0;
        public static Brush _BackgroundBrush = Brushes.Black;

        public static IEnumerable<KeyPairVm> _KeyPairVms
        {
            get
            {
                yield return new KeyPairVm(brush: Brushes.Green, orderInStage:0, hiKey:12, lowKey:8);
                yield return new KeyPairVm(brush: Brushes.Black, orderInStage: 0, hiKey: 15, lowKey: 13);
                yield return new KeyPairVm(brush: Brushes.Red, orderInStage: 0, hiKey: 5, lowKey: 4);

                yield return new KeyPairVm(brush: Brushes.Green, orderInStage: 1, hiKey: 1, lowKey: 0);
                yield return new KeyPairVm(brush: Brushes.Black, orderInStage: 1, hiKey: 3, lowKey: 2);
                yield return new KeyPairVm(brush: Brushes.Green, orderInStage: 1, hiKey: 5, lowKey: 4);
                yield return new KeyPairVm(brush: Brushes.Cyan, orderInStage: 1, hiKey: 7, lowKey: 6);

                yield return new KeyPairVm(brush: Brushes.Green, orderInStage: 2, hiKey: 2, lowKey: 0);
                yield return new KeyPairVm(brush: Brushes.DarkOliveGreen, orderInStage: 2, hiKey: 2, lowKey: 0);

                yield return new KeyPairVm(brush: Brushes.Crimson, orderInStage: 3, hiKey: 2, lowKey: 0);
                yield return new KeyPairVm(brush: Brushes.Green, orderInStage: 3, hiKey: 7, lowKey: 4);
                yield return new KeyPairVm(brush: Brushes.Blue, orderInStage: 3, hiKey: 13, lowKey: 10);

                yield return new KeyPairVm(brush: Brushes.Purple, orderInStage: 4, hiKey: 8, lowKey: 0);
                yield return new KeyPairVm(brush: Brushes.Crimson, orderInStage: 4, hiKey: 12, lowKey: 10);
                yield return new KeyPairVm(brush: Brushes.Brown, orderInStage: 4, hiKey: 15, lowKey: 13);
            }
        }
    }
}
