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
        }

        void StageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action(() =>
                {
                    InvalidateVisual();
                }));
        }



        protected override void OnRender(DrawingContext dc)
        {
            if (StageVm == null)
            {
                return;
            }

            dc.DrawKeyLines(StageVm, ActualWidth, ActualHeight);

            foreach(var kvm in StageVm.KeyPairVms)
            {
                dc.DrawSwitch(StageVm, kvm, ActualWidth, ActualHeight);
            }
            dc.DrawSortableValues(StageVm, ActualWidth, ActualHeight);
            //Point center = new Point(ActualWidth/2, ActualHeight/2);
            //Typeface typeface = new Typeface("Segoe UI");
            //double em_size = 140;
            //dc.DrawText(new FormattedText("yark", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, em_size, Brushes.Aqua, 1.0), center);
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

        //public static readonly DependencyProperty StageVmProperty =
        //    DependencyProperty.Register("StageVm", typeof(StageVm), typeof(StageControl));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterControl = (StageControl)d;
            //sorterControl.Width = sorterControl.StageWidth.Value;
            sorterControl.InvalidateVisual();
        }

        #endregion
    }
}
