using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VSRESTClient.UI.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.Equals(value))
                return new SolidColorBrush(Color.FromRgb(239,234,109));

            return new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
