using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sorting.KeyPairs;

namespace SorterControls.View.SorterOld
{
    public class SwitchVisual : Canvas
    {
        public SwitchVisual()
        {
            SizeChanged += (s, e) => DrawVisual();
        }

        private DrawingVisual _switchVisual;

        double HalfLineSpacing
        {
            get { return (0.5 * ActualHeight) / KeyCount; }
        }

        double ActualLineThickness
        {
            get { return ActualHeight * LineThickness / KeyCount; }
        }

        double ActualSwitchThickness
        {
            get { return ActualWidth * SwitchThickness; }
        }

        void DrawVisual()
        {
            if (KeyPair == null)
            {
                return;
            }

            if (_keyLines.Count == 0)
            {
                return;
            }

            DrawKeyLines();
            DrawSwitch();
        }

        void SetupResources()
        {
            if (KeyPair == null) { return; }

            _switchVisual = new DrawingVisual();

            for (var i = 0; i < KeyCount; i++)
            {
                var klvCur = new DrawingVisual();
                _keyLines.Add(klvCur);
                AddVisualChild(klvCur);
                AddLogicalChild(klvCur);
            }
        }

        void DrawKeyLines()
        {
            for (var keyDex = 0; keyDex < KeyCount; keyDex++)
            {
                using (var dc = _keyLines[keyDex].RenderOpen())
                {
                    dc.DrawGeometry(LineBrushes[keyDex], null, CreateKeyLineGeometry(keyDex));
                }
            }
        }

        void DrawSwitch()
        {
            using (var dc = _switchVisual.RenderOpen())
            {
                dc.DrawGeometry(SwitchBrush, null, CreateSwitchGeometry());
            }
        }

        private StreamGeometry CreateKeyLineGeometry(int keyDex)
        {
            var geometry = new StreamGeometry();

            using (var ctx = geometry.Open())
            {
                var firstPoint = true;

                //System.Diagnostics.Debug.WriteLine("Begin");
                foreach (var pt in KeyLinePoints(keyDex))
                {
                    if (firstPoint)
                    {
                        ctx.BeginFigure(pt, true, false);
                        firstPoint = false;
                    }
                    else
                    {
                        ctx.LineTo(pt, true, false);
                    }
                    //System.Diagnostics.Debug.WriteLine(pt.X.ToString("0.00") + ", " + pt.Y.ToString("0.00"));
                }
                //System.Diagnostics.Debug.WriteLine("End");
            }

            return geometry;
        }

        private StreamGeometry CreateSwitchGeometry()
        {
            var geometry = new StreamGeometry();

            using (var ctx = geometry.Open())
            {
                var firstPoint = true;

                foreach (var pt in SwitchPoints)
                {
                    if (firstPoint)
                    {
                        ctx.BeginFigure(pt, true, false);
                        firstPoint = false;
                    }
                    else
                    {
                        ctx.LineTo(pt, true, false);
                    }
                }
            }

            return geometry;
        }

        IEnumerable<Point> KeyLinePoints(int keyLineDex)
        {
            var lineHeight = HalfLineSpacing + ActualHeight * keyLineDex / KeyCount;

            yield return new Point(-1, lineHeight - ActualLineThickness);
            yield return new Point(ActualWidth, lineHeight - ActualLineThickness);
            yield return new Point(ActualWidth, lineHeight + ActualLineThickness);
            yield return new Point(-1, lineHeight + ActualLineThickness);
        }

