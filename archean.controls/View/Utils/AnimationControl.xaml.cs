﻿using archean.controls.Utils;
using archean.controls.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace archean.controls.View.Utils
{
    public partial class AnimationControl
    {
        public AnimationControl()
        {
            InitializeComponent();

        }

        protected void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (RenderTimer != null)
            {
                _renderTimer.Elapsed -= OnRenderTimerElapsed;
                _renderTimer.Dispose();
            }
        }


        #region StepCommand

        RelayCommand _stepCommand;

        public RelayCommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
                DoStep,
                CanStep
            ));

        private void DoStep()
        {
            if (AnimationState == null)
            {
                var s = "S";
            }
            else
            {
                AnimationState = AnimationState.Step();
            }
        }

        bool CanStep()
        {
            if (AnimationState == null)
            {
                return false;
            }
            else
            {
                return !AnimationState.IsRunning();
            }
        }

        #endregion // StepCommand


        #region RenderTimer

        Timer _renderTimer;
        public Timer RenderTimer
        {
            get  { return _renderTimer; }
            set
            {
                if (_renderTimer != null)
                {
                    _renderTimer.Elapsed -= OnRenderTimerElapsed;
                    _renderTimer.Dispose();
                }
                _renderTimer = value;
                _renderTimer.Elapsed += OnRenderTimerElapsed;
            }
        }

        private void ResetTimer(double renderInterval, bool keepRunning)
        {
            var wasRunning = false;
            if (RenderTimer != null)
            {
                wasRunning = RenderTimer.Enabled;
                StopTimer();
            }

            if (renderInterval > 0)
            {
                RenderTimer = new Timer(renderInterval);
                if(wasRunning && keepRunning)
                {
                    RenderTimer.Start();
                }
            }
        }

        private void StopTimer()
        {
            if (RenderTimer != null)
            {
                RenderTimer.Enabled = false;
            }
        }

        private double ticks = 0;
        async void OnRenderTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                DoStep();
            });
        }


        #endregion


        #region StartCommand

        RelayCommand _startCommand;

        public ICommand StartCommand => _startCommand ?? (_startCommand = new RelayCommand(
                DoStart,
                CanStart
            ));

        private void DoStart()
        {
            if ((RenderTimer != null) && (! RenderTimer.Enabled))
            {
                RenderTimer.Start();
            }
        }

        bool CanStart()
        {
            if(RenderTimer == null)
            {
                return false;
            }
            return ! RenderTimer.Enabled;
        }

        #endregion // StartCommand


        #region StopCommand

        RelayCommand _stopCommand;

        public ICommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(
                DoStop,
                CanStop
            ));

        private void DoStop()
        {
            StopTimer();
        }

        bool CanStop()
        {
            if (RenderTimer == null)
            {
                return false;
            }
            return RenderTimer.Enabled;
        }

        #endregion // StopCommand


        #region ResetCommand

        RelayCommand _resetCommand;

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new RelayCommand(
                DoReset,
                CanReset
            ));

        private void DoReset()
        {
            if (AnimationState == null)
            {
                var s = "S";
            }
            else
            {
                AnimationState = AnimationState.Reset();
            }
        }

        bool CanReset()
        {
            if (AnimationState == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion // ResetCommand


        #region AnimationState

        public AnimationState AnimationState
        {
            get { return (AnimationState)GetValue(AnimationStateProperty); }
            set { SetValue(AnimationStateProperty, value); }
        }

        public static readonly DependencyProperty AnimationStateProperty =
            DependencyProperty.Register("AnimationState", typeof(AnimationState), typeof(AnimationControl),
            new FrameworkPropertyMetadata(OnAnimationStatePropertyChanged));

        private static void OnAnimationStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animationControl = (AnimationControl)d;
        }

        #endregion


        #region TotalSteps

        public int TotalSteps
        {
            get { return (int)GetValue(TotalStepsProperty); }
            set { SetValue(TotalStepsProperty, value); }
        }

        public static readonly DependencyProperty TotalStepsProperty =
            DependencyProperty.Register("TotalSteps", typeof(int), typeof(AnimationControl),
            new FrameworkPropertyMetadata(0));

        #endregion


        #region AnimationSpeed

        public IEnumerable<AnimationSpeed> AnimationSpeeds
        {
            get
            {
                return Enum.GetValues(typeof(AnimationSpeed)).Cast<AnimationSpeed>();
            }
        }

        public AnimationSpeed AnimationSpeed
        {
            get { return (AnimationSpeed)GetValue(AnimationSpeedProperty); }
            set { SetValue(AnimationSpeedProperty, value); }
        }

        public static readonly DependencyProperty AnimationSpeedProperty =
            DependencyProperty.Register("AnimationSpeed", typeof(AnimationSpeed), typeof(AnimationControl),
            new FrameworkPropertyMetadata(OnAnimationSpeedPropertyChanged));

        private static void OnAnimationSpeedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animationControl = (AnimationControl)d;
            var newAnimationSpeed = (AnimationSpeed)e.NewValue;
            animationControl.ResetTimer(newAnimationSpeed.ToUpdateFrequency(), true);

            //animationControl.AnimationState =
            //      animationControl.AnimationState.ChangeAnimationSpeed(newAnimationSpeed);
        }

        #endregion


    }
}
