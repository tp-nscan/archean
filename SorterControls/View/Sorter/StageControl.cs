using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SorterControls.ViewModel.Sorter;

namespace SorterControls.View.Sorter
{
    public class StageControl : Control
    {
        public StageControl()
        {
            SizeChanged += StageControl_SizeChanged;    
        }

        void StageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _stageWidth = null;
            _switchidth = null;
            _lineThickness = null;
            _ballRadius = null;
            _keyHeight = null;
            _stageWidth = null;
            _stageWidth = null;
            _stageWidth = null;

            Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action(() =>
            {
                Width = StageWidth.Value;
                InvalidateVisual();
            }));

        }

        protected override void OnRender(DrawingContext dc)
        {
            if (StageVm == null)
            {
                return;
            }
            var pB = new Pen(StageVm.BackgroundBrush, Width);
            dc.DrawLine(pB, new Point(Width / 2, 0), new Point(Width / 2, Height));

            for (var i = 0; i < StageVm.KeyCount; i++)
            {
                var pL = new Pen(StageVm.LineBrush, LineThickness.Value);
                dc.DrawLine(pL, new Point(0, KeyHeight.Value * (i + 0.5)), new Point(Width, KeyHeight.Value * (i + 0.5)));
            }

            foreach (var keyPairVm in StageVm.KeyPairVms)
            {
                var pL = new Pen(keyPairVm.SwitchBrush, LineThickness.Value);
                dc.DrawLine(pL, SwitchBottom(keyPairVm.KeyPair.HiKey, keyPairVm.Position),
                                SwitchTop(keyPairVm.KeyPair.LowKey, keyPairVm.Position));

                dc.DrawEllipse(keyPairVm.SwitchBrush, null, SwitchBottom(keyPairVm.KeyPair.HiKey, keyPairVm.Position), BallRadius.Value, BallRadius.Value);

                dc.DrawEllipse(keyPairVm.SwitchBrush, null, SwitchTop(keyPairVm.KeyPair.LowKey, keyPairVm.Position), BallRadius.Value, BallRadius.Value);
            }

        }

        private double? _stageWidth;
        double? StageWidth
        {
            get
            {
                return _stageWidth ??
                    (
                        _stageWidth =
                            (StageVm.KeyPairVms.Max(vm => vm.Position) + 4)
                            * SwitchWidth
                    );
            }
        }

        private double? _switchidth;
        double? SwitchWidth
        {
            get
            {
                return _switchidth ??
                    (
                        _switchidth = KeyHeight * StageVm.SwitchWidth
                    );
            }
        }

        private double? _lineThickness;
        double? LineThickness
        {
            get
            {
                return _lineThickness ??
                    (
                        _lineThickness =
                            Height * StageVm.LineThickness /
                            StageVm.KeyCount
                    );
            }
        }

        private double? _ballRadius;
        double? BallRadius
        {
            get
            {
                return _ballRadius ??
                    (
                        _ballRadius =
                            LineThickness.Value * 2.0
                    );
            }
        }

        private double? _keyHeight;
        double? KeyHeight
        {
            get
            {
                return _keyHeight ??
                    (
                        _keyHeight = Height / StageVm.KeyCount
                    );
            }
        }

        Point SwitchBottom(int key, int position)
        {
            return new Point
                (
                    x: (position + 2.0) * SwitchWidth.Value,
                    y: (key + 0.5) * KeyHeight.Value
                );
        }

        Point SwitchTop(int key, int position)
        {
            return new Point
                (
                    x: (position + 2.0) * SwitchWidth.Value,
                    y: (key + 0.5) * KeyHeight.Value
                );
        }

        #region StageVm

        [Category("Custom Properties")]
        public IStageVm StageVm
        {
            get { return (IStageVm)GetValue(StageVmProperty); }
            set { SetValue(StageVmProperty, value); }
        }

        public static readonly DependencyProperty StageVmProperty =
            DependencyProperty.Register("StageVm", typeof(IStageVm), typeof(StageControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnStageVmPropertyChanged));

        private static void OnStageVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterControl = (StageControl)d;
            sorterControl.Width = sorterControl.StageWidth.Value;
            sorterControl.InvalidateVisual();
        }

        #endregion
    }
}
