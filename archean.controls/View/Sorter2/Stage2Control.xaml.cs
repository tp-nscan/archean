using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace archean.controls.View.Sorter2
{
    public partial class Stage2Control : UserControl
    {
        private const double DEFAULT_INTERVAL = 10;
        private const double DEFAULT_TICS_PER_STEP = 5;

        public Stage2Control()
        {
            InitializeComponent();
            SizeChanged += StageControl_SizeChanged;
        }

        void StageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                {
                    Width = ActualHeight * StageVm.WidthToHeight;
                    InvalidateVisual();
                }));
        }

        Timer _renderTimer;
        public Timer RenderTimer
        {
            get { return _renderTimer; }
            set
            {
                if(_renderTimer != null)
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
                InvalidateVisual();

                if(ticks == TicsPerStep)
                {
                    ticks = 0;
                    StopTimer();
                    StageVm.RaiseAnimationFinished();
                }
                else
                {
                    ticks++;
                }
            });
        }



        protected override void OnRender(DrawingContext dc)
        {
            if (StageVm == null)
            {
                return;
            }
            dc.DrawRectangle(StageVm.StageVmStyle.BackgroundBrush, null, new Rect(0.0, 0.0, ActualWidth, ActualHeight));

            dc.DrawKeyLines(StageVm, ActualWidth, ActualHeight);

            foreach(var kvm in StageVm.KeyPairVms)
            {
                dc.DrawSwitch(StageVm, kvm, ActualWidth, ActualHeight);
            }

            if((RenderTimer != null) && (RenderTimer.Enabled))
            {
                StageVm.DrawSortableValuesAnimate(StageVmOld, (ticks / TicsPerStep), dc, ActualWidth, ActualHeight);
            }
            else
            {
                StageVm.DrawSortableValues(dc, ActualWidth, ActualHeight);
            }
        }

        public StageVm StageVmOld;
        public double TicsPerStep = DEFAULT_TICS_PER_STEP;
        public double RenderInterval = DEFAULT_TICS_PER_STEP;


        #region StageVm

        [Category("Custom Properties")]
        public StageVm StageVm
        {
            get { return (StageVm)GetValue(StageVmProperty); }
            set { SetValue(StageVmProperty, value); }
        }

        public static readonly DependencyProperty StageVmProperty =
            DependencyProperty.Register("StageVm", typeof(StageVm), typeof(Stage2Control),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnStageVmPropertyChanged));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stageControl = (Stage2Control)d;
            stageControl.StageVmOld = (StageVm)e.OldValue;

            if ((stageControl.StageVm.SortableItemVms != null ) &&
                (stageControl.StageVmOld != null) &&
                (stageControl.StageVmOld.SortableItemVms != null) &&
                (stageControl.StageVm.StageVmStyle.AnimationSpeed != AnimationSpeed.None))
            {
                stageControl.StartTimer(stageControl.StageVm.StageVmStyle.AnimationSpeed.ToUpdateFrequency());
                stageControl.TicsPerStep = stageControl.StageVm.StageVmStyle.AnimationSpeed.ToUpdateSteps();
            }
            else
            {
                stageControl.InvalidateVisual();
            }
        }

        #endregion


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

    }
}
