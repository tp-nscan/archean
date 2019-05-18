namespace archean.controls.ViewModel.Sorter
{
    public enum StageVmStep
    {
        Left,
        Presort,
        PostSort,
        Right
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
                    return StageVmStep.Right;
                default:
                    throw new System.Exception($"{stageVmStep} not handled");
            }
        }
    }
}
