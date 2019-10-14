using System.Windows.Media;
using System.Globalization;
using System.Windows;
using archean.controls.Utils;

namespace archean.controls.ViewModel.Sorter2
{
    public static class StageVmRender
    {
        public static double RenderWidth(this SortableVmStyle vms, double widthInVm, double stageRenderWidth)
        {
            return (stageRenderWidth * widthInVm) / vms.StageVmWidth();
        }

        public static double RenderXMidStageSection(this SortableVmStyle sortableVmStyle, int sectionDex, double stageRenderWidth)
        {
            var vmX = (sectionDex + 0.5) * (sortableVmStyle.SectionWidth);
            return sortableVmStyle.RenderWidth(vmX, stageRenderWidth);
        }

        public static double RenderYKeyLineCenter(this SortableVmStyle sortableVmStyle, int keyDex, double stageRenderHeight)
        {
            var vmHeight = sortableVmStyle.VPadding +
                          (sortableVmStyle.Order - keyDex) * sortableVmStyle.KeyHeight;

            return RenderUtils.ChildRenderHeight(parentVmHeight: sortableVmStyle.StageVmHeight(),
                                                 childVmHeight: vmHeight,
                                                 parentRenderHeight: stageRenderHeight);
        }

        public static double RenderYKeyLineTop(this StageVmStyle stageVmStyle, int keyDex, double stageRenderHeight)
        {
            var vmHeight = stageVmStyle.SortableVmStyle.VPadding +
                          (stageVmStyle.SortableVmStyle.Order - keyDex) * stageVmStyle.SortableVmStyle.KeyHeight -
                          stageVmStyle.KeyLineThickness / 2;

            return RenderUtils.ChildRenderHeight(parentVmHeight: stageVmStyle.SortableVmStyle.StageVmHeight(),
                                                 childVmHeight: vmHeight,
                                                 parentRenderHeight: stageRenderHeight);
        }

        public static double RenderYKeyLineBottom(this StageVmStyle stageVmStyle, int keyDex, double stageRenderHeight)
        {
            var vmHeight = stageVmStyle.SortableVmStyle.VPadding +
                          (stageVmStyle.SortableVmStyle.Order - keyDex) * stageVmStyle.SortableVmStyle.KeyHeight +
                           stageVmStyle.KeyLineThickness / 2;

            return RenderUtils.ChildRenderHeight(parentVmHeight: stageVmStyle.SortableVmStyle.StageVmHeight(),
                                                 childVmHeight: vmHeight,
                                                 parentRenderHeight: stageRenderHeight);
        }

        public static Pen SwitchLinePen(this StageVmStyle stageVmStyle, KeyPairVm keyPairVm, double stageRenderWidth)
        {
            var brushWidth = stageVmStyle.SortableVmStyle.RenderWidth(stageVmStyle.SwitchLineWidth, stageRenderWidth);
            return new Pen(keyPairVm.Brush, brushWidth);
        }

        public static Pen KeyLinePen(this StageVmStyle stageVmStyle, double stageRenderHeight)
        {
            var brushHeight = 
                RenderUtils.ChildRenderHeight(parentVmHeight: stageVmStyle.SortableVmStyle.StageVmHeight(),
                                              childVmHeight: stageVmStyle.KeyLineThickness,
                                              parentRenderHeight: stageRenderHeight);

            return new Pen(stageVmStyle.KeyLineBrush, brushHeight);
        }

        public static void DrawSwitch(this DrawingContext dc, StageVmStyle stageVmStyle,
                                      int order, KeyPairVm keyPairVm, 
                                      double stageRenderWidth, double stageRenderHeight)
        {
            var pen = stageVmStyle.SwitchLinePen(keyPairVm, stageRenderWidth);
            var renderX = stageVmStyle.SortableVmStyle.RenderXMidStageSection(keyPairVm.StageSection, stageRenderWidth);
            var renderYLow = stageVmStyle.RenderYKeyLineBottom(keyPairVm.LowKey, stageRenderHeight);
            var renderYHigh = stageVmStyle.RenderYKeyLineTop(keyPairVm.HiKey, stageRenderHeight);
            var pointLow = new Point(renderX, renderYLow);
            var pointHigh = new Point(renderX, renderYHigh);
            dc.DrawLine(pen, pointLow, pointHigh);
        }

        public static void DrawKeyLine(this DrawingContext dc, StageVmStyle stageVmStyle, int keyDex,
                                                double stageRenderWidth, double stageRenderHeight)
        {
            var pen = stageVmStyle.KeyLinePen(stageRenderHeight);
            var renderY = stageVmStyle.SortableVmStyle.RenderYKeyLineCenter(keyDex, stageRenderHeight);
            var pointLeft = new Point(0, renderY);
            var pointRight = new Point(stageRenderWidth, renderY);
            dc.DrawLine(pen, pointLeft, pointRight);
        }

        public static void DrawKeyLines(this DrawingContext dc, StageVmStyle stageVmStyle,
                                        double stageRenderWidth, double stageRenderHeight)
        {
            for(var keyDex=0; keyDex < stageVmStyle.SortableVmStyle.Order; keyDex++)
            {
                dc.DrawKeyLine(stageVmStyle, keyDex, stageRenderWidth, stageRenderHeight);
            }
        }

