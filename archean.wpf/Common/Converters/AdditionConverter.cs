using System;
using System.Globalization;
using System.Windows.Data;

namespace archean.Common.Converters
{
    public class AdditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var baseAmt = (int)value;
                var deltaAmt = int.Parse((string)parameter);
                return baseAmt + deltaAmt;
            }
            catch (Exception ex)
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
