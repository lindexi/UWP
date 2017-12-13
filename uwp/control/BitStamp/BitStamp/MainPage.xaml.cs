using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace BitStamp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //Frame frame = Content as Frame;
            //if (frame == null)
            //{
            //    frame = new Frame();
            //    Content = frame;
            //}
            //frame.Navigate(typeof(View.AssBjPage));

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Color.FromArgb(0xFF, 140, 206, 205);
            ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = Color.FromArgb(0xFF, 250, 250, 250);
            ApplicationView.GetForCurrentView().TitleBar.InactiveForegroundColor = Color.FromArgb(0xFF, 250, 250, 250);

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
            Loaded += MainPage_Loaded;
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            var titleBar = sender;
            TitleBar.Padding = new Thickness(titleBar.SystemOverlayLeftInset, 0, titleBar.SystemOverlayRightInset, 0);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
