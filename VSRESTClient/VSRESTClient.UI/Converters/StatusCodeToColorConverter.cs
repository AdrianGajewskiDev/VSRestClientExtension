using Microsoft.VisualStudio.Utilities;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace VSRESTClient.UI.Converters
{
    [TypeConversion(typeof(int), typeof(SolidBrush))]
    public class StatusCodeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statusCode = (int)parameter;

            if (statusCode < 200)
                return new SolidBrush(Color.White);
            else if (statusCode >= 200 && statusCode < 300)
                return new SolidBrush(Color.Green);
            else if (statusCode >= 300 && statusCode < 500)
                return new SolidBrush(Color.Orange);
            else
                return new SolidBrush(Color.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw null;
        }
    }
}
