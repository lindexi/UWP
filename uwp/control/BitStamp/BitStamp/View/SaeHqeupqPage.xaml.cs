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
using BitStamp.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BitStamp.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SaeHqeupqPage : Page
    {
        public SaeHqeupqPage()
        {
            this.InitializeComponent();
        }

        public SaeHqeupqModel ViewModel { get; private set; }

        public Account View { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (SaeHqeupqModel) e.Parameter;
            View = ViewModel.Account;
            DataContext = ViewModel;

            base.OnNavigatedTo(e);
        }
    }

    public class ConvertBoolenVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is true)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}