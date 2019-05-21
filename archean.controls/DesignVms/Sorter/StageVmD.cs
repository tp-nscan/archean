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
                    keyLineBrush: StageVmStyles.Standard.LineBrush,
                    keyCount: _KeyCount,
                    switchLineWidth: StageVmStyles.Standard.SwitchWidth,
                    switchSpacing: StageVmStyles.Standard.SwitchSpacing,
                    lineThickness: StageVmStyles.Standard.LineThickness,
                    keyLineSpacing: StageVmStyles.Standard.LineSpacing,
                    hPadding: StageVmStyles.Standard.HPadding,
                    vPadding: StageVmStyles.Standard.VPadding,
                    backgroundBrush: StageVmStyles.Standard.BackgroundBrush,
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

        public static int _KeyCount = 16;


        public static IEnumerable<KeyPairVm> _KeyPairVms
        {
            get
            {
               var stbs = core.Sorting.StageLayout.LayoutRandomStage(_KeyCount, new System.Random()).ToArray();

                for(var i=0; i < stbs.Length; i++)
                {
                    var swb = stbs[i].ToArray();
                    for(var j=0; j< swb.Length; j++)
                    {
                        yield return new KeyPairVm(
                            brush: StageVmStyles.Standard.SwitchBrush, 
                            orderInStage: i, 
                            hiKey: swb[j].hi, 
                            lowKey: swb[j].low);
                    }
                }
            }
        }

    }
}
