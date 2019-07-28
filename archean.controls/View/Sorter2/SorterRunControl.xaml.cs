﻿using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace archean.controls.View.Sorter2
{
    public partial class SorterRunControl
    {
        public SorterRunControl()
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


        #region SorterDisplayVm

        //[Category("Custom Properties")]
        public SorterDisplayVm SorterDisplayVm
        {
            get { return (SorterDisplayVm)GetValue(SorterDisplayVmProperty); }
            set { SetValue(SorterDisplayVmProperty, value); }
        }

        public static readonly DependencyProperty SorterDisplayVmProperty =
            DependencyProperty.Register("SorterDisplayVm", typeof(SorterDisplayVm), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnSorterVmPropertyChanged));

        private static void OnSorterVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterControlControl = (SorterRunControl)d;
            var sorterDisplayVm = e.NewValue as SorterDisplayVm;
            if (sorterDisplayVm != null)
            {
            }
        }

        #endregion


        #region StepCommand

        RelayCommand _stepCommand;

        public RelayCommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
                DoStep,
                CanStep
            ));

        private void DoStep()
        {
            TotalUses++;
            //SorterDisplayVm.KeepGoing = false;
            //SorterDisplayVm.MakeStep();
            //if (SorterDisplayVm.CurrentStageVm.StageVmStep == StageVmStep.Right)
            //{
            //   SorterDisplayVm.MakeStep();
            //}
        }

        bool CanStep()
        {
            //if (SorterDisplayVm == null) return false;

            //return (SorterDisplayVm.CurrentStageVm != null) &&
            //       (
            //        (SorterDisplayVm.CurrentStageVm.StageVmStep != StageVmStep.Right) 
            //        ||
            //        (SorterDisplayVm.CurrentStageVm.StageIndex < SorterDisplayVm.StageVms.Count - 1)
            //       );
            return true;
        }

        #endregion // StepCommand


        #region RenderTimer

        Timer _renderTimer;
        public Timer RenderTimer
        {
            get
            {

                return _renderTimer;
            }
            set
            {
                if (_renderTimer != null)
                {
                    _renderTimer.Elapsed -= OnRenderTimerElapsed;
                }
                _renderTimer = value;
                _renderTimer.Elapsed += OnRenderTimerElapsed;
            }
        }

        private void StartTimer(double renderInterval)
        {
            if ((RenderTimer != null) && (RenderTimer.Enabled))
                return;
            RenderTimer = new Timer(renderInterval);
            RenderTimer.Enabled = true;
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
                TotalUses++;
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
            if ((RenderTimer != null) && (RenderTimer.Enabled))
            {
                RenderTimer.Start();
            }
        }

        bool CanStart()
        {
            if (SorterDisplayVm == null) return false;

            return true;
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
            TotalUses = 0;
        }

        bool CanStop()
        {
            if (SorterDisplayVm == null) return false;

            return true;
        }

        #endregion // StopCommand


        #region TotalUses

        public int TotalUses
        {
            get { return (int)GetValue(TotalUsesProperty); }
            set { SetValue(TotalUsesProperty, value); }
        }

        public static readonly DependencyProperty TotalUsesProperty =
            DependencyProperty.Register("TotalUses", typeof(int), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnTotalUsesChanged));

        private static void OnTotalUsesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterRunControl = (SorterRunControl)d;
            var totalUses = (int)e.NewValue;
            if (sorterRunControl.SorterDisplayVm != null)
            {
            }
        }

        #endregion


        #region ResetCommand

        RelayCommand _resetCommand;

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new RelayCommand(
                DoReset,
                CanReset
            ));

        private void DoReset()
        {
            AnimationSpeed = AnimationSpeed.Stopped;
            TotalUses = 0;
            SorterDisplayVm = new SorterDisplayVm(
                    order: SorterDisplayVm.Order,
                    stageVms: new ObservableCollection<StageVm>(
                                SorterDisplayVm.StageVms.Select(stvm => stvm.ClearAll())));

        }

        bool CanReset()
        {
            return (SorterDisplayVm != null);
        }

        #endregion // ResetCommand


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
            DependencyProperty.Register("AnimationSpeed", typeof(AnimationSpeed), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(OnAnimationSpeedPropertyChanged));

        private static void OnAnimationSpeedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterRunControl = (SorterRunControl)d;
            var animationSpeed = (AnimationSpeed)e.NewValue;
            if ((sorterRunControl.RenderTimer != null) && (sorterRunControl.RenderTimer.Enabled))
            {
                sorterRunControl.StopTimer();
            }

            sorterRunControl.StartTimer(0.0);


            if (animationSpeed != AnimationSpeed.Stopped)
            {
                sorterRunControl.StartTimer(sorterRunControl.AnimationSpeed.ToUpdateFrequency());
            }
        }

        #endregion


        #region StageLayout

        public IEnumerable<StageLayout> StageLayouts
        {
            get
            {
                yield return StageLayout.Single;
                yield return StageLayout.Loose;
                yield return StageLayout.Tight;
            }
        }

        public StageLayout StageLayout
        {
            get { return (StageLayout)GetValue(StageLayoutProperty); }
            set { SetValue(StageLayoutProperty, value); }
        }

        public static readonly DependencyProperty StageLayoutProperty =
            DependencyProperty.Register("StageLayout", typeof(StageLayout), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(StageLayout.Single));

        #endregion

    }
}