        public static Point GetSortableItemPosition(this SortableVmStyle sortableVmStyle,
                                                     SortableItemVm sortableItemVm,
                                                     double stageRenderWidth, double stageRenderHeight)
        {
            if ((stageRenderHeight <= 0) || (stageRenderHeight <= 0))   { return new Point(0, 0); }
            if (sortableItemVm.StagePos == StagePos.Missing)  { return new Point(0, 0); }

            var renderY = sortableVmStyle.RenderYKeyLineCenter(sortableItemVm.KeyLinePos, stageRenderHeight);

            var radius =
                    RenderUtils.ChildRenderHeight(
                        parentVmHeight: sortableVmStyle.StageVmHeight(),
                        childVmHeight: sortableVmStyle.Radius,
                        parentRenderHeight: stageRenderHeight);

            var renderX = 0.0;
            switch (sortableItemVm.StagePos)
            {
                case StagePos.Left:
                    renderX = radius * -1.2;
                    break;
                case StagePos.Center:
                    renderX = sortableVmStyle
                                .RenderXMidStageSection(
                                sortableItemVm.StageSection, stageRenderWidth);
                    break;
                case StagePos.Right:
                    renderX = stageRenderWidth - radius * 1.2;
                    break;
                default:
                    break;
            }

            return new Point(renderX, renderY);
        }

        static Pen _sortableBorderPen;
        static Pen SortableBorderPen
        {
            get
            {
                return _sortableBorderPen ?? (_sortableBorderPen = new Pen(Brushes.Black, 1.0));
            }
        }

        public static void DrawSortableValue(this DrawingContext dc, 
                                                  SortableVmStyle sortableVmStyle,
                                                  SortableItemVm sortableItemVm,
                                                  double stageRenderWidth, 
                                                  double stageRenderHeight)
        {
            if ((stageRenderHeight <= 0) || (stageRenderHeight <= 0)) { return; }
            if (sortableItemVm.StagePos == StagePos.Missing)  { return; }

            var radius =  RenderUtils.ChildRenderHeight(
                                              parentVmHeight: sortableVmStyle.StageVmHeight(),
                                              childVmHeight: sortableVmStyle.Radius,
                                              parentRenderHeight: stageRenderHeight);

            var sortableItemPosition = sortableVmStyle.GetSortableItemPosition(
                                                sortableItemVm:sortableItemVm,
                                                stageRenderWidth:stageRenderWidth,
                                                stageRenderHeight:stageRenderHeight);

            dc.DrawEllipse(sortableItemVm.BackgroundBrush, SortableBorderPen, sortableItemPosition, radius, radius);

            if(sortableItemVm.ShowLabel)
            {
                var txt = new FormattedText(sortableItemVm.Label.ToString(),
                                            CultureInfo.CurrentCulture,
                                            FlowDirection.LeftToRight,
                                            SortableItemVm.Typeface,
                                            radius, sortableItemVm.ForegroundBrush, 1.0);

                var textUpLeft = new Point(
                                    x: sortableItemPosition.X - txt.Width / 2, 
                                    y: sortableItemPosition.Y - txt.Height / 2);

                dc.DrawText(txt, textUpLeft);
            }
        }

        public static void DrawSortableValues(this DrawingContext dc, SortableVm sortableVm,  
                                                   double stageRenderWidth, double stageRenderHeight)
        {
            if (sortableVm?.CurrentSortableItemVms == null) return;

            foreach (var sortableItemVm in sortableVm.CurrentSortableItemVms)
            {
                dc.DrawSortableValue(sortableVm.SortableVmStyle, sortableItemVm, stageRenderWidth, stageRenderHeight);
            }
        }

        public static void DrawSortableValueAnimate(
                this DrawingContext dc,
                double animationPct,
                SortableVmStyle sortableVmStyle,
                SortableItemVm sortableItemVmOld,
                SortableItemVm sortableItemVm,
                double stageRenderWidth,
                double stageRenderHeight)
        {
            if ((stageRenderHeight <= 0) || (stageRenderHeight <= 0))
            {
                return;
            }
            if (sortableItemVm.StagePos == StagePos.Missing)
            {
                return;
            }
            var radius =
                RenderUtils.ChildRenderHeight(parentVmHeight: sortableVmStyle.StageVmHeight(),
                                              childVmHeight: sortableVmStyle.Radius,
                                              parentRenderHeight: stageRenderHeight);

            var centerO = sortableVmStyle.GetSortableItemPosition(sortableItemVmOld, stageRenderWidth, stageRenderHeight);
            var centerN = sortableVmStyle.GetSortableItemPosition(sortableItemVm, stageRenderWidth, stageRenderHeight);

            var center = centerN.Interpolate(centerO, animationPct);

            dc.DrawEllipse(sortableItemVm.BackgroundBrush, null, center, radius, radius);

            if (sortableItemVm.ShowLabel)
            {
                var txt = new FormattedText(sortableItemVm.Label.ToString(),
                                               CultureInfo.CurrentCulture,
                                               FlowDirection.LeftToRight,
                                               SortableItemVm.Typeface,
                                               radius, sortableItemVm.ForegroundBrush, 1.0);

                var upLeft = new Point(center.X - txt.Width / 2, center.Y - txt.Height / 2);
                dc.DrawText(txt, upLeft);
            }
        }

        public static void DrawSortableValuesAnimate(
            this DrawingContext dc, 
            SortableVm sortableVm,
            double stageRenderWidth,
            double stageRenderHeight)
        {
            if (sortableVm?.CurrentSortableItemVms == null) return;

            if (sortableVm?.PastSortableItemVms == null)
            {
                return;
            }


            for (var i = 0; i < sortableVm.CurrentSortableItemVms.Length; i++)
            {
                dc.DrawSortableValueAnimate(
                    animationPct: sortableVm.AnimationPct,
                    sortableVmStyle: sortableVm.SortableVmStyle,
                    sortableItemVmOld: sortableVm.PastSortableItemVms[i],
                    sortableItemVm: sortableVm.CurrentSortableItemVms[i],
                    stageRenderWidth: stageRenderWidth,
                    stageRenderHeight: stageRenderHeight);
            }
        }

    }

}
