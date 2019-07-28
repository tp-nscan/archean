using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace archean.controls.View.Sorter2
{
    public partial class SorterGenControl
    {
        public SorterGenControl()
        {
            InitializeComponent();

            var rs = typeof(core.SortersFromData.RefSorter);
            var ss2 = rs.GetProperties()
                .Where(p => p.PropertyType == typeof(core.SortersFromData.RefSorter))
                .Select(p => (core.SortersFromData.RefSorter)p.GetMethod.Invoke(null, null))
                .ToList();

        }


        #region RefSorters

        List<core.SortersFromData.RefSorter> _refSorters;

        public IEnumerable<core.SortersFromData.RefSorter> RefSorters
        {
            get
            {
                return _refSorters ?? 
                    (
                        _refSorters =
                            typeof(core.SortersFromData.RefSorter)
                            .GetProperties()
                            .Where(p => p.PropertyType == typeof(core.SortersFromData.RefSorter))
                            .Select(p => (core.SortersFromData.RefSorter)p.GetMethod.Invoke(null, null))
                            .ToList()
                    );
            }
        }

        public core.SortersFromData.RefSorter RefSorter
        {
            get { return (core.SortersFromData.RefSorter)GetValue(RefSorterProperty); }
            set { SetValue(RefSorterProperty, value); }
        }

        public static readonly DependencyProperty RefSorterProperty =
            DependencyProperty.Register("RefSorter", typeof(core.SortersFromData.RefSorter), typeof(SorterGenControl),
            new FrameworkPropertyMetadata(core.SortersFromData.RefSorter.Order25, 
                FrameworkPropertyMetadataOptions.None, OnRefSorterPropertyPropertyChanged));


        private static void OnRefSorterPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var sorterRunControl = (SorterRunControl)d;
            //var stageLayout = (StageLayout)e.NewValue;
            //if (sorterRunControl.SorterDisplayVm != null)
            //{
            //    sorterRunControl.SorterDisplayVm.StageLayout = stageLayout;
            //    sorterRunControl.DoReset();
            //}
        }

        #endregion

    }
}
