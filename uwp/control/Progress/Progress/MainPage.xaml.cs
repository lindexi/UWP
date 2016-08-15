using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Progress
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DispatcherTimer timer=new DispatcherTimer()
            {
                Interval = new TimeSpan(10)
            };

            timer.Tick += (o, e) =>
            {
                Value = Value == 100 ? 0 : Value + 1;
                //Value = 60;
            };

            timer.Start();
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(MainPage), new PropertyMetadata(default(double)));

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
    }
}
