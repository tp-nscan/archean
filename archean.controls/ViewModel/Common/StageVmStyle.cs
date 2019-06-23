using System.Windows.Media;

namespace archean.controls.ViewModel.Common
{
    public class StageVmStyle
    {
        public AnimationSpeed AnimationSpeed { get; set; }
        public Brush KeyLineBrush { get; set; }
        public Brush SwitchBrushNotUsed { get; set; }
        public Brush SwitchBrushInUse { get; set; }
        public Brush SwitchBrushWasUsed { get; set; }
        public double SwitchLineWidth { get; set; }
        public double StageRightMargin { get; set; }
        public double SwitchHSpacing { get; set; }
        public double KeyLineThickness { get; set; }
        public double KeyLineHeight { get; set; }
        public double VPadding { get; set; }
        public Brush BackgroundBrush { get; set; }

        public static StageVmStyle Standard(bool oddStep, AnimationSpeed animationSpeed)
        {
            return new StageVmStyle
            {
                AnimationSpeed = animationSpeed,
                KeyLineBrush = Brushes.Blue,
                SwitchBrushNotUsed = Brushes.Black,
                SwitchBrushInUse = Brushes.Yellow,
                SwitchBrushWasUsed = Brushes.GreenYellow,
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
        public static StageVmStyle ChangeAnimationSpeed(
                this StageVmStyle stageVmStyle,
                AnimationSpeed animationSpeed)
        {
            return new StageVmStyle
            {
                AnimationSpeed = animationSpeed,
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
                BackgroundBrush = stageVmStyle.BackgroundBrush
            };
        }
    }

}
