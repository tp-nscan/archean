using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;

namespace archean.controls.Utils.Converters
{
    public class UnitIntervalToBrushConverter : MarkupExtension, IValueConverter
    {
        static UnitIntervalToBrushConverter()
        {
            grayBrush = new SolidColorBrush(Colors.Gray);
            grayBrush.Freeze();
            blackBrush = new SolidColorBrush(Colors.Black);
            blackBrush.Freeze();
            blueVioletBrush = new SolidColorBrush(Colors.BlueViolet);
            blueVioletBrush.Freeze();
            blueBrush = new SolidColorBrush(Colors.Blue);
            blueBrush.Freeze();
            redOrangeBrush = new SolidColorBrush(Colors.OrangeRed);
            redOrangeBrush.Freeze();
            greenBrush = new SolidColorBrush(Colors.Green);
            greenBrush.Freeze();
            orangeBrush = new SolidColorBrush(Colors.Orange);
            orangeBrush.Freeze();
            redBrush = new SolidColorBrush(Colors.DarkRed);
            redBrush.Freeze();
            pinkBrush = new SolidColorBrush(Colors.Pink);
            pinkBrush.Freeze();
        }

        private static readonly Brush grayBrush;
        private static readonly Brush blackBrush;
        private static readonly Brush blueBrush;
        private static readonly Brush blueVioletBrush;
        private static readonly Brush greenBrush;
        private static readonly Brush orangeBrush; 
        private static readonly Brush redBrush;
        private static readonly Brush redOrangeBrush;
        private static readonly Brush pinkBrush;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var val =  (((double?)value).Value);

                if (Math.Abs(val - 0) < 0.0000000001) return grayBrush;
                if (val < 0.01) return blueVioletBrush;
                if (val < 0.1) return blueBrush;
                if (val < 0.5) return greenBrush;
                if (val < 0.9) return orangeBrush;
                if (val < 0.99) return redOrangeBrush;
                if (val < 0.99999999999) return redBrush;
                if (val < 1.00000000001) return blackBrush;
                return pinkBrush;
            }
            catch
            {
                return pinkBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class BoolToThicknessConverter : MarkupExtension, IValueConverter
    {
        #region FalseValue

        Thickness _falseValue = new Thickness(0);

        public Thickness FalseValue
        {
            get { return _falseValue; }
            set { _falseValue = value; }
        }

        #endregion

        #region TrueValue

        Thickness _trueValue = new Thickness(0);

        public Thickness TrueValue
        {
            get { return _trueValue; }
            set { _trueValue = value; }
        }

        #endregion

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (((bool?)value).Value) ? TrueValue : FalseValue;
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class BoolToColorConverter : MarkupExtension, IValueConverter
    {
        #region FalseValue

        Color _falseValue = Colors.Transparent;

        public Color FalseValue
        {
            get { return _falseValue; }
            set { _falseValue = value; }
        }

        #endregion

        #region TrueValue
        Color _trueValue = Colors.Transparent;

        public Color TrueValue
        {
            get { return _trueValue; }
            set { _trueValue = value; }
        }

        #endregion

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (((bool?)value).Value) ? TrueValue : FalseValue;
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// Converts points to a PointCollection
    /// </summary>
    public class PointCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<Point> enumerable = value as IEnumerable<Point>;
            PointCollection points = null;
            if (enumerable != null)
            {
                points = new PointCollection(enumerable);
            }
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack should never be called");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((bool?)value).Value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class InvBoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((bool?)value).Value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class VisibleIfHasItemsConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var col = value as ICollection;
            if (value == null) { return Visibility.Collapsed; }
            return col != null && (col.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class EnumBooleanConverter : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    } 
}
