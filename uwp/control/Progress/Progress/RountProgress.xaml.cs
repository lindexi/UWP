using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace lindexi.uwp.control.RountProgress.View
{
    public sealed partial class RountProgress : UserControl
    {
        public RountProgress()
        {
            this.InitializeComponent();

            //DispatcherTimer timer = new DispatcherTimer()
            //{
            //    Interval = new TimeSpan(10)
            //};

            //timer.Tick += (o, e) =>
            //{
            //    Value = Value == 100 ? Value + 1 : 0;
            //};
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(RountProgress), new PropertyMetadata(default(double)));

        public double Value
        {
            get
            {
                return (double) GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private Windows.UI.Xaml.Media.DoubleCollection _double = new DoubleCollection();
    }

    public class ConvertDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double thine = 3;
            double w = 100 - thine;
            double n = Math.PI * w / thine * (double) value / 100;
            DoubleCollection temp = new DoubleCollection()
            {
               n,
                1000
            };

            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
