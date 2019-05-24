using archean.controls.ViewModel.Sorter;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace archean.controls.View.Sorter
{
    public partial class StageControl : UserControl
    {
        public StageControl()
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


        protected override void OnRender(DrawingContext dc)
        {
            if (StageVm == null)
            {
                return;
            }
            var aw = ActualWidth;
            dc.DrawRectangle(StageVm.BackgroundBrush, null, new Rect(0.0, 0.0, ActualWidth, ActualHeight));

            dc.DrawKeyLines(StageVm, aw, ActualHeight);

            foreach(var kvm in StageVm.KeyPairVms)
            {
                dc.DrawSwitch(StageVm, kvm, aw, ActualHeight);
            }
            dc.DrawSortableValues(StageVm, aw, ActualHeight);
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
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnStageVmPropertyChanged));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stageControl = (StageControl)d;
           // stageControl.Width = stageControl.ActualHeight * stageControl.StageVm.WidthToHeight;
            stageControl.InvalidateVisual();
        }

        #endregion
    }
}
