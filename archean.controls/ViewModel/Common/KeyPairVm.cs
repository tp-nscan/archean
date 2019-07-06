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

    public class KeyPairVm : core.Sorting.ISwitch
    {
        public KeyPairVm(Brush notUsedBrush, 
                         Brush inUseBrush, 
                         Brush wasUsedBrush, 
                         KeyPairUse keyPairUse,
                         int stageSection,
                         int stageIndex,
                         int hiKey, 
                         int lowKey,
                         int useCount)
                        
        {
            NotUsedBrush = notUsedBrush;
            InUseBrush = inUseBrush;
            WasUsedBrush = wasUsedBrush;
            StageSection = stageSection;
            StageIndex = stageIndex;
            KeyPairUse = keyPairUse;
            HiKey = hiKey;
            LowKey = lowKey;
            UseCount = useCount;
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
        public KeyPairUse KeyPairUse { get; }
        public int UseCount { get; }
        public int StageSection { get; }
        public int HiKey { get; }
        public int LowKey { get; }
        public int StageIndex { get; }

        public int low => LowKey;

        public int hi => HiKey;
    }


    public static class KeyPairVmExt
    {
        public static IEnumerable<KeyPairVm> ToKeyPairVms(
                this core.Sorting.ISwitch[][] switchblocks, 
                int stageIndex,
                StageVmStyle stageVmStyle)
        {
            for (var stageSection = 0; stageSection < switchblocks.Length; stageSection++)
            {
                var swb = switchblocks[stageSection].ToArray();
                for (var i = 0; i < swb.Length; i++)
                {
                    yield return new KeyPairVm(
                        notUsedBrush: stageVmStyle.SwitchBrushNotUsed,
                        inUseBrush: stageVmStyle.SwitchBrushInUse,
                        wasUsedBrush: stageVmStyle.SwitchBrushWasUsed,
                        stageSection: stageSection,
                        keyPairUse: KeyPairUse.NotUsed,
                        stageIndex: stageIndex,
                        hiKey: swb[i].hi,
                        lowKey: swb[i].low,
                        useCount: 0);
                }
            }
        }

        public static IEnumerable<KeyPairVm> ToRandomKeyPairVms(this StageVmStyle stageVmStyle, int keyCount, int stageIndex)
        {
            var switchblocks = core.Sorting.StageLayout.LayoutRandomStage(keyCount, new System.Random()).ToArray();
            return switchblocks.ToKeyPairVms(stageIndex, stageVmStyle);
        }

        public static KeyPairVm ToWasUsed(this KeyPairVm keyPairVm)
        {
                return new KeyPairVm(
                        notUsedBrush: keyPairVm.NotUsedBrush,
                        inUseBrush: keyPairVm.InUseBrush,
                        wasUsedBrush: keyPairVm.WasUsedBrush,
                        stageSection: keyPairVm.StageSection,
                        keyPairUse: (keyPairVm.KeyPairUse == KeyPairUse.InUse) ? KeyPairUse.WasUsed : keyPairVm.KeyPairUse,
                        stageIndex: keyPairVm.StageIndex,
                        hiKey: keyPairVm.HiKey,
                        lowKey: keyPairVm.LowKey,
                        useCount: keyPairVm.UseCount);
        }

        public static KeyPairVm ToNotUsed(this KeyPairVm keyPairVm)
        {
            return new KeyPairVm(
                    notUsedBrush: keyPairVm.NotUsedBrush,
                    inUseBrush: keyPairVm.InUseBrush,
                    wasUsedBrush: keyPairVm.WasUsedBrush,
                    stageSection: keyPairVm.StageSection,
                    keyPairUse: KeyPairUse.NotUsed,
                    stageIndex: keyPairVm.StageIndex,
                    hiKey: keyPairVm.HiKey,
                    lowKey: keyPairVm.LowKey,
                    useCount: 0);
        }
    }

}
