using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace archean.controls.ViewModel.Sorter2
{
    public class StageVmStyle
    {
        public Brush KeyLineBrush { get; set; }
        public Brush SwitchBrushNotUsed { get; set; }
        public Brush SwitchBrushInUse { get; set; }
        public Func<int, Brush> SwitchBrushWasUsed { get; set; }
        public double SwitchLineWidth { get; set; }
        public double StageRightMargin { get; set; }
        public double SwitchHSpacing { get; set; }
        public double KeyLineThickness { get; set; }
        public double KeyLineHeight { get; set; }
        public double VPadding { get; set; }
        public Brush BackgroundBrush { get; set; }

        public static StageVmStyle Standard(
                            bool oddStep,
                            SwitchUseWrap maxSwitchUse)
        {
            return new StageVmStyle
            {
                KeyLineBrush = Brushes.Blue,
                SwitchBrushNotUsed = Brushes.Black,
                SwitchBrushInUse = Brushes.Pink,
                SwitchBrushWasUsed = maxSwitchUse.ToSwitchBrushFunc(),
                SwitchLineWidth = 1.0,
                SwitchHSpacing = 3.25,
                StageRightMargin = 3.25,
                KeyLineThickness = 1.0,
                KeyLineHeight = 4.0,
                VPadding = 1.0,
                BackgroundBrush = oddStep ? Brushes.Lavender : Brushes.White
            };
        }

    }

    public static class StageVmStyleExt
    {
        public static float colorSteps = 16;

        public static List<SolidColorBrush> SwitchBrushes =
            core.ColorSets.TwoColorSpan( Colors.Blue, Colors.Red, (int)colorSteps).Select(c => new SolidColorBrush(c)).ToList();


        public static Func<int, Brush> ToSwitchBrushFunc(this SwitchUseWrap maxUseCount)
        {
            return useCount =>
            {
                if (useCount == 0) return Brushes.Black;
                if (maxUseCount.Value < 1) return SwitchBrushes[SwitchBrushes.Count - 1];
                var step = (colorSteps * useCount) / maxUseCount.Value;
                if (step > colorSteps - 1) step = colorSteps - 1;
               // Debug.WriteLine($"maxUseCount:{maxUseCount.Value} useCount:{useCount} step:{step}");
                return SwitchBrushes[(int)step];
            };
        }

        public static Brush AlternatingBrush => new SolidColorBrush(Color.FromScRgb(0.3f, 0, 1.0f, 0));

        public static Func<int, Brush> ToSwitchBrushFunc0(this SwitchUseWrap maxUseCount)
        {
            return useCount =>
                        (useCount < maxUseCount.Value) ? Brushes.Blue : Brushes.GreenYellow;
        }

        public static StageVmStyle ChangeBackground(this StageVmStyle stageVmStyle, Brush newBrush)
        {
            return new StageVmStyle
            {
                KeyLineBrush = stageVmStyle.KeyLineBrush,
                SwitchBrushNotUsed = stageVmStyle.SwitchBrushNotUsed,
                SwitchBrushInUse = stageVmStyle.SwitchBrushInUse,
                SwitchBrushWasUsed = stageVmStyle.SwitchBrushWasUsed,
                SwitchLineWidth = stageVmStyle.SwitchLineWidth,
                SwitchHSpacing = stageVmStyle.SwitchHSpacing,
                StageRightMargin = stageVmStyle.StageRightMargin,
                KeyLineThickness = stageVmStyle.KeyLineThickness,
                KeyLineHeight = stageVmStyle.KeyLineHeight,
                VPadding = stageVmStyle.VPadding,
                BackgroundBrush = newBrush
            };
        }
    }

    public class SwitchUseWrap
    {
        public int Value { get; set; }
    }

}
