using archean.controls.ViewModel;

namespace archean.controls.Utils
{
    public class AnimationState
    {
        public AnimationState(
            AnimationMode animationMode,
            int currentStep,
            int currentTic,
            int ticsPerStep)
        {
            AnimationMode = animationMode;
            CurrentStep = currentStep;
            CurrentTic = currentTic;
            TicsPerStep = ticsPerStep;
        }
        public int CurrentTic { get; }
        public int CurrentStep { get; }
        public int TicsPerStep { get; }
        public AnimationMode AnimationMode { get; }
        public static AnimationState Initial(int ticsPerStep)
        {
            return new AnimationState(
              animationMode: AnimationMode.Stop,
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
            return animationState.AnimationMode == AnimationMode.Run;
        }

        public static AnimationState Stop(this AnimationState animationState)
        {
            return new AnimationState(
                animationMode: AnimationMode.Stop,
                currentStep: animationState.CurrentStep,
                currentTic: animationState.CurrentTic,
                ticsPerStep: animationState.TicsPerStep);
        }

        public static AnimationState Step(this AnimationState animationState)
        {
            return new AnimationState(
                animationMode: AnimationMode.Step,
                currentStep: animationState.CurrentStep + 1,
                currentTic: animationState.CurrentTic,
                ticsPerStep: animationState.TicsPerStep);
        }

        public static AnimationState Run(this AnimationState animationState)
        {
            if(animationState.CurrentTic == animationState.TicsPerStep)
            {
                return new AnimationState(
                    animationMode: AnimationMode.Step,
                    currentStep: animationState.CurrentStep,
                    currentTic: 0,
                    ticsPerStep: animationState.TicsPerStep);
            }

            return new AnimationState(
                    animationMode: AnimationMode.Run,
                    currentStep: animationState.CurrentStep,
                    currentTic: animationState.CurrentTic + 1,
                    ticsPerStep: animationState.TicsPerStep);
        }

        public static AnimationState Reset(this AnimationState animationState)
        {
            return new AnimationState(
                animationMode: AnimationMode.Reset,
                currentStep: 0,
                currentTic: 0,
                ticsPerStep: animationState.TicsPerStep);
        }

    }
}
