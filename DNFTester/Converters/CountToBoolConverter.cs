﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DNFTester.Converters
{
    public class CountToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is Int32 && ((Int32) value > 0))? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
