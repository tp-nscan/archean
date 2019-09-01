namespace archean.controls.ViewModel
{
    public enum AnimationSpeed
    {
        Stopped,
        Slow,
        Medium,
        Fast
    }

    public enum UpdateMode
    {
        Stop,
        Tic,
        Step,
        Reset
    }

    public enum StageLayout
    {
        Single,
        Loose,
        Tight,
        Undefined
    }

    public enum SortableType
    {
        Integer,
        Bool,
        Undefined
    }


    public static class EnumExt
    {
        public static double ToUpdateFrequency(this AnimationSpeed animationSpeed)
        {
            switch (animationSpeed)
            {
                case AnimationSpeed.Stopped:
                    return -1.0;
                case AnimationSpeed.Slow:
                    return 1000.0;
                case AnimationSpeed.Medium:
                    return 100.0;
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
                case AnimationSpeed.Stopped:
                    return -1.0;
                case AnimationSpeed.Slow:
                    return 125.0;
                case AnimationSpeed.Medium:
                    return 25.0;
                case AnimationSpeed.Fast:
                    return 5.0;
                default:
                    return -1.0;
            }
        }

    }
}
