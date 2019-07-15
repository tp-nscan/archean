namespace archean.controls.ViewModel.Sorter2
{
    public enum StageVmStep
    {
        Left,
        Presort,
        PostSort,
        Right,
        None
    }

    public static class StageVmStepExt
    {
        public static StageVmStep ToNextStep(this StageVmStep stageVmStep)
        {
            switch (stageVmStep)
            {
                case StageVmStep.Left:
                    return StageVmStep.Presort;
                case StageVmStep.Presort:
                    return StageVmStep.PostSort;
                case StageVmStep.PostSort:
                    return StageVmStep.Right;
                case StageVmStep.Right:
                    return StageVmStep.None;
                case StageVmStep.None:
                    return StageVmStep.None;
                default:
                    throw new System.Exception($"{stageVmStep} not handled");
            }
        }
    }
}
