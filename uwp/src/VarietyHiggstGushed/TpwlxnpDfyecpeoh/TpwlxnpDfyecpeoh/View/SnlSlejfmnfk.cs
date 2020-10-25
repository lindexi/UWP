using System;
using System.Globalization;
using System.Windows.Data;

namespace TpwlxnpDfyecpeoh.View
{
    public class SnlSlejfmnfk : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double kbjjDzn)
            {
                return ((int) kbjjDzn).ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