        IEnumerable<Point> SwitchPoints
        {
            get
            {
                var topLineHeight = HalfLineSpacing + ActualHeight * KeyPair.HiKey / KeyCount;
                var bottomLineHeight = HalfLineSpacing + ActualHeight * KeyPair.LowKey / KeyCount;

                yield return new Point(ActualWidth / 2 - ActualSwitchThickness, topLineHeight - ActualLineThickness);
                yield return new Point(ActualWidth / 2 + ActualSwitchThickness, topLineHeight - ActualLineThickness);
                yield return new Point(ActualWidth / 2 + ActualSwitchThickness, bottomLineHeight + ActualLineThickness);
                yield return new Point(ActualWidth / 2 - ActualSwitchThickness, bottomLineHeight + ActualLineThickness);
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return (KeyPair == null) ? 0 : KeyCount + 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return index < _keyLines.Count ? _keyLines[index] : _switchVisual;
        }

        readonly List<DrawingVisual> _keyLines = new List<DrawingVisual>();

        #region KeyPair

        [Category("Custom Properties")]
        public IKeyPair KeyPair
        {
            get { return (IKeyPair)GetValue(KeyPairProperty); }
            set { SetValue(KeyPairProperty, value); }
        }

        public static readonly DependencyProperty KeyPairProperty =
            DependencyProperty.Register("KeyPair", typeof(IKeyPair), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnKeyPairPropertyChanged));

        private static void OnKeyPairPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;

            if (!PropertiesAreSet(switchVisual))
            {
                return;
            }

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

        #region KeyCount

        private const int DefaultKeyCount = -1;

        [Category("Custom Properties")]
        public int KeyCount
        {
            get { return (int)GetValue(KeyCountProperty); }
            set { SetValue(KeyCountProperty, value); }
        }

        public static readonly DependencyProperty KeyCountProperty =
            DependencyProperty.Register("KeyCount", typeof(int), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(DefaultKeyCount, FrameworkPropertyMetadataOptions.AffectsRender, OnKeyCountPropertyChanged));

        private static void OnKeyCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;

            if (!PropertiesAreSet(switchVisual))
            {
                return;
            }

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

        #region LineBrushes

        private static readonly List<Brush> defaultLineBrushes = new List<Brush>();

        [Category("Custom Properties")]
        public List<Brush> LineBrushes
        {
            get { return (List<Brush>)GetValue(LineBrushesProperty); }
            set { SetValue(LineBrushesProperty, value); }
        }

        public static readonly DependencyProperty LineBrushesProperty =
            DependencyProperty.Register("LineBrushes", typeof(List<Brush>), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(defaultLineBrushes, FrameworkPropertyMetadataOptions.AffectsRender, OnLineBrushesChanged));

        private static void OnLineBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;

            if (!PropertiesAreSet(switchVisual))
            {
                return;
            }

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

        #region SwitchBrush

        [Category("Custom Properties")]
        public Brush SwitchBrush
        {
            get { return (Brush)GetValue(SwitchBrushProperty); }
            set { SetValue(SwitchBrushProperty, value); }
        }

        public static readonly DependencyProperty SwitchBrushProperty =
            DependencyProperty.Register("SwitchBrush", typeof(Brush), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSwitchBrushChanged));

        private static void OnSwitchBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;

            if (!PropertiesAreSet(switchVisual))
            {
                return;
            }

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

        #region LineThickness

        [Category("Custom Properties")]
        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnLineThicknessChanged));

        private static void OnLineThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;

            if (!PropertiesAreSet(switchVisual))
            {
                return;
            }

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

        #region SwitchThickness

        [Category("Custom Properties")]
        public double SwitchThickness
        {
            get { return (double)GetValue(SwitchThicknessProperty); }
            set { SetValue(SwitchThicknessProperty, value); }
        }

        public static readonly DependencyProperty SwitchThicknessProperty =
            DependencyProperty.Register("SwitchThickness", typeof(double), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnSwitchThicknessChanged));

        private static void OnSwitchThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;

            if (!PropertiesAreSet(switchVisual))
            {
                return;
            }

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

        static bool PropertiesAreSet(SwitchVisual switchVisual)
        {
            if (switchVisual.KeyCount == DefaultKeyCount) return false;
            if (switchVisual.KeyPair == null) return false;
            if (switchVisual.LineBrushes.Count == 0) return false;
            if (switchVisual.SwitchThickness < 0.001) return false;
            if (switchVisual.LineThickness < 0.001) return false;
            if (switchVisual.SwitchBrush == null) return false;
            return true;
        }
    }
}
