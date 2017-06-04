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
using lindexi.uwp.Framework.ViewModel;
using ViewModel = lindexi.uwp.Progress.ViewModel.ViewModel;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Progress
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(ViewModel = typeof(ViewModel))]
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = (ViewModel) DataContext;
            ViewModel.Content = frame;
            ViewModel.NavigatedTo(this,null);

            DispatcherTimer timer=new DispatcherTimer()
            {
                Interval = new TimeSpan(10)
            };

            timer.Tick += (o, e) =>
            {
                Value = Math.Abs(Value - 100) < 0.0001 ? 0 : Value + 1;
                //Value = 60;
            };

            _time = timer;

        }

        public ViewModel ViewModel { set; get; } 

        private DispatcherTimer _time;

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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //qut.Maximum-=10;
            _time.Start();
        }

     
    }
}
