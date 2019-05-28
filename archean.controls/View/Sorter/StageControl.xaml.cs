using archean.controls.ViewModel.Sorter;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace archean.controls.View.Sorter
{
    public partial class StageControl : UserControl
    {
        readonly Timer renderTimer = null;
        private const double DEFAULT_INTERVAL = 10;
        private const double DEFAULT_TICS_PER_STEP = 15;

        public StageControl()
        {
            InitializeComponent();
            renderTimer = new Timer(DEFAULT_INTERVAL);
            renderTimer.Elapsed += OnRenderTimerElapsed;
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


        private void StartTimer()
        {
            if ((renderTimer == null) || (renderTimer.Enabled))
                return;
            renderTimer.Enabled = true;
        }

        /// <summary>
        /// Stop the Tick Control rotation
        /// </summary>
        private void StopTimer()
        {
            if (renderTimer != null)
            {
                renderTimer.Enabled = false;
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
            dc.DrawRectangle(StageVm.BackgroundBrush, null, new Rect(0.0, 0.0, ActualWidth, ActualHeight));

            dc.DrawKeyLines(StageVm, ActualWidth, ActualHeight);

            foreach(var kvm in StageVm.KeyPairVms)
            {
                dc.DrawSwitch(StageVm, kvm, ActualWidth, ActualHeight);
            }

            if(renderTimer.Enabled)
            {
                StageVm.DrawSortableValues2(StageVmOld, (ticks / TicsPerStep), dc, ActualWidth, ActualHeight);
            }
            else
            {
                StageVm.DrawSortableValues(dc, ActualWidth, ActualHeight);
            }
        }

        public StageVm StageVmOld;
        public double TicsPerStep = DEFAULT_TICS_PER_STEP;

        #region StageVm

        [Category("Custom Properties")]
        public StageVm StageVm
        {
            get { return (StageVm)GetValue(StageVmProperty); }
            set { SetValue(StageVmProperty, value); }
        }

        public static readonly DependencyProperty StageVmProperty =
            DependencyProperty.Register("StageVm", typeof(StageVm), typeof(StageControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnStageVmPropertyChanged));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stageControl = (StageControl)d;
            stageControl.StageVmOld = (StageVm)e.OldValue;
            stageControl.TicsPerStep = (stageControl.StageVmOld == null) ? 0: DEFAULT_TICS_PER_STEP;
            if ((stageControl.StageVm.SortableItemVms != null ) && 
                (stageControl.StageVmOld != null) &&
                (stageControl.StageVmOld.SortableItemVms != null))
            {
                    stageControl.StartTimer();
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
            if (renderTimer != null)
            {
                renderTimer.Elapsed -= OnRenderTimerElapsed;
                renderTimer.Dispose();
            }		
        }

    }
}
