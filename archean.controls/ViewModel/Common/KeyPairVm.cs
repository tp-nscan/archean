using archean.controls.ViewModel.Common;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace archean.controls.ViewModel.Common
{
    public enum KeyPairUse
    {
        NotUsed,
        InUse,
        WasUsed
    }

    public class KeyPairVm
    {
        public KeyPairVm(Brush notUsedBrush, 
                         Brush inUseBrush, 
                         Brush wasUsedBrush, 
                         int orderInStage, 
                         int hiKey, int lowKey)
        {
            NotUsedBrush = notUsedBrush;
            InUseBrush = inUseBrush;
            WasUsedBrush = wasUsedBrush;
            StageSection = orderInStage;
            HiKey = hiKey;
            LowKey = lowKey;
        }

        public Brush Brush
        {
            get
            {
                switch (KeyPairUse)
                {
                    case KeyPairUse.NotUsed:
                        return NotUsedBrush;
                    case KeyPairUse.InUse:
                        return InUseBrush;
                    case KeyPairUse.WasUsed:
                        return WasUsedBrush;
                    default:
                        throw new System.Exception($"{KeyPairUse} not handled");
                }
            }
        }

        public Brush NotUsedBrush { get; }
        public Brush InUseBrush { get; }
        public Brush WasUsedBrush { get; }
        public KeyPairUse KeyPairUse { get; set; }
        public int StageSection { get; }
        public int HiKey { get; }
        public int LowKey { get; }
    }


    public static class KeyPairVmExt
    {
        public static IEnumerable<KeyPairVm> ToKeyPairVms(
                this core.Sorting.Switch[][] switchblocks, 
                StageVmStyle stageVmStyle)
        {
            for (var i = 0; i < switchblocks.Length; i++)
            {
                var swb = switchblocks[i].ToArray();
                for (var j = 0; j < swb.Length; j++)
                {
                    yield return new KeyPairVm(
                        notUsedBrush: stageVmStyle.SwitchBrushNotUsed,
                        inUseBrush: stageVmStyle.SwitchBrushInUse,
                        wasUsedBrush: stageVmStyle.SwitchBrushWasUsed,
                        orderInStage: i,
                        hiKey: swb[j].hi,
                        lowKey: swb[j].low);
                }
            }
        }

        public static IEnumerable<KeyPairVm> ToRandomKeyPairVms(this StageVmStyle stageVmStyle, int keyCount)
        {
            var switchblocks = core.Sorting.StageLayout.LayoutRandomStage(keyCount, new System.Random()).ToArray();
            return switchblocks.ToKeyPairVms(stageVmStyle);
        }
    }

}
