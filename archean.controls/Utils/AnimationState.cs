using archean.controls.ViewModel;

namespace archean.controls.Utils
{
    public class AnimationState
    {
        public AnimationState(
            UpdateMode updateMode,
            int currentStep,
            int currentTic,
            int ticsPerStep)
        {
            UpdateMode = updateMode;
            CurrentStep = currentStep;
            CurrentTic = currentTic;
            TicsPerStep = ticsPerStep;
        }
        public int CurrentTic { get; }
        public int CurrentStep { get; }
        public int TicsPerStep { get; }
        public UpdateMode UpdateMode { get; }
        public static AnimationState Initial(int ticsPerStep)
        {
            return new AnimationState(
              updateMode: UpdateMode.Stop,
              currentStep: 0,
              currentTic: 0,
              ticsPerStep: ticsPerStep);
        }
    }

    public static class AnimationStateExt
    {
        public static bool IsRunning(this AnimationState animationState)
        {
            if(animationState == null)
            {
                return false;
            }
            return animationState.UpdateMode == UpdateMode.Tic;
        }

        public static AnimationState Stop(this AnimationState animationState)
        {
            return new AnimationState(
                updateMode: UpdateMode.Stop,
                currentStep: animationState.CurrentStep,
                currentTic: animationState.CurrentTic,
                ticsPerStep: animationState.TicsPerStep);
        }

        public static AnimationState Step(this AnimationState animationState)
        {
            return new AnimationState(
                updateMode: UpdateMode.Step,
                currentStep: animationState.CurrentStep + 1,
                currentTic: animationState.CurrentTic,
                ticsPerStep: animationState.TicsPerStep);
        }

        public static AnimationState Run(this AnimationState animationState)
        {
            if(animationState.CurrentTic == animationState.TicsPerStep)
            {
                return new AnimationState(
                    updateMode: UpdateMode.Step,
                    currentStep: animationState.CurrentStep,
                    currentTic: 0,
                    ticsPerStep: animationState.TicsPerStep);
            }

            return new AnimationState(
                    updateMode: UpdateMode.Tic,
                    currentStep: animationState.CurrentStep,
                    currentTic: animationState.CurrentTic + 1,
                    ticsPerStep: animationState.TicsPerStep);
        }

        public static AnimationState Reset(this AnimationState animationState)
        {
            return new AnimationState(
                updateMode: UpdateMode.Reset,
                currentStep: 0,
                currentTic: 0,
                ticsPerStep: animationState.TicsPerStep);
        }

    }
}
