using System;
using Windows.UI.Xaml.Data;

namespace VarietyHiggstGushed
{
    public class ConverDoubleStr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var n = (double) value;
            return n.ToString("0.##");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
