namespace archean.controls.ViewModel.Sorter2
{
    public class SortableVmStyle
    {
        public double RightMargin { get; set; }
        public double VPadding { get; set; }
        public double KeyHeight { get; set; }
        public int Order { get; set; }
        public double Radius { get; set; }
        public double SectionWidth { get; set; }
        public int SectionCount { get; set; }

        public static SortableVmStyle Standard(int order, int sectionCount)
        {
            return new SortableVmStyle
            {
                KeyHeight = 4.0,
                Order = order,
                Radius = 1.5,
                RightMargin = 3.25,
                SectionWidth = 3.25,
                SectionCount = sectionCount,
                VPadding = 1.0
            };
        }
    }

    public static class SortableVmStyleExt
    {
        public static double StageVmHeight(this SortableVmStyle sortableVmStyle)
        {
            return 2 * sortableVmStyle.VPadding +
                       sortableVmStyle.KeyHeight * sortableVmStyle.Order +
                       sortableVmStyle.KeyHeight;
        }

        public static double StageVmWidth(this SortableVmStyle sortableVmStyle)
        {
            return 2 * sortableVmStyle.RightMargin +
                       sortableVmStyle.SectionWidth * sortableVmStyle.SectionCount;
        }

        public static double StageWidthToHeight(this SortableVmStyle sortableVmStyle)
        {
            var h = sortableVmStyle.StageVmHeight();
            return (h > 0) ? sortableVmStyle.StageVmWidth() / h : 0.0;
        }

        public static SortableVmStyle ChangeSectionCount(this SortableVmStyle sortableVmStyle, int sectionCount)
        {
            if(sortableVmStyle == null)
            {
                return null;
            }
            return new SortableVmStyle
            {
                KeyHeight = sortableVmStyle.KeyHeight,
                Order = sortableVmStyle.Order,
                Radius = sortableVmStyle.Radius,
                RightMargin = sortableVmStyle.RightMargin,
                SectionWidth = sortableVmStyle.SectionWidth,
                SectionCount = sectionCount,
                VPadding = sortableVmStyle.VPadding
            };
        }
    }

}
