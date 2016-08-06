using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace RountGradualFigure
{
    public class IntBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            byte r = 0, g = 0xff, b = 0;
            int n = (int)value;
            if (n > 0xff)
                return new SolidColorBrush(Colors.Red);
            g -= (byte)n;
            r += (byte)n;
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}