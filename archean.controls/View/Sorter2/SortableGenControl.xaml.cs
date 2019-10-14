
using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace archean.controls.View.Sorter2
{
    public partial class SortableGenControl
    {
        public SortableGenControl()
        {
            InitializeComponent();
        }


        #region SortableType

        public IEnumerable<SortableType> SortableTypes
        {
            get
            {
                return Enum.GetValues(typeof(SortableType)).Cast<SortableType>();
            }
        }

        public SortableType SortableType
        {
            get { return (SortableType)GetValue(SortableTypeProperty); }
            set { SetValue(SortableTypeProperty, value); }
        }

        public static readonly DependencyProperty SortableTypeProperty =
            DependencyProperty.Register("SortableType", typeof(SortableType), typeof(SortableGenControl),
            new FrameworkPropertyMetadata(propertyChangedCallback: OnSortableTypePropertyChanged));

        private static void OnSortableTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterGenControl = (SortableGenControl)d;
            var newType = (SortableType)e.NewValue;
            sorterGenControl.SortableItemVmsGen = MakeFunc(newType, sorterGenControl.Order);
        }

        #endregion


        #region Order

        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public static readonly DependencyProperty OrderProperty =
            DependencyProperty.Register("Order", typeof(int), typeof(SortableGenControl),
            new FrameworkPropertyMetadata(propertyChangedCallback: OnOrderPropertyChanged));


        private static void OnOrderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterGenControl = (SortableGenControl)d;
            var newOrder = (int)e.NewValue;
            sorterGenControl.SortableItemVmsGen = MakeFunc(sorterGenControl.SortableType, newOrder);
        }

        #endregion


        static Func<SortableItemVm[]> MakeFunc(SortableType sortableType, int order)
        {
            switch (sortableType)
            {
                case SortableType.Integer:
                    return () => SortableItemVmExt.RandomPermutationSortableItemVms(
                                        order,
                                        DateTime.Now.Millisecond, true);
                case SortableType.Bool:
                    return () => SortableItemVmExt.Random_0_1_SortableItemVms(
                                        order,
                                        DateTime.Now.Millisecond, true);
                default:
                    return null;
            }

        }


        #region SortableItemVmsGen

        public Func<SortableItemVm[]> SortableItemVmsGen
        {
            get { return (Func<SortableItemVm[]>)GetValue(SortableItemVmsGenProperty); }
            set { SetValue(SortableItemVmsGenProperty, value); }
        }

        public static readonly DependencyProperty SortableItemVmsGenProperty =
            DependencyProperty.Register("SortableItemVmsGen", typeof(Func<SortableItemVm[]>), typeof(SortableGenControl));

        #endregion


    }
}
