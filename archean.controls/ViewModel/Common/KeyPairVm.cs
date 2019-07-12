using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace archean.controls.ViewModel.Common
{
    public enum KeyPairUse
    {
        Disabled,
        InUse,
        Enabled
    }

    public class KeyPairVm : core.Sorting.ISwitch
    {
        public KeyPairVm(Brush disabledBrush,
                         Brush inUseBrush,
                         Func<int, Brush> wasUsedBrush, 
                         KeyPairUse keyPairUse,
                         int stageSection,
                         int stageIndex,
                         int hiKey, 
                         int lowKey,
                         int useCount)
                        
        {
            DisabledBrush = disabledBrush;
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
                    case KeyPairUse.Disabled:
                        return DisabledBrush;
                    case KeyPairUse.InUse:
                        return InUseBrush;
                    case KeyPairUse.Enabled:
                        return WasUsedBrush(UseCount);
                    default:
                        throw new System.Exception($"{KeyPairUse} not handled");
                }
            }
        }

        public Brush DisabledBrush { get; }
        public Brush InUseBrush { get; }
        public Func<int, Brush> WasUsedBrush { get; }
        public KeyPairUse KeyPairUse { get; }
        public int UseCount { get; }
        public int StageSection { get; }
        public int HiKey { get; }
        public int LowKey { get; }
        public int StageIndex { get; }

        // ISwitch impl
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
                        disabledBrush: stageVmStyle.SwitchBrushNotUsed,
                        inUseBrush: stageVmStyle.SwitchBrushInUse,
                        wasUsedBrush: stageVmStyle.SwitchBrushWasUsed,
                        stageSection: stageSection,
                        keyPairUse: KeyPairUse.Enabled,
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

        public static KeyPairVm ToInactive(this KeyPairVm keyPairVm)
        {
                return new KeyPairVm(
                        disabledBrush: keyPairVm.DisabledBrush,
                        inUseBrush: keyPairVm.InUseBrush,
                        wasUsedBrush: keyPairVm.WasUsedBrush,
                        stageSection: keyPairVm.StageSection,
                        keyPairUse: (keyPairVm.KeyPairUse == KeyPairUse.InUse) ? KeyPairUse.Enabled : keyPairVm.KeyPairUse,
                        stageIndex: keyPairVm.StageIndex,
                        hiKey: keyPairVm.HiKey,
                        lowKey: keyPairVm.LowKey,
                        useCount: keyPairVm.UseCount);
        }

        public static KeyPairVm ResetUseHistory(this KeyPairVm keyPairVm)
        {
            return new KeyPairVm(
                    disabledBrush: keyPairVm.DisabledBrush,
                    inUseBrush: keyPairVm.InUseBrush,
                    wasUsedBrush: keyPairVm.WasUsedBrush,
                    stageSection: keyPairVm.StageSection,
                    keyPairUse: (keyPairVm.KeyPairUse == KeyPairUse.InUse) ? KeyPairUse.Enabled : keyPairVm.KeyPairUse,
                    stageIndex: keyPairVm.StageIndex,
                    hiKey: keyPairVm.HiKey,
                    lowKey: keyPairVm.LowKey,
                    useCount: 0);
        }
    }

}
