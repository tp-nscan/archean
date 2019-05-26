using System.Windows.Media;
using System.Globalization;
using System.Windows;

namespace archean.controls.ViewModel.Sorter
{
    public static class StageVmRender
    {
        public static double RenderWidth(this StageVm vm, double widthInVm, double stageRenderWidth)
        {
            return (stageRenderWidth * widthInVm) / vm.VmWidth;
        }

        public static double RenderHeight(this StageVm vm, double heightInVm, double stageRenderHeight)
        {
            return (stageRenderHeight * heightInVm) / vm.VmHeight;
        }

        public static double KeyRenderYc(this StageVm vm, int keyDex, double stageRenderHeight)
        {
            var vmHeight = vm.VPadding + (vm.KeyCount - keyDex) * (vm.KeyLineSpacing + vm.KeyLineThickness);
            return vm.RenderHeight(vmHeight, stageRenderHeight);
        }

        public static double KeyRenderYh(this StageVm vm, int keyDex, double stageRenderHeight)
        {
            var vmHeight = vm.VPadding + (vm.KeyCount - keyDex) * (vm.KeyLineSpacing + vm.KeyLineThickness) - vm.KeyLineThickness / 2;
            return vm.RenderHeight(vmHeight, stageRenderHeight);
        }

        public static double KeyRenderYl(this StageVm vm, int keyDex, double stageRenderHeight)
        {
            var vmHeight = vm.VPadding + (vm.KeyCount - keyDex) * (vm.KeyLineSpacing + vm.KeyLineThickness) + vm.KeyLineThickness/2;
            return vm.RenderHeight(vmHeight, stageRenderHeight);
        }

        public static double SectionRenderX(this StageVm vm, int sectionDex, double stageRenderWidth)
        {
        //    var vmX =  vm.HPadding + (sectionDex + 0.5 ) * (vm.SwitchLineWidth + vm.SwitchSpacing);
            var vmX = vm.HPadding + (sectionDex + 0.5) * (vm.SwitchSpacing);
            return vm.RenderWidth(vmX, stageRenderWidth);
        }

        public static Pen SwitchLinePen(this StageVm stageVm, KeyPairVm keyPairVm, double stageRenderWidth)
        {
            var brushWidth = stageVm.RenderWidth(stageVm.SwitchLineWidth, stageRenderWidth);
            return new Pen(keyPairVm.Brush, brushWidth);
        }

        public static Pen KeyLinePen(this StageVm stageVm, double stageRenderHeight)
        {
            var brushHeight = stageVm.RenderHeight(stageVm.KeyLineThickness, stageRenderHeight);
            return new Pen(stageVm.KeyLineBrush, brushHeight);
        }

        public static void DrawSwitch(this DrawingContext dc, StageVm stageVm, KeyPairVm keyPairVm, 
                                        double stageRenderWidth, double stageRenderHeight)
        {
            var pen = stageVm.SwitchLinePen(keyPairVm, stageRenderWidth);
            var renderX = stageVm.SectionRenderX(keyPairVm.StageSection, stageRenderWidth);
            var renderYLow = stageVm.KeyRenderYl(keyPairVm.LowKey, stageRenderHeight);
            var renderYHigh = stageVm.KeyRenderYh(keyPairVm.HiKey, stageRenderHeight);
            var pointLow = new Point(renderX, renderYLow);
            var pointHigh = new Point(renderX, renderYHigh);
            dc.DrawLine(pen, pointLow, pointHigh);
        }

        public static void DrawKeyLine(this DrawingContext dc, StageVm stageVm, int keyDex,
                                                double stageRenderWidth, double stageRenderHeight)
        {
            var pen = stageVm.KeyLinePen(stageRenderHeight);
            var renderY = stageVm.KeyRenderYc(keyDex, stageRenderHeight);
            var pointLeft = new Point(0, renderY);
            var pointRight = new Point(stageRenderWidth, renderY);
            dc.DrawLine(pen, pointLeft, pointRight);
        }

        public static void DrawKeyLines(this DrawingContext dc, StageVm stageVm, 
                                        double stageRenderWidth, double stageRenderHeight)
        {
            for(var keyDex=0; keyDex < stageVm.KeyCount; keyDex++)
            {
                dc.DrawKeyLine(stageVm, keyDex, stageRenderWidth, stageRenderHeight);
            }
        }

        public static Point GetSortablePosition(this StageVm stageVm,
                                                     SortableItemVm sortableVm,
                                                     double stageRenderWidth, double stageRenderHeight)
        {
            if ((stageRenderHeight <= 0) || (stageRenderHeight <= 0))
            {
                return new Point(0, 0);
            }
            if (sortableVm.StagePos == StagePos.Missing)
            {
                return new Point(0, 0);
            }

            var renderY = stageVm.KeyRenderYc(sortableVm.KeyLinePos, stageRenderHeight);
            var radius = stageVm.RenderHeight(stageVm.KeyLineThickness * 1.5, stageRenderHeight);

            var renderX = 0.0;
            switch (sortableVm.StagePos)
            {
                case StagePos.Left:
                    renderX = radius * -1.0;
                    break;
                case StagePos.Center:
                    renderX = stageVm.SectionRenderX(sortableVm.StageSection, stageRenderWidth);
                    break;
                case StagePos.Right:
                    renderX = stageRenderWidth - radius * 1.2;
                    break;
                default:
                    break;
            }
            return new Point(renderX, renderY);

        }


        public static void DrawSortableValue(this StageVm stageVm, DrawingContext dc,
                                                  SortableItemVm sortableVm,
                                                  double stageRenderWidth, double stageRenderHeight)
        {
            if ((stageRenderHeight <= 0) || (stageRenderHeight <= 0))
            {
                return;
            }
            if (sortableVm.StagePos == StagePos.Missing)
            {
                return;
            }

            var radius = stageVm.RenderHeight(stageVm.KeyLineThickness * 1.5, stageRenderHeight);

            var center = stageVm.GetSortablePosition(sortableVm, stageRenderWidth, stageRenderHeight);
            dc.DrawEllipse(sortableVm.BackgroundBrush, null, center, radius, radius);

            if(sortableVm.ShowLabel)
            {
                var txt = new FormattedText(sortableVm.Label.ToString(),
                                               CultureInfo.CurrentCulture,
                                               FlowDirection.LeftToRight,
                                               SortableItemVm.Typeface,
                                               radius, sortableVm.ForegroundBrush, 1.0);

                var upLeft = new Point(center.X - txt.Width / 2, center.Y - txt.Height / 2);
                dc.DrawText(txt, upLeft);
            }
        }


        public static void DrawSortableValues(this StageVm stageVm, DrawingContext dc, 
                                                   double stageRenderWidth, double stageRenderHeight)
        {
            if (stageVm.SortableVms == null) return;

            foreach (var sortableVm in stageVm.SortableVms)
            {
                stageVm.DrawSortableValue(dc, sortableVm, stageRenderWidth, stageRenderHeight);
            }
        }


    }

}
