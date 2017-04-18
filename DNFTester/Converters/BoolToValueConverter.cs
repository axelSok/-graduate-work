using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DNFTester.Converters
{
    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }


        public T TrueValue { get; set; }


        public T NullValue { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullValue;
            return bool.Parse(value.ToString()) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(NullValue)) return NullValue;
            return value.Equals(TrueValue);
        }
    }

    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BoolToBrushConverter : BoolToValueConverter<Brush> { }

    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BoolToForegroundConverter : BoolToValueConverter<Brush> { }

    [ValueConversion(typeof(bool), typeof(ImageSource))]
    public class BoolToImageSourceConverter : BoolToValueConverter<ImageSource> { }

    [ValueConversion(typeof(bool), typeof(ImageSource))]
    public class AssistAndHasLivesToImageSourceConverter : IMultiValueConverter
    {

        public ImageSource HasLives { get; set; }
        public ImageSource AssistLive { get; set; }
        public ImageSource NoAssistLive { get; set; }


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasLive;
            bool.TryParse(values[0].ToString(), out hasLive);
            bool assistLive;
            bool.TryParse(values[1].ToString(), out assistLive);
            if (hasLive) return HasLives;
            return assistLive ? AssistLive : NoAssistLive;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(int))]
    public class BoolToIntConverter : BoolToValueConverter<int> { }


    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToOpacityConverter : BoolToValueConverter<double> { }

    [ValueConversion(typeof(bool), typeof(object))]
    public class BoolToObjectConverter : BoolToValueConverter<object> { }

    [ValueConversion(typeof(bool), typeof(ControlTemplate))]
    public class BoolToTemplateConverter : BoolToValueConverter<ControlTemplate> { }

    [ValueConversion(typeof(bool), typeof(DataTemplate))]
    public class BoolToDataTemplateConverter : BoolToValueConverter<DataTemplate> { }

    [ValueConversion(typeof(bool), typeof(FontWeight))]
    public class BoolToFontWeightConverter : BoolToValueConverter<FontWeight> { }

    [ValueConversion(typeof(object), typeof(Boolean))]
    public class ValueToBoolean : BoolToValueConverter<Boolean> { }


    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        #region Члены IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToWordConverter : BoolToValueConverter<string> { }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }

    }

