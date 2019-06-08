namespace archean.controls.ViewModel
{
    public enum AnimationSpeed
    {
        None,
        Slow,
        Medium,
        Fast
    }

    public static class EnumExt
    {
        public static double ToUpdateFrequency(this AnimationSpeed animationSpeed)
        {
            switch (animationSpeed)
            {
                case AnimationSpeed.None:
                    return -1.0;
                case AnimationSpeed.Slow:
                    return 40.0;
                case AnimationSpeed.Medium:
                    return 20.0;
                case AnimationSpeed.Fast:
                    return 10.0;
                default:
                    return -1.0;
            }
        }

        public static double ToUpdateSteps(this AnimationSpeed animationSpeed)
        {
            switch (animationSpeed)
            {
                case AnimationSpeed.None:
                    return -1.0;
                case AnimationSpeed.Slow:
                    return 20.0;
                case AnimationSpeed.Medium:
                    return 10.0;
                case AnimationSpeed.Fast:
                    return 5.0;
                default:
                    return -1.0;
            }
        }

    }
}
