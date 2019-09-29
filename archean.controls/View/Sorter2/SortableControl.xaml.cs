using archean.controls.ViewModel.Sorter2;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace archean.controls.View.Sorter2
{
    public partial class SortableControl : UserControl
    {
        public SortableControl()
        {
            InitializeComponent();

        }

        void StageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (SortableVm == null) return;
            //Dispatcher.BeginInvoke(
            //    DispatcherPriority.Normal,
            //    new Action(() =>
            //    {
            //        Width = ActualHeight * SortableVm.WidthToHeight;
            //        InvalidateVisual();
            //    }));
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (SortableVm == null)
            {
                return;
            }

           // Width = ActualHeight * SortableVm.WidthToHeight;

            //dc.DrawRectangle(StageVm.StageVmStyle.BackgroundBrush, null, new Rect(0.0, 0.0, ActualWidth, ActualHeight));

            //dc.DrawKeyLines(StageVm, ActualWidth, ActualHeight);

            //foreach (var kvm in StageVm.KeyPairVms)
            //{
            //    dc.DrawSwitch(StageVm, kvm, ActualWidth, ActualHeight);
            //}

            //StageVm.DrawSortableValues(dc, ActualWidth, ActualHeight);
        }


        #region SortableVm

        [Category("Custom Properties")]
        public SortableVm SortableVm
        {
            get { return (SortableVm)GetValue(StageVmProperty); }
            set { SetValue(StageVmProperty, value); }
        }

        public static readonly DependencyProperty StageVmProperty =
            DependencyProperty.Register("SortableVm", typeof(SortableVm), typeof(SortableControl),
            new FrameworkPropertyMetadata(OnStageVmPropertyChanged));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sortableControl = (SortableControl)d;
            sortableControl.InvalidateVisual();
        }

        #endregion

    }
}
