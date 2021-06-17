using Microsoft.VisualStudio.Utilities;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VSRESTClient.UI.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var requestedTab = (parameter as string);

            if (parameter.Equals(value))
                return Visibility.Visible;

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
