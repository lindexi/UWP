using System;
using System.Globalization;
using System.Windows.Data;

namespace TpwlxnpDfyecpeoh.View
{
    public class DyakmdgwuTlaukxbo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double kbjjDzn)
            {
                return kbjjDzn.ToString("F");
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}