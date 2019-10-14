using archean.controls.ViewModel.Sorter2;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace archean.controls.View.Sorter2
{
    public partial class StageControl : UserControl
    {
        private const double DEFAULT_INTERVAL = 10;
        private const double DEFAULT_TICS_PER_STEP = 5;

        public StageControl()
        {
            InitializeComponent();
            SizeChanged += StageControl_SizeChanged;
        }

        void StageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (StageVm == null) return;
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                {
                    Width = ActualHeight * StageVm.WidthToHeight();
                    InvalidateVisual();
                }));
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (StageVm == null) { return; }

            Width = ActualHeight * StageVm.WidthToHeight();

            dc.DrawRectangle(StageVm.StageVmStyle.BackgroundBrush, null, new Rect(0.0, 0.0, ActualWidth, ActualHeight));

            dc.DrawKeyLines(StageVm.StageVmStyle, ActualWidth, ActualHeight);

            foreach(var kvm in StageVm.KeyPairVms)
            {
                dc.DrawSwitch(StageVm.StageVmStyle, StageVm.Order, kvm, ActualWidth, ActualHeight);
            }

            dc.DrawSortableValuesAnimate(StageVm.SortableVm, ActualWidth, ActualHeight);
        }


        #region StageVm

        [Category("Custom Properties")]
        public StageVm StageVm
        {
            get { return (StageVm)GetValue(StageVmProperty); }
            set { SetValue(StageVmProperty, value); }
        }

        public static readonly DependencyProperty StageVmProperty =
            DependencyProperty.Register("StageVm", typeof(StageVm), typeof(StageControl),
            new FrameworkPropertyMetadata(OnStageVmPropertyChanged));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stageControl = (StageControl)d;
            stageControl.InvalidateVisual();
        }

        #endregion



    }
}
