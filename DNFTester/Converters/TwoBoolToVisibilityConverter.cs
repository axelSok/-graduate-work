using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DNFTester.Converters
{
    public class TwoBoolToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value1 = (bool)values[0];
            var value2 = (bool)values[1];
            return value1 && value2 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}