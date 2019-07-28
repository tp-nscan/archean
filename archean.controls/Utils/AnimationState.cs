using archean.controls.ViewModel;

namespace archean.controls.Utils
{
    public class AnimationState
    {
        public AnimationState(
            AnimationMode animationMode,
            AnimationSpeed animationSpeed,
            int totalSteps)
        {
            AnimationMode = animationMode;
            AnimationSpeed = animationSpeed;
            TotalSteps = totalSteps;
        }

        public AnimationSpeed AnimationSpeed { get; }
        public int TotalSteps { get; }
        public AnimationMode AnimationMode { get; }
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
                animationSpeed: animationState.AnimationSpeed,
                totalSteps: animationState.TotalSteps);
        }

        public static AnimationState ChangeAnimationSpeed(this AnimationState animationState, AnimationSpeed animationSpeed)
        {
            return new AnimationState(
                animationMode: animationState.AnimationMode,
                animationSpeed: animationSpeed,
                totalSteps: animationState.TotalSteps);
        }

        public static AnimationState Step(this AnimationState animationState)
        {
            return new AnimationState(
                animationMode: animationState.AnimationMode,
                animationSpeed: AnimationSpeed.Stopped,
                totalSteps: animationState.TotalSteps + 1);
        }

        public static AnimationState Reset(this AnimationState animationState)
        {
            return new AnimationState(
                animationMode: AnimationMode.Reset,
                animationSpeed: animationState.AnimationSpeed,
                totalSteps: 0);
        }
    }
}
